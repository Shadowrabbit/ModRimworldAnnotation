using System;
using UnityEngine.SceneManagement;

namespace Verse
{
	// Token: 0x02000142 RID: 322
	public static class QuickStarter
	{
		// Token: 0x060008EF RID: 2287 RVA: 0x000297D4 File Offset: 0x000279D4
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

		// Token: 0x0400082A RID: 2090
		private static bool quickStarted;
	}
}
