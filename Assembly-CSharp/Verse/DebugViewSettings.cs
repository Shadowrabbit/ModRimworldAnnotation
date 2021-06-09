using System;

namespace Verse
{
	// Token: 0x02000802 RID: 2050
	public static class DebugViewSettings
	{
		// Token: 0x060033B6 RID: 13238 RVA: 0x000288E1 File Offset: 0x00026AE1
		public static void drawTerrainWaterToggled()
		{
			if (Find.CurrentMap != null)
			{
				Find.CurrentMap.mapDrawer.WholeMapChanged(MapMeshFlag.Terrain);
			}
		}

		// Token: 0x060033B7 RID: 13239 RVA: 0x000288FB File Offset: 0x00026AFB
		public static void drawShadowsToggled()
		{
			if (Find.CurrentMap != null)
			{
				Find.CurrentMap.mapDrawer.WholeMapChanged((MapMeshFlag)(-1));
			}
		}

		// Token: 0x04002384 RID: 9092
		public static bool drawFog = true;

		// Token: 0x04002385 RID: 9093
		public static bool drawSnow = true;

		// Token: 0x04002386 RID: 9094
		public static bool drawTerrain = true;

		// Token: 0x04002387 RID: 9095
		public static bool drawTerrainWater = true;

		// Token: 0x04002388 RID: 9096
		public static bool drawThingsDynamic = true;

		// Token: 0x04002389 RID: 9097
		public static bool drawThingsPrinted = true;

		// Token: 0x0400238A RID: 9098
		public static bool drawShadows = true;

		// Token: 0x0400238B RID: 9099
		public static bool drawLightingOverlay = true;

		// Token: 0x0400238C RID: 9100
		public static bool drawWorldOverlays = true;

		// Token: 0x0400238D RID: 9101
		public static bool drawPaths = false;

		// Token: 0x0400238E RID: 9102
		public static bool drawCastPositionSearch = false;

		// Token: 0x0400238F RID: 9103
		public static bool drawDestSearch = false;

		// Token: 0x04002390 RID: 9104
		public static bool drawSectionEdges = false;

		// Token: 0x04002391 RID: 9105
		public static bool drawRiverDebug = false;

		// Token: 0x04002392 RID: 9106
		public static bool drawPawnDebug = false;

		// Token: 0x04002393 RID: 9107
		public static bool drawPawnRotatorTarget = false;

		// Token: 0x04002394 RID: 9108
		public static bool drawRegions = false;

		// Token: 0x04002395 RID: 9109
		public static bool drawRegionLinks = false;

		// Token: 0x04002396 RID: 9110
		public static bool drawRegionDirties = false;

		// Token: 0x04002397 RID: 9111
		public static bool drawRegionTraversal = false;

		// Token: 0x04002398 RID: 9112
		public static bool drawRegionThings = false;

		// Token: 0x04002399 RID: 9113
		public static bool drawRooms = false;

		// Token: 0x0400239A RID: 9114
		public static bool drawRoomGroups = false;

		// Token: 0x0400239B RID: 9115
		public static bool drawPower = false;

		// Token: 0x0400239C RID: 9116
		public static bool drawPowerNetGrid = false;

		// Token: 0x0400239D RID: 9117
		public static bool drawOpportunisticJobs = false;

		// Token: 0x0400239E RID: 9118
		public static bool drawTooltipEdges = false;

		// Token: 0x0400239F RID: 9119
		public static bool drawRecordedNoise = false;

		// Token: 0x040023A0 RID: 9120
		public static bool drawFoodSearchFromMouse = false;

		// Token: 0x040023A1 RID: 9121
		public static bool drawPreyInfo = false;

		// Token: 0x040023A2 RID: 9122
		public static bool drawGlow = false;

		// Token: 0x040023A3 RID: 9123
		public static bool drawAvoidGrid = false;

		// Token: 0x040023A4 RID: 9124
		public static bool drawLords = false;

		// Token: 0x040023A5 RID: 9125
		public static bool drawDuties = false;

		// Token: 0x040023A6 RID: 9126
		public static bool drawShooting = false;

		// Token: 0x040023A7 RID: 9127
		public static bool drawInfestationChance = false;

		// Token: 0x040023A8 RID: 9128
		public static bool drawStealDebug = false;

		// Token: 0x040023A9 RID: 9129
		public static bool drawDeepResources = false;

		// Token: 0x040023AA RID: 9130
		public static bool drawAttackTargetScores = false;

		// Token: 0x040023AB RID: 9131
		public static bool drawInteractionCells = false;

		// Token: 0x040023AC RID: 9132
		public static bool drawDoorsDebug = false;

		// Token: 0x040023AD RID: 9133
		public static bool drawDestReservations = false;

