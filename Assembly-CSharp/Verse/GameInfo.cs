using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020001D4 RID: 468
	public sealed class GameInfo : IExposable
	{
		// Token: 0x1700026E RID: 622
		// (get) Token: 0x06000C1B RID: 3099 RVA: 0x0000F4DE File Offset: 0x0000D6DE
		public float RealPlayTimeInteracting
		{
			get
			{
				return this.realPlayTimeInteracting;
			}
		}

		// Token: 0x06000C1C RID: 3100 RVA: 0x0000F4E6 File Offset: 0x0000D6E6
		public void GameInfoOnGUI()
		{
			if (Event.current.type == EventType.MouseDown || Event.current.type == EventType.MouseMove || Event.current.type == EventType.KeyDown)
			{
				this.lastInputRealTime = Time.realtimeSinceStartup;
			}
		}

		// Token: 0x06000C1D RID: 3101 RVA: 0x000A3604 File Offset: 0x000A1804
		public void GameInfoUpdate()
		{
			if (Time.realtimeSinceStartup < this.lastInputRealTime + 90f && Find.MainTabsRoot.OpenTab != MainButtonDefOf.Menu && Current.ProgramState == ProgramState.Playing && !Find.WindowStack.IsOpen<Dialog_Options>())
			{
				this.realPlayTimeInteracting += RealTime.realDeltaTime;
			}
		}

		// Token: 0x06000C1E RID: 3102 RVA: 0x0000F519 File Offset: 0x0000D719
		public void ExposeData()
		{
			Scribe_Values.Look<float>(ref this.realPlayTimeInteracting, "realPlayTimeInteracting", 0f, false);
			Scribe_Values.Look<bool>(ref this.permadeathMode, "permadeathMode", false, false);
			Scribe_Values.Look<string>(ref this.permadeathModeUniqueName, "permadeathModeUniqueName", null, false);
		}

		// Token: 0x04000A8B RID: 2699
		public bool permadeathMode;

		// Token: 0x04000A8C RID: 2700
		public string permadeathModeUniqueName;

		// Token: 0x04000A8D RID: 2701
		private float realPlayTimeInteracting;

		// Token: 0x04000A8E RID: 2702
		private float lastInputRealTime;
	}
}
