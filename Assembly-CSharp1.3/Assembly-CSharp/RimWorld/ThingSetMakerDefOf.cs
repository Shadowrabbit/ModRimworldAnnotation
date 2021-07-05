using System;

namespace RimWorld
{
	// Token: 0x02001444 RID: 5188
	[DefOf]
	public static class ThingSetMakerDefOf
	{
		// Token: 0x06007D37 RID: 32055 RVA: 0x002C49C3 File Offset: 0x002C2BC3
		static ThingSetMakerDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(ThingSetMakerDefOf));
		}

		// Token: 0x04004CA7 RID: 19623
		public static ThingSetMakerDef MapGen_AncientTempleContents;

		// Token: 0x04004CA8 RID: 19624
		public static ThingSetMakerDef MapGen_AncientPodContents;

		// Token: 0x04004CA9 RID: 19625
		public static ThingSetMakerDef MapGen_DefaultStockpile;

		// Token: 0x04004CAA RID: 19626
		public static ThingSetMakerDef MapGen_PrisonCellStockpile;

		// Token: 0x04004CAB RID: 19627
		[MayRequireIdeology]
		public static ThingSetMakerDef MapGen_AncientComplexRoomLoot_Default;

		// Token: 0x04004CAC RID: 19628
		[MayRequireIdeology]
		public static ThingSetMakerDef MapGen_AncientComplexRoomLoot_Better;

		// Token: 0x04004CAD RID: 19629
		[MayRequireIdeology]
		public static ThingSetMakerDef MapGen_AncientComplex_SecurityCrate;

		// Token: 0x04004CAE RID: 19630
		public static ThingSetMakerDef Reward_ItemsStandard;

		// Token: 0x04004CAF RID: 19631
		public static ThingSetMakerDef DebugCaravanInventory;

		// Token: 0x04004CB0 RID: 19632
		public static ThingSetMakerDef DebugQuestDropPodsContents;

		// Token: 0x04004CB1 RID: 19633
		public static ThingSetMakerDef TraderStock;

		// Token: 0x04004CB2 RID: 19634
		public static ThingSetMakerDef ResourcePod;

		// Token: 0x04004CB3 RID: 19635
		public static ThingSetMakerDef RefugeePod;

		// Token: 0x04004CB4 RID: 19636
		public static ThingSetMakerDef Meteorite;

		// Token: 0x04004CB5 RID: 19637
		public static ThingSetMakerDef VisitorGift;

		// Token: 0x04004CB6 RID: 19638
		[MayRequireIdeology]
		public static ThingSetMakerDef Reward_ReliquaryPilgrims;
	}
}
