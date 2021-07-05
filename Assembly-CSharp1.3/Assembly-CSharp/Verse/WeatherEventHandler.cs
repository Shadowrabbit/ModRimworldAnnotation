using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x0200022A RID: 554
	public class WeatherEventHandler
	{
		// Token: 0x17000308 RID: 776
		// (get) Token: 0x06000FBB RID: 4027 RVA: 0x000597A5 File Offset: 0x000579A5
		public List<WeatherEvent> LiveEventsListForReading
		{
			get
			{
				return this.liveEvents;
			}
		}

		// Token: 0x06000FBC RID: 4028 RVA: 0x000597AD File Offset: 0x000579AD
		public void AddEvent(WeatherEvent newEvent)
		{
			this.liveEvents.Add(newEvent);
			newEvent.FireEvent();
		}

		// Token: 0x06000FBD RID: 4029 RVA: 0x000597C4 File Offset: 0x000579C4
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

		// Token: 0x06000FBE RID: 4030 RVA: 0x0005981C File Offset: 0x00057A1C
		public void WeatherEventsDraw()
		{
			for (int i = 0; i < this.liveEvents.Count; i++)
			{
				this.liveEvents[i].WeatherEventDraw();
			}
		}

		// Token: 0x04000C5D RID: 3165
		private List<WeatherEvent> liveEvents = new List<WeatherEvent>();
	}
}
