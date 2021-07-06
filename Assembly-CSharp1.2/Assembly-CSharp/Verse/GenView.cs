using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020004F4 RID: 1268
	public static class GenView
	{
		// Token: 0x06001F8F RID: 8079 RVA: 0x0001BC78 File Offset: 0x00019E78
		public static bool ShouldSpawnMotesAt(this Vector3 loc, Map map)
		{
			return loc.ToIntVec3().ShouldSpawnMotesAt(map);
		}

		// Token: 0x06001F90 RID: 8080 RVA: 0x001006A0 File Offset: 0x000FE8A0
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

		// Token: 0x06001F91 RID: 8081 RVA: 0x0001BC86 File Offset: 0x00019E86
		public static Vector3 RandomPositionOnOrNearScreen()
		{
			GenView.viewRect = Find.CameraDriver.CurrentViewRect;
			GenView.viewRect = GenView.viewRect.ExpandedBy(5);
			GenView.viewRect.ClipInsideMap(Find.CurrentMap);
			return GenView.viewRect.RandomVector3;
		}

		// Token: 0x0400162B RID: 5675
		private static CellRect viewRect;

		// Token: 0x0400162C RID: 5676
		private const int ViewRectMargin = 5;
	}
}
