using System;
using UnityEngine;

namespace Verse.AI.Group
{
	// Token: 0x0200068C RID: 1676
	public class Trigger_TicksPassed : Trigger
	{
		// Token: 0x170008D4 RID: 2260
		// (get) Token: 0x06002F1B RID: 12059 RVA: 0x001182BD File Offset: 0x001164BD
		protected TriggerData_TicksPassed Data
		{
			get
			{
				return (TriggerData_TicksPassed)this.data;
			}
		}

		// Token: 0x170008D5 RID: 2261
		// (get) Token: 0x06002F1C RID: 12060 RVA: 0x001182CA File Offset: 0x001164CA
		public int TicksLeft
		{
			get
			{
				return Mathf.Max(this.duration - this.Data.ticksPassed, 0);
			}
		}

		// Token: 0x06002F1D RID: 12061 RVA: 0x001182E4 File Offset: 0x001164E4
		public Trigger_TicksPassed(int tickLimit)
		{
			this.data = new TriggerData_TicksPassed();
			this.duration = tickLimit;
		}

		// Token: 0x06002F1E RID: 12062 RVA: 0x00118308 File Offset: 0x00116508
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			if (signal.type == TriggerSignalType.Tick)
			{
				if (this.data == null || !(this.data is TriggerData_TicksPassed))
				{
					BackCompatibility.TriggerDataTicksPassedNull(this);
				}
				TriggerData_TicksPassed data = this.Data;
				data.ticksPassed++;
				return data.ticksPassed > this.duration;
			}
			return false;
		}

		// Token: 0x06002F1F RID: 12063 RVA: 0x0011835C File Offset: 0x0011655C
		public override void SourceToilBecameActive(Transition transition, LordToil previousToil)
		{
			if (!transition.sources.Contains(previousToil))
			{
				this.Data.ticksPassed = 0;
			}
		}

		// Token: 0x04001CD4 RID: 7380
		private int duration = 100;
	}
}
