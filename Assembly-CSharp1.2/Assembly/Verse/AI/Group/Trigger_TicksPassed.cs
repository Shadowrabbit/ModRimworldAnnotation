using System;
using UnityEngine;

namespace Verse.AI.Group
{
	// Token: 0x02000AF1 RID: 2801
	public class Trigger_TicksPassed : Trigger
	{
		// Token: 0x17000A3F RID: 2623
		// (get) Token: 0x060041F7 RID: 16887 RVA: 0x00031105 File Offset: 0x0002F305
		protected TriggerData_TicksPassed Data
		{
			get
			{
				return (TriggerData_TicksPassed)this.data;
			}
		}

		// Token: 0x17000A40 RID: 2624
		// (get) Token: 0x060041F8 RID: 16888 RVA: 0x00031112 File Offset: 0x0002F312
		public int TicksLeft
		{
			get
			{
				return Mathf.Max(this.duration - this.Data.ticksPassed, 0);
			}
		}

		// Token: 0x060041F9 RID: 16889 RVA: 0x0003112C File Offset: 0x0002F32C
		public Trigger_TicksPassed(int tickLimit)
		{
			this.data = new TriggerData_TicksPassed();
			this.duration = tickLimit;
		}

		// Token: 0x060041FA RID: 16890 RVA: 0x00188DE0 File Offset: 0x00186FE0
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

		// Token: 0x060041FB RID: 16891 RVA: 0x0003114E File Offset: 0x0002F34E
		public override void SourceToilBecameActive(Transition transition, LordToil previousToil)
		{
			if (!transition.sources.Contains(previousToil))
			{
				this.Data.ticksPassed = 0;
			}
		}

		// Token: 0x04002D4E RID: 11598
		private int duration = 100;
	}
}
