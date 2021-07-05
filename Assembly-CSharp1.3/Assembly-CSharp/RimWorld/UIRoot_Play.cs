using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020013DE RID: 5086
	public class UIRoot_Play : UIRoot
	{
		// Token: 0x06007BB2 RID: 31666 RVA: 0x002B9959 File Offset: 0x002B7B59
		public override void Init()
		{
			base.Init();
			Messages.Clear();
		}

		// Token: 0x06007BB3 RID: 31667 RVA: 0x002B9968 File Offset: 0x002B7B68
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
			DragAndDropWidget.DragAndDropWidgetOnGUI_BeforeWindowStack();
			this.windows.WindowStackOnGUI();
			DragAndDropWidget.DragAndDropWidgetOnGUI_AfterWindowStack();
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

		// Token: 0x06007BB4 RID: 31668 RVA: 0x002B9A64 File Offset: 0x002B7C64
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
				Log.Error("Exception in UIRootUpdate: " + ex.ToString());
			}
		}

		// Token: 0x06007BB5 RID: 31669 RVA: 0x002B9AD8 File Offset: 0x002B7CD8
		private void OpenMainMenuShortcut()
		{
			if ((Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Escape) || KeyBindingDefOf.Cancel.KeyDownEvent)
			{
				Event.current.Use();
				Find.MainTabsRoot.SetCurrentTab(MainButtonDefOf.Menu, true);
			}
		}

		// Token: 0x0400446B RID: 17515
		public MapInterface mapUI = new MapInterface();

		// Token: 0x0400446C RID: 17516
		public MainButtonsRoot mainButtonsRoot = new MainButtonsRoot();

		// Token: 0x0400446D RID: 17517
		public AlertsReadout alerts = new AlertsReadout();
	}
}
