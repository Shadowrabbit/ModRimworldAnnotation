using System;

namespace Verse
{
	// Token: 0x02000494 RID: 1172
	public static class DebugViewSettings
	{
		// Token: 0x060023C8 RID: 9160 RVA: 0x000DEA57 File Offset: 0x000DCC57
		public static void drawTerrainWaterToggled()
		{
			if (Find.CurrentMap != null)
			{
				Find.CurrentMap.mapDrawer.WholeMapChanged(MapMeshFlag.Terrain);
			}
		}

		// Token: 0x060023C9 RID: 9161 RVA: 0x000DEA71 File Offset: 0x000DCC71
		public static void drawShadowsToggled()
		{
			if (Find.CurrentMap != null)
			{
				Find.CurrentMap.mapDrawer.WholeMapChanged((MapMeshFlag)(-1));
			}
		}

		// Token: 0x04001634 RID: 5684
		public static bool drawFog = true;

		// Token: 0x04001635 RID: 5685
		public static bool drawSnow = true;

		// Token: 0x04001636 RID: 5686
		public static bool drawTerrain = true;

		// Token: 0x04001637 RID: 5687
		public static bool drawTerrainWater = true;

		// Token: 0x04001638 RID: 5688
		public static bool drawThingsDynamic = true;

		// Token: 0x04001639 RID: 5689
		public static bool drawThingsPrinted = true;

		// Token: 0x0400163A RID: 5690
		public static bool drawShadows = true;

		// Token: 0x0400163B RID: 5691
		public static bool drawLightingOverlay = true;

		// Token: 0x0400163C RID: 5692
		public static bool drawWorldOverlays = true;

		// Token: 0x0400163D RID: 5693
		public static bool drawPaths = false;

		// Token: 0x0400163E RID: 5694
		public static bool drawCastPositionSearch = false;

		// Token: 0x0400163F RID: 5695
		public static bool drawDestSearch = false;

		// Token: 0x04001640 RID: 5696
		public static bool drawStyleSearch = false;

		// Token: 0x04001641 RID: 5697
		public static bool drawSectionEdges = false;

		// Token: 0x04001642 RID: 5698
		public static bool drawRiverDebug = false;

		// Token: 0x04001643 RID: 5699
		public static bool drawPawnDebug = false;

		// Token: 0x04001644 RID: 5700
		public static bool drawPawnRotatorTarget = false;

		// Token: 0x04001645 RID: 5701
		public static bool drawRegions = false;

		// Token: 0x04001646 RID: 5702
		public static bool drawRegionLinks = false;

		// Token: 0x04001647 RID: 5703
		public static bool drawRegionDirties = false;

		// Token: 0x04001648 RID: 5704
		public static bool drawRegionTraversal = false;

		// Token: 0x04001649 RID: 5705
		public static bool drawRegionThings = false;

		// Token: 0x0400164A RID: 5706
		public static bool drawDistricts = false;

		// Token: 0x0400164B RID: 5707
		public static bool drawRooms = false;

		// Token: 0x0400164C RID: 5708
		public static bool drawPower = false;

		// Token: 0x0400164D RID: 5709
		public static bool drawPowerNetGrid = false;

		// Token: 0x0400164E RID: 5710
		public static bool drawOpportunisticJobs = false;

		// Token: 0x0400164F RID: 5711
		public static bool drawTooltipEdges = false;

		// Token: 0x04001650 RID: 5712
		public static bool drawRecordedNoise = false;

		// Token: 0x04001651 RID: 5713
		public static bool drawFoodSearchFromMouse = false;

		// Token: 0x04001652 RID: 5714
		public static bool drawPreyInfo = false;

		// Token: 0x04001653 RID: 5715
		public static bool drawGlow = false;

		// Token: 0x04001654 RID: 5716
		public static bool drawAvoidGrid = false;

		// Token: 0x04001655 RID: 5717
		public static bool drawBreachingGrid = false;

		// Token: 0x04001656 RID: 5718
		public static bool drawBreachingNoise = false;

		// Token: 0x04001657 RID: 5719
		public static bool drawLords = false;

		// Token: 0x04001658 RID: 5720
		public static bool drawDuties = false;

		// Token: 0x04001659 RID: 5721
		public static bool drawShooting = false;

		// Token: 0x0400165A RID: 5722
		public static bool drawInfestationChance = false;

		// Token: 0x0400165B RID: 5723
		public static bool drawStealDebug = false;

		// Token: 0x0400165C RID: 5724
		public static bool drawDeepResources = false;

		// Token: 0x0400165D RID: 5725
		public static bool drawAttackTargetScores = false;

		// Token: 0x0400165E RID: 5726
		public static bool drawInteractionCells = false;

		// Token: 0x0400165F RID: 5727
		public static bool drawDoorsDebug = false;

		// Token: 0x04001660 RID: 5728
		public static bool drawDestReservations = false;

		// Token: 0x04001661 RID: 5729
		public static bool drawDamageRects = false;

