using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x020001CC RID: 460
	public static class MapMeshFlagUtility
	{
		// Token: 0x06000D49 RID: 3401 RVA: 0x00047A98 File Offset: 0x00045C98
		static MapMeshFlagUtility()
		{
			foreach (object obj in Enum.GetValues(typeof(MapMeshFlag)))
			{
				MapMeshFlag mapMeshFlag = (MapMeshFlag)obj;
				if (mapMeshFlag != MapMeshFlag.None)
				{
					MapMeshFlagUtility.allFlags.Add(mapMeshFlag);
				}
			}
		}

		// Token: 0x04000AFC RID: 2812
		public static List<MapMeshFlag> allFlags = new List<MapMeshFlag>();
	}
}
