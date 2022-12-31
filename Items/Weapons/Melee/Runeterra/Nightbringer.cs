using tsorcRevamp.Projectiles.Swords.Runeterra;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace tsorcRevamp.Items.Weapons.Melee.Runeterra
{
    public class Nightbringer: ModItem
    {
        public float cooldown = 0;
        public float dashCD = 0f;
        public float dashTimer = 0f;
        public float wallCD = 0f;
        public float attackspeedscaling;
        public float invincibility = 0f;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Nightbringer");
            Tooltip.SetDefault("Doubled crit chance" +
                "\nStabs on right click dealing 125% damage, with a 4 second cooldown, scaling down with attack speed" +
                "\nGain a stack of Steel Tempest upon stabbing any enemy" +
                "\nUpon reaching 2 stacks, the next right click will release a devastating tornado dealing 175% damage" +
                "\nHover your mouse over an enemy and press Q hotkey on a cd to dash through the enemy" +
                "\nPress Q hotkey to create a stationary windwall which blocks all enemy projectiles for 5 seconds on a long cooldown" +
                "\nStabbing an enemy refunds some of these cooldown, the tornado refunds more");
        }
        public override void SetDefaults()
        {
            Item.rare = ItemRarityID.Cyan;
            Item.value = Item.buyPrice(1, 0, 0, 0);
            Item.damage = 120;
            Item.crit = 4;
            Item.width = 52;
            Item.height = 54;
            Item.knockBack = 1f;
            Item.autoReuse = true;
            Item.maxStack = 1;
            Item.scale = 2f;
            Item.DamageType = DamageClass.Melee;
            Item.useAnimation = 14;
            Item.useTime = 14;
            Item.noUseGraphic = false;
            Item.UseSound = SoundID.Item1;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.shootSpeed = 4.2f;
            Item.useTurn = false;
        }

        public override void ModifyWeaponCrit(Player player, ref float crit)
        {
            crit = player.GetTotalCritChance(DamageClass.Melee) * 2;
        }
        public override void HoldItem(Player player)
        {
            player.GetModPlayer<tsorcRevampPlayer>().DoubleCritChance = true;
            if (player.GetTotalAttackSpeed(DamageClass.Melee) >= 4)
            {
                attackspeedscaling = 1;
            }
            else
            {
                attackspeedscaling = 4 / player.GetTotalAttackSpeed(DamageClass.Melee);
            }
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC other = Main.npc[i];

                if (other.active & !other.friendly & other.Distance(Main.MouseWorld) <= 15 & other.Distance(player.Center) <= 10000 & (player.GetModPlayer<tsorcRevampPlayer>().DoubleCritChance) & dashCD <= 0)
                {
                    if (dashTimer > 0)
                    {
                        player.velocity = UsefulFunctions.GenerateTargetingVector(player.Center, other.Center, 15f);
                        invincibility = 1f;
                        dashCD = 30f;
                    }
                    break;
                }
            }
            if (dashTimer > 0)
            {
                player.immune = true;
            }
        }

        public override void UseStyle(Player player, Rectangle heldItemFrame)
        {
            if (Main.mouseRight & !Main.mouseLeft & player.GetModPlayer<tsorcRevampPlayer>().steeltempest >= 2 & cooldown <= 0)
            {
                player.altFunctionUse = 2;
                Item.useStyle = ItemUseStyleID.Swing;
                Item.shoot = ModContent.ProjectileType<NightbringerTornado>();
                cooldown = attackspeedscaling;
                player.GetModPlayer<tsorcRevampPlayer>().steeltempest = 0;
            } else
            if (Main.mouseRight & !Main.mouseLeft)
            {
                player.altFunctionUse = 2;
                Item.useStyle = ItemUseStyleID.Rapier;
                Item.noUseGraphic = true;
                Item.noMelee = true;
                cooldown = attackspeedscaling;
                Item.shoot = ModContent.ProjectileType<NightbringerThrust>();
            }
            if (Main.mouseLeft)
            {
                player.altFunctionUse = 1;
                Item.useStyle = ItemUseStyleID.Swing;
                Item.noUseGraphic = false;
                Item.noMelee = false;
                Item.shoot = ModContent.ProjectileType<Projectiles.Nothing>();
            }

        }
        public override void UpdateInventory(Player player)
        {
            if (invincibility > 0f)
            {
                player.immune = true;
            }
            if (dashCD <= 0)
            {
                player.GetModPlayer<tsorcRevampPlayer>().CanDash = true;
            }
            if (wallCD <= 0)
            {
                player.GetModPlayer<tsorcRevampPlayer>().CanWindwall = true;
            }
            if (Main.GameUpdateCount % 1 == 0)
            {
                cooldown -= 0.0167f;
                dashCD -= 0.0167f;
                dashTimer -= 0.0167f;
                wallCD -= 0.0167f;
                invincibility -= 0.0167f;
            }
        }

        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse != 2 || cooldown <= 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /*public override bool CanShoot(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                return true;
            } return false;
        }*/

        public override bool AltFunctionUse(Player player)
        {
                return true;
        }
        /*
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();

            recipe.AddIngredient(ModContent.ItemType<PlasmaWhirlwind>());
            recipe.AddIngredient(ItemID.LunarBar, 12);
            recipe.AddIngredient(ModContent.ItemType<DarkSoul>(), 70000);

            recipe.AddTile(TileID.DemonAltar);

            recipe.Register();
        }*/
    }
}