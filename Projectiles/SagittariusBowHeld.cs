﻿using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace tsorcRevamp.Projectiles {
    class SagittariusBowHeld : ModProjectile {

        private int charge;
        private int chargeTimer;

        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Bow Holdout");
            Main.projFrames[projectile.type] = 7;
            ProjectileID.Sets.NeedsUUID[projectile.type] = true;
        }

        public override void SetDefaults() {
            projectile.CloneDefaults(ProjectileID.LastPrism); //so the visual bow does no damage
            projectile.width = 50;
            projectile.height = 12;
            projectile.friendly = false;
        }

        public override Color? GetAlpha(Color lightColor) {
            return Color.White;
        }

        public override void AI() {
            const int MAX_CHARGE_COUNT = 6;
            Player player = Main.player[projectile.owner];
            Vector2 playerHandPos = player.RotatedRelativePoint(player.MountedCenter);
            //update character visuals while idle
            {
                projectile.Center = playerHandPos;
                projectile.rotation = projectile.velocity.ToRotation() + (float)Math.PI / 2f;
                player.heldProj = projectile.whoAmI;
                player.itemRotation = (projectile.velocity * projectile.direction).ToRotation();
                player.ChangeDir(projectile.direction);
            }
            if (projectile.owner == Main.myPlayer) {
                //update character visuals while aiming
                {
                    Vector2 aimVector = Vector2.Normalize(Main.MouseWorld - playerHandPos);
                    aimVector = Vector2.Normalize(Vector2.Lerp(Vector2.Normalize(projectile.velocity), aimVector, 0.3f)); //taken straight from RedLaserBeam, thanks past me!
                    aimVector *= player.HeldItem.shootSpeed;
                    if (aimVector != projectile.velocity) {
                        projectile.netUpdate = true; //update the bow visually to other players when we change aim
                    }
                    projectile.velocity = aimVector;
                }
                bool charging = player.channel && !player.noItems && !player.CCed; //not cursed or frozen, and holding lmb
                int maxChargeTime; //for modifying the max charge time based on prefix


                if ((player.HeldItem.useTime + 1) % MAX_CHARGE_COUNT == 0) { //for rounding up
                    maxChargeTime = player.HeldItem.useTime + 1;
                }
                else if ((player.HeldItem.useTime + 2) % MAX_CHARGE_COUNT == 0) { //for rounding up
                    maxChargeTime = player.HeldItem.useTime + 2;
                }
                else maxChargeTime = player.HeldItem.useTime - (player.HeldItem.useTime % MAX_CHARGE_COUNT); //round down if 3, 4, or 5

                int chargeInterval = maxChargeTime / MAX_CHARGE_COUNT;

                if (charging) {
                    chargeTimer++;
                    if ((chargeTimer % chargeInterval == 0) && (chargeTimer <= maxChargeTime)) { //gain one charge every chargeInterval frames, up to max of MAX_CHARGE_COUNT
                        projectile.frame++;
                        charge++;
                    }
                }
                else {
                    chargeTimer = 0;
                    Vector2 bowVelocity = Vector2.Normalize(projectile.velocity);

                    if (charge != 0) { //dont fire zero-velocity arrows, it looks silly

                        int ammoLocation = 0;
                        int ammoProjectileType = 0;

                        FindAmmo(player, ref ammoLocation, ref ammoProjectileType);

                        for (int i = 0; i < 2; i++) {
                            Vector2 inaccuracy = new Vector2(bowVelocity.X, bowVelocity.Y).RotatedByRandom(MathHelper.ToRadians((float)16f - (charge) * 2.5f)); //more accurate when charged

                            //Vector2 projectileVelocity = inaccuracy * (player.HeldItem.shootSpeed - (3 * (MAX_CHARGE_COUNT - charge))); //faster arrows when charged
                            Vector2 projectileVelocity = inaccuracy * (1 + (((8 * MAX_CHARGE_COUNT) / (3 * player.HeldItem.shootSpeed)) * (float)(Math.Pow((Math.Floor((double)charge)), 2))));
                            //this is the ugliest shit ive ever written in my entire life
                            //speed modifier = 1 + ((8a/3b) * (floor(c))^2)
                            //a = max_charge_count (6)
                            //b = helditem.shootspeed (24)
                            //c = charge
                            //aka y = 1 + ((2/3) * (floor(x))^2)

                            if ((ammoLocation != 0) && (player.inventory[ammoLocation].stack > 0)) {
                                Projectile.NewProjectile(projectile.Center, projectileVelocity, ammoProjectileType, projectile.damage, projectile.knockBack, projectile.owner);
                                player.inventory[ammoLocation].stack--;
                                if (player.inventory[ammoLocation].stack == 0) {
                                    player.inventory[ammoLocation].TurnToAir();
                                }
                            }
                        }
                        Main.PlaySound(SoundID.Item5.WithVolume(0.8f), player.position);
                    }
                    charge = 0; //reset the charge
                    projectile.Kill(); //and kill the bow so we dont keep shooting
                }
            }
        }


        private void FindAmmo(Player player, ref int ammoLocation, ref int ammoProjectileType) {
            for (int k = 54; k < 58; k++) {
                if (player.inventory[k].ammo == AmmoID.Arrow && player.inventory[k].stack > 0) {
                    ammoLocation = k;
                    ammoProjectileType = player.inventory[k].shoot;
                    break;
                }
            }
            if (ammoLocation == 0) {
                for (int j = 0; j < 54; j++) {
                    if (player.inventory[j].ammo == AmmoID.Arrow && player.inventory[j].stack > 0) {
                        ammoLocation = j;
                        ammoProjectileType = player.inventory[j].shoot;
                        break;
                    }
                }
            }
        }
    }
}