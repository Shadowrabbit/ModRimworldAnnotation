using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200081A RID: 2074
	public class WorkGiver_Deconstruct : WorkGiver_RemoveBuilding
	{
		// Token: 0x170009D5 RID: 2517
		// (get) Token: 0x06003734 RID: 14132 RVA: 0x0011EE02 File Offset: 0x0011D002
		protected override DesignationDef Designation
		{
			get
			{
				return DesignationDefOf.Deconstruct;
			}
		}

		// Token: 0x170009D6 RID: 2518
		// (get) Token: 0x06003735 RID: 14133 RVA: 0x00138390 File Offset: 0x00136590
		protected override JobDef RemoveBuildingJob
		{
			get
			{
				return JobDefOf.Deconstruct;
			}
		}

		// Token: 0x06003736 RID: 14134 RVA: 0x00138398 File Offset: 0x00136598
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Building building = t.GetInnerIfMinified() as Building;
			return building != null && building.DeconstructibleBy(pawn.Faction) && base.HasJobOnThing(pawn, t, forced);
		}
	}
}
