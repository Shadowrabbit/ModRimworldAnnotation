using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020019B4 RID: 6580
	public static class SmoothSurfaceDesignatorUtility
	{
		// Token: 0x06009178 RID: 37240 RVA: 0x000617BD File Offset: 0x0005F9BD
		public static bool CanSmoothFloorUnder(Building b)
		{
			return b.def.Fillage != FillCategory.Full || b.def.passability != Traversability.Impassable;
		}

		// Token: 0x06009179 RID: 37241 RVA: 0x0029CE78 File Offset: 0x0029B078
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

		// Token: 0x0600917A RID: 37242 RVA: 0x0029CEFC File Offset: 0x0029B0FC
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
