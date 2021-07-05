using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200081B RID: 2075
	public abstract class WorkGiver_RemoveBuilding : WorkGiver_Scanner
	{
		// Token: 0x170009D7 RID: 2519
		// (get) Token: 0x06003738 RID: 14136
		protected abstract DesignationDef Designation { get; }

		// Token: 0x170009D8 RID: 2520
		// (get) Token: 0x06003739 RID: 14137
		protected abstract JobDef RemoveBuildingJob { get; }

		// Token: 0x0600373A RID: 14138 RVA: 0x001383D7 File Offset: 0x001365D7
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

		// Token: 0x0600373B RID: 14139 RVA: 0x001383EE File Offset: 0x001365EE
		public override bool ShouldSkip(Pawn pawn, bool forced = false)
		{
			return !pawn.Map.designationManager.AnySpawnedDesignationOfDef(this.Designation);
		}

		// Token: 0x170009D9 RID: 2521
		// (get) Token: 0x0600373C RID: 14140 RVA: 0x0009007E File Offset: 0x0008E27E
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.Touch;
			}
		}

		// Token: 0x0600373D RID: 14141 RVA: 0x00138409 File Offset: 0x00136609
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			return pawn.CanReserve(t, 1, -1, null, forced) && pawn.Map.designationManager.DesignationOn(t, this.Designation) != null;
		}

		// Token: 0x0600373E RID: 14142 RVA: 0x0013843B File Offset: 0x0013663B
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			return JobMaker.MakeJob(this.RemoveBuildingJob, t);
		}
	}
}