		// Token: 0x04001662 RID: 5730
		public static bool writeGame = false;

		// Token: 0x04001663 RID: 5731
		public static bool writeSteamItems = false;

		// Token: 0x04001664 RID: 5732
		public static bool writeConcepts = false;

		// Token: 0x04001665 RID: 5733
		public static bool writeReservations = false;

		// Token: 0x04001666 RID: 5734
		public static bool writePathCosts = false;

		// Token: 0x04001667 RID: 5735
		public static bool writeFertility = false;

		// Token: 0x04001668 RID: 5736
		public static bool writeLinkFlags = false;

		// Token: 0x04001669 RID: 5737
		public static bool writeCover = false;

		// Token: 0x0400166A RID: 5738
		public static bool writeCellContents = false;

		// Token: 0x0400166B RID: 5739
		public static bool writeMusicManagerPlay = false;

		// Token: 0x0400166C RID: 5740
		public static bool writeStoryteller = false;

		// Token: 0x0400166D RID: 5741
		public static bool writePlayingSounds = false;

		// Token: 0x0400166E RID: 5742
		public static bool writeSoundEventsRecord = false;

		// Token: 0x0400166F RID: 5743
		public static bool writeMoteSaturation = false;

		// Token: 0x04001670 RID: 5744
		public static bool writeSnowDepth = false;

		// Token: 0x04001671 RID: 5745
		public static bool writeEcosystem = false;

		// Token: 0x04001672 RID: 5746
		public static bool writeRecentStrikes = false;

		// Token: 0x04001673 RID: 5747
		public static bool writeBeauty = false;

		// Token: 0x04001674 RID: 5748
		public static bool writeListRepairableBldgs = false;

		// Token: 0x04001675 RID: 5749
		public static bool writeListFilthInHomeArea = false;

		// Token: 0x04001676 RID: 5750
		public static bool writeListHaulables = false;

		// Token: 0x04001677 RID: 5751
		public static bool writeListMergeables = false;

		// Token: 0x04001678 RID: 5752
		public static bool writeTotalSnowDepth = false;

		// Token: 0x04001679 RID: 5753
		public static bool writeCanReachColony = false;

		// Token: 0x0400167A RID: 5754
		public static bool writeMentalStateCalcs = false;

		// Token: 0x0400167B RID: 5755
		public static bool writeWind = false;

		// Token: 0x0400167C RID: 5756
		public static bool writeTerrain = false;

		// Token: 0x0400167D RID: 5757
		public static bool writeApparelScore = false;

		// Token: 0x0400167E RID: 5758
		public static bool writeWorkSettings = false;

		// Token: 0x0400167F RID: 5759
		public static bool writeSkyManager = false;

		// Token: 0x04001680 RID: 5760
		public static bool writeMemoryUsage = false;

		// Token: 0x04001681 RID: 5761
		public static bool writeMapGameConditions = false;

		// Token: 0x04001682 RID: 5762
		public static bool writeAttackTargets = false;

		// Token: 0x04001683 RID: 5763
		public static bool writeRopesAndPens = false;

		// Token: 0x04001684 RID: 5764
		public static bool writeRoomRoles = false;

		// Token: 0x04001685 RID: 5765
		public static bool logIncapChance = false;

		// Token: 0x04001686 RID: 5766
		public static bool logInput = false;

		// Token: 0x04001687 RID: 5767
		public static bool logApparelGeneration = false;

		// Token: 0x04001688 RID: 5768
		public static bool logLordToilTransitions = false;

		// Token: 0x04001689 RID: 5769
		public static bool logGrammarResolution = false;

		// Token: 0x0400168A RID: 5770
		public static bool logCombatLogMouseover = false;

		// Token: 0x0400168B RID: 5771
		public static bool logCauseOfDeath = false;

		// Token: 0x0400168C RID: 5772
		public static bool logMapLoad = false;

		// Token: 0x0400168D RID: 5773
		public static bool logTutor = false;

		// Token: 0x0400168E RID: 5774
		public static bool logSignals = false;

		// Token: 0x0400168F RID: 5775
		public static bool logWorldPawnGC = false;

		// Token: 0x04001690 RID: 5776
		public static bool logTaleRecording = false;

		// Token: 0x04001691 RID: 5777
		public static bool logHourlyScreenshot = false;

		// Token: 0x04001692 RID: 5778
		public static bool logFilthSummary = false;

		// Token: 0x04001693 RID: 5779
		public static bool logCarriedBetweenJobs = false;

		// Token: 0x04001694 RID: 5780
		public static bool logComplexGenPoints = false;

		// Token: 0x04001695 RID: 5781
		public static bool debugApparelOptimize = false;

		// Token: 0x04001696 RID: 5782
		public static bool showAllRoomStats = false;

		// Token: 0x04001697 RID: 5783
		public static bool showFloatMenuWorkGivers = false;

		// Token: 0x04001698 RID: 5784
		public static bool neverForceNormalSpeed = false;
	}
}
