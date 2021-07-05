using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200150D RID: 5389
	public class WealthUtility
	{
		// Token: 0x170015DE RID: 5598
		// (get) Token: 0x06008061 RID: 32865 RVA: 0x002D7CEC File Offset: 0x002D5EEC
		public static float PlayerWealth
		{
			get
			{
				List<Map> maps = Find.Maps;
				float num = 0f;
				for (int i = 0; i < maps.Count; i++)
				{
					if (maps[i].IsPlayerHome)
					{
						num += maps[i].wealthWatcher.WealthTotal;
					}
				}
				return num;
			}
		}
	}
}
