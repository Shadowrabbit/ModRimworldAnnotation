using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001ED3 RID: 7891
	public static class QuestGen_Signal
	{
		// Token: 0x0600A953 RID: 43347 RVA: 0x00316AF0 File Offset: 0x00314CF0
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

		// Token: 0x0600A954 RID: 43348 RVA: 0x00316C04 File Offset: 0x00314E04
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

		// Token: 0x0600A955 RID: 43349 RVA: 0x00316D88 File Offset: 0x00314F88
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

		// Token: 0x0600A956 RID: 43350 RVA: 0x00316E1C File Offset: 0x0031501C
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

		// Token: 0x0600A957 RID: 43351 RVA: 0x00316F44 File Offset: 0x00315144
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

		// Token: 0x0600A958 RID: 43352 RVA: 0x00316FD4 File Offset: 0x003151D4
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

		// Token: 0x0600A959 RID: 43353 RVA: 0x0031706C File Offset: 0x0031526C
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

		// Token: 0x0600A95A RID: 43354 RVA: 0x003170C8 File Offset: 0x003152C8
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

		// Token: 0x0600A95B RID: 43355 RVA: 0x0031712C File Offset: 0x0031532C
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

		// Token: 0x0600A95C RID: 43356 RVA: 0x00317190 File Offset: 0x00315390
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
