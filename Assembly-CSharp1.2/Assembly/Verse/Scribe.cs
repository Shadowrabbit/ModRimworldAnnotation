using System;

namespace Verse
{
	// Token: 0x020004B5 RID: 1205
	public static class Scribe
	{
		// Token: 0x06001DF6 RID: 7670 RVA: 0x0001AB5D File Offset: 0x00018D5D
		public static void ForceStop()
		{
			Scribe.mode = LoadSaveMode.Inactive;
			Scribe.saver.ForceStop();
			Scribe.loader.ForceStop();
		}

		// Token: 0x06001DF7 RID: 7671 RVA: 0x000F91DC File Offset: 0x000F73DC
		public static bool EnterNode(string nodeName)
		{
			if (Scribe.mode == LoadSaveMode.Inactive)
			{
				return false;
			}
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				return Scribe.saver.EnterNode(nodeName);
			}
			return (Scribe.mode != LoadSaveMode.LoadingVars && Scribe.mode != LoadSaveMode.ResolvingCrossRefs && Scribe.mode != LoadSaveMode.PostLoadInit) || Scribe.loader.EnterNode(nodeName);
		}

		// Token: 0x06001DF8 RID: 7672 RVA: 0x0001AB79 File Offset: 0x00018D79
		public static void ExitNode()
		{
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				Scribe.saver.ExitNode();
			}
			if (Scribe.mode == LoadSaveMode.LoadingVars || Scribe.mode == LoadSaveMode.ResolvingCrossRefs || Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				Scribe.loader.ExitNode();
			}
		}

		// Token: 0x04001569 RID: 5481
		public static ScribeSaver saver = new ScribeSaver();

		// Token: 0x0400156A RID: 5482
		public static ScribeLoader loader = new ScribeLoader();

		// Token: 0x0400156B RID: 5483
		public static LoadSaveMode mode = LoadSaveMode.Inactive;
	}
}
