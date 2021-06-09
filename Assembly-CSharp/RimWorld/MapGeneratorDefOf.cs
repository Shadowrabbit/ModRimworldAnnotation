using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C7D RID: 7293
	[DefOf]
	public static class MapGeneratorDefOf
	{
		// Token: 0x06009F80 RID: 40832 RVA: 0x0006A4ED File Offset: 0x000686ED
		static MapGeneratorDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(MapGeneratorDefOf));
		}

		// Token: 0x04006BA3 RID: 27555
		public static MapGeneratorDef Encounter;

		// Token: 0x04006BA4 RID: 27556
		public static MapGeneratorDef Base_Player;

		// Token: 0x04006BA5 RID: 27557
		public static MapGeneratorDef Base_Faction;

		// Token: 0x04006BA6 RID: 27558
		public static MapGeneratorDef EscapeShip;
	}
}
