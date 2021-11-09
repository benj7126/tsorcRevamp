using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.NetModules;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using tsorcRevamp.Projectiles;
using tsorcRevamp.Projectiles.Enemy;
using tsorcRevamp.Projectiles.Enemy.DarkCloud;

namespace tsorcRevamp.NPCs.Bosses.SuperHardMode
{
    [AutoloadBossHead]
    class DarkCloudMirror : ModNPC
    {
        public override void SetStaticDefaults()
        {
            NPCID.Sets.TrailCacheLength[npc.type] = (int)TRAIL_LENGTH;    //The length of old position to be recorded
            NPCID.Sets.TrailingMode[npc.type] = 1;
        }
        public override void SetDefaults()
        {
            npc.npcSlots = 200;
            Main.npcFrameCount[npc.type] = 16;
            animationType = 0;
            npc.aiStyle = 0;
            npc.height = 40;
            npc.width = 20;
            music = 12;
            npc.damage = 105;
            npc.defense = 160;
            npc.lifeMax = 30000;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.value = 1500000;
            npc.knockBackResist = 0f;
            npc.timeLeft = 310;
            npc.noGravity = true;
            npc.noTileCollide = true;
        }
        const float TRAIL_LENGTH = 12;

        int divineSparkDamage = 150;
        int darkFlowDamage = 100;
        int antiMatDamage = 200;
        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            npc.lifeMax /= 2;
            npc.damage /= 2;
            divineSparkDamage /= 2;
            darkFlowDamage /= 2;
            antiMatDamage /= 2;
        }
        
        int attackModeTally = 0;

        public int MirrorAttackType
        {
            get => (int)npc.ai[0];
            set => npc.ai[0] = value;
        }
        public float AttackModeCounter;
        public float AttackModeLimit
        {
            get => (int)npc.ai[1];
            set => npc.ai[1] = value;
        }
        public Player PlayerTarget
        {
            get => Main.player[(int)npc.ai[2]];
        }

        //Depricated
        public Player Target
        {
            get => Main.player[npc.target];
        }

        Vector2 nextWarpPoint = Vector2.Zero;
        float warpRadius = 450;
        public override void AI()
        {
            Lighting.AddLight(npc.Center, Color.Blue.ToVector3() * 0.5f);
            UsefulFunctions.DustRing(npc.Center, 32, DustID.ShadowbeamStaff);

            if (MirrorAttackType == DarkCloud.DarkCloudAttackID.AntiMat)
            {
                AntiMatMove();
                if(Main.netMode != NetmodeID.MultiplayerClient)
                {
                    AntiMatAttack();
                }
                AttackModeCounter++;
                if (AttackModeCounter == AttackModeLimit + 10)
                {
                    npc.active = false;
                }
            }

            
            if (MirrorAttackType == DarkCloud.DarkCloudAttackID.TeleportingSlashes)
            {
                TeleportingSlashesAttack();
            }            
        }

        //Randomly pick a new unused attack and reset attack variables
       

        //These describe how the boss should move, and other things that should be done on every client to keep it deterministic
        #region Movements
        void AntiMatMove()
        {
            //Line up an Anti-Mat with a targeting laser, cause a flash near the rifle scope, then a half(?) second later fire a bolt at immense speed at the player
            //Requires dodging almost instantly after seeing the flash
           
            if (AttackModeCounter == 300)
            {
                attackModeTally++;
                AttackModeCounter = -1;
                if (attackModeTally == 3)
                {
                    MirrorAttackType = -1;
                }
            }            
        }
        #endregion

