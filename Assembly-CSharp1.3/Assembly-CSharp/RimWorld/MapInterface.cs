using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001375 RID: 4981
	public class MapInterface
	{
		// Token: 0x0600792E RID: 31022 RVA: 0x002AF01C File Offset: 0x002AD21C
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
				this.selector.SelectorOnGUI_BeforeMainTabs();
				Find.CurrentMap.tooltipGiverList.DispenseAllThingTooltips();
				Find.CurrentMap.flecks.FleckManagerOnGUI();
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

		// Token: 0x0600792F RID: 31023 RVA: 0x002AF110 File Offset: 0x002AD310
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

		// Token: 0x06007930 RID: 31024 RVA: 0x002AF160 File Offset: 0x002AD360
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

		// Token: 0x06007931 RID: 31025 RVA: 0x002AF187 File Offset: 0x002AD387
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

		// Token: 0x06007932 RID: 31026 RVA: 0x002AF1B4 File Offset: 0x002AD3B4
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
				this.selector.gotoController.Draw();
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

		// Token: 0x06007933 RID: 31027 RVA: 0x002AF2F8 File Offset: 0x002AD4F8
		public void Notify_SwitchedMap()
		{
			this.designatorManager.Deselect();
			this.reverseDesignatorDatabase.Reinit();
			this.selector.ClearSelection();
			this.selector.dragBox.active = false;
			this.selector.gotoController.Deactivate();
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

		// Token: 0x04004374 RID: 17268
		public ThingOverlays thingOverlays = new ThingOverlays();

		// Token: 0x04004375 RID: 17269
		public Selector selector = new Selector();

		// Token: 0x04004376 RID: 17270
		public Targeter targeter = new Targeter();

		// Token: 0x04004377 RID: 17271
		public DesignatorManager designatorManager = new DesignatorManager();

		// Token: 0x04004378 RID: 17272
		public ReverseDesignatorDatabase reverseDesignatorDatabase = new ReverseDesignatorDatabase();

		// Token: 0x04004379 RID: 17273
		private MouseoverReadout mouseoverReadout = new MouseoverReadout();

		// Token: 0x0400437A RID: 17274
		public GlobalControls globalControls = new GlobalControls();

		// Token: 0x0400437B RID: 17275
		protected ResourceReadout resourceReadout = new ResourceReadout();

		// Token: 0x0400437C RID: 17276
		public ColonistBar colonistBar = new ColonistBar();
	}
}
