using System;
using System.Collections.Generic;

namespace Verse.AI.Group
{
	// Token: 0x02000676 RID: 1654
	public static class LordUtility
	{
		// Token: 0x06002ED8 RID: 11992 RVA: 0x00117338 File Offset: 0x00115538
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

		// Token: 0x06002ED9 RID: 11993 RVA: 0x00117378 File Offset: 0x00115578
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
