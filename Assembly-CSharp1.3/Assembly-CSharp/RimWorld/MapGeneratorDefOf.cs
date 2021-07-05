using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200143D RID: 5181
	[DefOf]
	public static class MapGeneratorDefOf
	{
		// Token: 0x06007D30 RID: 32048 RVA: 0x002C494C File Offset: 0x002C2B4C
		static MapGeneratorDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(MapGeneratorDefOf));
		}

		// Token: 0x04004C85 RID: 19589
		public static MapGeneratorDef Encounter;

		// Token: 0x04004C86 RID: 19590
		public static MapGeneratorDef Base_Player;

		// Token: 0x04004C87 RID: 19591
		public static MapGeneratorDef Base_Faction;

		// Token: 0x04004C88 RID: 19592
		public static MapGeneratorDef EscapeShip;
	}
}
