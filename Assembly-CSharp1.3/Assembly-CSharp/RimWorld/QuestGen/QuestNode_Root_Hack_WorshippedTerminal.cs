using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x0200170A RID: 5898
	public class QuestNode_Root_Hack_WorshippedTerminal : QuestNode
	{
		// Token: 0x06008838 RID: 34872 RVA: 0x0030F2C4 File Offset: 0x0030D4C4
		protected override void RunInt()
		{
			if (!ModLister.CheckIdeology("Worshipped terminal"))
			{
				return;
			}
			Quest quest = QuestGen.quest;
			Slate slate = QuestGen.slate;
			Map map = QuestGen_Get.GetMap(false, null);
			QuestGenUtility.RunAdjustPointsForDistantFight();
			float num = slate.Get<float>("points", 0f, false);
			Precept_Relic precept_Relic = slate.Get<Precept_Relic>("relic", null, false);
			slate.Set<Faction>("playerFaction", Faction.OfPlayer, false);
			slate.Set<bool>("allowViolentQuests", Find.Storyteller.difficulty.allowViolentQuests, false);
			int tile;
			this.TryFindSiteTile(out tile);
			List<FactionRelation> list = new List<FactionRelation>();
			foreach (Faction faction in Find.FactionManager.AllFactionsListForReading)
			{
				if (!faction.def.permanentEnemy)
				{
					list.Add(new FactionRelation
					{
						other = faction,
						kind = FactionRelationKind.Neutral
					});
				}
			}
			FactionGeneratorParms factionGeneratorParms = new FactionGeneratorParms(FactionDefOf.TribeCivil, default(IdeoGenerationParms), new bool?(true));
			factionGeneratorParms.ideoGenerationParms = new IdeoGenerationParms(factionGeneratorParms.factionDef, false, null, null);
			Faction tribalFaction = FactionGenerator.NewGeneratedFactionWithRelations(factionGeneratorParms, list);
			tribalFaction.temporary = true;
			tribalFaction.factionHostileOnHarmByPlayer = Find.Storyteller.difficulty.allowViolentQuests;
			tribalFaction.neverFlee = true;
			Find.FactionManager.Add(tribalFaction);
			quest.ReserveFaction(tribalFaction);
			if (precept_Relic == null)
			{
				precept_Relic = Faction.OfPlayer.ideos.PrimaryIdeo.GetAllPreceptsOfType<Precept_Relic>().RandomElement<Precept_Relic>();
				Log.Warning("Worshipped terminal quest requires relic from parent quest. None found so picking random player relic");
			}
			QuestGen.GenerateNewSignal("RaidArrives", true);
			string inSignal = QuestGenUtility.HardcodedSignalWithQuestID("playerFaction.BuiltBuilding");
			string inSignal2 = QuestGenUtility.HardcodedSignalWithQuestID("playerFaction.PlacedBlueprint");
			num = Mathf.Max(num, tribalFaction.def.MinPointsToGeneratePawnGroup(PawnGroupKindDefOf.Settlement_RangedOnly, null));
			SitePartParams parms = new SitePartParams
			{
				points = num,
				threatPoints = num,
				relic = precept_Relic
			};
			Site site = QuestGen_Sites.GenerateSite(Gen.YieldSingle<SitePartDefWithParams>(new SitePartDefWithParams(SitePartDefOf.WorshippedTerminal, parms)), tile, tribalFaction, false, null);
			site.doorsAlwaysOpenForPlayerPawns = true;
			slate.Set<Site>("site", site, false);
			quest.SpawnWorldObject(site, null, null);
			int num2 = 25000;
			site.GetComponent<TimedMakeFactionHostile>().SetupTimer(num2, "WorshippedTerminalFactionBecameHostileTimed".Translate(tribalFaction.Named("FACTION")), null);
			SitePart sitePart = site.parts[0];
			Thing thing = site.parts[0].things.First((Thing t) => t.def == ThingDefOf.AncientTerminal);
			slate.Set<Thing>("terminal", thing, false);
			string inSignal3 = QuestGenUtility.HardcodedSignalWithQuestID("terminal.Destroyed");
			string text = QuestGenUtility.HardcodedSignalWithQuestID("terminal.Hacked");
			string text2 = QuestGenUtility.HardcodedSignalWithQuestID("terminal.HackingStarted");
			string text3 = QuestGenUtility.HardcodedSignalWithQuestID("site.MapRemoved");
			string inSignalEnable = QuestGenUtility.HardcodedSignalWithQuestID("site.MapGenerated");
			CompHackable compHackable = thing.TryGetComp<CompHackable>();
			compHackable.hackingStartedSignal = text2;
			compHackable.defence = (float)QuestNode_Root_Hack_WorshippedTerminal.HackDefenceRange.RandomInRange;
			quest.Message("[terminalHackedMessage]", null, true, null, null, text);
			quest.SetFactionHidden(tribalFaction, false, null);
			if (Find.Storyteller.difficulty.allowViolentQuests)
			{
				quest.FactionRelationToPlayerChange(tribalFaction, FactionRelationKind.Hostile, false, text2);
				quest.StartRecurringRaids(site, new FloatRange?(new FloatRange(2.5f, 2.5f)), new int?(2500), text2);
				quest.BuiltNearSettlement(tribalFaction, site, 6f, delegate
				{
					quest.FactionRelationToPlayerChange(tribalFaction, FactionRelationKind.Hostile, true, null);
				}, null, inSignal, null, null, QuestPart.SignalListenMode.OngoingOnly);
				quest.BuiltNearSettlement(tribalFaction, site, 6f, delegate
				{
					quest.Message("WarningBuildingCausesHostility".Translate(tribalFaction.Named("FACTION")), MessageTypeDefOf.CautionInput, false, null, null, null);
				}, null, inSignal2, null, null, QuestPart.SignalListenMode.OngoingOnly);
			}
			Reward_RelicInfo reward_RelicInfo = new Reward_RelicInfo();
			reward_RelicInfo.relic = precept_Relic;
			reward_RelicInfo.quest = quest;
			QuestPart_Choice questPart_Choice = quest.RewardChoice(null, null);
			QuestPart_Choice.Choice choice = new QuestPart_Choice.Choice();
			choice.rewards.Add(reward_RelicInfo);
			questPart_Choice.choices.Add(choice);
			quest.End(QuestEndOutcome.Fail, 0, null, inSignal3, QuestPart.SignalListenMode.OngoingOnly, true);
			quest.SignalPassActivable(delegate
			{
				quest.End(QuestEndOutcome.Fail, 0, null, null, QuestPart.SignalListenMode.OngoingOnly, true);
			}, inSignalEnable, text3, null, null, text, false);
			quest.SignalPassAll(delegate
			{
				quest.End(QuestEndOutcome.Success, 0, null, null, QuestPart.SignalListenMode.OngoingOnly, true);
			}, new List<string>
			{
				text,
				text3
			}, null);
			slate.Set<Map>("map", map, false);
			slate.Set<Precept_Relic>("relic", precept_Relic, false);
			slate.Set<int>("timer", num2, false);
		}

		// Token: 0x06008839 RID: 34873 RVA: 0x00309A7B File Offset: 0x00307C7B
		private bool TryFindSiteTile(out int tile)
		{
			return TileFinder.TryFindNewSiteTile(out tile, 2, 10, false, TileFinderMode.Near, -1, false);
		}

		// Token: 0x0600883A RID: 34874 RVA: 0x0030F7CC File Offset: 0x0030D9CC
		protected override bool TestRunInt(Slate slate)
		{
			int num;
			return this.TryFindSiteTile(out num);
		}

		// Token: 0x04005624 RID: 22052
		private const int MinDistanceFromColony = 2;

		// Token: 0x04005625 RID: 22053
		private const int MaxDistanceFromColony = 10;

		// Token: 0x04005626 RID: 22054
		private static IntRange HackDefenceRange = new IntRange(10, 100);

		// Token: 0x04005627 RID: 22055
		private const int FactionBecomesHostileAfterHours = 10;

		// Token: 0x04005628 RID: 22056
		private const float PointsMultiplierRaid = 0.2f;

		// Token: 0x04005629 RID: 22057
		private const float MinPointsRaid = 45f;
	}
}
