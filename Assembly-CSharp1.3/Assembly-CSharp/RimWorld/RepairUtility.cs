using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200085B RID: 2139
	public static class RepairUtility
	{
		// Token: 0x06003880 RID: 14464 RVA: 0x0013D5C0 File Offset: 0x0013B7C0
		public static bool PawnCanRepairEver(Pawn pawn, Thing t)
		{
			Building building;
			return (building = (t as Building)) != null && t.def.useHitPoints && building.def.building.repairable && t.Faction == pawn.Faction;
		}

		// Token: 0x06003881 RID: 14465 RVA: 0x0013D60D File Offset: 0x0013B80D
		public static bool PawnCanRepairNow(Pawn pawn, Thing t)
		{
			return RepairUtility.PawnCanRepairEver(pawn, t) && pawn.Map.listerBuildingsRepairable.Contains(pawn.Faction, (Building)t) && t.HitPoints != t.MaxHitPoints;
		}
	}
}
