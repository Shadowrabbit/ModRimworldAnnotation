using System;
using UnityEngine.SceneManagement;
using Verse.Profile;

namespace Verse
{
	// Token: 0x02000141 RID: 321
	public static class GenScene
	{
		// Token: 0x170001E9 RID: 489
		// (get) Token: 0x060008EC RID: 2284 RVA: 0x00029754 File Offset: 0x00027954
		public static bool InEntryScene
		{
			get
			{
				return SceneManager.GetActiveScene().name == "Entry";
			}
		}

		// Token: 0x170001EA RID: 490
		// (get) Token: 0x060008ED RID: 2285 RVA: 0x00029778 File Offset: 0x00027978
		public static bool InPlayScene
		{
			get
			{
				return SceneManager.GetActiveScene().name == "Play";
			}
		}

		// Token: 0x060008EE RID: 2286 RVA: 0x0002979C File Offset: 0x0002799C
		public static void GoToMainMenu()
		{
			LongEventHandler.ClearQueuedEvents();
			LongEventHandler.QueueLongEvent(delegate()
			{
				MemoryUtility.ClearAllMapsAndWorld();
				Current.Game = null;
			}, "Entry", "LoadingLongEvent", true, null, false);
		}

		// Token: 0x04000828 RID: 2088
		public const string EntrySceneName = "Entry";

		// Token: 0x04000829 RID: 2089
		public const string PlaySceneName = "Play";
	}
}
