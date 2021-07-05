using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x0200131E RID: 4894
	public static class PageUtility
	{
		// Token: 0x06007627 RID: 30247 RVA: 0x0028D5D8 File Offset: 0x0028B7D8
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

		// Token: 0x06007628 RID: 30248 RVA: 0x0028D647 File Offset: 0x0028B847
		public static void InitGameStart()
		{
			LongEventHandler.QueueLongEvent(delegate()
			{
				Find.GameInitData.PrepForMapGen();
				Find.Scenario.PreMapGenerate();
			}, "Play", "GeneratingMap", true, null, true);
		}
	}
}
