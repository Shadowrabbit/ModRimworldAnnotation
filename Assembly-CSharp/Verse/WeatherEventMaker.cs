using System;

namespace Verse
{
	// Token: 0x0200031C RID: 796
	public class WeatherEventMaker
	{
		// Token: 0x06001446 RID: 5190 RVA: 0x000CE28C File Offset: 0x000CC48C
		public void WeatherEventMakerTick(Map map, float strength)
		{
			if (Rand.Value < 1f / this.averageInterval * strength)
			{
				WeatherEvent newEvent = (WeatherEvent)Activator.CreateInstance(this.eventClass, new object[]
				{
					map
				});
				map.weatherManager.eventHandler.AddEvent(newEvent);
			}
		}

		// Token: 0x04000FE9 RID: 4073
		public float averageInterval = 100f;

		// Token: 0x04000FEA RID: 4074
		public Type eventClass;
	}
}
