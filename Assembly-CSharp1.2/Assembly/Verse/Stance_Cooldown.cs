using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000469 RID: 1129
	public class Stance_Cooldown : Stance_Busy
	{
		// Token: 0x06001CAA RID: 7338 RVA: 0x00019EBB File Offset: 0x000180BB
		public Stance_Cooldown()
		{
		}

		// Token: 0x06001CAB RID: 7339 RVA: 0x00019EC3 File Offset: 0x000180C3
		public Stance_Cooldown(int ticks, LocalTargetInfo focusTarg, Verb verb) : base(ticks, focusTarg, verb)
		{
		}

		// Token: 0x06001CAC RID: 7340 RVA: 0x000F18D4 File Offset: 0x000EFAD4
		public override void StanceDraw()
		{
			if (Find.Selector.IsSelected(this.stanceTracker.pawn))
			{
				float radius = Mathf.Min(0.5f, (float)this.ticksLeft * 0.002f);
				GenDraw.DrawCooldownCircle(this.stanceTracker.pawn.Drawer.DrawPos + new Vector3(0f, 0.2f, 0f), radius);
			}
		}

		// Token: 0x04001481 RID: 5249
		private const float RadiusPerTick = 0.002f;

		// Token: 0x04001482 RID: 5250
		private const float MaxRadius = 0.5f;
	}
}
