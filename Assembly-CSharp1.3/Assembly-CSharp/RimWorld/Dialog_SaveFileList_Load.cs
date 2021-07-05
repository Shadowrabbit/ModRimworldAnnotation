using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001301 RID: 4865
	public class Dialog_SaveFileList_Load : Dialog_SaveFileList
	{
		// Token: 0x060074FE RID: 29950 RVA: 0x0027DCA2 File Offset: 0x0027BEA2
		public Dialog_SaveFileList_Load()
		{
			this.interactButLabel = "LoadGameButton".Translate();
		}

		// Token: 0x060074FF RID: 29951 RVA: 0x0027DCBF File Offset: 0x0027BEBF
		protected override void DoFileInteraction(string saveFileName)
		{
			GameDataSaveLoader.CheckVersionAndLoadGame(saveFileName);
		}
	}
}
