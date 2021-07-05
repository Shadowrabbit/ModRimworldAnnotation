using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x0200161B RID: 5659
	public static class QuestGen_Signal
	{
		// Token: 0x060084AB RID: 33963 RVA: 0x002FA688 File Offset: 0x002F8888
		public static void Signal(this Quest quest, string inSignal = null, Action action = null, IEnumerable<string> outSignals = null, QuestPart.SignalListenMode signalListenMode = QuestPart.SignalListenMode.OngoingOnly)
		{
			Slate slate = QuestGen.slate;
			int num = ((outSignals != null) ? outSignals.Count<string>() : 0) + ((action != null) ? 1 : 0);
			if (num == 0)
			{
				return;
			}
			if (num == 1)
			{
				QuestPart_Pass questPart_Pass = new QuestPart_Pass();
				questPart_Pass.inSignal = QuestGenUtility.HardcodedSignalWithQuestID(inSignal);
				if (action != null)
				{
					questPart_Pass.outSignal = QuestGen.GenerateNewSignal("OuterNodeCompleted", true);
					QuestGenUtility.RunInner(action, questPart_Pass.outSignal);
				}
				else
				{
					questPart_Pass.outSignal = QuestGenUtility.HardcodedSignalWithQuestID(outSignals.First<string>());
				}
				questPart_Pass.signalListenMode = signalListenMode;
				quest.AddPart(questPart_Pass);
				return;
			}
			QuestPart_PassOutMany questPart_PassOutMany = new QuestPart_PassOutMany();
			questPart_PassOutMany.inSignal = QuestGenUtility.HardcodedSignalWithQuestID(inSignal);
			if (action != null)
			{
				string text = QuestGen.GenerateNewSignal("OuterNodeCompleted", true);
				questPart_PassOutMany.outSignals.Add(text);
				QuestGenUtility.RunInner(action, text);
			}
			foreach (string signal in outSignals)
			{
				questPart_PassOutMany.outSignals.Add(QuestGenUtility.HardcodedSignalWithQuestID(signal));
			}
			questPart_PassOutMany.signalListenMode = signalListenMode;
			quest.AddPart(questPart_PassOutMany);
		}

		// Token: 0x060084AC RID: 33964 RVA: 0x002FA79C File Offset: 0x002F899C
		public static void AnySignal(this Quest quest, IEnumerable<string> inSignals = null, Action action = null, IEnumerable<string> outSignals = null, QuestPart.SignalListenMode signalListenMode = QuestPart.SignalListenMode.OngoingOnly)
		{
			Slate slate = QuestGen.slate;
			int num = ((outSignals != null) ? outSignals.Count<string>() : 0) + ((action != null) ? 1 : 0);
			if (num == 0)
			{
				return;
			}
			if (num == 1)
			{
				QuestPart_PassAny questPart_PassAny = new QuestPart_PassAny();
				foreach (string signal in inSignals)
				{
					questPart_PassAny.inSignals.Add(QuestGenUtility.HardcodedSignalWithQuestID(signal));
				}
				if (action != null)
				{
					questPart_PassAny.outSignal = QuestGen.GenerateNewSignal("OuterNodeCompleted", true);
					QuestGenUtility.RunInner(action, questPart_PassAny.outSignal);
				}
				else
				{
					questPart_PassAny.outSignal = QuestGenUtility.HardcodedSignalWithQuestID(outSignals.First<string>());
				}
				questPart_PassAny.signalListenMode = signalListenMode;
				quest.AddPart(questPart_PassAny);
				return;
			}
			QuestPart_PassAnyOutMany questPart_PassAnyOutMany = new QuestPart_PassAnyOutMany();
			foreach (string signal2 in inSignals)
			{
				questPart_PassAnyOutMany.inSignals.Add(QuestGenUtility.HardcodedSignalWithQuestID(signal2));
			}
			if (action != null)
			{
				string text = QuestGen.GenerateNewSignal("OuterNodeCompleted", true);
				questPart_PassAnyOutMany.outSignals.Add(text);
				QuestGenUtility.RunInner(action, text);
			}
			foreach (string signal3 in outSignals)
			{
				questPart_PassAnyOutMany.outSignals.Add(QuestGenUtility.HardcodedSignalWithQuestID(signal3));
			}
			questPart_PassAnyOutMany.signalListenMode = signalListenMode;
			quest.AddPart(questPart_PassAnyOutMany);
		}

		// Token: 0x060084AD RID: 33965 RVA: 0x002FA920 File Offset: 0x002F8B20
		public static void SignalRandom(this Quest quest, IEnumerable<Action> actions, string inSignal = null, QuestPart.SignalListenMode signalListenMode = QuestPart.SignalListenMode.OngoingOnly)
		{
			QuestPart_PassOutRandom questPart_PassOutRandom = new QuestPart_PassOutRandom();
			questPart_PassOutRandom.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(inSignal) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			questPart_PassOutRandom.signalListenMode = signalListenMode;
			quest.AddPart(questPart_PassOutRandom);
			foreach (Action inner in actions)
			{
				string text = QuestGen.GenerateNewSignal("OuterNodeCompleted", true);
				questPart_PassOutRandom.outSignals.Add(text);
				QuestGenUtility.RunInner(inner, text);
			}
		}

		// Token: 0x060084AE RID: 33966 RVA: 0x002FA9B4 File Offset: 0x002F8BB4
		public static void SendSignals(this Quest quest, IEnumerable<string> outSignals, string outSignalsFormat = "", int outSignalsFormattedCount = 0)
		{
			Slate slate = QuestGen.slate;
			IEnumerable<string> enumerable = Enumerable.Empty<string>();
			if (outSignals != null)
			{
				enumerable = enumerable.Concat(outSignals);
			}
			if (outSignalsFormattedCount > 0)
			{
				for (int i = 0; i < outSignalsFormattedCount; i++)
				{
					enumerable = enumerable.Concat(Gen.YieldSingle<string>(outSignalsFormat.Formatted(i.Named("INDEX")).ToString()));
				}
			}
			if (enumerable.EnumerableNullOrEmpty<string>())
			{
				return;
			}
			if (enumerable.Count<string>() == 1)
			{
				QuestPart_Pass questPart_Pass = new QuestPart_Pass();
				questPart_Pass.inSignal = QuestGen.slate.Get<string>("inSignal", null, false);
				questPart_Pass.outSignal = QuestGenUtility.HardcodedSignalWithQuestID(enumerable.First<string>());
				QuestGen.quest.AddPart(questPart_Pass);
				return;
			}
			QuestPart_PassOutMany questPart_PassOutMany = new QuestPart_PassOutMany();
			questPart_PassOutMany.inSignal = QuestGen.slate.Get<string>("inSignal", null, false);
			foreach (string signal in enumerable)
			{
				questPart_PassOutMany.outSignals.Add(QuestGenUtility.HardcodedSignalWithQuestID(signal));
			}
			QuestGen.quest.AddPart(questPart_PassOutMany);
		}

		// Token: 0x060084AF RID: 33967 RVA: 0x002FAADC File Offset: 0x002F8CDC
		public static void SignalPassOutMany(this Quest quest, Action action = null, string inSignal = null, IEnumerable<string> outSignals = null)
		{
			QuestPart_PassOutMany questPart_PassOutMany = new QuestPart_PassOutMany();
			questPart_PassOutMany.inSignal = (inSignal ?? QuestGen.slate.Get<string>("inSignal", null, false));
			if (action != null)
			{
				string innerNodeInSignal = QuestGen.GenerateNewSignal("OuterNodeCompleted", true);
				QuestGenUtility.RunInner(action, innerNodeInSignal);
			}
			if (outSignals != null)
			{
				foreach (string item in outSignals)
				{
					questPart_PassOutMany.outSignals.Add(item);
				}
			}
			quest.AddPart(questPart_PassOutMany);
		}

		// Token: 0x060084B0 RID: 33968 RVA: 0x002FAB6C File Offset: 0x002F8D6C
		public static QuestPart_PassActivable SignalPassActivable(this Quest quest, Action action = null, string inSignalEnable = null, string inSignal = null, string outSignalCompleted = null, IEnumerable<string> outSignalsCompleted = null, string inSignalDisable = null, bool reactivatable = false)
		{
			QuestPart_PassActivable questPart_PassActivable = new QuestPart_PassActivable();
			questPart_PassActivable.inSignalEnable = (inSignalEnable ?? QuestGen.slate.Get<string>("inSignal", null, false));
			questPart_PassActivable.inSignalDisable = inSignalDisable;
			questPart_PassActivable.inSignal = inSignal;
			questPart_PassActivable.reactivatable = reactivatable;
			if (action != null)
			{
				string text = QuestGen.GenerateNewSignal("OuterNodeCompleted", true);
				QuestGenUtility.RunInner(action, text);
				questPart_PassActivable.outSignalsCompleted.Add(text);
			}
			if (outSignalsCompleted != null)
			{
				questPart_PassActivable.outSignalsCompleted.AddRange(outSignalsCompleted);
			}
			if (!outSignalCompleted.NullOrEmpty())
			{
				questPart_PassActivable.outSignalsCompleted.Add(outSignalCompleted);
			}
			quest.AddPart(questPart_PassActivable);
			return questPart_PassActivable;
		}

		// Token: 0x060084B1 RID: 33969 RVA: 0x002FAC04 File Offset: 0x002F8E04
		public static void SignalPass(this Quest quest, Action action = null, string inSignal = null, string outSignal = null)
		{
			QuestPart_Pass questPart_Pass = new QuestPart_Pass();
			questPart_Pass.inSignal = (inSignal ?? QuestGen.slate.Get<string>("inSignal", null, false));
			if (action != null)
			{
				outSignal = (outSignal ?? QuestGen.GenerateNewSignal("OuterNodeCompleted", true));
				QuestGenUtility.RunInner(action, outSignal);
			}
			questPart_Pass.outSignal = outSignal;
			quest.AddPart(questPart_Pass);
		}

		// Token: 0x060084B2 RID: 33970 RVA: 0x002FAC60 File Offset: 0x002F8E60
		public static void SignalPassAll(this Quest quest, Action action = null, List<string> inSignals = null, string outSignal = null)
		{
			QuestPart_PassAll questPart_PassAll = new QuestPart_PassAll();
			QuestPart_PassAll questPart_PassAll2 = questPart_PassAll;
			List<string> inSignals2 = inSignals;
			if (inSignals == null)
			{
				(inSignals2 = new List<string>()).Add(QuestGen.slate.Get<string>("inSignal", null, false));
			}
			questPart_PassAll2.inSignals = inSignals2;
			if (action != null)
			{
				outSignal = (outSignal ?? QuestGen.GenerateNewSignal("OuterNodeCompleted", true));
				QuestGenUtility.RunInner(action, outSignal);
			}
			questPart_PassAll.outSignal = outSignal;
			quest.AddPart(questPart_PassAll);
		}

		// Token: 0x060084B3 RID: 33971 RVA: 0x002FACC4 File Offset: 0x002F8EC4
		public static void SignalPassAllSequence(this Quest quest, Action action = null, List<string> inSignals = null, string outSignal = null)
		{
			QuestPart_PassAllSequence questPart_PassAllSequence = new QuestPart_PassAllSequence();
			QuestPart_PassAllSequence questPart_PassAllSequence2 = questPart_PassAllSequence;
			List<string> inSignals2 = inSignals;
			if (inSignals == null)
			{
				(inSignals2 = new List<string>()).Add(QuestGen.slate.Get<string>("inSignal", null, false));
			}
			questPart_PassAllSequence2.inSignals = inSignals2;
			if (action != null)
			{
				outSignal = (outSignal ?? QuestGen.GenerateNewSignal("OuterNodeCompleted", true));
				QuestGenUtility.RunInner(action, outSignal);
			}
			questPart_PassAllSequence.outSignal = outSignal;
			quest.AddPart(questPart_PassAllSequence);
		}

		// Token: 0x060084B4 RID: 33972 RVA: 0x002FAD28 File Offset: 0x002F8F28
		public static void SignalPassWithFaction(this Quest quest, Faction faction, Action action = null, Action outAction = null, string inSignal = null, string outSignal = null)
		{
			QuestPart_PassWithFactionArg questPart_PassWithFactionArg = new QuestPart_PassWithFactionArg();
			questPart_PassWithFactionArg.inSignal = (inSignal ?? QuestGen.slate.Get<string>("inSignal", null, false));
			questPart_PassWithFactionArg.faction = faction;
			questPart_PassWithFactionArg.outSignal = outSignal;
			if (action != null)
			{
				string innerNodeInSignal = QuestGen.GenerateNewSignal("OuterNodeCompleted", true);
				QuestGenUtility.RunInner(action, innerNodeInSignal);
			}
			if (outAction != null)
			{
				if (questPart_PassWithFactionArg.outSignal == null)
				{
					questPart_PassWithFactionArg.outSignal = QuestGen.GenerateNewSignal("OuterNodeCompleted", true);
				}
				QuestGenUtility.RunInner(outAction, questPart_PassWithFactionArg.outSignal);
			}
			quest.AddPart(questPart_PassWithFactionArg);
		}
	}
}
