using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x0200028D RID: 653
	public static class MapMeshFlagUtility
	{
		// Token: 0x060010F9 RID: 4345 RVA: 0x000BCD54 File Offset: 0x000BAF54
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

		// Token: 0x04000DDA RID: 3546
		public static List<MapMeshFlag> allFlags = new List<MapMeshFlag>();
	}
}
