using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000D58 RID: 3416
	public abstract class WorkGiver_RemoveBuilding : WorkGiver_Scanner
	{
		// Token: 0x17000BEC RID: 3052
		// (get) Token: 0x06004E0E RID: 19982
		protected abstract DesignationDef Designation { get; }

		// Token: 0x17000BED RID: 3053
		// (get) Token: 0x06004E0F RID: 19983
		protected abstract JobDef RemoveBuildingJob { get; }

		// Token: 0x06004E10 RID: 19984 RVA: 0x00037227 File Offset: 0x00035427
		public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
		{
			foreach (Designation designation in pawn.Map.designationManager.SpawnedDesignationsOfDef(this.Designation))
			{
				yield return designation.target.Thing;
			}
			IEnumerator<Designation> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x06004E11 RID: 19985 RVA: 0x0003723E File Offset: 0x0003543E
		public override bool ShouldSkip(Pawn pawn, bool forced = false)
		{
			return !pawn.Map.designationManager.AnySpawnedDesignationOfDef(this.Designation);
		}

		// Token: 0x17000BEE RID: 3054
		// (get) Token: 0x06004E12 RID: 19986 RVA: 0x0001B6B4 File Offset: 0x000198B4
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.Touch;
			}
		}

		// Token: 0x06004E13 RID: 19987 RVA: 0x00037259 File Offset: 0x00035459
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			return pawn.CanReserve(t, 1, -1, null, forced) && pawn.Map.designationManager.DesignationOn(t, this.Designation) != null;
		}

		// Token: 0x06004E14 RID: 19988 RVA: 0x0003728B File Offset: 0x0003548B
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			return JobMaker.MakeJob(this.RemoveBuildingJob, t);
		}
	}
}