		// Token: 0x040023AE RID: 9134
		public static bool drawDamageRects = false;

		// Token: 0x040023AF RID: 9135
		public static bool writeGame = false;

		// Token: 0x040023B0 RID: 9136
		public static bool writeSteamItems = false;

		// Token: 0x040023B1 RID: 9137
		public static bool writeConcepts = false;

		// Token: 0x040023B2 RID: 9138
		public static bool writeReservations = false;

		// Token: 0x040023B3 RID: 9139
		public static bool writePathCosts = false;

		// Token: 0x040023B4 RID: 9140
		public static bool writeFertility = false;

		// Token: 0x040023B5 RID: 9141
		public static bool writeLinkFlags = false;

		// Token: 0x040023B6 RID: 9142
		public static bool writeCover = false;

		// Token: 0x040023B7 RID: 9143
		public static bool writeCellContents = false;

		// Token: 0x040023B8 RID: 9144
		public static bool writeMusicManagerPlay = false;

		// Token: 0x040023B9 RID: 9145
		public static bool writeStoryteller = false;

		// Token: 0x040023BA RID: 9146
		public static bool writePlayingSounds = false;

		// Token: 0x040023BB RID: 9147
		public static bool writeSoundEventsRecord = false;

		// Token: 0x040023BC RID: 9148
		public static bool writeMoteSaturation = false;

		// Token: 0x040023BD RID: 9149
		public static bool writeSnowDepth = false;

		// Token: 0x040023BE RID: 9150
		public static bool writeEcosystem = false;

		// Token: 0x040023BF RID: 9151
		public static bool writeRecentStrikes = false;

		// Token: 0x040023C0 RID: 9152
		public static bool writeBeauty = false;

		// Token: 0x040023C1 RID: 9153
		public static bool writeListRepairableBldgs = false;

		// Token: 0x040023C2 RID: 9154
		public static bool writeListFilthInHomeArea = false;

		// Token: 0x040023C3 RID: 9155
		public static bool writeListHaulables = false;

		// Token: 0x040023C4 RID: 9156
		public static bool writeListMergeables = false;

		// Token: 0x040023C5 RID: 9157
		public static bool writeTotalSnowDepth = false;

		// Token: 0x040023C6 RID: 9158
		public static bool writeCanReachColony = false;

		// Token: 0x040023C7 RID: 9159
		public static bool writeMentalStateCalcs = false;

		// Token: 0x040023C8 RID: 9160
		public static bool writeWind = false;

		// Token: 0x040023C9 RID: 9161
		public static bool writeTerrain = false;

		// Token: 0x040023CA RID: 9162
		public static bool writeApparelScore = false;

		// Token: 0x040023CB RID: 9163
		public static bool writeWorkSettings = false;

		// Token: 0x040023CC RID: 9164
		public static bool writeSkyManager = false;

		// Token: 0x040023CD RID: 9165
		public static bool writeMemoryUsage = false;

		// Token: 0x040023CE RID: 9166
		public static bool writeMapGameConditions = false;

		// Token: 0x040023CF RID: 9167
		public static bool writeAttackTargets = false;

		// Token: 0x040023D0 RID: 9168
		public static bool logIncapChance = false;

		// Token: 0x040023D1 RID: 9169
		public static bool logInput = false;

		// Token: 0x040023D2 RID: 9170
		public static bool logApparelGeneration = false;

		// Token: 0x040023D3 RID: 9171
		public static bool logLordToilTransitions = false;

		// Token: 0x040023D4 RID: 9172
		public static bool logGrammarResolution = false;

		// Token: 0x040023D5 RID: 9173
		public static bool logCombatLogMouseover = false;

		// Token: 0x040023D6 RID: 9174
		public static bool logCauseOfDeath = false;

		// Token: 0x040023D7 RID: 9175
		public static bool logMapLoad = false;

		// Token: 0x040023D8 RID: 9176
		public static bool logTutor = false;

		// Token: 0x040023D9 RID: 9177
		public static bool logSignals = false;

		// Token: 0x040023DA RID: 9178
		public static bool logWorldPawnGC = false;

		// Token: 0x040023DB RID: 9179
		public static bool logTaleRecording = false;

		// Token: 0x040023DC RID: 9180
		public static bool logHourlyScreenshot = false;

		// Token: 0x040023DD RID: 9181
		public static bool logFilthSummary = false;

		// Token: 0x040023DE RID: 9182
		public static bool debugApparelOptimize = false;

		// Token: 0x040023DF RID: 9183
		public static bool showAllRoomStats = false;

		// Token: 0x040023E0 RID: 9184
		public static bool showFloatMenuWorkGivers = false;
	}
}
