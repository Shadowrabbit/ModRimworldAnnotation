using System;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;
using Verse.Steam;

namespace Verse
{
	// Token: 0x02000704 RID: 1796
	public class UIRoot_Entry : UIRoot
	{
		// Token: 0x170006CF RID: 1743
		// (get) Token: 0x06002D86 RID: 11654 RVA: 0x00134068 File Offset: 0x00132268
		private bool ShouldDoMainMenu
		{
			get
			{
				if (LongEventHandler.AnyEventNowOrWaiting)
				{
					return false;
				}
				for (int i = 0; i < Find.WindowStack.Count; i++)
				{
					if (this.windows[i].layer == WindowLayer.Dialog && !Find.WindowStack[i].IsDebug)
					{
						return false;
					}
				}
				return true;
			}
		}

		// Token: 0x06002D87 RID: 11655 RVA: 0x001340BC File Offset: 0x001322BC
		public override void Init()
		{
			base.Init();
			UIMenuBackgroundManager.background = new UI_BackgroundMain();
			MainMenuDrawer.Init();
			QuickStarter.CheckQuickStart();
			VersionUpdateDialogMaker.CreateVersionUpdateDialogIfNecessary();
			if (!SteamManager.Initialized)
			{
				Dialog_MessageBox window = new Dialog_MessageBox("SteamClientMissing".Translate(), "Quit".Translate(), delegate()
				{
					Application.Quit();
				}, "Ignore".Translate(), null, null, false, null, null);
				Find.WindowStack.Add(window);
			}
		}

		// Token: 0x06002D88 RID: 11656 RVA: 0x00134158 File Offset: 0x00132358
		public override void UIRootOnGUI()
		{
			base.UIRootOnGUI();
			if (Find.World != null)
			{
				Find.World.UI.WorldInterfaceOnGUI();
			}
			this.DoMainMenu();
			if (Current.Game != null)
			{
				Find.Tutor.TutorOnGUI();
			}
			ReorderableWidget.ReorderableWidgetOnGUI_BeforeWindowStack();
			this.windows.WindowStackOnGUI();
			ReorderableWidget.ReorderableWidgetOnGUI_AfterWindowStack();
			Widgets.WidgetsOnGUI();
			if (Find.World != null)
			{
				Find.World.UI.HandleLowPriorityInput();
			}
		}

		// Token: 0x06002D89 RID: 11657 RVA: 0x00023E3E File Offset: 0x0002203E
		public override void UIRootUpdate()
		{
			base.UIRootUpdate();
			if (Find.World != null)
			{
				Find.World.UI.WorldInterfaceUpdate();
			}
			if (Current.Game != null)
			{
				LessonAutoActivator.LessonAutoActivatorUpdate();
				Find.Tutor.TutorUpdate();
			}
		}

		// Token: 0x06002D8A RID: 11658 RVA: 0x00023E72 File Offset: 0x00022072
		private void DoMainMenu()
		{
			if (!WorldRendererUtility.WorldRenderedNow)
			{
				UIMenuBackgroundManager.background.BackgroundOnGUI();
				if (this.ShouldDoMainMenu)
				{
					Current.Game = null;
					MainMenuDrawer.MainMenuOnGUI();
				}
			}
		}
	}
}
