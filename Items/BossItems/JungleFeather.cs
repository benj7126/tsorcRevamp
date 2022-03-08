﻿using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace tsorcRevamp.Items.BossItems {
    class JungleFeather : ModItem {

        public override void SetStaticDefaults() {
            Tooltip.SetDefault("Summons the Jungle Wyvern \n" + "An ancient beast that once guarded an advanced civilization, \n" + "long since forgotten. To this day, it watches over the lost \n" + "city, ripping to shreds any traveler who should discover it.");
        }

        public override void SetDefaults() {
            item.width = 28;
            item.height = 28;
            item.useStyle = ItemUseStyleID.HoldingUp;
            item.useAnimation = 45;
            item.useTime = 45;
            item.maxStack = 1;
            item.consumable = false;
            item.rare = ItemRarityID.LightRed;
            item.consumable = false;
        }


        public override bool UseItem(Player player) {
            if (NPC.AnyNPCs(ModContent.NPCType<NPCs.Bosses.JungleWyvern.JungleWyvernHead>())) {
                return false;
            }
            if (Main.dayTime) {
                Main.NewText("The ancient Jungle Wyvern remains deep in slumber... Retry at night.", 175, 75, 255);
            }
            else if (!player.ZoneRockLayerHeight) {
                Main.NewText("The ancient Jungle Wyvern must be summoned underground.", 175, 75, 255);
            }
            else {
                Main.NewText("A rumbling thunder shakes the ground below you... ", 175, 75, 255);
                NPC.SpawnOnPlayer(player.whoAmI, ModContent.NPCType<NPCs.Bosses.JungleWyvern.JungleWyvernHead>());
            }
            return true;
        }

        public override void AddRecipes() {
            if (!ModContent.GetInstance<tsorcRevampConfig>().AdventureModeItems)
            {
                ModRecipe recipe = new ModRecipe(mod);
                recipe.AddIngredient(ItemID.Feather);
                recipe.AddIngredient(ItemID.ShadowScale, 1);
                recipe.AddIngredient(ItemID.Bone, 12);
                recipe.AddTile(TileID.DemonAltar);
                recipe.SetResult(this);
                recipe.AddRecipe();
            }
        }
    }
}
