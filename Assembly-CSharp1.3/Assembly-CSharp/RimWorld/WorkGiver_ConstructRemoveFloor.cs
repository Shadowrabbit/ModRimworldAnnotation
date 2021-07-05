using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000813 RID: 2067
	public class WorkGiver_ConstructRemoveFloor : WorkGiver_ConstructAffectFloor
	{
		// Token: 0x170009CF RID: 2511
		// (get) Token: 0x0600370E RID: 14094 RVA: 0x0011EB43 File Offset: 0x0011CD43
		protected override DesignationDef DesDef
		{
			get
			{
				return DesignationDefOf.RemoveFloor;
			}
		}

		// Token: 0x0600370F RID: 14095 RVA: 0x00137791 File Offset: 0x00135991
		public override Job JobOnCell(Pawn pawn, IntVec3 c, bool forced = false)
		{
			return JobMaker.MakeJob(JobDefOf.RemoveFloor, c);
		}

		// Token: 0x06003710 RID: 14096 RVA: 0x001377A3 File Offset: 0x001359A3
		public override bool HasJobOnCell(Pawn pawn, IntVec3 c, bool forced = false)
		{
			return base.HasJobOnCell(pawn, c, false) && pawn.Map.terrainGrid.CanRemoveTopLayerAt(c) && !WorkGiver_ConstructRemoveFloor.AnyBuildingBlockingFloorRemoval(c, pawn.Map);
		}

		// Token: 0x06003711 RID: 14097 RVA: 0x001377D8 File Offset: 0x001359D8
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
