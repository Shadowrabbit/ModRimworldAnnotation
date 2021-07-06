using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x0200031B RID: 795
	public class WeatherEventHandler
	{
		// Token: 0x170003B4 RID: 948
		// (get) Token: 0x06001441 RID: 5185 RVA: 0x00014957 File Offset: 0x00012B57
		public List<WeatherEvent> LiveEventsListForReading
		{
			get
			{
				return this.liveEvents;
			}
		}

		// Token: 0x06001442 RID: 5186 RVA: 0x0001495F File Offset: 0x00012B5F
		public void AddEvent(WeatherEvent newEvent)
		{
			this.liveEvents.Add(newEvent);
			newEvent.FireEvent();
		}

		// Token: 0x06001443 RID: 5187 RVA: 0x000CE200 File Offset: 0x000CC400
		public void WeatherEventHandlerTick()
		{
			for (int i = this.liveEvents.Count - 1; i >= 0; i--)
			{
				this.liveEvents[i].WeatherEventTick();
				if (this.liveEvents[i].Expired)
				{
					this.liveEvents.RemoveAt(i);
				}
			}
		}

		// Token: 0x06001444 RID: 5188 RVA: 0x000CE258 File Offset: 0x000CC458
		public void WeatherEventsDraw()
		{
			for (int i = 0; i < this.liveEvents.Count; i++)
			{
				this.liveEvents[i].WeatherEventDraw();
			}
		}

		// Token: 0x04000FE8 RID: 4072
		private List<WeatherEvent> liveEvents = new List<WeatherEvent>();
	}
}
