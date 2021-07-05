using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020012BD RID: 4797
	public static class SmoothSurfaceDesignatorUtility
	{
		// Token: 0x06007299 RID: 29337 RVA: 0x00263FB9 File Offset: 0x002621B9
		public static bool CanSmoothFloorUnder(Building b)
		{
			return b.def.Fillage != FillCategory.Full || b.def.passability != Traversability.Impassable;
		}

		// Token: 0x0600729A RID: 29338 RVA: 0x00263FDC File Offset: 0x002621DC
		public static void Notify_BuildingSpawned(Building b)
		{
			if (!SmoothSurfaceDesignatorUtility.CanSmoothFloorUnder(b))
			{
				foreach (IntVec3 c in b.OccupiedRect())
				{
					Designation designation = b.Map.designationManager.DesignationAt(c, DesignationDefOf.SmoothFloor);
					if (designation != null)
					{
						b.Map.designationManager.RemoveDesignation(designation);
					}
				}
			}
		}

		// Token: 0x0600729B RID: 29339 RVA: 0x00264060 File Offset: 0x00262260
		public static void Notify_BuildingDespawned(Building b, Map map)
		{
			foreach (IntVec3 c in b.OccupiedRect())
			{
				Designation designation = map.designationManager.DesignationAt(c, DesignationDefOf.SmoothWall);
				if (designation != null)
				{
					map.designationManager.RemoveDesignation(designation);
				}
			}
		}
	}
}
