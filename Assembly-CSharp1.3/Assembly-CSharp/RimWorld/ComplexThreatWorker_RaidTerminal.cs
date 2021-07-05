using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C6A RID: 3178
	public class ComplexThreatWorker_RaidTerminal : ComplexThreatWorker
	{
		// Token: 0x06004A38 RID: 19000 RVA: 0x00188608 File Offset: 0x00186808
		protected override bool CanResolveInt(ComplexResolveParams parms)
		{
			Faction faction;
			if (base.CanResolveInt(parms) && this.TryFindRandomEnemyFaction(out faction))
			{
				IntVec3 intVec;
				return ComplexUtility.TryFindRandomSpawnCell(ThingDefOf.AncientEnemyTerminal, parms.room.SelectMany((CellRect r) => r.Cells), parms.map, out intVec, 1, null);
			}
			return false;
		}

		// Token: 0x06004A39 RID: 19001 RVA: 0x00188670 File Offset: 0x00186870
		protected override void ResolveInt(ComplexResolveParams parms, ref float threatPointsUsed, List<Thing> outSpawnedThings)
		{
			IntVec3 loc;
			ComplexUtility.TryFindRandomSpawnCell(ThingDefOf.AncientEnemyTerminal, parms.room.SelectMany((CellRect r) => r.Cells), parms.map, out loc, 1, null);
			Thing thing = GenSpawn.Spawn(ThingDefOf.AncientEnemyTerminal, loc, parms.map, WipeMode.Vanish);
			Faction faction;
			this.TryFindRandomEnemyFaction(out faction);
			float num = Mathf.Max(parms.points, faction.def.MinPointsToGeneratePawnGroup(PawnGroupKindDefOf.Combat, null));
			IncidentParms incidentParms = new IncidentParms();
			incidentParms.forced = true;
			incidentParms.target = parms.map;
			incidentParms.points = parms.points;
			incidentParms.faction = faction;
			incidentParms.raidArrivalMode = PawnsArrivalModeDefOf.EdgeWalkIn;
			incidentParms.raidStrategy = RaidStrategyDefOf.ImmediateAttack;
			CompHackable compHackable = thing.TryGetComp<CompHackable>();
			string text = compHackable.hackingCompletedSignal;
			if (text.NullOrEmpty())
			{
				text = "RaidSignal" + Find.UniqueIDsManager.GetNextSignalTagID();
				compHackable.hackingCompletedSignal = text;
			}
			SignalAction_Incident signalAction_Incident = (SignalAction_Incident)ThingMaker.MakeThing(ThingDefOf.SignalAction_Incident, null);
			signalAction_Incident.incident = IncidentDefOf.RaidEnemy;
			signalAction_Incident.incidentParms = incidentParms;
			signalAction_Incident.signalTag = text;
			GenSpawn.Spawn(signalAction_Incident, parms.room[0].CenterCell, parms.map, WipeMode.Vanish);
			threatPointsUsed += num;
		}

		// Token: 0x06004A3A RID: 19002 RVA: 0x001887CE File Offset: 0x001869CE
		private bool TryFindRandomEnemyFaction(out Faction faction)
		{
			faction = Find.FactionManager.RandomEnemyFaction(false, false, false, TechLevel.Undefined);
			return faction != null;
		}

		// Token: 0x04002D1E RID: 11550
		private const string SignalPrefix = "RaidSignal";
	}
}