        //These describe projectiles the boss should shoot, and other things that should *not* be done for every multiplayer client
        #region Attacks
        GenericLaser laser;
        void AntiMatAttack()
        {
            if(AttackModeCounter == 0)
            {
                
                laser = (GenericLaser)Projectile.NewProjectileDirect(npc.Center, Vector2.Zero, ModContent.ProjectileType<GenericLaser>(), 0, 0.5f, Main.myPlayer).modProjectile;
                laser.LaserOrigin = npc.Center;                    
                laser.LaserTarget = Target.Center;

                Vector2 offset = UsefulFunctions.GenerateTargetingVector(npc.Center, Target.Center, 5).RotatedBy(MathHelper.ToRadians(90));
                offset *= ((300 - AttackModeCounter) / 300) * (float)Math.Sin(MathHelper.ToRadians(AttackModeCounter));
                laser.LaserTarget += offset;
                    
                laser.TelegraphTime = 99999;
                laser.LaserLength = 4000;
                laser.LaserColor = Color.Red;
                laser.TargetingMode = 1;
                laser.lightColor = Color.OrangeRed;
                laser.TileCollide = false;
                laser.CastLight = false;
                laser.MaxCharge = 5;
                laser.FiringDuration = (int)AttackModeLimit + 1;
                laser.LaserVolume = 0;
                
            }
            if (AttackModeCounter < AttackModeLimit)
            {
                Vector2 offset = new Vector2(0,0);
                for (int i = 0; i < 3; i++)
                {
                    offset = UsefulFunctions.GenerateTargetingVector(npc.Center, Target.Center, 128).RotatedBy(MathHelper.ToRadians(90));
                    offset *= ((300 - AttackModeCounter) / 300);
                    offset = offset.RotatedBy(MathHelper.ToRadians(AttackModeCounter + (120 * i)));
                    laser.LaserTarget = Target.Center + offset;
                }
            }
            if (AttackModeCounter == AttackModeLimit)
            {
                Projectile.NewProjectile(npc.Center, UsefulFunctions.GenerateTargetingVector(npc.Center, Target.Center, 7), ModContent.ProjectileType<DarkAntiMatRound>(), antiMatDamage / 2, 0.5f, Main.myPlayer);
            }
        }

