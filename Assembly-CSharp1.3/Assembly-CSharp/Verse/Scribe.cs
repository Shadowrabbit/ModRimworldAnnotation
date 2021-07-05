using System;

namespace Verse
{
	// Token: 0x0200032E RID: 814
	public static class Scribe
	{
		// Token: 0x06001729 RID: 5929 RVA: 0x0008932B File Offset: 0x0008752B
		public static void ForceStop()
		{
			Scribe.mode = LoadSaveMode.Inactive;
			Scribe.saver.ForceStop();
			Scribe.loader.ForceStop();
		}

		// Token: 0x0600172A RID: 5930 RVA: 0x00089348 File Offset: 0x00087548
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

		// Token: 0x0600172B RID: 5931 RVA: 0x00089397 File Offset: 0x00087597
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

		// Token: 0x04001018 RID: 4120
		public static ScribeSaver saver = new ScribeSaver();

		// Token: 0x04001019 RID: 4121
		public static ScribeLoader loader = new ScribeLoader();

		// Token: 0x0400101A RID: 4122
		public static LoadSaveMode mode = LoadSaveMode.Inactive;
	}
}
