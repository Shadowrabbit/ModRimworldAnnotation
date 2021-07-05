using System;

namespace RimWorld
{
	// Token: 0x02001470 RID: 5232
	[DefOf]
	public static class PreceptDefOf
	{
		// Token: 0x06007D62 RID: 32098 RVA: 0x002C4C9E File Offset: 0x002C2E9E
		static PreceptDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(PreceptDefOf));
		}

		// Token: 0x04004E15 RID: 19989
		[MayRequireRoyalty]
		public static PreceptDef AnimaTreeLinking;

		// Token: 0x04004E16 RID: 19990
		[MayRequireIdeology]
		public static PreceptDef Slavery_Acceptable;

		// Token: 0x04004E17 RID: 19991
		[MayRequireIdeology]
		public static PreceptDef Slavery_Honorable;

		// Token: 0x04004E18 RID: 19992
		[MayRequireIdeology]
		public static PreceptDef IdeoBuilding;

		// Token: 0x04004E19 RID: 19993
		[MayRequireIdeology]
		public static PreceptDef IdeoRelic;

		// Token: 0x04004E1A RID: 19994
		[MayRequireIdeology]
		public static PreceptDef AnimalVenerated;

		// Token: 0x04004E1B RID: 19995
		[MayRequireIdeology]
		public static PreceptDef NobleDespisedWeapons;

		// Token: 0x04004E1C RID: 19996
		[MayRequireIdeology]
		public static PreceptDef IdeoRole_Leader;

		// Token: 0x04004E1D RID: 19997
		[MayRequireIdeology]
		public static PreceptDef IdeoRole_Moralist;

		// Token: 0x04004E1E RID: 19998
		[MayRequireIdeology]
		public static PreceptDef AnimalConnection_Strong;

		// Token: 0x04004E1F RID: 19999
		[MayRequireIdeology]
		public static PreceptDef AgeReversal_Demanded;

		// Token: 0x04004E20 RID: 20000
		[MayRequireIdeology]
		public static PreceptDef NeuralSupercharge_Preferred;

		// Token: 0x04004E21 RID: 20001
		[MayRequireIdeology]
		public static PreceptDef Skullspike_Desired;

		// Token: 0x04004E22 RID: 20002
		[MayRequireIdeology]
		public static PreceptDef MeatEating_NonMeat_Disapproved;

		// Token: 0x04004E23 RID: 20003
		[MayRequireIdeology]
		public static PreceptDef MeatEating_NonMeat_Horrible;

		// Token: 0x04004E24 RID: 20004
		[MayRequireIdeology]
		public static PreceptDef MeatEating_NonMeat_Abhorrent;

		// Token: 0x04004E25 RID: 20005
		[MayRequireIdeology]
		public static PreceptDef Cannibalism_Preferred;

		// Token: 0x04004E26 RID: 20006
		[MayRequireIdeology]
		public static PreceptDef Cannibalism_RequiredStrong;

		// Token: 0x04004E27 RID: 20007
		[MayRequireIdeology]
		public static PreceptDef Cannibalism_RequiredRavenous;
	}
}
