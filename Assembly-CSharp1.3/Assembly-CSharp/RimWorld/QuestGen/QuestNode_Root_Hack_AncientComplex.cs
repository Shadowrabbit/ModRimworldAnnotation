using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x020016FA RID: 5882
	public class QuestNode_Root_Hack_AncientComplex : QuestNode_Root_AncientComplex
	{
		// Token: 0x060087C2 RID: 34754 RVA: 0x003096B0 File Offset: 0x003078B0
		protected override void RunInt()
		{
			if (!ModLister.CheckIdeology("Ancient complex rescue"))
			{
				return;
			}
			Slate slate = QuestGen.slate;
			Quest quest = QuestGen.quest;
			Map map = QuestGen_Get.GetMap(false, null);
			float num = slate.Get<float>("points", 0f, false);
			Precept_Relic precept_Relic = slate.Get<Precept_Relic>("relic", null, false);
			int tile;
			this.TryFindSiteTile(out tile);
			Faction faction;
			this.TryFindEnemyFaction(out faction);
			if (precept_Relic == null)
			{
				precept_Relic = Faction.OfPlayer.ideos.PrimaryIdeo.GetAllPreceptsOfType<Precept_Relic>().RandomElement<Precept_Relic>();
				Log.Warning("Ancient Complex quest requires relic from parent quest. None found so picking random player relic");
			}
			string inSignal = QuestGenUtility.HardcodedSignalWithQuestID("terminals.Destroyed");
			string text = QuestGen.GenerateNewSignal("TerminalHacked", true);
			string text2 = QuestGen.GenerateNewSignal("AllTerminalsHacked", true);
			QuestGen.GenerateNewSignal("RaidArrives", true);
			string inSignal2 = QuestGenUtility.HardcodedSignalWithQuestID("site.Destroyed");
			ComplexSketch complexSketch = this.GenerateSketch(num, true);
			complexSketch.thingDiscoveredMessage = "MessageAncientTerminalDiscovered".Translate(precept_Relic.Label);
			List<string> list = new List<string>();
			for (int i = 0; i < complexSketch.thingsToSpawn.Count; i++)
			{
				Thing thing = complexSketch.thingsToSpawn[i];
				string text3 = QuestGenUtility.HardcodedTargetQuestTagWithQuestID("terminal" + i);
				QuestUtility.AddQuestTag(thing, text3);
				string item = QuestGenUtility.HardcodedSignalWithQuestID(text3 + ".Hacked");
				list.Add(item);
				thing.TryGetComp<CompHackable>().defence = (float)(Rand.Chance(0.5f) ? QuestNode_Root_Hack_AncientComplex.HackDefenceRange.min : QuestNode_Root_Hack_AncientComplex.HackDefenceRange.max);
			}
			float num2 = Find.Storyteller.difficulty.allowViolentQuests ? QuestNode_Root_AncientComplex.ThreatPointsOverPointsCurve.Evaluate(num) : 0f;
			SitePartParams parms = new SitePartParams
			{
				ancientComplexSketch = complexSketch,
				ancientComplexRewardMaker = ThingSetMakerDefOf.MapGen_AncientComplexRoomLoot_Default,
				threatPoints = num2
			};
			Site site = QuestGen_Sites.GenerateSite(Gen.YieldSingle<SitePartDefWithParams>(new SitePartDefWithParams(SitePartDefOf.AncientComplex, parms)), tile, Faction.OfAncients, false, null);
			quest.SpawnWorldObject(site, null, null);
			TimedDetectionRaids component = site.GetComponent<TimedDetectionRaids>();
			if (component != null)
			{
				component.alertRaidsArrivingIn = true;
			}
			quest.AddPart(new QuestPart_PassAllActivable
			{
				inSignalEnable = QuestGen.slate.Get<string>("inSignal", null, false),
				inSignals = list,
				outSignalsCompleted = 
				{
					text2
				},
				outSignalAny = text,
				expiryInfoPartKey = "TerminalsHacked"
			});
			quest.Message("[terminalHackedMessage]", null, true, null, null, text);
			quest.Message("[allTerminalsHackedMessage]", MessageTypeDefOf.PositiveEvent, false, null, null, text2);
			if (Find.Storyteller.difficulty.allowViolentQuests && Rand.Chance(0.5f))
			{
				quest.RandomRaid(site, QuestNode_Root_Hack_AncientComplex.RandomRaidPointsFactorRange * num2, faction, text2, PawnsArrivalModeDefOf.EdgeWalkIn, RaidStrategyDefOf.ImmediateAttack, null, null);
			}
			Reward_RelicInfo reward_RelicInfo = new Reward_RelicInfo();
			reward_RelicInfo.relic = precept_Relic;
			reward_RelicInfo.quest = quest;
			QuestPart_Choice questPart_Choice = quest.RewardChoice(null, null);
			QuestPart_Choice.Choice choice = new QuestPart_Choice.Choice();
			choice.rewards.Add(reward_RelicInfo);
			questPart_Choice.choices.Add(choice);
			QuestPart_Filter_Hacked questPart_Filter_Hacked = new QuestPart_Filter_Hacked();
			questPart_Filter_Hacked.inSignal = inSignal;
			questPart_Filter_Hacked.outSignalElse = QuestGen.GenerateNewSignal("FailQuestTerminalDestroyed", true);
			quest.AddPart(questPart_Filter_Hacked);
			quest.End(QuestEndOutcome.Success, 0, null, text2, QuestPart.SignalListenMode.OngoingOnly, true);
			quest.End(QuestEndOutcome.Fail, 0, null, inSignal2, QuestPart.SignalListenMode.OngoingOnly, true);
			quest.End(QuestEndOutcome.Fail, 0, null, questPart_Filter_Hacked.outSignalElse, QuestPart.SignalListenMode.OngoingOnly, true);
			slate.Set<List<Thing>>("terminals", complexSketch.thingsToSpawn, false);
			slate.Set<int>("terminalCount", complexSketch.thingsToSpawn.Count, false);
			slate.Set<Map>("map", map, false);
			slate.Set<Precept_Relic>("relic", precept_Relic, false);
			slate.Set<Site>("site", site, false);
		}

		// Token: 0x060087C3 RID: 34755 RVA: 0x00309A7B File Offset: 0x00307C7B
		private bool TryFindSiteTile(out int tile)
		{
			return TileFinder.TryFindNewSiteTile(out tile, 2, 10, false, TileFinderMode.Near, -1, false);
		}

		// Token: 0x060087C4 RID: 34756 RVA: 0x00309A8A File Offset: 0x00307C8A
		private bool TryFindEnemyFaction(out Faction enemyFaction)
		{
			enemyFaction = Find.FactionManager.RandomEnemyFaction(false, false, true, TechLevel.Undefined);
			return enemyFaction != null;
		}

		// Token: 0x060087C5 RID: 34757 RVA: 0x00309AA4 File Offset: 0x00307CA4
		protected override bool TestRunInt(Slate slate)
		{
			int num;
			Faction faction;
			return this.TryFindSiteTile(out num) && this.TryFindEnemyFaction(out faction);
		}

		// Token: 0x040055C4 RID: 21956
		private const int MinDistanceFromColony = 2;

		// Token: 0x040055C5 RID: 21957
		private const int MaxDistanceFromColony = 10;

		// Token: 0x040055C6 RID: 21958
		private static IntRange HackDefenceRange = new IntRange(300, 1800);

		// Token: 0x040055C7 RID: 21959
		private const float MinMaxHackDefenceChance = 0.5f;

		// Token: 0x040055C8 RID: 21960
		private static readonly FloatRange RandomRaidPointsFactorRange = new FloatRange(0.25f, 0.35f);

		// Token: 0x040055C9 RID: 21961
		private const float ChanceToSpawnAllTerminalsHackedRaid = 0.5f;
	}
}
