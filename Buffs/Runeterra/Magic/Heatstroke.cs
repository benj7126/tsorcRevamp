using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using tsorcRevamp.Items.Weapons.Magic.Runeterra;
using tsorcRevamp.NPCs;

namespace tsorcRevamp.Buffs.Runeterra.Magic
{
    public class Heatstroke : ModBuff
	{
		public override void SetStaticDefaults()
		{
			BuffID.Sets.IsATagBuff[Type] = true;
		}

        public override void Update(NPC npc, ref int buffIndex)
        {
            npc.GetGlobalNPC<tsorcRevampGlobalNPC>().Sundered = true;
            Dust.NewDust(npc.VisualPosition, npc.width, npc.height, DustID.Torch, 0, 0, 0, default, 0.5f);
        }
    }
}