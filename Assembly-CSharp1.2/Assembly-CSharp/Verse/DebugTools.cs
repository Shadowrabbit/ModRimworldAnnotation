using System;

namespace Verse
{
	// Token: 0x020006A4 RID: 1700
	public static class DebugTools
	{
		// Token: 0x06002C53 RID: 11347 RVA: 0x000233D9 File Offset: 0x000215D9
		public static void DebugToolsOnGUI()
		{
			if (DebugTools.curTool != null)
			{
				DebugTools.curTool.DebugToolOnGUI();
			}
		}

		// Token: 0x04001DF7 RID: 7671
		public static DebugTool curTool;
	}
}
