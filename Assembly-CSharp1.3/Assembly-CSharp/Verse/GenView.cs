using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000364 RID: 868
	public static class GenView
	{
		// Token: 0x06001896 RID: 6294 RVA: 0x00091621 File Offset: 0x0008F821
		public static bool ShouldSpawnMotesAt(this Vector3 loc, Map map)
		{
			return loc.ToIntVec3().ShouldSpawnMotesAt(map);
		}

		// Token: 0x06001897 RID: 6295 RVA: 0x00091630 File Offset: 0x0008F830
		public static bool ShouldSpawnMotesAt(this IntVec3 loc, Map map)
		{
			if (map != Find.CurrentMap)
			{
				return false;
			}
			if (!loc.InBounds(map))
			{
				return false;
			}
			GenView.viewRect = Find.CameraDriver.CurrentViewRect;
			GenView.viewRect = GenView.viewRect.ExpandedBy(5);
			return GenView.viewRect.Contains(loc);
		}

		// Token: 0x06001898 RID: 6296 RVA: 0x0009167C File Offset: 0x0008F87C
		public static Vector3 RandomPositionOnOrNearScreen()
		{
			GenView.viewRect = Find.CameraDriver.CurrentViewRect;
			GenView.viewRect = GenView.viewRect.ExpandedBy(5);
			GenView.viewRect.ClipInsideMap(Find.CurrentMap);
			return GenView.viewRect.RandomVector3;
		}

		// Token: 0x040010AD RID: 4269
		private static CellRect viewRect;

		// Token: 0x040010AE RID: 4270
		private const int ViewRectMargin = 5;
	}
}
