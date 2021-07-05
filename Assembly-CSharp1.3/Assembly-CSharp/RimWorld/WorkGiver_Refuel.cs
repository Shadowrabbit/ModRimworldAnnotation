using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000857 RID: 2135
	public class WorkGiver_Refuel : WorkGiver_Scanner
	{
		// Token: 0x17000A0A RID: 2570
		// (get) Token: 0x06003867 RID: 14439 RVA: 0x0013D0EB File Offset: 0x0013B2EB
		public override ThingRequest PotentialWorkThingRequest
		{
			get
			{
				return ThingRequest.ForGroup(ThingRequestGroup.Refuelable);
			}
		}

		// Token: 0x17000A0B RID: 2571
		// (get) Token: 0x06003868 RID: 14440 RVA: 0x0009007E File Offset: 0x0008E27E
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.Touch;
			}
		}

		// Token: 0x17000A0C RID: 2572
		// (get) Token: 0x06003869 RID: 14441 RVA: 0x0013D0F4 File Offset: 0x0013B2F4
		public virtual JobDef JobStandard
		{
			get
			{
				return JobDefOf.Refuel;
			}
		}

		// Token: 0x17000A0D RID: 2573
		// (get) Token: 0x0600386A RID: 14442 RVA: 0x0013D0FB File Offset: 0x0013B2FB
		public virtual JobDef JobAtomic
		{
			get
			{
				return JobDefOf.RefuelAtomic;
			}
		}

		// Token: 0x0600386B RID: 14443 RVA: 0x0013D102 File Offset: 0x0013B302
		public virtual bool CanRefuelThing(Thing t)
		{
			return !(t is Building_Turret);
		}

		// Token: 0x0600386C RID: 14444 RVA: 0x0013D110 File Offset: 0x0013B310
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			return this.CanRefuelThing(t) && RefuelWorkGiverUtility.CanRefuel(pawn, t, forced);
		}

		// Token: 0x0600386D RID: 14445 RVA: 0x0013D125 File Offset: 0x0013B325
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			return RefuelWorkGiverUtility.RefuelJob(pawn, t, forced, this.JobStandard, this.JobAtomic);
		}
	}
}
