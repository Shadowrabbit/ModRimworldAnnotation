using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C5A RID: 7258
	[DefOf]
	public static class BodyDefOf
	{
		// Token: 0x06009F5D RID: 40797 RVA: 0x0006A29A File Offset: 0x0006849A
		static BodyDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(BodyDefOf));
		}

		// Token: 0x040068CE RID: 26830
		public static BodyDef Human;

		// Token: 0x040068CF RID: 26831
		public static BodyDef MechanicalCentipede;
	}
}
