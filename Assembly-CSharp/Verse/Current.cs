using System;
using RimWorld.Planet;
using UnityEngine;
using UnityStandardAssets.ImageEffects;

namespace Verse
{
	// Token: 0x020001E9 RID: 489
	public static class Current
	{
		// Token: 0x1700028F RID: 655
		// (get) Token: 0x06000CB0 RID: 3248 RVA: 0x0000FCBA File Offset: 0x0000DEBA
		public static Root Root
		{
			get
			{
				return Current.rootInt;
			}
		}

		// Token: 0x17000290 RID: 656
		// (get) Token: 0x06000CB1 RID: 3249 RVA: 0x0000FCC1 File Offset: 0x0000DEC1
		public static Root_Entry Root_Entry
		{
			get
			{
				return Current.rootEntryInt;
			}
		}

		// Token: 0x17000291 RID: 657
		// (get) Token: 0x06000CB2 RID: 3250 RVA: 0x0000FCC8 File Offset: 0x0000DEC8
		public static Root_Play Root_Play
		{
			get
			{
				return Current.rootPlayInt;
			}
		}

		// Token: 0x17000292 RID: 658
		// (get) Token: 0x06000CB3 RID: 3251 RVA: 0x0000FCCF File Offset: 0x0000DECF
		public static Camera Camera
		{
			get
			{
				return Current.cameraInt;
			}
		}

		// Token: 0x17000293 RID: 659
		// (get) Token: 0x06000CB4 RID: 3252 RVA: 0x0000FCD6 File Offset: 0x0000DED6
		public static CameraDriver CameraDriver
		{
			get
			{
				return Current.cameraDriverInt;
			}
		}

		// Token: 0x17000294 RID: 660
		// (get) Token: 0x06000CB5 RID: 3253 RVA: 0x0000FCDD File Offset: 0x0000DEDD
		public static ColorCorrectionCurves ColorCorrectionCurves
		{
			get
			{
				return Current.colorCorrectionCurvesInt;
			}
		}

		// Token: 0x17000295 RID: 661
		// (get) Token: 0x06000CB6 RID: 3254 RVA: 0x0000FCE4 File Offset: 0x0000DEE4
		public static SubcameraDriver SubcameraDriver
		{
			get
			{
				return Current.subcameraDriverInt;
			}
		}

		// Token: 0x17000296 RID: 662
		// (get) Token: 0x06000CB7 RID: 3255 RVA: 0x0000FCEB File Offset: 0x0000DEEB
		// (set) Token: 0x06000CB8 RID: 3256 RVA: 0x0000FCF2 File Offset: 0x0000DEF2
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

		// Token: 0x17000297 RID: 663
		// (get) Token: 0x06000CB9 RID: 3257 RVA: 0x0000FCFA File Offset: 0x0000DEFA
		// (set) Token: 0x06000CBA RID: 3258 RVA: 0x0000FD01 File Offset: 0x0000DF01
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

		// Token: 0x17000298 RID: 664
		// (get) Token: 0x06000CBB RID: 3259 RVA: 0x0000FD09 File Offset: 0x0000DF09
		// (set) Token: 0x06000CBC RID: 3260 RVA: 0x0000FD10 File Offset: 0x0000DF10
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

		// Token: 0x06000CBD RID: 3261 RVA: 0x000A5488 File Offset: 0x000A3688
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

		// Token: 0x04000AF4 RID: 2804
		private static ProgramState programStateInt;

		// Token: 0x04000AF5 RID: 2805
		private static Root rootInt;

		// Token: 0x04000AF6 RID: 2806
		private static Root_Entry rootEntryInt;

		// Token: 0x04000AF7 RID: 2807
		private static Root_Play rootPlayInt;

		// Token: 0x04000AF8 RID: 2808
		private static Camera cameraInt;

		// Token: 0x04000AF9 RID: 2809
		private static CameraDriver cameraDriverInt;

		// Token: 0x04000AFA RID: 2810
		private static ColorCorrectionCurves colorCorrectionCurvesInt;

		// Token: 0x04000AFB RID: 2811
		private static SubcameraDriver subcameraDriverInt;

		// Token: 0x04000AFC RID: 2812
		private static Game gameInt;

		// Token: 0x04000AFD RID: 2813
		private static World creatingWorldInt;
	}
}
