using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000AA3 RID: 2723
	public abstract class PawnsArrivalModeWorker
	{
		// Token: 0x060040C2 RID: 16578 RVA: 0x0015DD20 File Offset: 0x0015BF20
		public virtual bool CanUseWith(IncidentParms parms)
		{
			return (parms.faction == null || this.def.minTechLevel == TechLevel.Undefined || parms.faction.def.techLevel >= this.def.minTechLevel) && (!parms.raidArrivalModeForQuickMilitaryAid || this.def.forQuickMilitaryAid) && (parms.raidStrategy == null || parms.raidStrategy.arriveModes.Contains(this.def));
		}

		// Token: 0x060040C3 RID: 16579 RVA: 0x0015DD9B File Offset: 0x0015BF9B
		public virtual float GetSelectionWeight(IncidentParms parms)
		{
			if (this.def.selectionWeightCurve != null)
			{
				return this.def.selectionWeightCurve.Evaluate(parms.points);
			}
			return 0f;
		}

		// Token: 0x060040C4 RID: 16580
		public abstract void Arrive(List<Pawn> pawns, IncidentParms parms);

		// Token: 0x060040C5 RID: 16581 RVA: 0x0015DDC6 File Offset: 0x0015BFC6
		public virtual void TravelingTransportPodsArrived(List<ActiveDropPodInfo> dropPods, Map map)
		{
			throw new NotSupportedException("Traveling transport pods arrived with mode " + this.def.defName);
		}

		// Token: 0x060040C6 RID: 16582
		public abstract bool TryResolveRaidSpawnCenter(IncidentParms parms);

		// Token: 0x040025AF RID: 9647
		public PawnsArrivalModeDef def;
	}
}
