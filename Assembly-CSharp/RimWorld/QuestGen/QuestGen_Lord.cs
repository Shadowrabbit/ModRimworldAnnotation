using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001EBF RID: 7871
	public static class QuestGen_Lord
	{
		// Token: 0x0600A8F7 RID: 43255 RVA: 0x00314530 File Offset: 0x00312730
		public static QuestPart_DefendPoint DefendPoint(this Quest quest, MapParent mapParent, IntVec3 point, IEnumerable<Pawn> pawns, Faction faction, string inSignal = null, string inSignalRemovePawn = null, float? wanderRadius = null, bool isCaravanSendable = false, bool addFleeToil = true)
		{
			QuestPart_DefendPoint questPart_DefendPoint = new QuestPart_DefendPoint();
			questPart_DefendPoint.inSignal = (inSignal ?? QuestGen.slate.Get<string>("inSignal", null, false));
			questPart_DefendPoint.point = point;
			questPart_DefendPoint.mapParent = mapParent;
			questPart_DefendPoint.pawns.AddRange(pawns);
			questPart_DefendPoint.inSignalRemovePawn = inSignalRemovePawn;
			questPart_DefendPoint.wanderRadius = wanderRadius;
			questPart_DefendPoint.isCaravanSendable = isCaravanSendable;
			questPart_DefendPoint.faction = faction;
			questPart_DefendPoint.addFleeToil = addFleeToil;
			quest.AddPart(questPart_DefendPoint);
			return questPart_DefendPoint;
		}

		// Token: 0x0600A8F8 RID: 43256 RVA: 0x003145AC File Offset: 0x003127AC
		public static QuestPart_AssaultThings AssaultThings(this Quest quest, MapParent mapParent, IEnumerable<Pawn> pawns, Faction assaulterFaction, IEnumerable<Thing> things, string inSignal = null, string inSignalRemovePawn = null, bool excludeFromLookTargets = false)
		{
			QuestPart_AssaultThings questPart_AssaultThings = new QuestPart_AssaultThings();
			questPart_AssaultThings.inSignal = (inSignal ?? QuestGen.slate.Get<string>("inSignal", null, false));
			questPart_AssaultThings.mapParent = mapParent;
			questPart_AssaultThings.pawns.AddRange(pawns);
			questPart_AssaultThings.inSignalRemovePawn = inSignalRemovePawn;
			questPart_AssaultThings.faction = assaulterFaction;
			questPart_AssaultThings.things.AddRange(things);
			questPart_AssaultThings.excludeFromLookTargets = excludeFromLookTargets;
			quest.AddPart(questPart_AssaultThings);
			return questPart_AssaultThings;
		}

		// Token: 0x0600A8F9 RID: 43257 RVA: 0x0031461C File Offset: 0x0031281C
		public static QuestPart_ExitOnShuttle ExitOnShuttle(this Quest quest, MapParent mapParent, IEnumerable<Pawn> pawns, Faction faction, Thing shuttle, string inSignal = null, bool addFleeToil = true)
		{
			QuestPart_ExitOnShuttle questPart_ExitOnShuttle = new QuestPart_ExitOnShuttle();
			questPart_ExitOnShuttle.inSignal = (inSignal ?? QuestGen.slate.Get<string>("inSignal", null, false));
			questPart_ExitOnShuttle.pawns.AddRange(pawns);
			questPart_ExitOnShuttle.mapParent = mapParent;
			questPart_ExitOnShuttle.faction = faction;
			questPart_ExitOnShuttle.shuttle = shuttle;
			questPart_ExitOnShuttle.addFleeToil = addFleeToil;
			quest.AddPart(questPart_ExitOnShuttle);
			return questPart_ExitOnShuttle;
		}

		// Token: 0x0600A8FA RID: 43258 RVA: 0x00314680 File Offset: 0x00312880
		public static QuestPart_WaitForEscort WaitForEscort(this Quest quest, MapParent mapParent, IEnumerable<Pawn> pawns, Faction faction, IntVec3 position, string inSignal = null, bool addFleeToil = true)
		{
			QuestPart_WaitForEscort questPart_WaitForEscort = new QuestPart_WaitForEscort();
			questPart_WaitForEscort.point = position;
			questPart_WaitForEscort.inSignal = (inSignal ?? QuestGen.slate.Get<string>("inSignal", null, false));
			questPart_WaitForEscort.pawns.AddRange(pawns);
			questPart_WaitForEscort.mapParent = mapParent;
			questPart_WaitForEscort.faction = Faction.Empire;
			questPart_WaitForEscort.addFleeToil = addFleeToil;
			quest.AddPart(questPart_WaitForEscort);
			return questPart_WaitForEscort;
		}
	}
}
