using System;
using UnityEngine.SceneManagement;

namespace Verse
{
	// Token: 0x020001E7 RID: 487
	public static class QuickStarter
	{
		// Token: 0x06000CAE RID: 3246 RVA: 0x0000FC8B File Offset: 0x0000DE8B
		public static bool CheckQuickStart()
		{
			if (GenCommandLine.CommandLineArgPassed("quicktest") && !QuickStarter.quickStarted && GenScene.InEntryScene)
			{
				QuickStarter.quickStarted = true;
				SceneManager.LoadScene("Play");
				return true;
			}
			return false;
		}

		// Token: 0x04000AEF RID: 2799
		private static bool quickStarted;
	}
}
