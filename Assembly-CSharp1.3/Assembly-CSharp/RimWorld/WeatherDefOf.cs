using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200143B RID: 5179
	[DefOf]
	public static class WeatherDefOf
	{
		// Token: 0x06007D2E RID: 32046 RVA: 0x002C492A File Offset: 0x002C2B2A
		static WeatherDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(WeatherDefOf));
		}

		// Token: 0x04004C75 RID: 19573
		public static WeatherDef Clear;
	}
}
