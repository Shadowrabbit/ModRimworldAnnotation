using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000D08 RID: 3336
	[StaticConstructorOnStartup]
	public class WeatherOverlay_Rain : SkyOverlay
	{
		// Token: 0x06004E06 RID: 19974 RVA: 0x001A2C10 File Offset: 0x001A0E10
		public WeatherOverlay_Rain()
		{
			this.worldOverlayMat = WeatherOverlay_Rain.RainOverlayWorld;
			this.worldOverlayPanSpeed1 = 0.015f;
			this.worldPanDir1 = new Vector2(-0.25f, -1f);
			this.worldPanDir1.Normalize();
			this.worldOverlayPanSpeed2 = 0.022f;
			this.worldPanDir2 = new Vector2(-0.24f, -1f);
			this.worldPanDir2.Normalize();
		}

		// Token: 0x04002F19 RID: 12057
		private static readonly Material RainOverlayWorld = MatLoader.LoadMat("Weather/RainOverlayWorld", -1);
	}
}
