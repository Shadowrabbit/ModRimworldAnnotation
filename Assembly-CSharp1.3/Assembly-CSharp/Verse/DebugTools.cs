using System;

namespace Verse
{
	// Token: 0x020003B6 RID: 950
	public static class DebugTools
	{
		// Token: 0x06001D70 RID: 7536 RVA: 0x000B7ACB File Offset: 0x000B5CCB
		public static void DebugToolsOnGUI()
		{
			if (DebugTools.curTool != null)
			{
				DebugTools.curTool.DebugToolOnGUI();
			}
		}

		// Token: 0x040011A6 RID: 4518
		public static DebugTool curTool;
	}
}