        void ThunderstormAttack()
        {

        }
        void TeleportingSlashesAttack()
        {
            warpRadius = 750;
            npc.noGravity = true;
            npc.noTileCollide = true;
            npc.aiStyle = 0;
            npc.velocity.Y += 0.09f;
            npc.velocity.X *= 1.07f;
            if (Target.Center.X > npc.Center.X)
            {
                npc.direction = 1;
            }
            else
            {
                npc.direction = -1;
            }

            if (AttackModeCounter == 0)
            {
                nextWarpPoint = Target.Center + Main.rand.NextVector2CircularEdge(warpRadius, warpRadius);
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, ModContent.NPCType<DarkUltimaWeapon>(), ai0: npc.whoAmI, ai2: DarkCloud.DarkCloudAttackID.TeleportingSlashes);
                }
            }

            if (AttackModeCounter % 80 == 0 && AttackModeCounter != 80)
            {
                if (nextWarpPoint != null)
                {
                    DarkCloudParticleEffect(-2);
                    npc.Center = nextWarpPoint;
                    DarkCloudParticleEffect(6);
                    npc.velocity = UsefulFunctions.GenerateTargetingVector(npc.Center, Target.Center, 17);
                }

                nextWarpPoint = Target.Center + Main.rand.NextVector2CircularEdge(warpRadius, warpRadius);
            }

            for (int i = 0; i < 50; i++)
            {
                Dust.NewDustPerfect(nextWarpPoint + Main.rand.NextVector2Circular(30, 60), DustID.ShadowbeamStaff, Main.rand.NextVector2CircularEdge(3, 3));
            }


            if (AttackModeCounter == 640)
            {
                npc.active = false;
            }
            AttackModeCounter++;
        }

        void BulletPortalsAttack()
        {

        }
        #endregion

        

        #region Teleport Functions
        //These functions make the boss move to various places (and mainly exist so I don't have to rewrite the same teleporting code 100 times...)
        void TeleportBehindPlayer()
        {
            DarkCloudParticleEffect(-2);
            npc.Center = Main.player[npc.target].Center;
            if (Main.player[npc.target].direction == 1) {
                npc.position.X -= 128;
            }
            else
            {
                npc.position.X += 128;
            }
            DarkCloudParticleEffect(6);
        }

        void TeleportAroundPlayer(float radius = 192)
        {
            DarkCloudParticleEffect(-2);
            npc.position = Main.player[npc.target].position + Main.rand.NextVector2CircularEdge(radius, radius);
            DarkCloudParticleEffect(6);
        }

        void DashToAroundPlayer()
        {
            //TODO: Implement
        }

        void TeleportToArenaCenter()
        {
            DarkCloudParticleEffect(-2);
            npc.Center = new Vector2(5827.5f, 1698) * 16;
            DarkCloudParticleEffect(6);
        }
        #endregion

        //The dust ring particle effect the boss uses
        void DarkCloudParticleEffect(float dustSpeed, float dustAmount = 50)
        {
            for(int i = 0; i < dustAmount; i++)
            {
                Vector2 offset = Main.rand.NextVector2CircularEdge(64, 64);
                Vector2 velocity = new Vector2(dustSpeed, 0).RotatedBy(offset.ToRotation()) * Main.rand.NextFloat(2);
                Dust.NewDustPerfect(npc.Center + offset, DustID.ShadowbeamStaff, velocity, Scale: 2).noGravity = true;
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            Texture2D texture = ModContent.GetTexture("tsorcRevamp/NPCs/Bosses/SuperHardMode/DarkCloud");
            Rectangle sourceRectangle = new Rectangle(0, 0, texture.Width, texture.Height / Main.npcFrameCount[npc.type]);
            Vector2 origin = sourceRectangle.Size() / 2f;
            SpriteEffects spriteEffects = SpriteEffects.None;
            if (npc.spriteDirection == 1)
            {
                spriteEffects = SpriteEffects.FlipHorizontally;
            }
            for (float i = TRAIL_LENGTH - 1; i >= 0 ; i--)
            {
                Main.spriteBatch.Draw(texture, npc.oldPos[(int)i] - Main.screenPosition + new Vector2(12, 16), sourceRectangle, drawColor * ((TRAIL_LENGTH - i) / TRAIL_LENGTH), npc.rotation, origin, npc.scale, spriteEffects, 0f);
            }
           
            return true;
        }

        public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            if (MirrorAttackType == DarkCloud.DarkCloudAttackID.AntiMat)
            {
                AntiMatDraw(spriteBatch, drawColor);
            }
        }

        #region Draw Functions

        public void AntiMatDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            float targetPoint = UsefulFunctions.GenerateTargetingVector(npc.Center, Target.Center, 1).ToRotation();
            if (!Main.gamePaused && (AttackModeCounter % 3 == 0))
            {                
                Vector2 thisPos = npc.Center + new Vector2(0, 128).RotatedBy(targetPoint - MathHelper.PiOver2) + Main.rand.NextVector2Circular(32, 32);
                Vector2 thisVel = UsefulFunctions.GenerateTargetingVector(thisPos, npc.Center + Main.rand.NextVector2Circular(10, 10), 8);
                Dust.NewDustPerfect(thisPos, DustID.FireworkFountain_Red, thisVel, 100, default, 0.5f).noGravity = true;                
            }
            

            Texture2D texture = ModContent.GetTexture(ModContent.GetModItem(ModContent.ItemType<Items.Weapons.Ranged.AntimatRifle>()).Texture);
            Rectangle sourceRectangle = new Rectangle(0, 0, texture.Width, texture.Height);
            Vector2 origin = new Vector2(0, sourceRectangle.Height / 2);
            SpriteEffects theseEffects = (npc.Center.X < Target.Center.X) ? SpriteEffects.None : SpriteEffects.FlipVertically;
            Main.spriteBatch.Draw(texture, npc.Center - Main.screenPosition, sourceRectangle, drawColor, targetPoint, origin, npc.scale, theseEffects, 0f);
        }
        #endregion

        

        public override void NPCLoot()
        {
            Dust.NewDust(npc.position, npc.width, npc.height, 52, 0.3f, 0.3f, 200, default(Color), 1f);
            Dust.NewDust(npc.position, npc.height, npc.width, 52, 0.2f, 0.2f, 200, default(Color), 3f);
            Dust.NewDust(npc.position, npc.width, npc.height, 52, 0.2f, 0.2f, 200, default(Color), 3f);
            Dust.NewDust(npc.position, npc.height, npc.width, 52, 0.2f, 0.2f, 200, default(Color), 3f);        
            Main.NewText("Just a reflection...", Color.Blue);
        }

        #region Debuffs
        public override void OnHitPlayer(Player player, int damage, bool crit)
        {
            int expertScale = 1;
            if (Main.expertMode) expertScale = 2;

            if (Main.rand.Next(4) == 0)
            {
                player.AddBuff(BuffID.BrokenArmor, 600 / expertScale, false);
                player.AddBuff(BuffID.Poisoned, 1800 / expertScale, false);
                player.AddBuff(BuffID.Bleeding, 1800 / expertScale, false);

            }
            if (Main.rand.Next(2) == 0)
            {
                player.AddBuff(BuffID.BrokenArmor, 120 / expertScale, false); //broken armor
                player.AddBuff(BuffID.OnFire, 180 / expertScale, false); //on fire!
                player.AddBuff(ModContent.BuffType<Buffs.FracturingArmor>(), 3600, false); //defense goes time on every hit
            }
        }
        #endregion

        #region Vanilla overrides and misc
        public override bool CheckActive()
        {
            return false;
        }
        public override void BossLoot(ref string name, ref int potionType)
        {
            potionType = ItemID.SuperHealingPotion;
        }
        //Takes double damage from melee weapons
        public override void ModifyHitByItem(Player player, Item item, ref int damage, ref float knockback, ref bool crit)
        {
            damage *= 2;
            crit = true;
        }
        public override void ModifyHitByProjectile(Projectile projectile, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            if (projectile.melee)
            {
                damage *= 2;
                crit = true;
            }
        }

        #endregion
    }

}