using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001706 RID: 5894
	public abstract class QuestNode_Root_WandererJoin : QuestNode
	{
		// Token: 0x17001627 RID: 5671
		// (get) Token: 0x0600881B RID: 34843 RVA: 0x0030E173 File Offset: 0x0030C373
		protected virtual int AllowKilledBeforeTicks
		{
			get
			{
				return 15000;
			}
		}

		// Token: 0x0600881C RID: 34844
		public abstract Pawn GeneratePawn();

		// Token: 0x0600881D RID: 34845
		public abstract void SendLetter(Quest quest, Pawn pawn);

		// Token: 0x0600881E RID: 34846 RVA: 0x0030E17C File Offset: 0x0030C37C
		protected virtual void AddSpawnPawnQuestParts(Quest quest, Map map, Pawn pawn)
		{
			quest.DropPods(map.Parent, Gen.YieldSingle<Pawn>(pawn), null, null, null, null, new bool?(false), false, false, false, null, null, QuestPart.SignalListenMode.OngoingOnly, null, true);
		}

		// Token: 0x0600881F RID: 34847 RVA: 0x0030E1B8 File Offset: 0x0030C3B8
		protected virtual void AddLeftMapQuestParts(Quest quest, Map map, Pawn pawn)
		{
			Action <>9__2;
			quest.AnyPawnUnhealthy(Gen.YieldSingle<Pawn>(pawn), delegate
			{
				quest.End(QuestEndOutcome.Unknown, 0, null, null, QuestPart.SignalListenMode.OngoingOnly, false);
			}, delegate
			{
				Quest quest2 = quest;
				Action action;
				if ((action = <>9__2) == null)
				{
					action = (<>9__2 = delegate()
					{
						quest.Message("MessageCharityEventFulfilled".Translate() + ": " + "MessageWandererLeftHealthy".Translate(pawn), MessageTypeDefOf.PositiveEvent, false, null, pawn, null);
					});
				}
				quest2.AnyColonistWithCharityPrecept(action, null, null, null, null, QuestPart.SignalListenMode.OngoingOnly);
				if (ModsConfig.IdeologyActive)
				{
					quest.RecordHistoryEvent(HistoryEventDefOf.CharityFulfilled_WandererJoins, null);
				}
				quest.End(QuestEndOutcome.Unknown, 0, null, null, QuestPart.SignalListenMode.OngoingOnly, false);
			}, null, null, null, null, QuestPart.SignalListenMode.OngoingOnly);
		}

		// Token: 0x06008820 RID: 34848 RVA: 0x0030E210 File Offset: 0x0030C410
		protected override void RunInt()
		{
			Quest quest = QuestGen.quest;
			Slate slate = QuestGen.slate;
			Map map;
			if (!slate.TryGet<Map>("map", out map, false))
			{
				map = QuestGen_Get.GetMap(false, null);
			}
			Pawn pawn = this.GeneratePawn();
			this.AddSpawnPawnQuestParts(quest, map, pawn);
			slate.Set<Pawn>("pawn", pawn, false);
			this.SendLetter(quest, pawn);
			string inSignal = QuestGenUtility.HardcodedSignalWithQuestID("pawn.Killed");
			string inSignal2 = QuestGenUtility.HardcodedSignalWithQuestID("pawn.PlayerTended");
			string inSignal3 = QuestGenUtility.HardcodedSignalWithQuestID("pawn.LeftMap");
			string inSignal4 = QuestGenUtility.HardcodedSignalWithQuestID("pawn.Recruited");
			quest.End(QuestEndOutcome.Unknown, 0, null, inSignal2, QuestPart.SignalListenMode.OngoingOnly, false);
			Action <>9__5;
			Action <>9__3;
			Action <>9__4;
			quest.Signal(inSignal, delegate
			{
				Quest quest = quest;
				int allowKilledBeforeTicks = this.AllowKilledBeforeTicks;
				Action action;
				if ((action = <>9__3) == null)
				{
					action = (<>9__3 = delegate()
					{
						Quest quest2 = quest;
						Action action2;
						if ((action2 = <>9__5) == null)
						{
							action2 = (<>9__5 = delegate()
							{
								quest.Message("MessageCharityEventRefused".Translate() + ": " + "MessageWandererLeftToDie".Translate(pawn), MessageTypeDefOf.NegativeEvent, false, null, pawn, null);
							});
						}
						quest2.AnyColonistWithCharityPrecept(action2, null, null, null, null, QuestPart.SignalListenMode.OngoingOnly);
						quest.RecordHistoryEvent(HistoryEventDefOf.CharityRefused_WandererJoins, null);
						quest.End(QuestEndOutcome.Unknown, 0, null, null, QuestPart.SignalListenMode.OngoingOnly, false);
					});
				}
				Action elseAction;
				if ((elseAction = <>9__4) == null)
				{
					elseAction = (<>9__4 = delegate()
					{
						quest.End(QuestEndOutcome.Unknown, 0, null, null, QuestPart.SignalListenMode.OngoingOnly, false);
					});
				}
				quest.AcceptedAfterTicks(allowKilledBeforeTicks, action, elseAction, null, null, null, null, QuestPart.SignalListenMode.OngoingOnly);
			}, null, QuestPart.SignalListenMode.OngoingOnly);
			quest.AnyColonistWithCharityPrecept(delegate
			{
				quest.Message("MessageCharityEventFulfilled".Translate() + ": " + "MessageWandererRecruited".Translate(pawn), MessageTypeDefOf.PositiveEvent, false, null, pawn, null);
			}, null, inSignal4, null, null, QuestPart.SignalListenMode.OngoingOnly);
			quest.End(QuestEndOutcome.Unknown, 0, null, inSignal4, QuestPart.SignalListenMode.OngoingOnly, false);
			quest.Signal(inSignal3, delegate
			{
				this.AddLeftMapQuestParts(quest, map, pawn);
			}, null, QuestPart.SignalListenMode.OngoingOnly);
		}

		// Token: 0x06008821 RID: 34849 RVA: 0x000126F5 File Offset: 0x000108F5
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}
	}
}
