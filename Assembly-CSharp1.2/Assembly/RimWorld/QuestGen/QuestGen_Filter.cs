using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001EBB RID: 7867
	public static class QuestGen_Filter
	{
		// Token: 0x0600A8E7 RID: 43239 RVA: 0x00313F0C File Offset: 0x0031210C
		public static QuestPart_Filter_AnyPawnAlive AnyPawnAlive(this Quest quest, IEnumerable<Pawn> pawns, Action action = null, Action elseAction = null, string inSignal = null, string outSignal = null, string outSignalElse = null, string inSignalRemovePawn = null, QuestPart.SignalListenMode signalListenMode = QuestPart.SignalListenMode.OngoingOnly)
		{
			QuestPart_Filter_AnyPawnAlive questPart_Filter_AnyPawnAlive = new QuestPart_Filter_AnyPawnAlive();
			questPart_Filter_AnyPawnAlive.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(inSignal) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			questPart_Filter_AnyPawnAlive.signalListenMode = signalListenMode;
			questPart_Filter_AnyPawnAlive.pawns = pawns.ToList<Pawn>();
			questPart_Filter_AnyPawnAlive.inSignalRemovePawn = inSignalRemovePawn;
			if (action != null)
			{
				string text = QuestGen.GenerateNewSignal("OuterNodeCompleted", true);
				questPart_Filter_AnyPawnAlive.outSignal = text;
				QuestGenUtility.RunInner(action, text);
			}
			if (elseAction != null)
			{
				string text2 = QuestGen.GenerateNewSignal("OuterNodeCompleted", true);
				questPart_Filter_AnyPawnAlive.outSignalElse = text2;
				QuestGenUtility.RunInner(elseAction, text2);
			}
			quest.AddPart(questPart_Filter_AnyPawnAlive);
			return questPart_Filter_AnyPawnAlive;
		}

		// Token: 0x0600A8E8 RID: 43240 RVA: 0x00313FA0 File Offset: 0x003121A0
		public static QuestPart_Filter_AllPawnsDespawned AllPawnsDespawned(this Quest quest, IEnumerable<Pawn> pawns, Action action = null, Action elseAction = null, string inSignal = null, string outSignal = null, string outSignalElse = null, string inSignalRemovePawn = null, QuestPart.SignalListenMode signalListenMode = QuestPart.SignalListenMode.OngoingOnly)
		{
			QuestPart_Filter_AllPawnsDespawned questPart_Filter_AllPawnsDespawned = new QuestPart_Filter_AllPawnsDespawned();
			questPart_Filter_AllPawnsDespawned.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(inSignal) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			questPart_Filter_AllPawnsDespawned.signalListenMode = signalListenMode;
			questPart_Filter_AllPawnsDespawned.pawns = pawns.ToList<Pawn>();
			questPart_Filter_AllPawnsDespawned.inSignalRemovePawn = inSignalRemovePawn;
			questPart_Filter_AllPawnsDespawned.outSignal = outSignal;
			questPart_Filter_AllPawnsDespawned.outSignalElse = outSignalElse;
			if (action != null)
			{
				string text = QuestGen.GenerateNewSignal("OuterNodeCompleted", true);
				questPart_Filter_AllPawnsDespawned.outSignal = text;
				QuestGenUtility.RunInner(action, text);
			}
			if (elseAction != null)
			{
				string text2 = QuestGen.GenerateNewSignal("OuterNodeCompleted", true);
				questPart_Filter_AllPawnsDespawned.outSignalElse = text2;
				QuestGenUtility.RunInner(elseAction, text2);
			}
			quest.AddPart(questPart_Filter_AllPawnsDespawned);
			return questPart_Filter_AllPawnsDespawned;
		}

		// Token: 0x0600A8E9 RID: 43241 RVA: 0x00314044 File Offset: 0x00312244
		public static QuestPart_Filter_AnyPawnUnhealthy AnyPawnUnhealthy(this Quest quest, IEnumerable<Pawn> pawns, Action action = null, Action elseAction = null, string inSignal = null, string outSignal = null, string outSignalElse = null, string inSignalRemovePawn = null, QuestPart.SignalListenMode signalListenMode = QuestPart.SignalListenMode.OngoingOnly)
		{
			QuestPart_Filter_AnyPawnUnhealthy questPart_Filter_AnyPawnUnhealthy = new QuestPart_Filter_AnyPawnUnhealthy();
			questPart_Filter_AnyPawnUnhealthy.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(inSignal) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			questPart_Filter_AnyPawnUnhealthy.signalListenMode = signalListenMode;
			questPart_Filter_AnyPawnUnhealthy.pawns = pawns.ToList<Pawn>();
			questPart_Filter_AnyPawnUnhealthy.inSignalRemovePawn = inSignalRemovePawn;
			if (action != null)
			{
				string text = QuestGen.GenerateNewSignal("OuterNodeCompleted", true);
				questPart_Filter_AnyPawnUnhealthy.outSignal = text;
				QuestGenUtility.RunInner(action, text);
			}
			if (elseAction != null)
			{
				string text2 = QuestGen.GenerateNewSignal("OuterNodeCompleted", true);
				questPart_Filter_AnyPawnUnhealthy.outSignalElse = text2;
				QuestGenUtility.RunInner(elseAction, text2);
			}
			quest.AddPart(questPart_Filter_AnyPawnUnhealthy);
			return questPart_Filter_AnyPawnUnhealthy;
		}

		// Token: 0x0600A8EA RID: 43242 RVA: 0x003140D8 File Offset: 0x003122D8
		public static QuestPart_Filter_FactionHostileToOtherFaction FactionHostileToOtherFaction(this Quest quest, Faction faction, Faction other, Action action = null, Action elseAction = null, string inSignal = null, string outSignal = null, string outSignalElse = null, QuestPart.SignalListenMode signalListenMode = QuestPart.SignalListenMode.OngoingOnly)
		{
			QuestPart_Filter_FactionHostileToOtherFaction questPart_Filter_FactionHostileToOtherFaction = new QuestPart_Filter_FactionHostileToOtherFaction();
			questPart_Filter_FactionHostileToOtherFaction.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(inSignal) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			questPart_Filter_FactionHostileToOtherFaction.signalListenMode = signalListenMode;
			questPart_Filter_FactionHostileToOtherFaction.faction = faction;
			questPart_Filter_FactionHostileToOtherFaction.other = other;
			if (action != null)
			{
				string text = QuestGen.GenerateNewSignal("OuterNodeCompleted", true);
				questPart_Filter_FactionHostileToOtherFaction.outSignal = text;
				QuestGenUtility.RunInner(action, text);
			}
			if (elseAction != null)
			{
				string text2 = QuestGen.GenerateNewSignal("OuterNodeCompleted", true);
				questPart_Filter_FactionHostileToOtherFaction.outSignalElse = text2;
				QuestGenUtility.RunInner(elseAction, text2);
			}
			quest.AddPart(questPart_Filter_FactionHostileToOtherFaction);
			return questPart_Filter_FactionHostileToOtherFaction;
		}

		// Token: 0x0600A8EB RID: 43243 RVA: 0x00314168 File Offset: 0x00312368
		public static QuestPart_Filter_AnyPawnPlayerControlled AnyPawnPlayerControlled(this Quest quest, IEnumerable<Pawn> pawns, Action action = null, Action elseAction = null, string inSignal = null, string outSignal = null, string outSignalElse = null, string inSignalRemovePawn = null, QuestPart.SignalListenMode signalListenMode = QuestPart.SignalListenMode.OngoingOnly)
		{
			QuestPart_Filter_AnyPawnPlayerControlled questPart_Filter_AnyPawnPlayerControlled = new QuestPart_Filter_AnyPawnPlayerControlled();
			questPart_Filter_AnyPawnPlayerControlled.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(inSignal) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			questPart_Filter_AnyPawnPlayerControlled.signalListenMode = signalListenMode;
			questPart_Filter_AnyPawnPlayerControlled.pawns = pawns.ToList<Pawn>();
			questPart_Filter_AnyPawnPlayerControlled.inSignalRemovePawn = inSignalRemovePawn;
			if (action != null)
			{
				string text = QuestGen.GenerateNewSignal("OuterNodeCompleted", true);
				questPart_Filter_AnyPawnPlayerControlled.outSignal = text;
				QuestGenUtility.RunInner(action, text);
			}
			if (elseAction != null)
			{
				string text2 = QuestGen.GenerateNewSignal("OuterNodeCompleted", true);
				questPart_Filter_AnyPawnPlayerControlled.outSignalElse = text2;
				QuestGenUtility.RunInner(elseAction, text2);
			}
			quest.AddPart(questPart_Filter_AnyPawnPlayerControlled);
			return questPart_Filter_AnyPawnPlayerControlled;
		}

		// Token: 0x0600A8EC RID: 43244 RVA: 0x003141FC File Offset: 0x003123FC
		public static QuestPart_Filter_AllPawnsDestroyed AllPawnsDestroyed(this Quest quest, IEnumerable<Pawn> pawns, Action action = null, Action elseAction = null, string inSignal = null, string outSignal = null, string outSignalElse = null, string inSignalRemovePawn = null, QuestPart.SignalListenMode signalListenMode = QuestPart.SignalListenMode.OngoingOnly)
		{
			QuestPart_Filter_AllPawnsDestroyed questPart_Filter_AllPawnsDestroyed = new QuestPart_Filter_AllPawnsDestroyed();
			questPart_Filter_AllPawnsDestroyed.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(inSignal) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			questPart_Filter_AllPawnsDestroyed.signalListenMode = signalListenMode;
			questPart_Filter_AllPawnsDestroyed.pawns = pawns.ToList<Pawn>();
			questPart_Filter_AllPawnsDestroyed.inSignalRemovePawn = inSignalRemovePawn;
			questPart_Filter_AllPawnsDestroyed.outSignal = outSignal;
			questPart_Filter_AllPawnsDestroyed.outSignalElse = outSignalElse;
			if (action != null)
			{
				string text = QuestGen.GenerateNewSignal("OuterNodeCompleted", true);
				questPart_Filter_AllPawnsDestroyed.outSignal = text;
				QuestGenUtility.RunInner(action, text);
			}
			if (elseAction != null)
			{
				string text2 = QuestGen.GenerateNewSignal("OuterNodeCompleted", true);
				questPart_Filter_AllPawnsDestroyed.outSignalElse = text2;
				QuestGenUtility.RunInner(elseAction, text2);
			}
			quest.AddPart(questPart_Filter_AllPawnsDestroyed);
			return questPart_Filter_AllPawnsDestroyed;
		}

		// Token: 0x0600A8ED RID: 43245 RVA: 0x003142A0 File Offset: 0x003124A0
		public static QuestPart_Filter_FactionNonPlayer FactionNonPlayer(this Quest quest, Action action = null, Action elseAction = null, string inSignal = null, string outSignal = null, string outSignalElse = null, QuestPart.SignalListenMode signalListenMode = QuestPart.SignalListenMode.OngoingOnly)
		{
			QuestPart_Filter_FactionNonPlayer questPart_Filter_FactionNonPlayer = new QuestPart_Filter_FactionNonPlayer();
			questPart_Filter_FactionNonPlayer.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(inSignal) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			questPart_Filter_FactionNonPlayer.signalListenMode = signalListenMode;
			if (action != null)
			{
				string text = QuestGen.GenerateNewSignal("OuterNodeCompleted", true);
				questPart_Filter_FactionNonPlayer.outSignal = text;
				QuestGenUtility.RunInner(action, text);
			}
			if (elseAction != null)
			{
				string text2 = QuestGen.GenerateNewSignal("OuterNodeCompleted", true);
				questPart_Filter_FactionNonPlayer.outSignalElse = text2;
				QuestGenUtility.RunInner(elseAction, text2);
			}
			quest.AddPart(questPart_Filter_FactionNonPlayer);
			return questPart_Filter_FactionNonPlayer;
		}

		// Token: 0x0600A8EE RID: 43246 RVA: 0x00314320 File Offset: 0x00312520
		public static QuestPart_Filter_AllPawnsDowned AllPawnsDowned(this Quest quest, IEnumerable<Pawn> pawns, Action action = null, Action elseAction = null, string inSignal = null, string outSignal = null, string outSignalElse = null, string inSignalRemovePawn = null, QuestPart.SignalListenMode signalListenMode = QuestPart.SignalListenMode.OngoingOnly)
		{
			QuestPart_Filter_AllPawnsDowned questPart_Filter_AllPawnsDowned = new QuestPart_Filter_AllPawnsDowned();
			questPart_Filter_AllPawnsDowned.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(inSignal) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			questPart_Filter_AllPawnsDowned.signalListenMode = signalListenMode;
			questPart_Filter_AllPawnsDowned.pawns = pawns.ToList<Pawn>();
			questPart_Filter_AllPawnsDowned.inSignalRemovePawn = inSignalRemovePawn;
			questPart_Filter_AllPawnsDowned.outSignal = outSignal;
			questPart_Filter_AllPawnsDowned.outSignalElse = outSignalElse;
			if (action != null)
			{
				string text = QuestGen.GenerateNewSignal("OuterNodeCompleted", true);
				questPart_Filter_AllPawnsDowned.outSignal = text;
				QuestGenUtility.RunInner(action, text);
			}
			if (elseAction != null)
			{
				string text2 = QuestGen.GenerateNewSignal("OuterNodeCompleted", true);
				questPart_Filter_AllPawnsDowned.outSignalElse = text2;
				QuestGenUtility.RunInner(elseAction, text2);
			}
			quest.AddPart(questPart_Filter_AllPawnsDowned);
			return questPart_Filter_AllPawnsDowned;
		}

		// Token: 0x0600A8EF RID: 43247 RVA: 0x003143C4 File Offset: 0x003125C4
		public static QuestPart_Filter_AnyOnTransporter AnyOnTransporter(this Quest quest, IEnumerable<Pawn> pawns, Thing shuttle, Action action = null, Action elseAction = null, string inSignal = null, string outSignal = null, string outSignalElse = null, string inSignalRemovePawn = null, QuestPart.SignalListenMode signalListenMode = QuestPart.SignalListenMode.OngoingOnly)
		{
			QuestPart_Filter_AnyOnTransporter questPart_Filter_AnyOnTransporter = new QuestPart_Filter_AnyOnTransporter();
			questPart_Filter_AnyOnTransporter.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(inSignal) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			questPart_Filter_AnyOnTransporter.signalListenMode = signalListenMode;
			questPart_Filter_AnyOnTransporter.pawns = pawns.ToList<Pawn>();
			questPart_Filter_AnyOnTransporter.outSignal = outSignal;
			questPart_Filter_AnyOnTransporter.outSignalElse = outSignalElse;
			questPart_Filter_AnyOnTransporter.transporter = shuttle;
			if (action != null)
			{
				string text = QuestGen.GenerateNewSignal("OuterNodeCompleted", true);
				questPart_Filter_AnyOnTransporter.outSignal = text;
				QuestGenUtility.RunInner(action, text);
			}
			if (elseAction != null)
			{
				string text2 = QuestGen.GenerateNewSignal("OuterNodeCompleted", true);
				questPart_Filter_AnyOnTransporter.outSignalElse = text2;
				QuestGenUtility.RunInner(elseAction, text2);
			}
			quest.AddPart(questPart_Filter_AnyOnTransporter);
			return questPart_Filter_AnyOnTransporter;
		}
	}
}
