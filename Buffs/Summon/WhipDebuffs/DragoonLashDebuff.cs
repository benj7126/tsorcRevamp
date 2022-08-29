using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Humanizer.In;

namespace tsorcRevamp.Buffs.Summon.WhipDebuffs
{
	public class DragoonLashDebuff : ModBuff
	{

		public override void SetStaticDefaults()
		{
			// This allows the debuff to be inflicted on NPCs that would otherwise be immune to all debuffs.
			// Other mods may check it for different purposes.
			BuffID.Sets.IsAnNPCWhipDebuff[Type] = true;
		}

		public override void Update(NPC npc, ref int buffIndex)
		{
			npc.GetGlobalNPC<DragoonLashDebuffNPC>().markedByDragoonLash = true;
		}
	}

	public class DragoonLashDebuffNPC : GlobalNPC
	{
        // This is required to store information on entities that isn't shared between them.
        public static float fireBreathTimer = 0f;
        public override bool InstancePerEntity => true;

		public bool markedByDragoonLash;

		public override void ResetEffects(NPC npc)
		{
			markedByDragoonLash = false;
		}

		// TODO: Inconsistent with vanilla, increasing damage AFTER it is randomised, not before. Change to a different hook in the future.
		public override void ModifyHitByProjectile(NPC npc, Projectile projectile, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
		{
            var owner = Main.player[projectile.owner];
            int whipDamage = (int)owner.GetTotalDamage(DamageClass.SummonMeleeSpeed).ApplyTo(56);
            // Only player attacks should benefit from this buff, hence the NPC and trap checks.
            if (markedByDragoonLash && !projectile.npcProj && !projectile.trap && (projectile.minion || ProjectileID.Sets.MinionShot[projectile.type]))
			{
				if (fireBreathTimer >= 1)
				{
                    Projectile.NewProjectile(Projectile.GetSource_None(), owner.Center, npc.Center - owner.Center, ProjectileID.ImpFireball, whipDamage, 1f, Main.myPlayer);
					fireBreathTimer = 0;
                }
                damage += 3;
				if (Main.rand.NextBool(50))
				{
					crit = true;
				}
			}
		}
	}
}
