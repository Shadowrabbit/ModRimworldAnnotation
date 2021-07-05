using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000A8D RID: 2701
	public static class MainTabWindowUtility
	{
		// Token: 0x06004077 RID: 16503 RVA: 0x0015CB6C File Offset: 0x0015AD6C
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
