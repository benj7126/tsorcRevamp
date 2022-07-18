using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace tsorcRevamp.Items.Weapons.Melee.Flails
{
    public class HeavensTear : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Heaven's Tear");
            Tooltip.SetDefault("Heaven splits with each swing" +
                "\nDeals double damage to mages and ghosts");

        }

        public override void SetDefaults()
        {

            Item.width = 32;
            Item.height = 32;
            //item.pretendType=389;
            //item.prefixType=368;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.channel = true;
            Item.useAnimation = 38;
            Item.useTime = 38;
            Item.maxStack = 1;
            Item.damage = 200;
            Item.knockBack = (float)10;
            Item.scale = (float)1;
            Item.UseSound = SoundID.Item1;
            Item.rare = ItemRarityID.Red;
            Item.shootSpeed = (float)14;
            Item.noUseGraphic = true;
            Item.noMelee = true;
            Item.value = PriceByRarity.Red_10;
            Item.DamageType = DamageClass.Melee;
            Item.shoot = ModContent.ProjectileType<Projectiles.Flails.HeavensTearBall>();
        }

        public override void AddRecipes()
        {
            Terraria.Recipe recipe = CreateRecipe();

            recipe.AddIngredient(ItemID.FlowerPow, 1);
            recipe.AddIngredient(Mod.Find<ModItem>("GuardianSoul").Type, 1);
            recipe.AddIngredient(Mod.Find<ModItem>("CursedSoul").Type, 20);
            recipe.AddIngredient(Mod.Find<ModItem>("SoulOfArtorias").Type, 1);
            recipe.AddIngredient(Mod.Find<ModItem>("DarkSoul").Type, 120000);

            recipe.AddTile(TileID.DemonAltar);

            recipe.Register();
        }

        public override void ModifyHitNPC(Player player, NPC npc, ref int damage, ref float knockBack, ref bool crit)
        {
            //damage = (int) ((Main.rand.Next(26)) * (P.GetDamage(DamageClass.Melee)));
            if (npc.FullName == "Tim"
                || npc.FullName == "Dark Caster"
                || npc.FullName == "Goblin Sorcerer"
                || npc.FullName == "Undead Caster"
                || npc.FullName == "Mindflayer Servant"
                || npc.FullName == "Dungeon Mage"
                || npc.FullName == "Demon Spirit"
                || npc.FullName == "Crazed Demon Spirit"
                || npc.FullName == "Shadow Mage"
                || npc.FullName == "Attraidies Illusion"
                || npc.FullName == "Attraidies Manifestation"
                || npc.FullName == "Mindflayer King"
                || npc.FullName == "Dark Shogun Mask"
                || npc.FullName == "Dark Dragon Mask"
                || npc.FullName == "Broken Okiku"
                || npc.FullName == "Okiku"
                || npc.FullName == "Wyvern Mage"
                || npc.FullName == "Ghost of the Forgotten Knight"
                || npc.FullName == "Barrow Wight Nemesis"
                || npc.FullName == "Oolacile Sorcerer"
                || npc.FullName == "Abysmal Oolacile Sorcerer"
                || npc.FullName == "Dark Cloud"
                || npc.FullName == "Barrow Wight"
                ) damage *= 2;
        }

    }
}