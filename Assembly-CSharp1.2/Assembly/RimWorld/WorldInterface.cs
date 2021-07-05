using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001BF0 RID: 7152
	public class WorldInterface
	{
		// Token: 0x170018B4 RID: 6324
		// (get) Token: 0x06009D64 RID: 40292 RVA: 0x00068C22 File Offset: 0x00066E22
		// (set) Token: 0x06009D65 RID: 40293 RVA: 0x00068C2F File Offset: 0x00066E2F
		public int SelectedTile
		{
			get
			{
				return this.selector.selectedTile;
			}
			set
			{
				this.selector.selectedTile = value;
			}
		}

		// Token: 0x06009D66 RID: 40294 RVA: 0x002E0D28 File Offset: 0x002DEF28
		public void Reset()
		{
			this.everReset = true;
			this.inspectPane.Reset();
			if (Current.ProgramState == ProgramState.Playing)
			{
				if (Find.CurrentMap != null)
				{
					this.SelectedTile = Find.CurrentMap.Tile;
				}
				else
				{
					this.SelectedTile = -1;
				}
			}
			else if (Find.GameInitData != null)
			{
				if (Find.GameInitData.startingTile >= 0 && Find.World != null && !Find.WorldGrid.InBounds(Find.GameInitData.startingTile))
				{
					Log.Error("Map world tile was out of bounds.", false);
					Find.GameInitData.startingTile = -1;
				}
				this.SelectedTile = Find.GameInitData.startingTile;
				this.inspectPane.OpenTabType = typeof(WITab_Terrain);
			}
			else
			{
				this.SelectedTile = -1;
			}
			if (this.SelectedTile >= 0)
			{
				Find.WorldCameraDriver.JumpTo(this.SelectedTile);
			}
			else
			{
				Find.WorldCameraDriver.JumpTo(Find.WorldGrid.viewCenter);
			}
			Find.WorldCameraDriver.ResetAltitude();
		}

		// Token: 0x06009D67 RID: 40295 RVA: 0x00068C3D File Offset: 0x00066E3D
		public void WorldInterfaceUpdate()
		{
			if (WorldRendererUtility.WorldRenderedNow)
			{
				this.targeter.TargeterUpdate();
				WorldSelectionDrawer.DrawSelectionOverlays();
				Find.WorldDebugDrawer.WorldDebugDrawerUpdate();
			}
			else
			{
				this.targeter.StopTargeting();
			}
			this.routePlanner.WorldRoutePlannerUpdate();
		}

		// Token: 0x06009D68 RID: 40296 RVA: 0x002E0E20 File Offset: 0x002DF020
		public void WorldInterfaceOnGUI()
		{
			bool worldRenderedNow = WorldRendererUtility.WorldRenderedNow;
			this.CheckOpenOrCloseInspectPane();
			if (worldRenderedNow)
			{
				ScreenshotModeHandler screenshotMode = Find.UIRoot.screenshotMode;
				ExpandableWorldObjectsUtility.ExpandableWorldObjectsOnGUI();
				WorldSelectionDrawer.SelectionOverlaysOnGUI();
				this.routePlanner.WorldRoutePlannerOnGUI();
				if (!screenshotMode.FiltersCurrentEvent && Current.ProgramState == ProgramState.Playing)
				{
					Find.ColonistBar.ColonistBarOnGUI();
				}
				this.selector.dragBox.DragBoxOnGUI();
				this.targeter.TargeterOnGUI();
				if (!screenshotMode.FiltersCurrentEvent)
				{
					this.globalControls.WorldGlobalControlsOnGUI();
				}
				Find.WorldDebugDrawer.WorldDebugDrawerOnGUI();
			}
		}

		// Token: 0x06009D69 RID: 40297 RVA: 0x00068C78 File Offset: 0x00066E78
		public void HandleLowPriorityInput()
		{
			if (WorldRendererUtility.WorldRenderedNow)
			{
				this.targeter.ProcessInputEvents();
				this.selector.WorldSelectorOnGUI();
			}
		}

		// Token: 0x06009D6A RID: 40298 RVA: 0x002E0EAC File Offset: 0x002DF0AC
		private void CheckOpenOrCloseInspectPane()
		{
			if (this.selector.AnyObjectOrTileSelected && WorldRendererUtility.WorldRenderedNow && (Current.ProgramState != ProgramState.Playing || Find.MainTabsRoot.OpenTab == null))
			{
				if (!Find.WindowStack.IsOpen<WorldInspectPane>())
				{
					Find.WindowStack.Add(this.inspectPane);
					return;
				}
			}
			else if (Find.WindowStack.IsOpen<WorldInspectPane>())
			{
				Find.WindowStack.TryRemove(this.inspectPane, false);
			}
		}

		// Token: 0x0400642C RID: 25644
		public WorldSelector selector = new WorldSelector();

		// Token: 0x0400642D RID: 25645
		public WorldTargeter targeter = new WorldTargeter();

		// Token: 0x0400642E RID: 25646
		public WorldInspectPane inspectPane = new WorldInspectPane();

		// Token: 0x0400642F RID: 25647
		public WorldGlobalControls globalControls = new WorldGlobalControls();

		// Token: 0x04006430 RID: 25648
		public WorldRoutePlanner routePlanner = new WorldRoutePlanner();

		// Token: 0x04006431 RID: 25649
		public bool everReset;
	}
}
