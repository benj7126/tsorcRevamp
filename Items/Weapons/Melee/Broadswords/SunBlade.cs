﻿using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using tsorcRevamp.NPCs.Bosses.SuperHardMode.Fiends;

namespace tsorcRevamp.Items.Weapons.Melee.Broadswords
{
    class SunBlade : ModItem
    {

        public override void SetStaticDefaults()
        {
            /* Tooltip.SetDefault("Praise the sun!" +
                                "\nDoes quadruple damage against the heartless"); */
        }

        public override void SetDefaults()
        {
            Item.rare = ItemRarityID.LightRed;
            Item.damage = 30;
            Item.width = 36;
            Item.height = 36;
            Item.knockBack = 9;
            Item.maxStack = 1;
            Item.DamageType = DamageClass.Melee;
            Item.useAnimation = 20;
            Item.UseSound = SoundID.Item1;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 20;
            Item.value = PriceByRarity.LightRed_4;
            Item.shoot = ModContent.ProjectileType<Projectiles.Nothing>();
            tsorcInstancedGlobalItem instancedGlobal = Item.GetGlobalItem<tsorcInstancedGlobalItem>();
            instancedGlobal.slashColor = Microsoft.Xna.Framework.Color.Orange;
        }

        public override void ModifyHitNPC(Player player, NPC target, ref NPC.HitModifiers modifiers)
        {
            //todo add mod NPCs to this list
            if (NPCID.Sets.Skeletons[target.type]
                    || target.type == NPCID.Zombie
                    || target.type == NPCID.BaldZombie
                    || target.type == NPCID.AngryBones
                    || target.type == NPCID.DarkCaster
                    || target.type == NPCID.CursedSkull
                    || target.type == NPCID.UndeadMiner
                    || target.type == NPCID.Tim
                    || target.type == NPCID.DoctorBones
                    || target.type == NPCID.ArmoredSkeleton
                    || target.type == NPCID.Mummy
                    || target.type == NPCID.DarkMummy
                    || target.type == NPCID.LightMummy
                    || target.type == NPCID.Wraith
                    || target.type == NPCID.SkeletonArcher
                    || target.type == NPCID.PossessedArmor
                    || target.type == NPCID.TheGroom
                    || target.type == NPCID.SkeletronHand
                    || target.type == NPCID.SkeletronHead
                    || target.type == ModContent.NPCType<LichKingSerpentHead>()
                    || target.type == ModContent.NPCType<LichKingSerpentBody>()
                    || target.type == ModContent.NPCType<LichKingSerpentTail>()
                    || target.type == ModContent.NPCType<NPCs.Enemies.DemonSpirit>()
                    || target.type == ModContent.NPCType<NPCs.Enemies.CrazedDemonSpirit>()
                    || target.type == ModContent.NPCType<NPCs.Enemies.ParasyticWormHead>()
                    || target.type == ModContent.NPCType<NPCs.Enemies.ParasyticWormBody>()
                    || target.type == ModContent.NPCType<NPCs.Enemies.ParasyticWormTail>()
                    )
            {
                modifiers.FinalDamage *= 4;
            }
        }
    }
}
