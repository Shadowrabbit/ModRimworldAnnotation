using System;

namespace Verse
{
	// Token: 0x0200022B RID: 555
	public class WeatherEventMaker
	{
		// Token: 0x06000FC0 RID: 4032 RVA: 0x00059864 File Offset: 0x00057A64
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

		// Token: 0x04000C5E RID: 3166
		public float averageInterval = 100f;

		// Token: 0x04000C5F RID: 3167
		public Type eventClass;
	}
}
