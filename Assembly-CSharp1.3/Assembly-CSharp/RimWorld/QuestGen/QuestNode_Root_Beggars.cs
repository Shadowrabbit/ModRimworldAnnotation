using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x020016F7 RID: 5879
	public class QuestNode_Root_Beggars : QuestNode
	{
		// Token: 0x1700161F RID: 5663
		// (get) Token: 0x060087B4 RID: 34740 RVA: 0x0030876D File Offset: 0x0030696D
		private static IEnumerable<ThingDef> AllowedThings
		{
			get
			{
				yield return ThingDefOf.Silver;
				yield return ThingDefOf.MedicineHerbal;
				yield return ThingDefOf.MedicineIndustrial;
				yield return ThingDefOf.Penoxycyline;
				yield return ThingDefOf.Beer;
				yield break;
			}
		}

		// Token: 0x060087B5 RID: 34741 RVA: 0x00308778 File Offset: 0x00306978
		protected override void RunInt()
		{
			if (!ModLister.CheckIdeology("Beggars"))
			{
				return;
			}
			Quest quest = QuestGen.quest;
			Slate slate = QuestGen.slate;
			Map map = QuestGen_Get.GetMap(false, null);
			float num = slate.Get<float>("points", 0f, false);
			slate.Set<int>("visitDurationTicks", 60000, false);
			ThingDef thingDef;
			int num2;
			if (QuestNode_Root_Beggars.TryFindRandomRequestedThing(map, num * 0.85f, out thingDef, out num2, QuestNode_Root_Beggars.AllowedThings))
			{
				slate.Set<ThingDef>("requestedThing", thingDef, false);
				slate.Set<string>("requestedThingDefName", thingDef.defName, false);
				slate.Set<int>("requestedThingCount", num2, false);
			}
			int num3 = slate.Exists("population", false) ? slate.Get<int>("population", 0, false) : map.mapPawns.FreeColonistsSpawnedCount;
			int num4 = Mathf.Max(Mathf.RoundToInt(QuestNode_Root_Beggars.LodgerCountBasedOnColonyPopulationFactorRange.RandomInRange * (float)num3), 1);
			List<FactionRelation> list = new List<FactionRelation>();
			foreach (Faction faction2 in Find.FactionManager.AllFactionsListForReading)
			{
				if (!faction2.def.permanentEnemy)
				{
					list.Add(new FactionRelation
					{
						other = faction2,
						kind = FactionRelationKind.Neutral
					});
				}
			}
			Faction faction = FactionGenerator.NewGeneratedFactionWithRelations(FactionDefOf.Beggars, list, true);
			faction.temporary = true;
			Find.FactionManager.Add(faction);
			slate.Set<Faction>("faction", faction, false);
			List<Pawn> pawns = new List<Pawn>();
			for (int i = 0; i < num4; i++)
			{
				pawns.Add(quest.GeneratePawn(PawnKindDefOf.Beggar, faction, true, null, 0f, true, null, 0f, 0f, false, true));
			}
			slate.Set<List<Pawn>>("beggars", pawns, false);
			slate.Set<int>("beggarCount", num4, false);
			quest.SetFactionHidden(faction, false, null);
			quest.PawnsArrive(pawns, null, map.Parent, null, false, null, null, null, null, null, false, false, false);
			string itemsReceivedSignal = QuestGen.GenerateNewSignal("ItemsReceived", true);
			QuestPart_BegForItems questPart_BegForItems = new QuestPart_BegForItems();
			questPart_BegForItems.inSignal = QuestGen.slate.Get<string>("inSignal", null, false);
			questPart_BegForItems.outSignalItemsReceived = itemsReceivedSignal;
			questPart_BegForItems.pawns.AddRange(pawns);
			questPart_BegForItems.target = pawns[0];
			questPart_BegForItems.faction = faction;
			questPart_BegForItems.mapParent = map.Parent;
			questPart_BegForItems.thingDef = thingDef;
			questPart_BegForItems.amount = num2;
			quest.AddPart(questPart_BegForItems);
			string pawnLabelSingleOrPlural = (num4 > 1) ? faction.def.pawnsPlural : faction.def.pawnSingular;
			Action <>9__5;
			quest.Delay(60000, delegate
			{
				Quest quest;
				quest.Leave(pawns, null, false, false, null, false);
				quest.RecordHistoryEvent(HistoryEventDefOf.CharityRefused_Beggars, null);
				quest = quest;
				Action action;
				if ((action = <>9__5) == null)
				{
					action = (<>9__5 = delegate()
					{
						quest.Message(string.Format("{0}: {1}", "MessageCharityEventRefused".Translate(), "MessageBeggarsLeavingWithNoItems".Translate(pawnLabelSingleOrPlural)), MessageTypeDefOf.NegativeEvent, false, null, pawns, null);
					});
				}
				quest.AnyColonistWithCharityPrecept(action, null, null, null, null, QuestPart.SignalListenMode.OngoingOnly);
			}, null, null, null, false, null, null, false, null, null, null, false, QuestPart.SignalListenMode.OngoingOnly);
			string text = QuestGenUtility.HardcodedSignalWithQuestID("beggars.Arrested");
			string text2 = QuestGenUtility.HardcodedSignalWithQuestID("beggars.Killed");
			string inSignal = QuestGenUtility.HardcodedSignalWithQuestID("beggars.LeftMap");
			quest.AnyColonistWithCharityPrecept(delegate
			{
				quest.Message("MessageCharityEventFulfilled".Translate() + ": " + "MessageBeggarsLeavingWithItems".Translate(pawnLabelSingleOrPlural), MessageTypeDefOf.PositiveEvent, false, null, pawns, null);
			}, delegate
			{
				quest.Message("MessageBeggarsLeavingWithItems".Translate(pawnLabelSingleOrPlural), MessageTypeDefOf.PositiveEvent, false, null, pawns, null);
			}, itemsReceivedSignal, null, null, QuestPart.SignalListenMode.OngoingOnly);
			if (ModsConfig.IdeologyActive)
			{
				quest.RecordHistoryEvent(HistoryEventDefOf.CharityFulfilled_Beggars, itemsReceivedSignal);
			}
			Action <>9__7;
			Action <>9__6;
			quest.AnySignal(new string[]
			{
				text2,
				text
			}, delegate
			{
				Quest quest = quest;
				Action action;
				if ((action = <>9__6) == null)
				{
					action = (<>9__6 = delegate()
					{
						Quest quest2 = quest;
						Action action2;
						if ((action2 = <>9__7) == null)
						{
							action2 = (<>9__7 = delegate()
							{
								quest.Message(string.Format("{0}: {1}", "MessageCharityEventRefused".Translate(), "MessageBeggarsLeavingWithNoItems".Translate(pawnLabelSingleOrPlural)), MessageTypeDefOf.NegativeEvent, false, null, pawns, null);
							});
						}
						quest2.AnyColonistWithCharityPrecept(action2, null, null, null, null, QuestPart.SignalListenMode.OngoingOnly);
					});
				}
				quest.SignalPassActivable(action, null, null, null, null, itemsReceivedSignal, false);
				quest.Letter(LetterDefOf.NegativeEvent, null, null, null, null, false, QuestPart.SignalListenMode.OngoingOnly, pawns, false, "[letterTextBeggarsBetrayed]", null, "[letterLabelBeggarsBetrayed]", null, null);
				QuestPart_FactionRelationChange questPart_FactionRelationChange = new QuestPart_FactionRelationChange();
				questPart_FactionRelationChange.faction = faction;
				questPart_FactionRelationChange.relationKind = FactionRelationKind.Hostile;
				questPart_FactionRelationChange.canSendHostilityLetter = false;
				questPart_FactionRelationChange.inSignal = QuestGen.slate.Get<string>("inSignal", null, false);
				quest.AddPart(questPart_FactionRelationChange);
				quest.RecordHistoryEvent(HistoryEventDefOf.CharityRefused_Beggars_Betrayed, null);
			}, null, QuestPart.SignalListenMode.OngoingOnly);
			quest.End(QuestEndOutcome.Fail, 0, null, QuestGenUtility.HardcodedSignalWithQuestID("faction.BecameHostileToPlayer"), QuestPart.SignalListenMode.OngoingOnly, false);
			quest.AllPawnsDespawned(pawns, delegate
			{
				quest.End(QuestEndOutcome.Success, 0, null, null, QuestPart.SignalListenMode.OngoingOnly, false);
			}, null, inSignal, null, null, null, QuestPart.SignalListenMode.OngoingOnly);
		}

		// Token: 0x060087B6 RID: 34742 RVA: 0x00308B9C File Offset: 0x00306D9C
		private static bool TryFindRandomRequestedThing(Map map, float value, out ThingDef thingDef, out int count, IEnumerable<ThingDef> allowedThings)
		{
			QuestNode_Root_Beggars.requestCountDict.Clear();
			Func<ThingDef, bool> globalValidator = delegate(ThingDef td)
			{
				if (!td.PlayerAcquirable)
				{
					return false;
				}
				int num = ThingUtility.RoundedResourceStackCount(Mathf.Max(1, Mathf.RoundToInt(value / td.BaseMarketValue)));
				QuestNode_Root_Beggars.requestCountDict.Add(td, num);
				return PlayerItemAccessibilityUtility.Accessible(td, num, map);
			};
			if ((from def in allowedThings
			where globalValidator(def)
			select def).TryRandomElement(out thingDef))
			{
				count = QuestNode_Root_Beggars.requestCountDict[thingDef];
				return true;
			}
			count = 0;
			return false;
		}

		// Token: 0x060087B7 RID: 34743 RVA: 0x00308C08 File Offset: 0x00306E08
		protected override bool TestRunInt(Slate slate)
		{
			Map map = QuestGen_Get.GetMap(false, null);
			float num = slate.Get<float>("points", 0f, false);
			ThingDef thingDef;
			int num2;
			return FactionDefOf.Beggars.allowedArrivalTemperatureRange.Includes(map.mapTemperature.OutdoorTemp) && QuestNode_Root_Beggars.TryFindRandomRequestedThing(map, num * 0.85f, out thingDef, out num2, QuestNode_Root_Beggars.AllowedThings);
		}

		// Token: 0x060087B8 RID: 34744 RVA: 0x00308C6C File Offset: 0x00306E6C
		[DebugOutput]
		public static void BeggarQuestItems()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("Possible items with quantities based on points using the current map:");
			for (int i = 0; i < 10000; i += 100)
			{
				stringBuilder.AppendLine(string.Format("========== points {0} ==========", i));
				foreach (ThingDef thingDef in QuestNode_Root_Beggars.AllowedThings)
				{
					ThingDef thingDef2;
					int num;
					if (QuestNode_Root_Beggars.TryFindRandomRequestedThing(Find.CurrentMap, (float)i * 0.85f, out thingDef2, out num, Gen.YieldSingle<ThingDef>(thingDef)))
					{
						stringBuilder.AppendLine(string.Format("<color=green>[POSSIBLE]</color> {0} x{1}", thingDef2.label, num));
					}
					else
					{
						stringBuilder.AppendLine("<color=red>[NOT POSSIBLE]</color> " + thingDef.label);
					}
				}
			}
			Log.Message(stringBuilder.ToString());
		}

		// Token: 0x040055BF RID: 21951
		private static FloatRange LodgerCountBasedOnColonyPopulationFactorRange = new FloatRange(0.3f, 1f);

		// Token: 0x040055C0 RID: 21952
		private const int VisitDuration = 60000;

		// Token: 0x040055C1 RID: 21953
		private const float BeggarRequestValueFactor = 0.85f;

		// Token: 0x040055C2 RID: 21954
		private static Dictionary<ThingDef, int> requestCountDict = new Dictionary<ThingDef, int>();
	}
}
