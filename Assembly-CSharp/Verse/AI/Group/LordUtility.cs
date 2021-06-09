using System;
using System.Collections.Generic;

namespace Verse.AI.Group
{
	// Token: 0x02000ADB RID: 2779
	public static class LordUtility
	{
		// Token: 0x060041B5 RID: 16821 RVA: 0x001882D0 File Offset: 0x001864D0
		public static Lord GetLord(this Pawn p)
		{
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				Lord lord = maps[i].lordManager.LordOf(p);
				if (lord != null)
				{
					return lord;
				}
			}
			return null;
		}

		// Token: 0x060041B6 RID: 16822 RVA: 0x00188310 File Offset: 0x00186510
		public static Lord GetLord(this Building b)
		{
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				Lord lord = maps[i].lordManager.LordOf(b);
				if (lord != null)
				{
					return lord;
				}
			}
			return null;
		}
	}
}
