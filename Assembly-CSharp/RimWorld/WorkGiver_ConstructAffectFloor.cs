using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000D4A RID: 3402
	public abstract class WorkGiver_ConstructAffectFloor : WorkGiver_Scanner
	{
		// Token: 0x17000BDD RID: 3037
		// (get) Token: 0x06004DC5 RID: 19909
		protected abstract DesignationDef DesDef { get; }

		// Token: 0x17000BDE RID: 3038
		// (get) Token: 0x06004DC6 RID: 19910 RVA: 0x0001B6B4 File Offset: 0x000198B4
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.Touch;
			}
		}

		// Token: 0x06004DC7 RID: 19911 RVA: 0x00036EFF File Offset: 0x000350FF
		public override IEnumerable<IntVec3> PotentialWorkCellsGlobal(Pawn pawn)
		{
			foreach (Designation designation in pawn.Map.designationManager.SpawnedDesignationsOfDef(this.DesDef))
			{
				yield return designation.target.Cell;
			}
			IEnumerator<Designation> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x06004DC8 RID: 19912 RVA: 0x00036F16 File Offset: 0x00035116
		public override bool ShouldSkip(Pawn pawn, bool forced = false)
		{
			return !pawn.Map.designationManager.AnySpawnedDesignationOfDef(this.DesDef);
		}

		// Token: 0x06004DC9 RID: 19913 RVA: 0x00036F31 File Offset: 0x00035131
		public override bool HasJobOnCell(Pawn pawn, IntVec3 c, bool forced = false)
		{
			return !c.IsForbidden(pawn) && pawn.Map.designationManager.DesignationAt(c, this.DesDef) != null && pawn.CanReserve(c, 1, -1, ReservationLayerDefOf.Floor, forced);
		}
	}
}
