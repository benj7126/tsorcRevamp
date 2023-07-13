﻿using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace tsorcRevamp.Items.Accessories.Expert
{
    [AutoloadEquip(EquipType.Face)]
    public class PhoenixSkull : ModItem
    {
        public static int Cooldown = 90;
        public static float HealthPercent = 25f;
        public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(Cooldown, HealthPercent);
        public override void SetStaticDefaults()
        {
        }

        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.width = 18;
            Item.height = 18;
            Item.expert = true;
            Item.value = PriceByRarity.LightRed_4;
        }
        public override void UpdateEquip(Player player)
        {
            player.GetModPlayer<tsorcRevampPlayer>().PhoenixSkull = true;
        }
    }
}