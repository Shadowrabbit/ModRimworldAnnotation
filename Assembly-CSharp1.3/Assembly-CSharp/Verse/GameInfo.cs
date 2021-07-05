using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000134 RID: 308
	public sealed class GameInfo : IExposable
	{
		// Token: 0x170001D0 RID: 464
		// (get) Token: 0x0600086F RID: 2159 RVA: 0x00027850 File Offset: 0x00025A50
		public float RealPlayTimeInteracting
		{
			get
			{
				return this.realPlayTimeInteracting;
			}
		}

		// Token: 0x06000870 RID: 2160 RVA: 0x00027858 File Offset: 0x00025A58
		public void GameInfoOnGUI()
		{
			if (Event.current.type == EventType.MouseDown || Event.current.type == EventType.MouseMove || Event.current.type == EventType.KeyDown)
			{
				this.lastInputRealTime = Time.realtimeSinceStartup;
			}
		}

		// Token: 0x06000871 RID: 2161 RVA: 0x0002788C File Offset: 0x00025A8C
		public void GameInfoUpdate()
		{
			if (Time.realtimeSinceStartup < this.lastInputRealTime + 90f && Find.MainTabsRoot.OpenTab != MainButtonDefOf.Menu && Current.ProgramState == ProgramState.Playing && !Find.WindowStack.IsOpen<Dialog_Options>())
			{
				this.realPlayTimeInteracting += RealTime.realDeltaTime;
			}
		}

		// Token: 0x06000872 RID: 2162 RVA: 0x000278E3 File Offset: 0x00025AE3
		public void ExposeData()
		{
			Scribe_Values.Look<float>(ref this.realPlayTimeInteracting, "realPlayTimeInteracting", 0f, false);
			Scribe_Values.Look<bool>(ref this.permadeathMode, "permadeathMode", false, false);
			Scribe_Values.Look<string>(ref this.permadeathModeUniqueName, "permadeathModeUniqueName", null, false);
		}

		// Token: 0x040007E3 RID: 2019
		public bool permadeathMode;

		// Token: 0x040007E4 RID: 2020
		public string permadeathModeUniqueName;

		// Token: 0x040007E5 RID: 2021
		private float realPlayTimeInteracting;

		// Token: 0x040007E6 RID: 2022
		private float lastInputRealTime;
	}
}
