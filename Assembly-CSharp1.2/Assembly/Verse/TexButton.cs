using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000782 RID: 1922
	[StaticConstructorOnStartup]
	internal class TexButton
	{
		// Token: 0x040020C5 RID: 8389
		public static readonly Texture2D CloseXBig = ContentFinder<Texture2D>.Get("UI/Widgets/CloseX", true);

		// Token: 0x040020C6 RID: 8390
		public static readonly Texture2D CloseXSmall = ContentFinder<Texture2D>.Get("UI/Widgets/CloseXSmall", true);

		// Token: 0x040020C7 RID: 8391
		public static readonly Texture2D NextBig = ContentFinder<Texture2D>.Get("UI/Widgets/NextArrow", true);

		// Token: 0x040020C8 RID: 8392
		public static readonly Texture2D DeleteX = ContentFinder<Texture2D>.Get("UI/Buttons/Delete", true);

		// Token: 0x040020C9 RID: 8393
		public static readonly Texture2D ReorderUp = ContentFinder<Texture2D>.Get("UI/Buttons/ReorderUp", true);

		// Token: 0x040020CA RID: 8394
		public static readonly Texture2D ReorderDown = ContentFinder<Texture2D>.Get("UI/Buttons/ReorderDown", true);

		// Token: 0x040020CB RID: 8395
		public static readonly Texture2D Plus = ContentFinder<Texture2D>.Get("UI/Buttons/Plus", true);

		// Token: 0x040020CC RID: 8396
		public static readonly Texture2D Minus = ContentFinder<Texture2D>.Get("UI/Buttons/Minus", true);

		// Token: 0x040020CD RID: 8397
		public static readonly Texture2D Suspend = ContentFinder<Texture2D>.Get("UI/Buttons/Suspend", true);

		// Token: 0x040020CE RID: 8398
		public static readonly Texture2D SelectOverlappingNext = ContentFinder<Texture2D>.Get("UI/Buttons/SelectNextOverlapping", true);

		// Token: 0x040020CF RID: 8399
		public static readonly Texture2D Info = ContentFinder<Texture2D>.Get("UI/Buttons/InfoButton", true);

		// Token: 0x040020D0 RID: 8400
		public static readonly Texture2D Rename = ContentFinder<Texture2D>.Get("UI/Buttons/Rename", true);

		// Token: 0x040020D1 RID: 8401
		public static readonly Texture2D Banish = ContentFinder<Texture2D>.Get("UI/Buttons/Banish", true);

		// Token: 0x040020D2 RID: 8402
		public static readonly Texture2D OpenStatsReport = ContentFinder<Texture2D>.Get("UI/Buttons/OpenStatsReport", true);

		// Token: 0x040020D3 RID: 8403
		public static readonly Texture2D RenounceTitle = ContentFinder<Texture2D>.Get("UI/Buttons/Renounce", true);

		// Token: 0x040020D4 RID: 8404
		public static readonly Texture2D Copy = ContentFinder<Texture2D>.Get("UI/Buttons/Copy", true);

		// Token: 0x040020D5 RID: 8405
		public static readonly Texture2D Paste = ContentFinder<Texture2D>.Get("UI/Buttons/Paste", true);

		// Token: 0x040020D6 RID: 8406
		public static readonly Texture2D Drop = ContentFinder<Texture2D>.Get("UI/Buttons/Drop", true);

		// Token: 0x040020D7 RID: 8407
		public static readonly Texture2D Ingest = ContentFinder<Texture2D>.Get("UI/Buttons/Ingest", true);

		// Token: 0x040020D8 RID: 8408
		public static readonly Texture2D DragHash = ContentFinder<Texture2D>.Get("UI/Buttons/DragHash", true);

		// Token: 0x040020D9 RID: 8409
		public static readonly Texture2D ToggleLog = ContentFinder<Texture2D>.Get("UI/Buttons/DevRoot/ToggleLog", true);

		// Token: 0x040020DA RID: 8410
		public static readonly Texture2D OpenDebugActionsMenu = ContentFinder<Texture2D>.Get("UI/Buttons/DevRoot/OpenDebugActionsMenu", true);

		// Token: 0x040020DB RID: 8411
		public static readonly Texture2D OpenInspector = ContentFinder<Texture2D>.Get("UI/Buttons/DevRoot/OpenInspector", true);

		// Token: 0x040020DC RID: 8412
		public static readonly Texture2D OpenInspectSettings = ContentFinder<Texture2D>.Get("UI/Buttons/DevRoot/OpenInspectSettings", true);

		// Token: 0x040020DD RID: 8413
		public static readonly Texture2D ToggleGodMode = ContentFinder<Texture2D>.Get("UI/Buttons/DevRoot/ToggleGodMode", true);

		// Token: 0x040020DE RID: 8414
		public static readonly Texture2D TogglePauseOnError = ContentFinder<Texture2D>.Get("UI/Buttons/DevRoot/TogglePauseOnError", true);

		// Token: 0x040020DF RID: 8415
		public static readonly Texture2D ToggleTweak = ContentFinder<Texture2D>.Get("UI/Buttons/DevRoot/ToggleTweak", true);

		// Token: 0x040020E0 RID: 8416
		public static readonly Texture2D Add = ContentFinder<Texture2D>.Get("UI/Buttons/Dev/Add", true);

		// Token: 0x040020E1 RID: 8417
		public static readonly Texture2D NewItem = ContentFinder<Texture2D>.Get("UI/Buttons/Dev/NewItem", true);

		// Token: 0x040020E2 RID: 8418
		public static readonly Texture2D Reveal = ContentFinder<Texture2D>.Get("UI/Buttons/Dev/Reveal", true);

		// Token: 0x040020E3 RID: 8419
		public static readonly Texture2D Collapse = ContentFinder<Texture2D>.Get("UI/Buttons/Dev/Collapse", true);

		// Token: 0x040020E4 RID: 8420
		public static readonly Texture2D Empty = ContentFinder<Texture2D>.Get("UI/Buttons/Dev/Empty", true);

		// Token: 0x040020E5 RID: 8421
		public static readonly Texture2D Save = ContentFinder<Texture2D>.Get("UI/Buttons/Dev/Save", true);

		// Token: 0x040020E6 RID: 8422
		public static readonly Texture2D NewFile = ContentFinder<Texture2D>.Get("UI/Buttons/Dev/NewFile", true);

		// Token: 0x040020E7 RID: 8423
		public static readonly Texture2D RenameDev = ContentFinder<Texture2D>.Get("UI/Buttons/Dev/Rename", true);

		// Token: 0x040020E8 RID: 8424
		public static readonly Texture2D Reload = ContentFinder<Texture2D>.Get("UI/Buttons/Dev/Reload", true);

		// Token: 0x040020E9 RID: 8425
		public static readonly Texture2D Play = ContentFinder<Texture2D>.Get("UI/Buttons/Dev/Play", true);

		// Token: 0x040020EA RID: 8426
		public static readonly Texture2D Stop = ContentFinder<Texture2D>.Get("UI/Buttons/Dev/Stop", true);

		// Token: 0x040020EB RID: 8427
		public static readonly Texture2D RangeMatch = ContentFinder<Texture2D>.Get("UI/Buttons/Dev/RangeMatch", true);

		// Token: 0x040020EC RID: 8428
		public static readonly Texture2D InspectModeToggle = ContentFinder<Texture2D>.Get("UI/Buttons/Dev/InspectModeToggle", true);

		// Token: 0x040020ED RID: 8429
		public static readonly Texture2D CenterOnPointsTex = ContentFinder<Texture2D>.Get("UI/Buttons/Dev/CenterOnPoints", true);

		// Token: 0x040020EE RID: 8430
		public static readonly Texture2D CurveResetTex = ContentFinder<Texture2D>.Get("UI/Buttons/Dev/CurveReset", true);

		// Token: 0x040020EF RID: 8431
		public static readonly Texture2D QuickZoomHor1Tex = ContentFinder<Texture2D>.Get("UI/Buttons/Dev/QuickZoomHor1", true);

		// Token: 0x040020F0 RID: 8432
		public static readonly Texture2D QuickZoomHor100Tex = ContentFinder<Texture2D>.Get("UI/Buttons/Dev/QuickZoomHor100", true);

		// Token: 0x040020F1 RID: 8433
		public static readonly Texture2D QuickZoomHor20kTex = ContentFinder<Texture2D>.Get("UI/Buttons/Dev/QuickZoomHor20k", true);

		// Token: 0x040020F2 RID: 8434
		public static readonly Texture2D QuickZoomVer1Tex = ContentFinder<Texture2D>.Get("UI/Buttons/Dev/QuickZoomVer1", true);

		// Token: 0x040020F3 RID: 8435
		public static readonly Texture2D QuickZoomVer100Tex = ContentFinder<Texture2D>.Get("UI/Buttons/Dev/QuickZoomVer100", true);

		// Token: 0x040020F4 RID: 8436
		public static readonly Texture2D QuickZoomVer20kTex = ContentFinder<Texture2D>.Get("UI/Buttons/Dev/QuickZoomVer20k", true);

		// Token: 0x040020F5 RID: 8437
		public static readonly Texture2D IconBlog = ContentFinder<Texture2D>.Get("UI/HeroArt/WebIcons/Blog", true);

		// Token: 0x040020F6 RID: 8438
		public static readonly Texture2D IconForums = ContentFinder<Texture2D>.Get("UI/HeroArt/WebIcons/Forums", true);

		// Token: 0x040020F7 RID: 8439
		public static readonly Texture2D IconTwitter = ContentFinder<Texture2D>.Get("UI/HeroArt/WebIcons/Twitter", true);

		// Token: 0x040020F8 RID: 8440
		public static readonly Texture2D IconBook = ContentFinder<Texture2D>.Get("UI/HeroArt/WebIcons/Book", true);

		// Token: 0x040020F9 RID: 8441
		public static readonly Texture2D IconSoundtrack = ContentFinder<Texture2D>.Get("UI/HeroArt/WebIcons/Soundtrack", true);

		// Token: 0x040020FA RID: 8442
		public static readonly Texture2D ShowLearningHelper = ContentFinder<Texture2D>.Get("UI/Buttons/ShowLearningHelper", true);

		// Token: 0x040020FB RID: 8443
		public static readonly Texture2D ShowZones = ContentFinder<Texture2D>.Get("UI/Buttons/ShowZones", true);

		// Token: 0x040020FC RID: 8444
		public static readonly Texture2D ShowFertilityOverlay = ContentFinder<Texture2D>.Get("UI/Buttons/ShowFertilityOverlay", true);

		// Token: 0x040020FD RID: 8445
		public static readonly Texture2D ShowTerrainAffordanceOverlay = ContentFinder<Texture2D>.Get("UI/Buttons/ShowTerrainAffordanceOverlay", true);

		// Token: 0x040020FE RID: 8446
		public static readonly Texture2D ShowBeauty = ContentFinder<Texture2D>.Get("UI/Buttons/ShowBeauty", true);

		// Token: 0x040020FF RID: 8447
		public static readonly Texture2D ShowRoomStats = ContentFinder<Texture2D>.Get("UI/Buttons/ShowRoomStats", true);

		// Token: 0x04002100 RID: 8448
		public static readonly Texture2D ShowColonistBar = ContentFinder<Texture2D>.Get("UI/Buttons/ShowColonistBar", true);

		// Token: 0x04002101 RID: 8449
		public static readonly Texture2D ShowRoofOverlay = ContentFinder<Texture2D>.Get("UI/Buttons/ShowRoofOverlay", true);

		// Token: 0x04002102 RID: 8450
		public static readonly Texture2D AutoHomeArea = ContentFinder<Texture2D>.Get("UI/Buttons/AutoHomeArea", true);

		// Token: 0x04002103 RID: 8451
		public static readonly Texture2D AutoRebuild = ContentFinder<Texture2D>.Get("UI/Buttons/AutoRebuild", true);

		// Token: 0x04002104 RID: 8452
		public static readonly Texture2D CategorizedResourceReadout = ContentFinder<Texture2D>.Get("UI/Buttons/ResourceReadoutCategorized", true);

		// Token: 0x04002105 RID: 8453
		public static readonly Texture2D LockNorthUp = ContentFinder<Texture2D>.Get("UI/Buttons/LockNorthUp", true);

		// Token: 0x04002106 RID: 8454
		public static readonly Texture2D UsePlanetDayNightSystem = ContentFinder<Texture2D>.Get("UI/Buttons/UsePlanetDayNightSystem", true);

		// Token: 0x04002107 RID: 8455
		public static readonly Texture2D ShowExpandingIcons = ContentFinder<Texture2D>.Get("UI/Buttons/ShowExpandingIcons", true);

		// Token: 0x04002108 RID: 8456
		public static readonly Texture2D ShowWorldFeatures = ContentFinder<Texture2D>.Get("UI/Buttons/ShowWorldFeatures", true);

		// Token: 0x04002109 RID: 8457
		public static readonly Texture2D[] SpeedButtonTextures = new Texture2D[]
		{
			ContentFinder<Texture2D>.Get("UI/TimeControls/TimeSpeedButton_Pause", true),
			ContentFinder<Texture2D>.Get("UI/TimeControls/TimeSpeedButton_Normal", true),
			ContentFinder<Texture2D>.Get("UI/TimeControls/TimeSpeedButton_Fast", true),
			ContentFinder<Texture2D>.Get("UI/TimeControls/TimeSpeedButton_Superfast", true),
			ContentFinder<Texture2D>.Get("UI/TimeControls/TimeSpeedButton_Superfast", true)
		};
	}
}
