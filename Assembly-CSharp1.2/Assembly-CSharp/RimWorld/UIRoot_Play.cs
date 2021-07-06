using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001BEE RID: 7150
	public class UIRoot_Play : UIRoot
	{
		// Token: 0x06009D5A RID: 40282 RVA: 0x00068BEC File Offset: 0x00066DEC
		public override void Init()
		{
			base.Init();
			Messages.Clear();
		}

		// Token: 0x06009D5B RID: 40283 RVA: 0x002E04BC File Offset: 0x002DE6BC
		public override void UIRootOnGUI()
		{
			base.UIRootOnGUI();
			Find.GameInfo.GameInfoOnGUI();
			Find.World.UI.WorldInterfaceOnGUI();
			this.mapUI.MapInterfaceOnGUI_BeforeMainTabs();
			if (!this.screenshotMode.FiltersCurrentEvent)
			{
				this.mainButtonsRoot.MainButtonsOnGUI();
				this.alerts.AlertsReadoutOnGUI();
			}
			this.mapUI.MapInterfaceOnGUI_AfterMainTabs();
			if (!this.screenshotMode.FiltersCurrentEvent)
			{
				Find.Tutor.TutorOnGUI();
			}
			ReorderableWidget.ReorderableWidgetOnGUI_BeforeWindowStack();
			this.windows.WindowStackOnGUI();
			ReorderableWidget.ReorderableWidgetOnGUI_AfterWindowStack();
			Widgets.WidgetsOnGUI();
			this.mapUI.HandleMapClicks();
			if (Find.DesignatorManager.SelectedDesignator != null)
			{
				Find.DesignatorManager.SelectedDesignator.SelectedProcessInput(Event.current);
			}
			DebugTools.DebugToolsOnGUI();
			this.mainButtonsRoot.HandleLowPriorityShortcuts();
			Find.World.UI.HandleLowPriorityInput();
			this.mapUI.HandleLowPriorityInput();
			this.OpenMainMenuShortcut();
		}

		// Token: 0x06009D5C RID: 40284 RVA: 0x002E05B0 File Offset: 0x002DE7B0
		public override void UIRootUpdate()
		{
			base.UIRootUpdate();
			try
			{
				Find.World.UI.WorldInterfaceUpdate();
				this.mapUI.MapInterfaceUpdate();
				this.alerts.AlertsReadoutUpdate();
				LessonAutoActivator.LessonAutoActivatorUpdate();
				Find.Tutor.TutorUpdate();
			}
			catch (Exception ex)
			{
				Log.Error("Exception in UIRootUpdate: " + ex.ToString(), false);
			}
		}

		// Token: 0x06009D5D RID: 40285 RVA: 0x002E0624 File Offset: 0x002DE824
		private void OpenMainMenuShortcut()
		{
			if ((Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Escape) || KeyBindingDefOf.Cancel.KeyDownEvent)
			{
				Event.current.Use();
				Find.MainTabsRoot.SetCurrentTab(MainButtonDefOf.Menu, true);
			}
		}

		// Token: 0x0400641C RID: 25628
		public MapInterface mapUI = new MapInterface();

		// Token: 0x0400641D RID: 25629
		public MainButtonsRoot mainButtonsRoot = new MainButtonsRoot();

		// Token: 0x0400641E RID: 25630
		public AlertsReadout alerts = new AlertsReadout();
	}
}
