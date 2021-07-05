using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200084E RID: 2126
	public class WorkGiver_Open : WorkGiver_Scanner
	{
		// Token: 0x0600383D RID: 14397 RVA: 0x0013CA36 File Offset: 0x0013AC36
		public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
		{
			foreach (Designation designation in pawn.Map.designationManager.SpawnedDesignationsOfDef(DesignationDefOf.Open))
			{
				yield return designation.target.Thing;
			}
			IEnumerator<Designation> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x0600383E RID: 14398 RVA: 0x0013CA46 File Offset: 0x0013AC46
		public override bool ShouldSkip(Pawn pawn, bool forced = false)
		{
			return !pawn.Map.designationManager.AnySpawnedDesignationOfDef(DesignationDefOf.Open);
		}

		// Token: 0x17000A04 RID: 2564
		// (get) Token: 0x0600383F RID: 14399 RVA: 0x00034716 File Offset: 0x00032916
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.ClosestTouch;
			}
		}

		// Token: 0x06003840 RID: 14400 RVA: 0x0013CA60 File Offset: 0x0013AC60
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			return pawn.Map.designationManager.DesignationOn(t, DesignationDefOf.Open) != null && pawn.CanReserve(t, 1, -1, null, forced);
		}

		// Token: 0x06003841 RID: 14401 RVA: 0x0013CA91 File Offset: 0x0013AC91
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			return JobMaker.MakeJob(JobDefOf.Open, t);
		}
	}
}
