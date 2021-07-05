using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C47 RID: 7239
	[DefOf]
	public static class EffecterDefOf
	{
		// Token: 0x06009F4A RID: 40778 RVA: 0x0006A157 File Offset: 0x00068357
		static EffecterDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(EffecterDefOf));
		}

		// Token: 0x0400671E RID: 26398
		public static EffecterDef Clean;

		// Token: 0x0400671F RID: 26399
		public static EffecterDef ConstructMetal;

		// Token: 0x04006720 RID: 26400
		public static EffecterDef ConstructWood;

		// Token: 0x04006721 RID: 26401
		public static EffecterDef ConstructDirt;

		// Token: 0x04006722 RID: 26402
		public static EffecterDef RoofWork;

		// Token: 0x04006723 RID: 26403
		public static EffecterDef EatMeat;

		// Token: 0x04006724 RID: 26404
		public static EffecterDef ProgressBar;

		// Token: 0x04006725 RID: 26405
		public static EffecterDef Mine;

		// Token: 0x04006726 RID: 26406
		public static EffecterDef Deflect_Metal;

		// Token: 0x04006727 RID: 26407
		public static EffecterDef Deflect_Metal_Bullet;

		// Token: 0x04006728 RID: 26408
		public static EffecterDef Deflect_General;

		// Token: 0x04006729 RID: 26409
		public static EffecterDef Deflect_General_Bullet;

		// Token: 0x0400672A RID: 26410
		public static EffecterDef DamageDiminished_Metal;

		// Token: 0x0400672B RID: 26411
		public static EffecterDef DamageDiminished_General;

		// Token: 0x0400672C RID: 26412
		public static EffecterDef Drill;

		// Token: 0x0400672D RID: 26413
		public static EffecterDef Research;

		// Token: 0x0400672E RID: 26414
		public static EffecterDef ClearSnow;

		// Token: 0x0400672F RID: 26415
		public static EffecterDef Sow;

		// Token: 0x04006730 RID: 26416
		public static EffecterDef Harvest;

		// Token: 0x04006731 RID: 26417
		public static EffecterDef Vomit;

		// Token: 0x04006732 RID: 26418
		public static EffecterDef PlayPoker;

		// Token: 0x04006733 RID: 26419
		public static EffecterDef Interceptor_BlockedProjectile;

		// Token: 0x04006734 RID: 26420
		public static EffecterDef DisabledByEMP;

		// Token: 0x04006735 RID: 26421
		[MayRequireRoyalty]
		public static EffecterDef ActivatorProximityTriggered;

		// Token: 0x04006736 RID: 26422
		[MayRequireRoyalty]
		public static EffecterDef Skip_Entry;

		// Token: 0x04006737 RID: 26423
		[MayRequireRoyalty]
		public static EffecterDef Skip_Exit;

		// Token: 0x04006738 RID: 26424
		[MayRequireRoyalty]
		public static EffecterDef Skip_EntryNoDelay;

		// Token: 0x04006739 RID: 26425
		[MayRequireRoyalty]
		public static EffecterDef Skip_ExitNoDelay;
	}
}
