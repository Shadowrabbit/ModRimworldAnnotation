using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x020013E1 RID: 5089
	public class WorldInterface
	{
		// Token: 0x170015AC RID: 5548
		// (get) Token: 0x06007BC4 RID: 31684 RVA: 0x002BA5C2 File Offset: 0x002B87C2
		// (set) Token: 0x06007BC5 RID: 31685 RVA: 0x002BA5CF File Offset: 0x002B87CF
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

		// Token: 0x06007BC6 RID: 31686 RVA: 0x002BA5E0 File Offset: 0x002B87E0
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
					Log.Error("Map world tile was out of bounds.");
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

		// Token: 0x06007BC7 RID: 31687 RVA: 0x002BA6D8 File Offset: 0x002B88D8
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
				this.tilePicker.StopTargeting();
			}
			this.routePlanner.WorldRoutePlannerUpdate();
		}

		// Token: 0x06007BC8 RID: 31688 RVA: 0x002BA72C File Offset: 0x002B892C
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
				if (this.tilePicker.Active)
				{
					this.tilePicker.TileSelectorOnGUI();
				}
				if (!screenshotMode.FiltersCurrentEvent)
				{
					this.globalControls.WorldGlobalControlsOnGUI();
				}
				Find.WorldDebugDrawer.WorldDebugDrawerOnGUI();
			}
		}

		// Token: 0x06007BC9 RID: 31689 RVA: 0x002BA7D1 File Offset: 0x002B89D1
		public void HandleLowPriorityInput()
		{
			if (WorldRendererUtility.WorldRenderedNow)
			{
				this.targeter.ProcessInputEvents();
				this.selector.WorldSelectorOnGUI();
			}
		}

		// Token: 0x06007BCA RID: 31690 RVA: 0x002BA7F0 File Offset: 0x002B89F0
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

		// Token: 0x04004484 RID: 17540
		public WorldSelector selector = new WorldSelector();

		// Token: 0x04004485 RID: 17541
		public WorldTargeter targeter = new WorldTargeter();

		// Token: 0x04004486 RID: 17542
		public WorldInspectPane inspectPane = new WorldInspectPane();

		// Token: 0x04004487 RID: 17543
		public WorldGlobalControls globalControls = new WorldGlobalControls();

		// Token: 0x04004488 RID: 17544
		public WorldRoutePlanner routePlanner = new WorldRoutePlanner();

		// Token: 0x04004489 RID: 17545
		public TilePicker tilePicker = new TilePicker();

		// Token: 0x0400448A RID: 17546
		public bool everReset;
	}
}
