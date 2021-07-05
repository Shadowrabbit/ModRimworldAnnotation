using System;
using RimWorld.Planet;
using UnityEngine;
using UnityStandardAssets.ImageEffects;

namespace Verse
{
	// Token: 0x02000144 RID: 324
	public static class Current
	{
		// Token: 0x170001EB RID: 491
		// (get) Token: 0x060008F1 RID: 2289 RVA: 0x00029803 File Offset: 0x00027A03
		public static Root Root
		{
			get
			{
				return Current.rootInt;
			}
		}

		// Token: 0x170001EC RID: 492
		// (get) Token: 0x060008F2 RID: 2290 RVA: 0x0002980A File Offset: 0x00027A0A
		public static Root_Entry Root_Entry
		{
			get
			{
				return Current.rootEntryInt;
			}
		}

		// Token: 0x170001ED RID: 493
		// (get) Token: 0x060008F3 RID: 2291 RVA: 0x00029811 File Offset: 0x00027A11
		public static Root_Play Root_Play
		{
			get
			{
				return Current.rootPlayInt;
			}
		}

		// Token: 0x170001EE RID: 494
		// (get) Token: 0x060008F4 RID: 2292 RVA: 0x00029818 File Offset: 0x00027A18
		public static Camera Camera
		{
			get
			{
				return Current.cameraInt;
			}
		}

		// Token: 0x170001EF RID: 495
		// (get) Token: 0x060008F5 RID: 2293 RVA: 0x0002981F File Offset: 0x00027A1F
		public static CameraDriver CameraDriver
		{
			get
			{
				return Current.cameraDriverInt;
			}
		}

		// Token: 0x170001F0 RID: 496
		// (get) Token: 0x060008F6 RID: 2294 RVA: 0x00029826 File Offset: 0x00027A26
		public static ColorCorrectionCurves ColorCorrectionCurves
		{
			get
			{
				return Current.colorCorrectionCurvesInt;
			}
		}

		// Token: 0x170001F1 RID: 497
		// (get) Token: 0x060008F7 RID: 2295 RVA: 0x0002982D File Offset: 0x00027A2D
		public static SubcameraDriver SubcameraDriver
		{
			get
			{
				return Current.subcameraDriverInt;
			}
		}

		// Token: 0x170001F2 RID: 498
		// (get) Token: 0x060008F8 RID: 2296 RVA: 0x00029834 File Offset: 0x00027A34
		// (set) Token: 0x060008F9 RID: 2297 RVA: 0x0002983B File Offset: 0x00027A3B
		public static Game Game
		{
			get
			{
				return Current.gameInt;
			}
			set
			{
				Current.gameInt = value;
			}
		}

		// Token: 0x170001F3 RID: 499
		// (get) Token: 0x060008FA RID: 2298 RVA: 0x00029843 File Offset: 0x00027A43
		// (set) Token: 0x060008FB RID: 2299 RVA: 0x0002984A File Offset: 0x00027A4A
		public static World CreatingWorld
		{
			get
			{
				return Current.creatingWorldInt;
			}
			set
			{
				Current.creatingWorldInt = value;
			}
		}

		// Token: 0x170001F4 RID: 500
		// (get) Token: 0x060008FC RID: 2300 RVA: 0x00029852 File Offset: 0x00027A52
		// (set) Token: 0x060008FD RID: 2301 RVA: 0x00029859 File Offset: 0x00027A59
		public static ProgramState ProgramState
		{
			get
			{
				return Current.programStateInt;
			}
			set
			{
				Current.programStateInt = value;
			}
		}

		// Token: 0x060008FE RID: 2302 RVA: 0x00029864 File Offset: 0x00027A64
		public static void Notify_LoadedSceneChanged()
		{
			Current.cameraInt = GameObject.Find("Camera").GetComponent<Camera>();
			if (GenScene.InEntryScene)
			{
				Current.ProgramState = ProgramState.Entry;
				Current.rootEntryInt = GameObject.Find("GameRoot").GetComponent<Root_Entry>();
				Current.rootPlayInt = null;
				Current.rootInt = Current.rootEntryInt;
				Current.cameraDriverInt = null;
				Current.colorCorrectionCurvesInt = null;
				return;
			}
			if (GenScene.InPlayScene)
			{
				Current.ProgramState = ProgramState.MapInitializing;
				Current.rootEntryInt = null;
				Current.rootPlayInt = GameObject.Find("GameRoot").GetComponent<Root_Play>();
				Current.rootInt = Current.rootPlayInt;
				Current.cameraDriverInt = Current.cameraInt.GetComponent<CameraDriver>();
				Current.colorCorrectionCurvesInt = Current.cameraInt.GetComponent<ColorCorrectionCurves>();
				Current.subcameraDriverInt = GameObject.Find("Subcameras").GetComponent<SubcameraDriver>();
			}
		}

		// Token: 0x0400082F RID: 2095
		private static ProgramState programStateInt;

		// Token: 0x04000830 RID: 2096
		private static Root rootInt;

		// Token: 0x04000831 RID: 2097
		private static Root_Entry rootEntryInt;

		// Token: 0x04000832 RID: 2098
		private static Root_Play rootPlayInt;

		// Token: 0x04000833 RID: 2099
		private static Camera cameraInt;

		// Token: 0x04000834 RID: 2100
		private static CameraDriver cameraDriverInt;

		// Token: 0x04000835 RID: 2101
		private static ColorCorrectionCurves colorCorrectionCurvesInt;

		// Token: 0x04000836 RID: 2102
		private static SubcameraDriver subcameraDriverInt;

		// Token: 0x04000837 RID: 2103
		private static Game gameInt;

		// Token: 0x04000838 RID: 2104
		private static World creatingWorldInt;
	}
}
