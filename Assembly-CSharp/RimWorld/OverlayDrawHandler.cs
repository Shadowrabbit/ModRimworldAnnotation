using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001271 RID: 4721
	public static class OverlayDrawHandler
	{
		// Token: 0x060066F7 RID: 26359 RVA: 0x000465E1 File Offset: 0x000447E1
		public static void DrawPowerGridOverlayThisFrame()
		{
			OverlayDrawHandler.lastPowerGridDrawFrame = Time.frameCount;
		}

		// Token: 0x17000FEE RID: 4078
		// (get) Token: 0x060066F8 RID: 26360 RVA: 0x000465ED File Offset: 0x000447ED
		public static bool ShouldDrawPowerGrid
		{
			get
			{
				return OverlayDrawHandler.lastPowerGridDrawFrame + 1 >= Time.frameCount;
			}
		}

		// Token: 0x060066F9 RID: 26361 RVA: 0x00046600 File Offset: 0x00044800
		public static void DrawZonesThisFrame()
		{
			OverlayDrawHandler.lastZoneDrawFrame = Time.frameCount;
		}

		// Token: 0x17000FEF RID: 4079
		// (get) Token: 0x060066FA RID: 26362 RVA: 0x0004660C File Offset: 0x0004480C
		public static bool ShouldDrawZones
		{
			get
			{
				return Find.PlaySettings.showZones || Time.frameCount <= OverlayDrawHandler.lastZoneDrawFrame + 1;
			}
		}

		// Token: 0x04004475 RID: 17525
		private static int lastPowerGridDrawFrame;

		// Token: 0x04004476 RID: 17526
		private static int lastZoneDrawFrame;
	}
}
