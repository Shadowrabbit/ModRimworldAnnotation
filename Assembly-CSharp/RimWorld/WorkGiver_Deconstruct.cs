using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000D57 RID: 3415
	public class WorkGiver_Deconstruct : WorkGiver_RemoveBuilding
	{
		// Token: 0x17000BEA RID: 3050
		// (get) Token: 0x06004E0A RID: 19978 RVA: 0x0003283A File Offset: 0x00030A3A
		protected override DesignationDef Designation
		{
			get
			{
				return DesignationDefOf.Deconstruct;
			}
		}

		// Token: 0x17000BEB RID: 3051
		// (get) Token: 0x06004E0B RID: 19979 RVA: 0x00037218 File Offset: 0x00035418
		protected override JobDef RemoveBuildingJob
		{
			get
			{
				return JobDefOf.Deconstruct;
			}
		}

		// Token: 0x06004E0C RID: 19980 RVA: 0x001B0484 File Offset: 0x001AE684
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Building building = t.GetInnerIfMinified() as Building;
			return building != null && building.DeconstructibleBy(pawn.Faction) && base.HasJobOnThing(pawn, t, forced);
		}
	}
}
