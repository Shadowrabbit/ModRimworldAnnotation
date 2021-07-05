using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C83 RID: 3203
	public static class OverlayDrawHandler
	{
		// Token: 0x06004AC0 RID: 19136 RVA: 0x0018B1C8 File Offset: 0x001893C8
		public static void DrawPowerGridOverlayThisFrame()
		{
			OverlayDrawHandler.lastPowerGridDrawFrame = Time.frameCount;
		}

		// Token: 0x17000CE8 RID: 3304
		// (get) Token: 0x06004AC1 RID: 19137 RVA: 0x0018B1D4 File Offset: 0x001893D4
		public static bool ShouldDrawPowerGrid
		{
			get
			{
				return OverlayDrawHandler.lastPowerGridDrawFrame + 1 >= Time.frameCount;
			}
		}

		// Token: 0x06004AC2 RID: 19138 RVA: 0x0018B1E7 File Offset: 0x001893E7
		public static void DrawZonesThisFrame()
		{
			OverlayDrawHandler.lastZoneDrawFrame = Time.frameCount;
		}

		// Token: 0x17000CE9 RID: 3305
		// (get) Token: 0x06004AC3 RID: 19139 RVA: 0x0018B1F3 File Offset: 0x001893F3
		public static bool ShouldDrawZones
		{
			get
			{
				return Find.PlaySettings.showZones || Time.frameCount <= OverlayDrawHandler.lastZoneDrawFrame + 1;
			}
		}

		// Token: 0x04002D56 RID: 11606
		private static int lastPowerGridDrawFrame;

		// Token: 0x04002D57 RID: 11607
		private static int lastZoneDrawFrame;
	}
}
