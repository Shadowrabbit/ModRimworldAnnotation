using System;
using System.IO;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B40 RID: 6976
	public class MainTabWindow_Menu : MainTabWindow
	{
		// Token: 0x17001847 RID: 6215
		// (get) Token: 0x060099A9 RID: 39337 RVA: 0x000666D6 File Offset: 0x000648D6
		public override Vector2 RequestedTabSize
		{
			get
			{
				return new Vector2(450f, 390f);
			}
		}

		// Token: 0x17001848 RID: 6216
		// (get) Token: 0x060099AA RID: 39338 RVA: 0x0000A2A7 File Offset: 0x000084A7
		public override MainTabWindowAnchor Anchor
		{
			get
			{
				return MainTabWindowAnchor.Right;
			}
		}

		// Token: 0x060099AB RID: 39339 RVA: 0x000666E7 File Offset: 0x000648E7
		public MainTabWindow_Menu()
		{
			this.forcePause = true;
		}

		// Token: 0x060099AC RID: 39340 RVA: 0x000666F6 File Offset: 0x000648F6
		public override void PreOpen()
		{
			base.PreOpen();
			PlayerKnowledgeDatabase.Save();
			ShipCountdown.CancelCountdown();
			this.anyGameFiles = GenFilePaths.AllSavedGameFiles.Any<FileInfo>();
		}

		// Token: 0x060099AD RID: 39341 RVA: 0x00066718 File Offset: 0x00064918
		public override void ExtraOnGUI()
		{
			base.ExtraOnGUI();
			VersionControl.DrawInfoInCorner();
		}

		// Token: 0x060099AE RID: 39342 RVA: 0x00066725 File Offset: 0x00064925
		public override void DoWindowContents(Rect rect)
		{
			base.DoWindowContents(rect);
			MainMenuDrawer.DoMainMenuControls(rect, this.anyGameFiles);
		}

		// Token: 0x04006229 RID: 25129
		private bool anyGameFiles;
	}
}
