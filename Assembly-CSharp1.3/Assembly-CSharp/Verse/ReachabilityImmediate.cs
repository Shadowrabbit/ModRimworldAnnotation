using System;
using Verse.AI;

namespace Verse
{
	// Token: 0x020001FA RID: 506
	public static class ReachabilityImmediate
	{
		// Token: 0x06000E44 RID: 3652 RVA: 0x00050814 File Offset: 0x0004EA14
		public static bool CanReachImmediate(IntVec3 start, LocalTargetInfo target, Map map, PathEndMode peMode, Pawn pawn)
		{
			if (!target.IsValid)
			{
				return false;
			}
			target = (LocalTargetInfo)GenPath.ResolvePathMode(pawn, target.ToTargetInfo(map), ref peMode);
			if (target.HasThing)
			{
				Thing thing = target.Thing;
				if (!thing.Spawned)
				{
					if (pawn != null)
					{
						if (pawn.carryTracker.innerContainer.Contains(thing))
						{
							return true;
						}
						if (pawn.inventory.innerContainer.Contains(thing))
						{
							return true;
						}
						if (pawn.apparel != null && pawn.apparel.Contains(thing))
						{
							return true;
						}
						if (pawn.equipment != null && pawn.equipment.Contains(thing))
						{
							return true;
						}
					}
					return false;
				}
				if (thing.Map != map)
				{
					return false;
				}
			}
			if (!target.HasThing || (target.Thing.def.size.x == 1 && target.Thing.def.size.z == 1))
			{
				if (start == target.Cell)
				{
					return true;
				}
			}
			else if (start.IsInside(target.Thing))
			{
				return true;
			}
			return peMode == PathEndMode.Touch && TouchPathEndModeUtility.IsAdjacentOrInsideAndAllowedToTouch(start, target, map.pathing.For(pawn));
		}

		// Token: 0x06000E45 RID: 3653 RVA: 0x0005094B File Offset: 0x0004EB4B
		public static bool CanReachImmediate(this Pawn pawn, LocalTargetInfo target, PathEndMode peMode)
		{
			return pawn.Spawned && ReachabilityImmediate.CanReachImmediate(pawn.Position, target, pawn.Map, peMode, pawn);
		}

		// Token: 0x06000E46 RID: 3654 RVA: 0x0005096B File Offset: 0x0004EB6B
		public static bool CanReachImmediateNonLocal(this Pawn pawn, TargetInfo target, PathEndMode peMode)
		{
			return pawn.Spawned && (target.Map == null || target.Map == pawn.Map) && pawn.CanReachImmediate((LocalTargetInfo)target, peMode);
		}

		// Token: 0x06000E47 RID: 3655 RVA: 0x000509A0 File Offset: 0x0004EBA0
		public static bool CanReachImmediate(IntVec3 start, CellRect rect, Map map, PathEndMode peMode, Pawn pawn)
		{
			IntVec3 c = rect.ClosestCellTo(start);
			return ReachabilityImmediate.CanReachImmediate(start, c, map, peMode, pawn);
		}
	}
}
