using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace tsorcRevamp.Items.Accessories.Defensive
{
    public class PoisonbiteRing : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("One of the infamous bite rings commissioned by Sir Arstor of Carim." +
                                "\nDespite the dreadful rumors surrounding its creation, this ring is an unmistakable asset," +
                                "\ndue to its ability to prevent becoming poisoned.\n+3 defense");
        }

        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 24;
            Item.accessory = true;
            Item.value = PriceByRarity.Blue_1;
            Item.rare = ItemRarityID.Blue;
        }

        public override void AddRecipes()
        {
            Terraria.Recipe recipe = CreateRecipe();
            recipe.AddIngredient(Mod.Find<ModItem>("SilverRing").Type, 1);
            recipe.AddIngredient(Mod.Find<ModItem>("BloodredMossClump").Type, 20);
            recipe.AddIngredient(Mod.Find<ModItem>("DarkSoul").Type, 2000);
            recipe.AddTile(TileID.DemonAltar);

            recipe.Register();
        }

        public override void UpdateEquip(Player player)
        {
            player.buffImmune[BuffID.Poisoned] = true;
            player.statDefense += 3;
        }

    }
}
