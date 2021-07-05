using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001410 RID: 5136
	[DefOf]
	public static class DesignationDefOf
	{
		// Token: 0x06007D03 RID: 32003 RVA: 0x002C464F File Offset: 0x002C284F
		static DesignationDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(DesignationDefOf));
		}

		// Token: 0x04004867 RID: 18535
		public static DesignationDef Haul;

		// Token: 0x04004868 RID: 18536
		public static DesignationDef Mine;

		// Token: 0x04004869 RID: 18537
		public static DesignationDef Deconstruct;

		// Token: 0x0400486A RID: 18538
		public static DesignationDef Uninstall;

		// Token: 0x0400486B RID: 18539
		public static DesignationDef CutPlant;

		// Token: 0x0400486C RID: 18540
		public static DesignationDef HarvestPlant;

		// Token: 0x0400486D RID: 18541
		public static DesignationDef Hunt;

		// Token: 0x0400486E RID: 18542
		public static DesignationDef SmoothFloor;

		// Token: 0x0400486F RID: 18543
		public static DesignationDef RemoveFloor;

		// Token: 0x04004870 RID: 18544
		public static DesignationDef SmoothWall;

		// Token: 0x04004871 RID: 18545
		public static DesignationDef Flick;

		// Token: 0x04004872 RID: 18546
		public static DesignationDef Plan;

		// Token: 0x04004873 RID: 18547
		public static DesignationDef Strip;

		// Token: 0x04004874 RID: 18548
		public static DesignationDef Slaughter;

		// Token: 0x04004875 RID: 18549
		public static DesignationDef Tame;

		// Token: 0x04004876 RID: 18550
		public static DesignationDef Open;

		// Token: 0x04004877 RID: 18551
		public static DesignationDef ReleaseAnimalToWild;

		// Token: 0x04004878 RID: 18552
		public static DesignationDef Study;

		// Token: 0x04004879 RID: 18553
		[MayRequireIdeology]
		public static DesignationDef ExtractSkull;
	}
}
