using System;
using Verse.AI;

namespace Verse
{
	// Token: 0x020002CC RID: 716
	public static class ReachabilityWithinRegion
	{
		// Token: 0x0600121B RID: 4635 RVA: 0x000C4DF8 File Offset: 0x000C2FF8
		public static bool ThingFromRegionListerReachable(Thing thing, Region region, PathEndMode peMode, Pawn traveler)
		{
			Map map = region.Map;
			if (peMode == PathEndMode.ClosestTouch)
			{
				peMode = GenPath.ResolveClosestTouchPathMode(traveler, map, thing.Position);
			}
			switch (peMode)
			{
			case PathEndMode.None:
				return false;
			case PathEndMode.OnCell:
				if (thing.def.size.x != 1 || thing.def.size.z != 1)
				{
					using (CellRect.Enumerator enumerator = thing.OccupiedRect().GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							if (enumerator.Current.GetRegion(map, RegionType.Set_Passable) == region)
							{
								return true;
							}
						}
					}
					return false;
				}
				if (thing.Position.GetRegion(map, RegionType.Set_Passable) == region)
				{
					return true;
				}
				return false;
			case PathEndMode.Touch:
				return true;
			case PathEndMode.InteractionCell:
				return thing.InteractionCell.GetRegion(map, RegionType.Set_Passable) == region;
			}
			Log.Error("Unsupported PathEndMode: " + peMode, false);
			return false;
		}
	}
}
