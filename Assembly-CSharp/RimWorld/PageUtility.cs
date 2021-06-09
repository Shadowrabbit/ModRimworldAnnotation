using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02001A8F RID: 6799
	public static class PageUtility
	{
		// Token: 0x0600962C RID: 38444 RVA: 0x002BA07C File Offset: 0x002B827C
		public static Page StitchedPages(IEnumerable<Page> pages)
		{
			List<Page> list = pages.ToList<Page>();
			if (list.Count == 0)
			{
				return null;
			}
			for (int i = 0; i < list.Count; i++)
			{
				if (i > 0)
				{
					list[i].prev = list[i - 1];
				}
				if (i < list.Count - 1)
				{
					list[i].next = list[i + 1];
				}
			}
			return list[0];
		}

		// Token: 0x0600962D RID: 38445 RVA: 0x000643CB File Offset: 0x000625CB
		public static void InitGameStart()
		{
			LongEventHandler.QueueLongEvent(delegate()
			{
				Find.GameInitData.PrepForMapGen();
				Find.GameInitData.startedFromEntry = true;
				Find.Scenario.PreMapGenerate();
			}, "Play", "GeneratingMap", true, null, true);
		}
	}
}
