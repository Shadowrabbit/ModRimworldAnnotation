using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C7B RID: 7291
	[DefOf]
	public static class WeatherDefOf
	{
		// Token: 0x06009F7E RID: 40830 RVA: 0x0006A4CB File Offset: 0x000686CB
		static WeatherDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(WeatherDefOf));
		}

		// Token: 0x04006B95 RID: 27541
		public static WeatherDef Clear;
	}
}
