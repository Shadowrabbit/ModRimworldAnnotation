using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C4A RID: 7242
	[DefOf]
	public static class GameConditionDefOf
	{
		// Token: 0x06009F4D RID: 40781 RVA: 0x0006A18A File Offset: 0x0006838A
		static GameConditionDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(GameConditionDefOf));
		}

		// Token: 0x0400674F RID: 26447
		public static GameConditionDef SolarFlare;

		// Token: 0x04006750 RID: 26448
		public static GameConditionDef Eclipse;

		// Token: 0x04006751 RID: 26449
		public static GameConditionDef PsychicDrone;

		// Token: 0x04006752 RID: 26450
		public static GameConditionDef PsychicSoothe;

		// Token: 0x04006753 RID: 26451
		public static GameConditionDef HeatWave;

		// Token: 0x04006754 RID: 26452
		public static GameConditionDef ColdSnap;

		// Token: 0x04006755 RID: 26453
		public static GameConditionDef Flashstorm;

		// Token: 0x04006756 RID: 26454
		public static GameConditionDef VolcanicWinter;

		// Token: 0x04006757 RID: 26455
		public static GameConditionDef ToxicFallout;

		// Token: 0x04006758 RID: 26456
		public static GameConditionDef Aurora;

		// Token: 0x04006759 RID: 26457
		[MayRequireRoyalty]
		public static GameConditionDef PsychicSuppression;

		// Token: 0x0400675A RID: 26458
		[MayRequireRoyalty]
		public static GameConditionDef WeatherController;

		// Token: 0x0400675B RID: 26459
		[MayRequireRoyalty]
		public static GameConditionDef EMIField;

		// Token: 0x0400675C RID: 26460
		[MayRequireRoyalty]
		public static GameConditionDef ToxicSpewer;
	}
}
