using System;
using UnityEngine;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020008E1 RID: 2273
	public class Trigger_FractionColonyDamageTaken : Trigger
	{
		// Token: 0x17000AB0 RID: 2736
		// (get) Token: 0x06003B9E RID: 15262 RVA: 0x0014C687 File Offset: 0x0014A887
		private TriggerData_FractionColonyDamageTaken Data
		{
			get
			{
				return (TriggerData_FractionColonyDamageTaken)this.data;
			}
		}

		// Token: 0x06003B9F RID: 15263 RVA: 0x0014C694 File Offset: 0x0014A894
		public Trigger_FractionColonyDamageTaken(float desiredColonyDamageFraction, float minDamage = 3.4028235E+38f)
		{
			this.data = new TriggerData_FractionColonyDamageTaken();
			this.desiredColonyDamageFraction = desiredColonyDamageFraction;
			this.minDamage = minDamage;
		}

		// Token: 0x06003BA0 RID: 15264 RVA: 0x0014C6B5 File Offset: 0x0014A8B5
		public override void SourceToilBecameActive(Transition transition, LordToil previousToil)
		{
			if (!transition.sources.Contains(previousToil))
			{
				this.Data.startColonyDamage = transition.Map.damageWatcher.DamageTakenEver;
			}
		}

		// Token: 0x06003BA1 RID: 15265 RVA: 0x0014C6E0 File Offset: 0x0014A8E0
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

		// Token: 0x0400206D RID: 8301
		private float desiredColonyDamageFraction;

		// Token: 0x0400206E RID: 8302
		private float minDamage;
	}
}
