using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse.AI.Group
{
	// Token: 0x020006B0 RID: 1712
	public class Trigger_ThingsDamageTaken : Trigger
	{
		// Token: 0x06002F6D RID: 12141 RVA: 0x00118F0C File Offset: 0x0011710C
		public Trigger_ThingsDamageTaken(List<Thing> things, float damageFraction)
		{
			this.things = things;
			this.damageFraction = damageFraction;
		}

		// Token: 0x06002F6E RID: 12142 RVA: 0x00118F30 File Offset: 0x00117130
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			if (signal.type != TriggerSignalType.Tick)
			{
				return false;
			}
			if (this.things.Count == 0)
			{
				return true;
			}
			float num = 0f;
			int num2 = 0;
			for (int i = 0; i < this.things.Count; i++)
			{
				if (this.things[i] != null && this.things[i].Spawned)
				{
					if (this.things[i] is Pawn)
					{
						Pawn t = (Pawn)this.things[i];
						num += (t.DestroyedOrNull() ? 0f : 1f);
					}
					else
					{
						num += (float)this.things[i].HitPoints / (float)this.things[i].MaxHitPoints;
					}
					num2++;
				}
			}
			if (Mathf.Approximately(num, 0f) || num2 == 0)
			{
				return true;
			}
			num /= (float)num2;
			return num < 1f - this.damageFraction;
		}

		// Token: 0x04001CF9 RID: 7417
		private List<Thing> things;

		// Token: 0x04001CFA RID: 7418
		private float damageFraction = 0.5f;
	}
}
