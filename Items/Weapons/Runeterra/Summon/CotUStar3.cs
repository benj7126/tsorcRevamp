using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace tsorcRevamp.Items.Weapons.Runeterra.Summon
{
	public class CotUStar3 : ModProjectile
	{
		public float angularSpeed;

        public static float currentAngle3;

        public static int timer3 = 0;

		public override void SetStaticDefaults()
		{
			Main.projFrames[Projectile.type] = 5;
			Main.projPet[Projectile.type] = true; // Denotes that this projectile is a pet or minion
			ProjectileID.Sets.MinionSacrificable[Projectile.type] = true; // This is needed so your minion can properly spawn when summoned and replaced when other minions are summoned
		}
		public sealed override void SetDefaults()
		{
			Projectile.width = 54;
			Projectile.height = 54;
			Projectile.tileCollide = false; // Makes the minion go through tiles freely

			Projectile.friendly = true; // Only controls if it deals damage to enemies on contact (more on that later)
			Projectile.minion = true; // Declares this as a minion (has many effects)
			Projectile.DamageType = DamageClass.Summon; // Declares the damage type (needed for it to deal damage)
			Projectile.minionSlots = 1f; // Amount of slots this minion occupies from the total minion slots available to the player (more on that later)
			Projectile.penetrate = -1; // Needed so the minion doesn't despawn on collision with enemies or tiles
            Projectile.extraUpdates = 1;

            Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 15;
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (crit)
			{
				CotUBuff3.critcounter += 1;
				CotUBuff3.hascrit3 = true;
			}
		}
		public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
		{
			if (timer3 > 0)
			{
				damage += (Projectile.damage / 4);
			}
		}
		public override bool? CanCutTiles()
		{
			return false;
		}

		public override bool MinionContactDamage()
		{
			return true;
		}
		public override void AI()
		{


			Player owner = Main.player[Projectile.owner];

			Vector2 visualplayercenter = owner.Center + new Vector2(-27, -27);

			if (!CheckActive(owner))
			{
				return;
			}

			if (timer3 > 0)
			{
				angularSpeed = 0.25f;
			}
			if (timer3 < 0)
			{
				angularSpeed = 0.1f;
			}

			currentAngle3 += angularSpeed / (CotUStar1.circleRad / 15);

			Vector2 offset = new Vector2(MathF.Sin(currentAngle3), MathF.Cos(currentAngle3)) * CotUStar1.circleRad;

			Projectile.position = visualplayercenter + offset;

			Visuals();

		}
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            float distance = Vector2.Distance(projHitbox.Center.ToVector2(), targetHitbox.Center.ToVector2());
            if (distance < Projectile.height && distance > Projectile.height - 32)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private bool CheckActive(Player owner)
		{
			if (owner.dead || !owner.active)
			{
				owner.ClearBuff(ModContent.BuffType<CotUBuff3>());

				return false;
			}

			if (!owner.HasBuff(ModContent.BuffType<CotUBuff3>()))
            {
				CotUStar1.circleRad = 50f;
            }

				if (owner.HasBuff(ModContent.BuffType<CotUBuff3>()))
			{
				Projectile.timeLeft = 2;
			}

			return true;
		}

        private void Visuals()
		{

			Projectile.rotation = currentAngle3 * -1f;

            float frameSpeed = 3f;

            Projectile.frameCounter++;

			if (Projectile.frameCounter >= frameSpeed)
			{
				Projectile.frameCounter = 0;
				Projectile.frame++;

				if (Projectile.frame >= Main.projFrames[Projectile.type])
				{
					Projectile.frame = 0;
				}
			}

			// Some visuals here
			Lighting.AddLight(Projectile.Center, Color.Gold.ToVector3() * 0.78f);
		}
	}
}