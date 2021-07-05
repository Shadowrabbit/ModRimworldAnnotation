using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020004E2 RID: 1250
	public class PrefsData
	{
		// Token: 0x060025CE RID: 9678 RVA: 0x000EA60C File Offset: 0x000E880C
		public void Apply()
		{
			if (!UnityData.IsInMainThread)
			{
				return;
			}
			if (this.customCursorEnabled)
			{
				CustomCursor.Activate();
			}
			else
			{
				CustomCursor.Deactivate();
			}
			AudioListener.volume = this.volumeGame;
			Application.runInBackground = this.runInBackground;
			if (this.screenWidth == 0 || this.screenHeight == 0)
			{
				ResolutionUtility.SetNativeResolutionRaw();
				return;
			}
			ResolutionUtility.SetResolutionRaw(this.screenWidth, this.screenHeight, this.fullscreen);
		}

		// Token: 0x04001787 RID: 6023
		public float volumeGame = 0.8f;

		// Token: 0x04001788 RID: 6024
		public float volumeMusic = 0.4f;

		// Token: 0x04001789 RID: 6025
		public float volumeAmbient = 1f;

		// Token: 0x0400178A RID: 6026
		public int screenWidth;

		// Token: 0x0400178B RID: 6027
		public int screenHeight;

		// Token: 0x0400178C RID: 6028
		public bool fullscreen;

		// Token: 0x0400178D RID: 6029
		public float uiScale = 1f;

		// Token: 0x0400178E RID: 6030
		public bool customCursorEnabled = true;

		// Token: 0x0400178F RID: 6031
		public bool hatsOnlyOnMap;

		// Token: 0x04001790 RID: 6032
		public bool plantWindSway = true;

		// Token: 0x04001791 RID: 6033
		public bool showRealtimeClock;

		// Token: 0x04001792 RID: 6034
		public AnimalNameDisplayMode animalNameMode;

		// Token: 0x04001793 RID: 6035
		public string backgroundExpansionId;

		// Token: 0x04001794 RID: 6036
		[Obsolete]
		public bool extremeDifficultyUnlocked;

		// Token: 0x04001795 RID: 6037
		public bool adaptiveTrainingEnabled = true;

		// Token: 0x04001796 RID: 6038
		public List<string> preferredNames = new List<string>();

		// Token: 0x04001797 RID: 6039
		public bool resourceReadoutCategorized;

		// Token: 0x04001798 RID: 6040
		public bool runInBackground;

		// Token: 0x04001799 RID: 6041
		public bool edgeScreenScroll = true;

		// Token: 0x0400179A RID: 6042
		public TemperatureDisplayMode temperatureMode;

		// Token: 0x0400179B RID: 6043
		public float autosaveIntervalDays = 1f;

		// Token: 0x0400179C RID: 6044
		public bool testMapSizes;

		// Token: 0x0400179D RID: 6045
		[LoadAlias("maxNumberOfPlayerHomes")]
		public int maxNumberOfPlayerSettlements = 1;

		// Token: 0x0400179E RID: 6046
		public bool pauseOnLoad;

		// Token: 0x0400179F RID: 6047
		public AutomaticPauseMode automaticPauseMode = AutomaticPauseMode.MajorThreat;

		// Token: 0x040017A0 RID: 6048
		public float mapDragSensitivity = 1.3f;

		// Token: 0x040017A1 RID: 6049
		[Unsaved(true)]
		public bool? pauseOnUrgentLetter;

		// Token: 0x040017A2 RID: 6050
		public bool devMode;

		// Token: 0x040017A3 RID: 6051
		public string langFolderName = "unknown";

		// Token: 0x040017A4 RID: 6052
		public bool logVerbose;

		// Token: 0x040017A5 RID: 6053
		public bool pauseOnError;

		// Token: 0x040017A6 RID: 6054
		public bool resetModsConfigOnCrash = true;

		// Token: 0x040017A7 RID: 6055
		public bool simulateNotOwningRoyalty;

		// Token: 0x040017A8 RID: 6056
		public bool simulateNotOwningIdeology;
	}
}
