using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200140A RID: 5130
	[DefOf]
	public static class GameConditionDefOf
	{
		// Token: 0x06007CFD RID: 31997 RVA: 0x002C45E9 File Offset: 0x002C27E9
		static GameConditionDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(GameConditionDefOf));
		}

		// Token: 0x04004772 RID: 18290
		public static GameConditionDef SolarFlare;

		// Token: 0x04004773 RID: 18291
		public static GameConditionDef Eclipse;

		// Token: 0x04004774 RID: 18292
		public static GameConditionDef PsychicDrone;

		// Token: 0x04004775 RID: 18293
		public static GameConditionDef PsychicSoothe;

		// Token: 0x04004776 RID: 18294
		public static GameConditionDef HeatWave;

		// Token: 0x04004777 RID: 18295
		public static GameConditionDef ColdSnap;

		// Token: 0x04004778 RID: 18296
		public static GameConditionDef Flashstorm;

		// Token: 0x04004779 RID: 18297
		public static GameConditionDef VolcanicWinter;

		// Token: 0x0400477A RID: 18298
		public static GameConditionDef ToxicFallout;

		// Token: 0x0400477B RID: 18299
		public static GameConditionDef Aurora;

		// Token: 0x0400477C RID: 18300
		[MayRequireRoyalty]
		public static GameConditionDef PsychicSuppression;

		// Token: 0x0400477D RID: 18301
		[MayRequireRoyalty]
		public static GameConditionDef WeatherController;

		// Token: 0x0400477E RID: 18302
		[MayRequireRoyalty]
		public static GameConditionDef EMIField;

		// Token: 0x0400477F RID: 18303
		[MayRequireRoyalty]
		public static GameConditionDef ToxicSpewer;
	}
}
