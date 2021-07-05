using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000D4D RID: 3405
	public class WorkGiver_ConstructRemoveFloor : WorkGiver_ConstructAffectFloor
	{
		// Token: 0x17000BE2 RID: 3042
		// (get) Token: 0x06004DD7 RID: 19927 RVA: 0x000325C4 File Offset: 0x000307C4
		protected override DesignationDef DesDef
		{
			get
			{
				return DesignationDefOf.RemoveFloor;
			}
		}

		// Token: 0x06004DD8 RID: 19928 RVA: 0x00036FDB File Offset: 0x000351DB
		public override Job JobOnCell(Pawn pawn, IntVec3 c, bool forced = false)
		{
			return JobMaker.MakeJob(JobDefOf.RemoveFloor, c);
		}

		// Token: 0x06004DD9 RID: 19929 RVA: 0x00036FED File Offset: 0x000351ED
		public override bool HasJobOnCell(Pawn pawn, IntVec3 c, bool forced = false)
		{
			return base.HasJobOnCell(pawn, c, false) && pawn.Map.terrainGrid.CanRemoveTopLayerAt(c) && !WorkGiver_ConstructRemoveFloor.AnyBuildingBlockingFloorRemoval(c, pawn.Map);
		}

		// Token: 0x06004DDA RID: 19930 RVA: 0x001AF938 File Offset: 0x001ADB38
		public static bool AnyBuildingBlockingFloorRemoval(IntVec3 c, Map map)
		{
			if (!map.terrainGrid.CanRemoveTopLayerAt(c))
			{
				return false;
			}
			Building firstBuilding = c.GetFirstBuilding(map);
			return firstBuilding != null && firstBuilding.def.terrainAffordanceNeeded != null && !map.terrainGrid.UnderTerrainAt(c).affordances.Contains(firstBuilding.def.terrainAffordanceNeeded);
		}
	}
}
