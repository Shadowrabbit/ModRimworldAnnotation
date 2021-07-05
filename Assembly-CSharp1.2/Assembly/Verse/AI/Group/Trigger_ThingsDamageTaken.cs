using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse.AI.Group
{
	// Token: 0x02000B12 RID: 2834
	public class Trigger_ThingsDamageTaken : Trigger
	{
		// Token: 0x0600423F RID: 16959 RVA: 0x000315F7 File Offset: 0x0002F7F7
		public Trigger_ThingsDamageTaken(List<Thing> things, float damageFraction)
		{
			this.things = things;
			this.damageFraction = damageFraction;
		}

		// Token: 0x06004240 RID: 16960 RVA: 0x00189418 File Offset: 0x00187618
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

		// Token: 0x04002D6E RID: 11630
		private List<Thing> things;

		// Token: 0x04002D6F RID: 11631
		private float damageFraction = 0.5f;
	}
}
