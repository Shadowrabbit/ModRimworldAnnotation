using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020013B3 RID: 5043
	public class PawnBreathMoteMaker
	{
		// Token: 0x06006D67 RID: 28007 RVA: 0x0004A5A7 File Offset: 0x000487A7
		public PawnBreathMoteMaker(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x06006D68 RID: 28008 RVA: 0x0021882C File Offset: 0x00216A2C
		public void BreathMoteMakerTick()
		{
			if (!this.pawn.RaceProps.Humanlike || this.pawn.RaceProps.IsMechanoid)
			{
				return;
			}
			int num = Mathf.Abs(Find.TickManager.TicksGame + this.pawn.HashOffset()) % 320;
			if (num == 0)
			{
				this.doThisBreath = (this.pawn.AmbientTemperature < 0f && this.pawn.GetPosture() == PawnPosture.Standing);
			}
			if (this.doThisBreath && num < 80 && num % 8 == 0)
			{
				this.TryMakeBreathMote();
			}
		}

		// Token: 0x06006D69 RID: 28009 RVA: 0x002188C4 File Offset: 0x00216AC4
		private void TryMakeBreathMote()
		{
			Vector3 loc = this.pawn.Drawer.DrawPos + this.pawn.Drawer.renderer.BaseHeadOffsetAt(this.pawn.Rotation) + this.pawn.Rotation.FacingCell.ToVector3() * 0.21f + PawnBreathMoteMaker.BreathOffset;
			Vector3 lastTickTweenedVelocity = this.pawn.Drawer.tweener.LastTickTweenedVelocity;
			MoteMaker.ThrowBreathPuff(loc, this.pawn.Map, this.pawn.Rotation.AsAngle, lastTickTweenedVelocity);
		}

		// Token: 0x0400484C RID: 18508
		private Pawn pawn;

		// Token: 0x0400484D RID: 18509
		private bool doThisBreath;

		// Token: 0x0400484E RID: 18510
		private const int BreathDuration = 80;

		// Token: 0x0400484F RID: 18511
		private const int BreathInterval = 320;

		// Token: 0x04004850 RID: 18512
		private const int MoteInterval = 8;

		// Token: 0x04004851 RID: 18513
		private const float MaxBreathTemperature = 0f;

		// Token: 0x04004852 RID: 18514
		private static readonly Vector3 BreathOffset = new Vector3(0f, 0f, -0.04f);

		// Token: 0x04004853 RID: 18515
		private const float BreathRotationOffsetDist = 0.21f;
	}
}
