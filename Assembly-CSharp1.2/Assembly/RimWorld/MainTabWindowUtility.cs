using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000FB7 RID: 4023
	public static class MainTabWindowUtility
	{
		// Token: 0x060057FF RID: 22527 RVA: 0x001CEF60 File Offset: 0x001CD160
		public static void NotifyAllPawnTables_PawnsChanged()
		{
			if (Find.WindowStack == null)
			{
				return;
			}
			WindowStack windowStack = Find.WindowStack;
			for (int i = 0; i < windowStack.Count; i++)
			{
				MainTabWindow_PawnTable mainTabWindow_PawnTable = windowStack[i] as MainTabWindow_PawnTable;
				if (mainTabWindow_PawnTable != null)
				{
					mainTabWindow_PawnTable.Notify_PawnsChanged();
				}
			}
		}
	}
}
