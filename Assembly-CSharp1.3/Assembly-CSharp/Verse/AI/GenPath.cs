using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x020005FB RID: 1531
	public static class GenPath
	{
		// Token: 0x06002BF5 RID: 11253 RVA: 0x00105528 File Offset: 0x00103728
		public static TargetInfo ResolvePathMode(Pawn pawn, TargetInfo dest, ref PathEndMode peMode)
		{
			if (dest.HasThing && !dest.Thing.Spawned)
			{
				peMode = PathEndMode.Touch;
				return dest;
			}
			if (peMode == PathEndMode.InteractionCell)
			{
				if (!dest.HasThing)
				{
					Log.Error("Pathed to cell " + dest + " with PathEndMode.InteractionCell.");
				}
				peMode = PathEndMode.OnCell;
				return new TargetInfo(dest.Thing.InteractionCell, dest.Thing.Map, false);
			}
			if (peMode == PathEndMode.ClosestTouch)
			{
				peMode = GenPath.ResolveClosestTouchPathMode(pawn, dest.Map, dest.Cell);
			}
			return dest;
		}

		// Token: 0x06002BF6 RID: 11254 RVA: 0x001055B7 File Offset: 0x001037B7
		public static PathEndMode ResolveClosestTouchPathMode(Pawn pawn, Map map, IntVec3 target)
		{
			if (GenPath.ShouldNotEnterCell(pawn, map, target))
			{
				return PathEndMode.Touch;
			}
			return PathEndMode.OnCell;
		}

		// Token: 0x06002BF7 RID: 11255 RVA: 0x001055C8 File Offset: 0x001037C8
		private static bool ShouldNotEnterCell(Pawn pawn, Map map, IntVec3 dest)
		{
			if (map.pathing.For(pawn).pathGrid.PerceivedPathCostAt(dest) > 30)
			{
				return true;
			}
			if (!dest.Walkable(map))
			{
				return true;
			}
			if (pawn != null)
			{
				if (dest.IsForbidden(pawn))
				{
					return true;
				}
				Building edifice = dest.GetEdifice(map);
				if (edifice != null)
				{
					Building_Door building_Door = edifice as Building_Door;
					if (building_Door != null)
					{
						if (building_Door.IsForbidden(pawn))
						{
							return true;
						}
						if (!building_Door.PawnCanOpen(pawn))
						{
							return true;
						}
					}
				}
			}
			return false;
		}
	}
}
