using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000D8D RID: 3469
	public class WorkGiver_Open : WorkGiver_Scanner
	{
		// Token: 0x06004F1D RID: 20253 RVA: 0x00037AEA File Offset: 0x00035CEA
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

		// Token: 0x06004F1E RID: 20254 RVA: 0x00037AFA File Offset: 0x00035CFA
		public override bool ShouldSkip(Pawn pawn, bool forced = false)
		{
			return !pawn.Map.designationManager.AnySpawnedDesignationOfDef(DesignationDefOf.Open);
		}

		// Token: 0x17000C19 RID: 3097
		// (get) Token: 0x06004F1F RID: 20255 RVA: 0x0002EB44 File Offset: 0x0002CD44
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.ClosestTouch;
			}
		}

		// Token: 0x06004F20 RID: 20256 RVA: 0x00037B14 File Offset: 0x00035D14
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			return pawn.Map.designationManager.DesignationOn(t, DesignationDefOf.Open) != null && pawn.CanReserve(t, 1, -1, null, forced);
		}

		// Token: 0x06004F21 RID: 20257 RVA: 0x00037B45 File Offset: 0x00035D45
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			return JobMaker.MakeJob(JobDefOf.Open, t);
		}
	}
}
