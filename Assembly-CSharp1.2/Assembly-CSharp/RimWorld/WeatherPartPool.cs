using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001343 RID: 4931
	public static class WeatherPartPool
	{
		// Token: 0x06006AFA RID: 27386 RVA: 0x002104E8 File Offset: 0x0020E6E8
		public static SkyOverlay GetInstanceOf<T>() where T : SkyOverlay
		{
			for (int i = 0; i < WeatherPartPool.instances.Count; i++)
			{
				T t = WeatherPartPool.instances[i] as T;
				if (t != null)
				{
					return t;
				}
			}
			SkyOverlay skyOverlay = Activator.CreateInstance<T>();
			WeatherPartPool.instances.Add(skyOverlay);
			return skyOverlay;
		}

		// Token: 0x0400472B RID: 18219
		private static List<SkyOverlay> instances = new List<SkyOverlay>();
	}
}
