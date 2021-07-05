using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200135E RID: 4958
	internal static class SelectorUtility
	{
		// Token: 0x0600783D RID: 30781 RVA: 0x002A6554 File Offset: 0x002A4754
		public static void SortInColonistBarOrder(List<Thing> things)
		{
			SelectorUtility.tmp_thingsToSort.Clear();
			SelectorUtility.tmp_thingsToSort.AddRange(things);
			things.Clear();
			foreach (Pawn item in Find.ColonistBar.GetColonistsInOrder())
			{
				int num = SelectorUtility.tmp_thingsToSort.IndexOf(item);
				if (num != -1)
				{
					things.Add(item);
					SelectorUtility.tmp_thingsToSort.RemoveAt(num);
				}
			}
			things.AddRange(SelectorUtility.tmp_thingsToSort);
			SelectorUtility.tmp_thingsToSort.Clear();
		}

		// Token: 0x040042D7 RID: 17111
		private static List<Thing> tmp_thingsToSort = new List<Thing>();
	}
}
