using Humanizer;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using tsorcRevamp.Items.Armors.Magic;

namespace tsorcRevamp.Items.VanillaItems
{
    class MagicEdits : GlobalItem
    {
        public static int RedClothMaxManaBoost = 40;
        public static float RedClothManaCostReduction = 5f;
        public override void SetDefaults(Item item)
        {
            //Why is this eventide's internal name i'm literally going to go feral
            if (item.type == ItemID.SparkleGuitar)
            {
                item.mana = 25;
            }
            if (item.type == ItemID.CrimsonRod)
            {
                item.DamageType = DamageClass.MagicSummonHybrid;
            }
            if (item.type == ItemID.NimbusRod)
            {
                item.DamageType = DamageClass.MagicSummonHybrid;
            }
            if (item.type == ItemID.ClingerStaff)
            {
                item.DamageType = DamageClass.MagicSummonHybrid;
            }
            if (item.type == ItemID.NimbusRod)
            {
                item.DamageType = DamageClass.MagicSummonHybrid;
            }

            if (item.type == ItemID.FairyQueenMagicItem)
            {
                item.damage = 38;
            }
            if (item.type == ItemID.SparkleGuitar)
            {
                item.damage = 50;
            }

            //Lunar items
            if (item.type == ItemID.NebulaBlaze)
            {
                item.mana = 24;
            }
            if (item.type == ItemID.NebulaArcanum)
            {
                item.mana = 60;
            }
            if (item.type == ItemID.LastPrism)
            {
                item.mana = 30;
            }
            if (item.type == ItemID.LunarFlareBook)
            {
                item.damage = 80;
                item.mana = 39;
            }
        }
        public override string IsArmorSet(Item head, Item body, Item legs)
        {
            if (head.type == ModContent.ItemType<RedClothHat>() && body.type == ItemID.GypsyRobe && legs.type == ModContent.ItemType<RedClothPants>())
            {
                return "RedClothRobe";
            }
            else return base.IsArmorSet(head, body, legs);
        }
        public override void UpdateArmorSet(Player player, string set)
        {
            if (set == "RedClothRobe")
            {
                player.setBonus = Language.GetTextValue("Mods.tsorcRevamp.Items.VanillaItems.RedClothRobe").FormatWith(RedClothMaxManaBoost, RedClothManaCostReduction);

                player.statManaMax2 += RedClothMaxManaBoost;
                player.manaCost -= RedClothManaCostReduction / 100f;
            }
        }
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            Player player = Main.player[Main.myPlayer];
            if (item.type == ItemID.CrimsonRod)
            {
                int ttindex = tooltips.FindIndex(t => t.Name == "Damage");
                if (ttindex != -1)
                {
                    tooltips.RemoveAt(ttindex);
                    tooltips.Insert(ttindex, new TooltipLine(Mod, "DamageType", $"{(int)player.GetTotalDamage(DamageClass.MagicSummonHybrid).ApplyTo(item.damage)} " + Language.GetTextValue("Mods.tsorcRevamp.Items.VanillaItems.MagicSummonHybridDamageClass")));
                }
            }
            if (item.type == ItemID.NimbusRod)
            {
                int ttindex = tooltips.FindIndex(t => t.Name == "Damage");
                if (ttindex != -1)
                {
                    tooltips.RemoveAt(ttindex);
                    tooltips.Insert(ttindex, new TooltipLine(Mod, "DamageType", $"{(int)player.GetTotalDamage(DamageClass.MagicSummonHybrid).ApplyTo(item.damage)} " + Language.GetTextValue("Mods.tsorcRevamp.Items.VanillaItems.MagicSummonHybridDamageClass")));
                }
            }
            if (item.type == ItemID.ClingerStaff)
            {
                int ttindex = tooltips.FindIndex(t => t.Name == "Damage");
                if (ttindex != -1)
                {
                    tooltips.RemoveAt(ttindex);
                    tooltips.Insert(ttindex, new TooltipLine(Mod, "DamageType", $"{(int)player.GetTotalDamage(DamageClass.MagicSummonHybrid).ApplyTo(item.damage)} " + Language.GetTextValue("Mods.tsorcRevamp.Items.VanillaItems.MagicSummonHybridDamageClass")));
                }
            }
            if (item.type == ItemID.MagnetSphere)
            {
                int ttindex = tooltips.FindIndex(t => t.Name == "Damage");
                if (ttindex != -1)
                {
                    tooltips.RemoveAt(ttindex);
                    tooltips.Insert(ttindex, new TooltipLine(Mod, "DamageType", $"{(int)player.GetTotalDamage(DamageClass.MagicSummonHybrid).ApplyTo(item.damage)} " + Language.GetTextValue("Mods.tsorcRevamp.Items.VanillaItems.MagicSummonHybridDamageClass")));
                }
            }
        }
    }
}