using System;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;
using Verse.Steam;

namespace Verse
{
	// Token: 0x020003EC RID: 1004
	public class UIRoot_Entry : UIRoot
	{
		// Token: 0x170005C1 RID: 1473
		// (get) Token: 0x06001E43 RID: 7747 RVA: 0x000BD4F8 File Offset: 0x000BB6F8
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

		// Token: 0x06001E44 RID: 7748 RVA: 0x000BD54C File Offset: 0x000BB74C
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

		// Token: 0x06001E45 RID: 7749 RVA: 0x000BD5E8 File Offset: 0x000BB7E8
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
			DragAndDropWidget.DragAndDropWidgetOnGUI_BeforeWindowStack();
			this.windows.WindowStackOnGUI();
			DragAndDropWidget.DragAndDropWidgetOnGUI_AfterWindowStack();
			ReorderableWidget.ReorderableWidgetOnGUI_AfterWindowStack();
			Widgets.WidgetsOnGUI();
			if (Find.World != null)
			{
				Find.World.UI.HandleLowPriorityInput();
			}
		}

		// Token: 0x06001E46 RID: 7750 RVA: 0x000BD662 File Offset: 0x000BB862
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

		// Token: 0x06001E47 RID: 7751 RVA: 0x000BD696 File Offset: 0x000BB896
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
