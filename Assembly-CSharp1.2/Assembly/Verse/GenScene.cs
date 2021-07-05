using System;
using UnityEngine.SceneManagement;
using Verse.Profile;

namespace Verse
{
	// Token: 0x020001E5 RID: 485
	public static class GenScene
	{
		// Token: 0x1700028D RID: 653
		// (get) Token: 0x06000CA8 RID: 3240 RVA: 0x000A5440 File Offset: 0x000A3640
		public static bool InEntryScene
		{
			get
			{
				return SceneManager.GetActiveScene().name == "Entry";
			}
		}

		// Token: 0x1700028E RID: 654
		// (get) Token: 0x06000CA9 RID: 3241 RVA: 0x000A5464 File Offset: 0x000A3664
		public static bool InPlayScene
		{
			get
			{
				return SceneManager.GetActiveScene().name == "Play";
			}
		}

		// Token: 0x06000CAA RID: 3242 RVA: 0x0000FC3A File Offset: 0x0000DE3A
		public static void GoToMainMenu()
		{
			LongEventHandler.ClearQueuedEvents();
			LongEventHandler.QueueLongEvent(delegate()
			{
				MemoryUtility.ClearAllMapsAndWorld();
				Current.Game = null;
			}, "Entry", "LoadingLongEvent", true, null, false);
		}

		// Token: 0x04000AEB RID: 2795
		public const string EntrySceneName = "Entry";

		// Token: 0x04000AEC RID: 2796
		public const string PlaySceneName = "Play";
	}
}
