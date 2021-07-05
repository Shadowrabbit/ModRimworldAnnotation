using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000811 RID: 2065
	public abstract class WorkGiver_ConstructAffectFloor : WorkGiver_Scanner
	{
		// Token: 0x170009CC RID: 2508
		// (get) Token: 0x06003705 RID: 14085
		protected abstract DesignationDef DesDef { get; }

		// Token: 0x170009CD RID: 2509
		// (get) Token: 0x06003706 RID: 14086 RVA: 0x0009007E File Offset: 0x0008E27E
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.Touch;
			}
		}

		// Token: 0x06003707 RID: 14087 RVA: 0x00137708 File Offset: 0x00135908
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

		// Token: 0x06003708 RID: 14088 RVA: 0x0013771F File Offset: 0x0013591F
		public override bool ShouldSkip(Pawn pawn, bool forced = false)
		{
			return !pawn.Map.designationManager.AnySpawnedDesignationOfDef(this.DesDef);
		}

		// Token: 0x06003709 RID: 14089 RVA: 0x0013773A File Offset: 0x0013593A
		public override bool HasJobOnCell(Pawn pawn, IntVec3 c, bool forced = false)
		{
			return !c.IsForbidden(pawn) && pawn.Map.designationManager.DesignationAt(c, this.DesDef) != null && pawn.CanReserve(c, 1, -1, ReservationLayerDefOf.Floor, forced);
		}
	}
}
