using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x020016F3 RID: 5875
	public abstract class QuestNode_Root_ArchonexusVictory_Cycle : QuestNode
	{
		// Token: 0x1700161B RID: 5659
		// (get) Token: 0x060087A2 RID: 34722
		protected abstract int ArchonexusCycle { get; }

		// Token: 0x060087A3 RID: 34723 RVA: 0x003080F8 File Offset: 0x003062F8
		protected override void RunInt()
		{
			Quest quest = QuestGen.quest;
			Slate slate = QuestGen.slate;
			this.map = QuestGen_Get.GetMap(false, null);
			string text = QuestGen.GenerateNewSignal("PlayerWealthSatisfied", true);
			string text2 = QuestGen.GenerateNewSignal("SendLetterReminder", true);
			QuestGen.GenerateNewSignal("ActivateLetterReminderSignal", true);
			QuestPart_RequirementsToAcceptPlayerWealth questPart_RequirementsToAcceptPlayerWealth = new QuestPart_RequirementsToAcceptPlayerWealth();
			questPart_RequirementsToAcceptPlayerWealth.requiredPlayerWealth = 600000f;
			quest.AddPart(questPart_RequirementsToAcceptPlayerWealth);
			QuestPart_PlayerWealth questPart_PlayerWealth = new QuestPart_PlayerWealth();
			questPart_PlayerWealth.inSignalEnable = quest.AddedSignal;
			questPart_PlayerWealth.playerWealth = 600000f;
			questPart_PlayerWealth.outSignalsCompleted.Add(text);
			questPart_PlayerWealth.signalListenMode = QuestPart.SignalListenMode.NotYetAcceptedOnly;
			quest.AddPart(questPart_PlayerWealth);
			QuestPart_PassOutInterval questPart_PassOutInterval = new QuestPart_PassOutInterval();
			questPart_PassOutInterval.signalListenMode = QuestPart.SignalListenMode.NotYetAcceptedOnly;
			questPart_PassOutInterval.inSignalEnable = text;
			questPart_PassOutInterval.ticksInterval = new IntRange(3600000, 3600000);
			questPart_PassOutInterval.outSignals.Add(text2);
			quest.AddPart(questPart_PassOutInterval);
			QuestPart_Filter_PlayerWealth questPart_Filter_PlayerWealth = new QuestPart_Filter_PlayerWealth();
			questPart_Filter_PlayerWealth.minPlayerWealth = 600000f;
			questPart_Filter_PlayerWealth.inSignal = text2;
			questPart_Filter_PlayerWealth.outSignal = QuestGen.GenerateNewSignal("OuterNodeCompleted", true);
			questPart_Filter_PlayerWealth.signalListenMode = QuestPart.SignalListenMode.NotYetAcceptedOnly;
			quest.AddPart(questPart_Filter_PlayerWealth);
			quest.CanAcceptQuest(delegate
			{
				QuestNode_ResolveQuestName.Resolve();
				string value = slate.Get<string>("resolvedQuestName", null, false);
				Quest quest = quest;
				LetterDef positiveEvent = LetterDefOf.PositiveEvent;
				string inSignal = null;
				string chosenPawnSignal = null;
				Faction relatedFaction = null;
				MapParent useColonistsOnMap = null;
				bool useColonistsFromCaravanArg = false;
				QuestPart.SignalListenMode signalListenMode = QuestPart.SignalListenMode.NotYetAcceptedOnly;
				IEnumerable<object> lookTargets = null;
				bool filterDeadPawnsFromLookTargets = false;
				string label = "LetterLabelArchonexusWealthReached".Translate(value);
				quest.Letter(positiveEvent, inSignal, chosenPawnSignal, relatedFaction, useColonistsOnMap, useColonistsFromCaravanArg, signalListenMode, lookTargets, filterDeadPawnsFromLookTargets, "LetterTextArchonexusWealthReached".Translate(value), null, label, null, null);
			}, null, questPart_Filter_PlayerWealth.outSignal, null, null, QuestPart.SignalListenMode.NotYetAcceptedOnly);
			Reward_ArchonexusMap reward_ArchonexusMap = new Reward_ArchonexusMap();
			reward_ArchonexusMap.currentPart = this.ArchonexusCycle;
			QuestPart_Choice questPart_Choice = quest.RewardChoice(null, null);
			QuestPart_Choice.Choice choice = new QuestPart_Choice.Choice();
			choice.rewards.Add(reward_ArchonexusMap);
			questPart_Choice.choices.Add(choice);
			List<MapParent> list = new List<MapParent>();
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				Map map = maps[i];
				if (map.IsPlayerHome)
				{
					list.Add(map.Parent);
				}
			}
			quest.End(QuestEndOutcome.Fail, 0, null, QuestGenUtility.HardcodedSignalWithQuestID("mapParent.MapRemoved"), QuestPart.SignalListenMode.NotYetAcceptedOnly, false);
			slate.Set<List<MapParent>>("playerSettlements", list, false);
			slate.Set<int>("playerSettlementsCount", list.Count, false);
			slate.Set<int>("colonistsAllowed", 5, false);
			slate.Set<int>("animalsAllowed", 5, false);
			slate.Set<float>("requiredWealth", 600000f, false);
			slate.Set<Map>("map", this.map, false);
			slate.Set<MapParent>("mapParent", this.map.Parent, false);
		}

		// Token: 0x060087A4 RID: 34724 RVA: 0x003083A8 File Offset: 0x003065A8
		protected void PickNewColony(Faction takeOverFaction, WorldObjectDef worldObjectDef, SoundDef colonyStartSounDef, int maxRelics = 1)
		{
			Quest quest = QuestGen.quest;
			Slate slate = QuestGen.slate;
			string text = QuestGen.GenerateNewSignal("NewColonyCreated", true);
			string text2 = QuestGen.GenerateNewSignal("NewColonyCancelled", true);
			quest.AddPart(new QuestPart_NewColony
			{
				inSignal = slate.Get<string>("inSignal", null, false),
				otherFaction = takeOverFaction,
				outSignalCompleted = text,
				outSignalCancelled = text2,
				worldObjectDef = worldObjectDef,
				colonyStartSound = colonyStartSounDef,
				maxRelics = maxRelics
			});
			quest.SetQuestNotYetAccepted(text2, true);
			quest.End(QuestEndOutcome.Success, 0, null, text, QuestPart.SignalListenMode.OngoingOnly, false);
		}

		// Token: 0x060087A5 RID: 34725 RVA: 0x00308438 File Offset: 0x00306638
		protected void TryAddstudyRequirement(Quest quest, int researchCost, Slate slate)
		{
			Thing thing = this.map.listerThings.ThingsOfDef(ThingDefOf.ArchonexusSuperstructureMajorStudiable).FirstOrDefault<Thing>();
			if (thing != null)
			{
				thing.TryGetComp<CompStudiable>().cost = researchCost;
				slate.Set<Thing>("archonexusMajorStructure", thing, false);
				slate.Set<bool>("studyRequirement", true, false);
				LetterDef positiveEvent = LetterDefOf.PositiveEvent;
				string inSignal = QuestGenUtility.HardcodedSignalWithQuestID("archonexusMajorStructure.Researched");
				string chosenPawnSignal = null;
				Faction relatedFaction = null;
				MapParent useColonistsOnMap = null;
				bool useColonistsFromCaravanArg = false;
				QuestPart.SignalListenMode signalListenMode = QuestPart.SignalListenMode.NotYetAcceptedOnly;
				IEnumerable<object> lookTargets = null;
				bool filterDeadPawnsFromLookTargets = false;
				string label = "LetterLabelArchonexusStructureResearched".Translate(thing);
				quest.Letter(positiveEvent, inSignal, chosenPawnSignal, relatedFaction, useColonistsOnMap, useColonistsFromCaravanArg, signalListenMode, lookTargets, filterDeadPawnsFromLookTargets, "LetterTextArchonexusStructureResearched".Translate(thing, thing.Map), null, label, null, null);
				quest.AddPart(new QuestPart_RequirementsToAcceptThingStudied
				{
					thing = thing
				});
				return;
			}
			slate.Set<bool>("studyRequirement", false, false);
		}

		// Token: 0x060087A6 RID: 34726 RVA: 0x00308508 File Offset: 0x00306708
		protected override bool TestRunInt(Slate slate)
		{
			return QuestGen_Get.GetMap(false, null) != null;
		}

		// Token: 0x040055B4 RID: 21940
		public const int ColonistsAllowed = 5;

		// Token: 0x040055B5 RID: 21941
		public const int AnimalsAllowed = 5;

		// Token: 0x040055B6 RID: 21942
		public const float RequiredWealth = 600000f;

		// Token: 0x040055B7 RID: 21943
		public const int LetterReminderInterval = 3600000;

		// Token: 0x040055B8 RID: 21944
		protected Map map;
	}
}
