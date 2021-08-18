﻿using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Utilities;
using Terraria.Localization;

namespace tsorcRevamp.NPCs.Friendly
{

	class DoctorJones : ModNPC
	{
		public override bool Autoload(ref string name) => false;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Doctor Jones");
			Main.npcFrameCount[npc.type] = 26;
			//NPCID.Sets.ExtraFramesCount[npc.type] = 10;
			//NPCID.Sets.AttackFrameCount[npc.type] = 5;
			//NPCID.Sets.DangerDetectRange[npc.type] = 140;
			//NPCID.Sets.AttackType[npc.type] = 1; // 0 is throwing, 1 is shooting, 2 is magic, 3 is melee
			//NPCID.Sets.AttackTime[npc.type] = 25;
			//NPCID.Sets.AttackAverageChance[npc.type] = 10;
			//NPCID.Sets.HatOffsetY[npc.type] = 4;
		}

		/*public static List<string> Names = new List<string> {
			"Harrison", "Han", "Indie", "Rick"
		};*/

		/*public override string TownNPCName()
		{
			string name = Names[Main.rand.Next(Names.Count)]; //pick a random name from the list
			return name;
		}*/

		public override void SetDefaults()
		{
			//npc.townNPC = true;
			npc.friendly = true;
			npc.width = 18;
			npc.height = 40;
			//npc.aiStyle = ModContent.NPCType<NPCs.Friendly.Archeologist>();
			npc.aiStyle = 7;
			npc.damage = 50;
			npc.defense = 15;
			npc.lifeMax = 1000;
			npc.HitSound = SoundID.NPCHit1;
			npc.DeathSound = SoundID.NPCDeath1;
			npc.knockBackResist = 0.3f;
			animationType = NPCID.Guide;
		}

        public override bool PreAI() {
			npc.Transform(ModContent.NPCType<Archaeologist>());
			return true;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			float chance = 0;

			if (spawnInfo.player.ZoneJungle && !NPC.AnyNPCs(mod.NPCType("DoctorJones")) && !NPC.AnyNPCs(mod.NPCType("Archaeologist")) && Main.rand.Next(10) == 0)
			{
				Main.NewText("The spirit of adventure is nearby...", 255, 255, 0);
				return 1f;
			}
			return chance;
		}

		public override string GetChat()
		{
			WeightedRandom<string> chat = new WeightedRandom<string>();
			chat.Add("Remember, X never marks the spot.");
			chat.Add("The report of my undeath was an exaggeration.");
			chat.Add("I *hate* snakes.");
			chat.Add("Seen any fortune or glory lately?");
			chat.Add("Trust me.");
			chat.Add("Ha! You think THESE boulder traps are bad...");
			chat.Add("I have no idea what I'm doing, but I know I'm doing it really, really well.");
			chat.Add("Having tree troubles? Try fire... Or, you know, an axe.");
			return chat;
		}

		public override void SetChatButtons(ref string button, ref string button2)
		{
			button = Language.GetTextValue("LegacyInterface.28");
		}

		public override void OnChatButtonClicked(bool firstButton, ref bool shop)
		{
			if (firstButton)
			{
				shop = true;
				return;
			}
		}

		public override void SetupShop(Chest shop, ref int nextSlot)
		{
			shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.MysteriousIdol>());
			nextSlot++;
			shop.item[nextSlot].SetDefaults(ItemID.MeteorShot);
			nextSlot++;
			shop.item[nextSlot].SetDefaults(ItemID.WoodenArrow);
			nextSlot++;
			shop.item[nextSlot].SetDefaults(ItemID.Torch);
			nextSlot++;
			shop.item[nextSlot].SetDefaults(ItemID.ManaPotion);
			nextSlot++;
			shop.item[nextSlot].SetDefaults(ItemID.HealingPotion);
			nextSlot++;
			shop.item[nextSlot].SetDefaults(ItemID.PurificationPowder);
			nextSlot++;
			shop.item[nextSlot].SetDefaults(ItemID.NightOwlPotion);
			nextSlot++;
			shop.item[nextSlot].SetDefaults(ItemID.BottledWater);
			nextSlot++;
			shop.item[nextSlot].SetDefaults(ItemID.GlowingMushroom);
			nextSlot++;
			shop.item[nextSlot].SetDefaults(ItemID.FamiliarWig);
			nextSlot++;
			shop.item[nextSlot].SetDefaults(ItemID.PoisonedKnife);
			nextSlot++;
			shop.item[nextSlot].SetDefaults(ItemID.Grenade);
			nextSlot++;
			shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Accessories.CosmicWatch>());
			nextSlot++;
			shop.item[nextSlot].SetDefaults(ItemID.TinkerersWorkshop);
			nextSlot++;
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			if (base.npc.life <= 0) //even though npcs are immortal
			{
				Gore.NewGore(npc.position, new Vector2((float)Main.rand.Next(-30, 31) * 0.2f, (float)Main.rand.Next(-30, 31) * 0.2f), mod.GetGoreSlot("Gores/Archeologist Head Gore"));
				Gore.NewGore(npc.position, new Vector2((float)Main.rand.Next(-30, 31) * 0.2f, (float)Main.rand.Next(-30, 31) * 0.2f), mod.GetGoreSlot("Gores/Archeologist Arm Gore"));
				Gore.NewGore(npc.position, new Vector2((float)Main.rand.Next(-30, 31) * 0.2f, (float)Main.rand.Next(-30, 31) * 0.2f), mod.GetGoreSlot("Gores/Archeologist Arm Gore"));
				Gore.NewGore(npc.position, new Vector2((float)Main.rand.Next(-30, 31) * 0.2f, (float)Main.rand.Next(-30, 31) * 0.2f), mod.GetGoreSlot("Gores/Archeologist Leg Gore"));
				Gore.NewGore(npc.position, new Vector2((float)Main.rand.Next(-30, 31) * 0.2f, (float)Main.rand.Next(-30, 31) * 0.2f), mod.GetGoreSlot("Gores/Archeologist Leg Gore"));
			}
		}
	}
}