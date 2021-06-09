using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001B5F RID: 7007
	public class MapInterface
	{
		// Token: 0x06009A6B RID: 39531 RVA: 0x002D75A8 File Offset: 0x002D57A8
		public void MapInterfaceOnGUI_BeforeMainTabs()
		{
			if (Find.CurrentMap == null)
			{
				return;
			}
			if (!WorldRendererUtility.WorldRenderedNow)
			{
				ScreenshotModeHandler screenshotMode = Find.UIRoot.screenshotMode;
				this.thingOverlays.ThingOverlaysOnGUI();
				MapComponentUtility.MapComponentOnGUI(Find.CurrentMap);
				BeautyDrawer.BeautyDrawerOnGUI();
				if (!screenshotMode.FiltersCurrentEvent)
				{
					this.colonistBar.ColonistBarOnGUI();
				}
				this.selector.dragBox.DragBoxOnGUI();
				this.designatorManager.DesignationManagerOnGUI();
				this.targeter.TargeterOnGUI();
				Find.CurrentMap.tooltipGiverList.DispenseAllThingTooltips();
				if (DebugViewSettings.drawFoodSearchFromMouse)
				{
					FoodUtility.DebugFoodSearchFromMouse_OnGUI();
				}
				if (DebugViewSettings.drawAttackTargetScores)
				{
					AttackTargetFinder.DebugDrawAttackTargetScores_OnGUI();
				}
				if (!screenshotMode.FiltersCurrentEvent)
				{
					this.mouseoverReadout.MouseoverReadoutOnGUI();
					this.globalControls.GlobalControlsOnGUI();
					this.resourceReadout.ResourceReadoutOnGUI();
					return;
				}
			}
			else
			{
				this.targeter.StopTargeting();
			}
		}

		// Token: 0x06009A6C RID: 39532 RVA: 0x002D7680 File Offset: 0x002D5880
		public void MapInterfaceOnGUI_AfterMainTabs()
		{
			if (Find.CurrentMap == null)
			{
				return;
			}
			if (!WorldRendererUtility.WorldRenderedNow && !Find.UIRoot.screenshotMode.FiltersCurrentEvent)
			{
				EnvironmentStatsDrawer.EnvironmentStatsOnGUI();
				Find.CurrentMap.deepResourceGrid.DeepResourcesOnGUI();
				Find.CurrentMap.debugDrawer.DebugDrawerOnGUI();
			}
		}

		// Token: 0x06009A6D RID: 39533 RVA: 0x00066CF3 File Offset: 0x00064EF3
		public void HandleMapClicks()
		{
			if (Find.CurrentMap == null)
			{
				return;
			}
			if (!WorldRendererUtility.WorldRenderedNow)
			{
				this.designatorManager.ProcessInputEvents();
				this.targeter.ProcessInputEvents();
			}
		}

		// Token: 0x06009A6E RID: 39534 RVA: 0x00066D1A File Offset: 0x00064F1A
		public void HandleLowPriorityInput()
		{
			if (Find.CurrentMap == null)
			{
				return;
			}
			if (!WorldRendererUtility.WorldRenderedNow)
			{
				this.selector.SelectorOnGUI();
				Find.CurrentMap.lordManager.LordManagerOnGUI();
			}
		}

		// Token: 0x06009A6F RID: 39535 RVA: 0x002D76D0 File Offset: 0x002D58D0
		public void MapInterfaceUpdate()
		{
			if (Find.CurrentMap == null)
			{
				return;
			}
			if (!WorldRendererUtility.WorldRenderedNow)
			{
				this.targeter.TargeterUpdate();
				SelectionDrawer.DrawSelectionOverlays();
				EnvironmentStatsDrawer.DrawRoomOverlays();
				this.designatorManager.DesignatorManagerUpdate();
				Find.CurrentMap.roofGrid.RoofGridUpdate();
				Find.CurrentMap.fertilityGrid.FertilityGridUpdate();
				Find.CurrentMap.terrainGrid.TerrainGridUpdate();
				Find.CurrentMap.exitMapGrid.ExitMapGridUpdate();
				Find.CurrentMap.deepResourceGrid.DeepResourceGridUpdate();
				if (DebugViewSettings.drawPawnDebug)
				{
					Find.CurrentMap.pawnDestinationReservationManager.DebugDrawDestinations();
					Find.CurrentMap.reservationManager.DebugDrawReservations();
				}
				if (DebugViewSettings.drawDestReservations)
				{
					Find.CurrentMap.pawnDestinationReservationManager.DebugDrawReservations();
				}
				if (DebugViewSettings.drawFoodSearchFromMouse)
				{
					FoodUtility.DebugFoodSearchFromMouse_Update();
				}
				if (DebugViewSettings.drawPreyInfo)
				{
					FoodUtility.DebugDrawPredatorFoodSource();
				}
				if (DebugViewSettings.drawAttackTargetScores)
				{
					AttackTargetFinder.DebugDrawAttackTargetScores_Update();
				}
				MiscDebugDrawer.DebugDrawInteractionCells();
				Find.CurrentMap.debugDrawer.DebugDrawerUpdate();
				Find.CurrentMap.regionGrid.DebugDraw();
				InfestationCellFinder.DebugDraw();
				StealAIDebugDrawer.DebugDraw();
				if (DebugViewSettings.drawRiverDebug)
				{
					Find.CurrentMap.waterInfo.DebugDrawRiver();
				}
				BuildingsDamageSectionLayerUtility.DebugDraw();
			}
		}

		// Token: 0x06009A70 RID: 39536 RVA: 0x002D7804 File Offset: 0x002D5A04
		public void Notify_SwitchedMap()
		{
			this.designatorManager.Deselect();
			this.reverseDesignatorDatabase.Reinit();
			this.selector.ClearSelection();
			this.selector.dragBox.active = false;
			this.targeter.StopTargeting();
			MainButtonDef openTab = Find.MainTabsRoot.OpenTab;
			List<MainButtonDef> allDefsListForReading = DefDatabase<MainButtonDef>.AllDefsListForReading;
			for (int i = 0; i < allDefsListForReading.Count; i++)
			{
				allDefsListForReading[i].Notify_SwitchedMap();
			}
			if (openTab != null && openTab != MainButtonDefOf.Inspect)
			{
				Find.MainTabsRoot.SetCurrentTab(openTab, false);
			}
			if (Find.CurrentMap != null)
			{
				RememberedCameraPos rememberedCameraPos = Find.CurrentMap.rememberedCameraPos;
				Find.CameraDriver.SetRootPosAndSize(rememberedCameraPos.rootPos, rememberedCameraPos.rootSize);
			}
		}

		// Token: 0x040062BB RID: 25275
		public ThingOverlays thingOverlays = new ThingOverlays();

		// Token: 0x040062BC RID: 25276
		public Selector selector = new Selector();

		// Token: 0x040062BD RID: 25277
		public Targeter targeter = new Targeter();

		// Token: 0x040062BE RID: 25278
		public DesignatorManager designatorManager = new DesignatorManager();

		// Token: 0x040062BF RID: 25279
		public ReverseDesignatorDatabase reverseDesignatorDatabase = new ReverseDesignatorDatabase();

		// Token: 0x040062C0 RID: 25280
		private MouseoverReadout mouseoverReadout = new MouseoverReadout();

		// Token: 0x040062C1 RID: 25281
		public GlobalControls globalControls = new GlobalControls();

		// Token: 0x040062C2 RID: 25282
		protected ResourceReadout resourceReadout = new ResourceReadout();

		// Token: 0x040062C3 RID: 25283
		public ColonistBar colonistBar = new ColonistBar();
	}
}
