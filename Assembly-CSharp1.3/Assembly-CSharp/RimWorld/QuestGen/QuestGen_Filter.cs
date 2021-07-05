using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001612 RID: 5650
	public static class QuestGen_Filter
	{
		// Token: 0x0600845B RID: 33883 RVA: 0x002F71B4 File Offset: 0x002F53B4
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

		// Token: 0x0600845C RID: 33884 RVA: 0x002F7248 File Offset: 0x002F5448
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

		// Token: 0x0600845D RID: 33885 RVA: 0x002F72EC File Offset: 0x002F54EC
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

		// Token: 0x0600845E RID: 33886 RVA: 0x002F7380 File Offset: 0x002F5580
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

		// Token: 0x0600845F RID: 33887 RVA: 0x002F7410 File Offset: 0x002F5610
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

		// Token: 0x06008460 RID: 33888 RVA: 0x002F74A4 File Offset: 0x002F56A4
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

		// Token: 0x06008461 RID: 33889 RVA: 0x002F7548 File Offset: 0x002F5748
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

		// Token: 0x06008462 RID: 33890 RVA: 0x002F75C8 File Offset: 0x002F57C8
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

		// Token: 0x06008463 RID: 33891 RVA: 0x002F766C File Offset: 0x002F586C
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

		// Token: 0x06008464 RID: 33892 RVA: 0x002F7710 File Offset: 0x002F5910
		public static QuestPart_Filter_AcceptedAfterTicks AcceptedAfterTicks(this Quest quest, int ticks, Action action = null, Action elseAction = null, string inSignal = null, string outSignal = null, string outSignalElse = null, string inSignalRemovePawn = null, QuestPart.SignalListenMode signalListenMode = QuestPart.SignalListenMode.OngoingOnly)
		{
			QuestPart_Filter_AcceptedAfterTicks questPart_Filter_AcceptedAfterTicks = new QuestPart_Filter_AcceptedAfterTicks();
			questPart_Filter_AcceptedAfterTicks.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(inSignal) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			questPart_Filter_AcceptedAfterTicks.signalListenMode = signalListenMode;
			questPart_Filter_AcceptedAfterTicks.timeTicks = ticks;
			questPart_Filter_AcceptedAfterTicks.outSignal = outSignal;
			questPart_Filter_AcceptedAfterTicks.outSignalElse = outSignalElse;
			if (action != null)
			{
				string text = QuestGen.GenerateNewSignal("OuterNodeCompleted", true);
				questPart_Filter_AcceptedAfterTicks.outSignal = text;
				QuestGenUtility.RunInner(action, text);
			}
			if (elseAction != null)
			{
				string text2 = QuestGen.GenerateNewSignal("OuterNodeCompleted", true);
				questPart_Filter_AcceptedAfterTicks.outSignalElse = text2;
				QuestGenUtility.RunInner(elseAction, text2);
			}
			quest.AddPart(questPart_Filter_AcceptedAfterTicks);
			return questPart_Filter_AcceptedAfterTicks;
		}

		// Token: 0x06008465 RID: 33893 RVA: 0x002F77A8 File Offset: 0x002F59A8
		public static QuestPart_Filter_AnyColonistWithCharityPrecept AnyColonistWithCharityPrecept(this Quest quest, Action action = null, Action elseAction = null, string inSignal = null, string outSignal = null, string outSignalElse = null, QuestPart.SignalListenMode signalListenMode = QuestPart.SignalListenMode.OngoingOnly)
		{
			QuestPart_Filter_AnyColonistWithCharityPrecept questPart_Filter_AnyColonistWithCharityPrecept = new QuestPart_Filter_AnyColonistWithCharityPrecept();
			questPart_Filter_AnyColonistWithCharityPrecept.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(inSignal) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			questPart_Filter_AnyColonistWithCharityPrecept.signalListenMode = signalListenMode;
			questPart_Filter_AnyColonistWithCharityPrecept.outSignal = outSignal;
			questPart_Filter_AnyColonistWithCharityPrecept.outSignalElse = outSignalElse;
			if (action != null)
			{
				string text = QuestGen.GenerateNewSignal("OuterNodeCompleted", true);
				questPart_Filter_AnyColonistWithCharityPrecept.outSignal = text;
				QuestGenUtility.RunInner(action, text);
			}
			if (elseAction != null)
			{
				string text2 = QuestGen.GenerateNewSignal("OuterNodeCompleted", true);
				questPart_Filter_AnyColonistWithCharityPrecept.outSignalElse = text2;
				QuestGenUtility.RunInner(elseAction, text2);
			}
			quest.AddPart(questPart_Filter_AnyColonistWithCharityPrecept);
			return questPart_Filter_AnyColonistWithCharityPrecept;
		}

		// Token: 0x06008466 RID: 33894 RVA: 0x002F7838 File Offset: 0x002F5A38
		public static QuestPart_Filter_BuiltNearSettlement BuiltNearSettlement(this Quest quest, Faction settlementFaction, MapParent mapParent, float searchRadius = 6f, Action action = null, Action elseAction = null, string inSignal = null, string outSignal = null, string outSignalElse = null, QuestPart.SignalListenMode signalListenMode = QuestPart.SignalListenMode.OngoingOnly)
		{
			QuestPart_Filter_BuiltNearSettlement questPart_Filter_BuiltNearSettlement = new QuestPart_Filter_BuiltNearSettlement();
			questPart_Filter_BuiltNearSettlement.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(inSignal) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			questPart_Filter_BuiltNearSettlement.signalListenMode = signalListenMode;
			questPart_Filter_BuiltNearSettlement.settlementFaction = settlementFaction;
			questPart_Filter_BuiltNearSettlement.mapParent = mapParent;
			questPart_Filter_BuiltNearSettlement.radius = searchRadius;
			questPart_Filter_BuiltNearSettlement.outSignal = outSignal;
			questPart_Filter_BuiltNearSettlement.outSignalElse = outSignalElse;
			if (action != null)
			{
				string text = QuestGen.GenerateNewSignal("OuterNodeCompleted", true);
				questPart_Filter_BuiltNearSettlement.outSignal = text;
				QuestGenUtility.RunInner(action, text);
			}
			if (elseAction != null)
			{
				string text2 = QuestGen.GenerateNewSignal("OuterNodeCompleted", true);
				questPart_Filter_BuiltNearSettlement.outSignalElse = text2;
				QuestGenUtility.RunInner(elseAction, text2);
			}
			quest.AddPart(questPart_Filter_BuiltNearSettlement);
			return questPart_Filter_BuiltNearSettlement;
		}

		// Token: 0x06008467 RID: 33895 RVA: 0x002F78E0 File Offset: 0x002F5AE0
		public static QuestPart_Filter_AnyHostileThreatToPlayer AnyHostileThreatToPlayer(this Quest quest, MapParent mapParent, bool countDormantPawns = false, Action action = null, Action elseAction = null, string inSignal = null, string outSignal = null, string outSignalElse = null, string inSignalRemovePawn = null, QuestPart.SignalListenMode signalListenMode = QuestPart.SignalListenMode.OngoingOnly)
		{
			QuestPart_Filter_AnyHostileThreatToPlayer questPart_Filter_AnyHostileThreatToPlayer = new QuestPart_Filter_AnyHostileThreatToPlayer();
			questPart_Filter_AnyHostileThreatToPlayer.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(inSignal) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			questPart_Filter_AnyHostileThreatToPlayer.signalListenMode = signalListenMode;
			questPart_Filter_AnyHostileThreatToPlayer.outSignal = outSignal;
			questPart_Filter_AnyHostileThreatToPlayer.outSignalElse = outSignalElse;
			questPart_Filter_AnyHostileThreatToPlayer.countDormantPawnsAsHostile = countDormantPawns;
			questPart_Filter_AnyHostileThreatToPlayer.mapParent = mapParent;
			if (action != null)
			{
				string text = QuestGen.GenerateNewSignal("OuterNodeCompleted", true);
				questPart_Filter_AnyHostileThreatToPlayer.outSignal = text;
				QuestGenUtility.RunInner(action, text);
			}
			if (elseAction != null)
			{
				string text2 = QuestGen.GenerateNewSignal("OuterNodeCompleted", true);
				questPart_Filter_AnyHostileThreatToPlayer.outSignalElse = text2;
				QuestGenUtility.RunInner(elseAction, text2);
			}
			quest.AddPart(questPart_Filter_AnyHostileThreatToPlayer);
			return questPart_Filter_AnyHostileThreatToPlayer;
		}

		// Token: 0x06008468 RID: 33896 RVA: 0x002F7980 File Offset: 0x002F5B80
		public static QuestPart_Filter_CanAcceptQuest CanAcceptQuest(this Quest quest, Action action = null, Action elseAction = null, string inSignal = null, string outSignal = null, string outSignalElse = null, QuestPart.SignalListenMode signalListenMode = QuestPart.SignalListenMode.OngoingOnly)
		{
			QuestPart_Filter_CanAcceptQuest questPart_Filter_CanAcceptQuest = new QuestPart_Filter_CanAcceptQuest();
			questPart_Filter_CanAcceptQuest.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(inSignal) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			questPart_Filter_CanAcceptQuest.signalListenMode = signalListenMode;
			questPart_Filter_CanAcceptQuest.outSignal = outSignal;
			questPart_Filter_CanAcceptQuest.outSignalElse = outSignalElse;
			if (action != null)
			{
				string text = QuestGen.GenerateNewSignal("OuterNodeCompleted", true);
				questPart_Filter_CanAcceptQuest.outSignal = text;
				QuestGenUtility.RunInner(action, text);
			}
			if (elseAction != null)
			{
				string text2 = QuestGen.GenerateNewSignal("OuterNodeCompleted", true);
				questPart_Filter_CanAcceptQuest.outSignalElse = text2;
				QuestGenUtility.RunInner(elseAction, text2);
			}
			quest.AddPart(questPart_Filter_CanAcceptQuest);
			return questPart_Filter_CanAcceptQuest;
		}
	}
}
