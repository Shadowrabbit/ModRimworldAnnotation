using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000D0C RID: 3340
	public static class WeatherPartPool
	{
		// Token: 0x06004E0E RID: 19982 RVA: 0x001A2E30 File Offset: 0x001A1030
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

		// Token: 0x04002F1D RID: 12061
		private static List<SkyOverlay> instances = new List<SkyOverlay>();
	}
}
