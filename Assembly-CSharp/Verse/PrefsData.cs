using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200088E RID: 2190
	public class PrefsData
	{
		// Token: 0x0600365E RID: 13918 RVA: 0x0015BAE0 File Offset: 0x00159CE0
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

		// Token: 0x040025C5 RID: 9669
		public float volumeGame = 0.8f;

		// Token: 0x040025C6 RID: 9670
		public float volumeMusic = 0.4f;

		// Token: 0x040025C7 RID: 9671
		public float volumeAmbient = 1f;

		// Token: 0x040025C8 RID: 9672
		public int screenWidth;

		// Token: 0x040025C9 RID: 9673
		public int screenHeight;

		// Token: 0x040025CA RID: 9674
		public bool fullscreen;

		// Token: 0x040025CB RID: 9675
		public float uiScale = 1f;

		// Token: 0x040025CC RID: 9676
		public bool customCursorEnabled = true;

		// Token: 0x040025CD RID: 9677
		public bool hatsOnlyOnMap;

		// Token: 0x040025CE RID: 9678
		public bool plantWindSway = true;

		// Token: 0x040025CF RID: 9679
		public bool showRealtimeClock;

		// Token: 0x040025D0 RID: 9680
		public AnimalNameDisplayMode animalNameMode;

		// Token: 0x040025D1 RID: 9681
		[Obsolete]
		public bool extremeDifficultyUnlocked;

		// Token: 0x040025D2 RID: 9682
		public bool adaptiveTrainingEnabled = true;

		// Token: 0x040025D3 RID: 9683
		public List<string> preferredNames = new List<string>();

		// Token: 0x040025D4 RID: 9684
		public bool resourceReadoutCategorized;

		// Token: 0x040025D5 RID: 9685
		public bool runInBackground;

		// Token: 0x040025D6 RID: 9686
		public bool edgeScreenScroll = true;

		// Token: 0x040025D7 RID: 9687
		public TemperatureDisplayMode temperatureMode;

		// Token: 0x040025D8 RID: 9688
		public float autosaveIntervalDays = 1f;

		// Token: 0x040025D9 RID: 9689
		public bool testMapSizes;

		// Token: 0x040025DA RID: 9690
		[LoadAlias("maxNumberOfPlayerHomes")]
		public int maxNumberOfPlayerSettlements = 1;

		// Token: 0x040025DB RID: 9691
		public bool pauseOnLoad;

		// Token: 0x040025DC RID: 9692
		public AutomaticPauseMode automaticPauseMode = AutomaticPauseMode.MajorThreat;

		// Token: 0x040025DD RID: 9693
		public float mapDragSensitivity = 1.3f;

		// Token: 0x040025DE RID: 9694
		[Unsaved(true)]
		public bool? pauseOnUrgentLetter;

		// Token: 0x040025DF RID: 9695
		public bool devMode;

		// Token: 0x040025E0 RID: 9696
		public string langFolderName = "unknown";

		// Token: 0x040025E1 RID: 9697
		public bool logVerbose;

		// Token: 0x040025E2 RID: 9698
		public bool pauseOnError;

		// Token: 0x040025E3 RID: 9699
		public bool resetModsConfigOnCrash = true;

		// Token: 0x040025E4 RID: 9700
		public bool simulateNotOwningRoyalty;
	}
}
