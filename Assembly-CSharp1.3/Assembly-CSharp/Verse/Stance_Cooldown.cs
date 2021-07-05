using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020002F9 RID: 761
	public class Stance_Cooldown : Stance_Busy
	{
		// Token: 0x06001616 RID: 5654 RVA: 0x000808BC File Offset: 0x0007EABC
		public Stance_Cooldown()
		{
		}

		// Token: 0x06001617 RID: 5655 RVA: 0x000808C4 File Offset: 0x0007EAC4
		public Stance_Cooldown(int ticks, LocalTargetInfo focusTarg, Verb verb) : base(ticks, focusTarg, verb)
		{
		}

		// Token: 0x06001618 RID: 5656 RVA: 0x000808D0 File Offset: 0x0007EAD0
		public override void StanceDraw()
		{
			if (Find.Selector.IsSelected(this.stanceTracker.pawn))
			{
				float radius = Mathf.Min(0.5f, (float)this.ticksLeft * 0.002f);
				GenDraw.DrawCooldownCircle(this.stanceTracker.pawn.Drawer.DrawPos + new Vector3(0f, 0.2f, 0f), radius);
			}
		}

		// Token: 0x04000F53 RID: 3923
		private const float RadiusPerTick = 0.002f;

		// Token: 0x04000F54 RID: 3924
		private const float MaxRadius = 0.5f;
	}
}
