using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001A19 RID: 6681
	public class Dialog_SaveFileList_Load : Dialog_SaveFileList
	{
		// Token: 0x060093A2 RID: 37794 RVA: 0x00062E59 File Offset: 0x00061059
		public Dialog_SaveFileList_Load()
		{
			this.interactButLabel = "LoadGameButton".Translate();
		}

		// Token: 0x060093A3 RID: 37795 RVA: 0x00062E76 File Offset: 0x00061076
		protected override void DoFileInteraction(string saveFileName)
		{
			GameDataSaveLoader.CheckVersionAndLoadGame(saveFileName);
		}
	}
}
