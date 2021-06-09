using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C52 RID: 7250
	[DefOf]
	public static class PawnKindDefOf
	{
		// Token: 0x06009F55 RID: 40789 RVA: 0x0006A212 File Offset: 0x00068412
		static PawnKindDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(PawnKindDefOf));
		}

		// Token: 0x0400682D RID: 26669
		public static PawnKindDef Colonist;

		// Token: 0x0400682E RID: 26670
		public static PawnKindDef Slave;

		// Token: 0x0400682F RID: 26671
		public static PawnKindDef Villager;

		// Token: 0x04006830 RID: 26672
		public static PawnKindDef Drifter;

		// Token: 0x04006831 RID: 26673
		public static PawnKindDef SpaceRefugee;

		// Token: 0x04006832 RID: 26674
		public static PawnKindDef AncientSoldier;

		// Token: 0x04006833 RID: 26675
		public static PawnKindDef WildMan;

		// Token: 0x04006834 RID: 26676
		public static PawnKindDef Thrumbo;

		// Token: 0x04006835 RID: 26677
		public static PawnKindDef Alphabeaver;

		// Token: 0x04006836 RID: 26678
		public static PawnKindDef Muffalo;

		// Token: 0x04006837 RID: 26679
		public static PawnKindDef Megascarab;

		// Token: 0x04006838 RID: 26680
		public static PawnKindDef Spelopede;

		// Token: 0x04006839 RID: 26681
		public static PawnKindDef Megaspider;

		// Token: 0x0400683A RID: 26682
		[MayRequireRoyalty]
		public static PawnKindDef Empire_Royal_Bestower;

		// Token: 0x0400683B RID: 26683
		[MayRequireRoyalty]
		public static PawnKindDef Empire_Royal_NobleWimp;

		// Token: 0x0400683C RID: 26684
		[MayRequireRoyalty]
		public static PawnKindDef Empire_Fighter_Janissary;

		// Token: 0x0400683D RID: 26685
		[MayRequireRoyalty]
		public static PawnKindDef Empire_Fighter_Trooper;

		// Token: 0x0400683E RID: 26686
		[MayRequireRoyalty]
		public static PawnKindDef Empire_Fighter_Cataphract;

		// Token: 0x0400683F RID: 26687
		[MayRequireRoyalty]
		public static PawnKindDef Empire_Common_Lodger;

		// Token: 0x04006840 RID: 26688
		[MayRequireRoyalty]
		public static PawnKindDef Refugee;
	}
}
