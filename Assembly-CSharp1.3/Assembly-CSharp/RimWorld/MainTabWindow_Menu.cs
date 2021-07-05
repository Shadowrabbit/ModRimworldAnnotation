using System;
using System.IO;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200136C RID: 4972
	public class MainTabWindow_Menu : MainTabWindow
	{
		// Token: 0x17001545 RID: 5445
		// (get) Token: 0x060078AD RID: 30893 RVA: 0x002A8DF7 File Offset: 0x002A6FF7
		public override Vector2 RequestedTabSize
		{
			get
			{
				return new Vector2(450f, 390f);
			}
		}

		// Token: 0x17001546 RID: 5446
		// (get) Token: 0x060078AE RID: 30894 RVA: 0x000126F5 File Offset: 0x000108F5
		public override MainTabWindowAnchor Anchor
		{
			get
			{
				return MainTabWindowAnchor.Right;
			}
		}

		// Token: 0x060078AF RID: 30895 RVA: 0x002A8E08 File Offset: 0x002A7008
		public MainTabWindow_Menu()
		{
			this.forcePause = true;
			this.layer = WindowLayer.Super;
		}

		// Token: 0x060078B0 RID: 30896 RVA: 0x002A8E1E File Offset: 0x002A701E
		public override void PreOpen()
		{
			base.PreOpen();
			PlayerKnowledgeDatabase.Save();
			ShipCountdown.CancelCountdown();
			if (ModsConfig.IdeologyActive)
			{
				ArchonexusCountdown.CancelCountdown();
			}
			this.anyGameFiles = GenFilePaths.AllSavedGameFiles.Any<FileInfo>();
		}

		// Token: 0x060078B1 RID: 30897 RVA: 0x002A8E4C File Offset: 0x002A704C
		public override void ExtraOnGUI()
		{
			base.ExtraOnGUI();
			VersionControl.DrawInfoInCorner();
		}

		// Token: 0x060078B2 RID: 30898 RVA: 0x002A8E59 File Offset: 0x002A7059
		public override void DoWindowContents(Rect rect)
		{
			MainMenuDrawer.DoMainMenuControls(rect, this.anyGameFiles);
		}

		// Token: 0x04004311 RID: 17169
		private bool anyGameFiles;
	}
}
