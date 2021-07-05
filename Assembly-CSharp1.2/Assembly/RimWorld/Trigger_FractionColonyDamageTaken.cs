using System;
using UnityEngine;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000E2E RID: 3630
	public class Trigger_FractionColonyDamageTaken : Trigger
	{
		// Token: 0x17000CB2 RID: 3250
		// (get) Token: 0x0600525A RID: 21082 RVA: 0x0003996B File Offset: 0x00037B6B
		private TriggerData_FractionColonyDamageTaken Data
		{
			get
			{
				return (TriggerData_FractionColonyDamageTaken)this.data;
			}
		}

		// Token: 0x0600525B RID: 21083 RVA: 0x00039978 File Offset: 0x00037B78
		public Trigger_FractionColonyDamageTaken(float desiredColonyDamageFraction, float minDamage = 3.4028235E+38f)
		{
			this.data = new TriggerData_FractionColonyDamageTaken();
			this.desiredColonyDamageFraction = desiredColonyDamageFraction;
			this.minDamage = minDamage;
		}

		// Token: 0x0600525C RID: 21084 RVA: 0x00039999 File Offset: 0x00037B99
		public override void SourceToilBecameActive(Transition transition, LordToil previousToil)
		{
			if (!transition.sources.Contains(previousToil))
			{
				this.Data.startColonyDamage = transition.Map.damageWatcher.DamageTakenEver;
			}
		}

		// Token: 0x0600525D RID: 21085 RVA: 0x001BE1DC File Offset: 0x001BC3DC
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			if (signal.type == TriggerSignalType.Tick)
			{
				if (this.data == null || !(this.data is TriggerData_FractionColonyDamageTaken))
				{
					BackCompatibility.TriggerDataFractionColonyDamageTakenNull(this, lord.Map);
				}
				float num = Mathf.Max((float)lord.initialColonyHealthTotal * this.desiredColonyDamageFraction, this.minDamage);
				return lord.Map.damageWatcher.DamageTakenEver > this.Data.startColonyDamage + num;
			}
			return false;
		}

		// Token: 0x040034C5 RID: 13509
		private float desiredColonyDamageFraction;

		// Token: 0x040034C6 RID: 13510
		private float minDamage;
	}
}
