using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000FC6 RID: 4038
	public abstract class PawnsArrivalModeWorker
	{
		// Token: 0x0600584C RID: 22604 RVA: 0x001CFCDC File Offset: 0x001CDEDC
		public virtual bool CanUseWith(IncidentParms parms)
		{
			return (parms.faction == null || this.def.minTechLevel == TechLevel.Undefined || parms.faction.def.techLevel >= this.def.minTechLevel) && (!parms.raidArrivalModeForQuickMilitaryAid || this.def.forQuickMilitaryAid) && (parms.raidStrategy == null || parms.raidStrategy.arriveModes.Contains(this.def));
		}

		// Token: 0x0600584D RID: 22605 RVA: 0x0003D5A7 File Offset: 0x0003B7A7
		public virtual float GetSelectionWeight(IncidentParms parms)
		{
			if (this.def.selectionWeightCurve != null)
			{
				return this.def.selectionWeightCurve.Evaluate(parms.points);
			}
			return 0f;
		}

		// Token: 0x0600584E RID: 22606
		public abstract void Arrive(List<Pawn> pawns, IncidentParms parms);

		// Token: 0x0600584F RID: 22607 RVA: 0x0003D5D2 File Offset: 0x0003B7D2
		public virtual void TravelingTransportPodsArrived(List<ActiveDropPodInfo> dropPods, Map map)
		{
			throw new NotSupportedException("Traveling transport pods arrived with mode " + this.def.defName);
		}

		// Token: 0x06005850 RID: 22608
		public abstract bool TryResolveRaidSpawnCenter(IncidentParms parms);

		// Token: 0x04003A58 RID: 14936
		public PawnsArrivalModeDef def;
	}
}
