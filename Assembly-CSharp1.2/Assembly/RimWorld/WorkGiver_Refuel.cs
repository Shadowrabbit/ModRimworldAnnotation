using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000D95 RID: 3477
	public class WorkGiver_Refuel : WorkGiver_Scanner
	{
		// Token: 0x17000C22 RID: 3106
		// (get) Token: 0x06004F4A RID: 20298 RVA: 0x00037C88 File Offset: 0x00035E88
		public override ThingRequest PotentialWorkThingRequest
		{
			get
			{
				return ThingRequest.ForGroup(ThingRequestGroup.Refuelable);
			}
		}

		// Token: 0x17000C23 RID: 3107
		// (get) Token: 0x06004F4B RID: 20299 RVA: 0x0001B6B4 File Offset: 0x000198B4
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.Touch;
			}
		}

		// Token: 0x17000C24 RID: 3108
		// (get) Token: 0x06004F4C RID: 20300 RVA: 0x00037C91 File Offset: 0x00035E91
		public virtual JobDef JobStandard
		{
			get
			{
				return JobDefOf.Refuel;
			}
		}

		// Token: 0x17000C25 RID: 3109
		// (get) Token: 0x06004F4D RID: 20301 RVA: 0x00037C98 File Offset: 0x00035E98
		public virtual JobDef JobAtomic
		{
			get
			{
				return JobDefOf.RefuelAtomic;
			}
		}

		// Token: 0x06004F4E RID: 20302 RVA: 0x00037C9F File Offset: 0x00035E9F
		public virtual bool CanRefuelThing(Thing t)
		{
			return !(t is Building_Turret);
		}

		// Token: 0x06004F4F RID: 20303 RVA: 0x00037CAD File Offset: 0x00035EAD
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			return this.CanRefuelThing(t) && RefuelWorkGiverUtility.CanRefuel(pawn, t, forced);
		}

		// Token: 0x06004F50 RID: 20304 RVA: 0x00037CC2 File Offset: 0x00035EC2
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			return RefuelWorkGiverUtility.RefuelJob(pawn, t, forced, this.JobStandard, this.JobAtomic);
		}
	}
}
