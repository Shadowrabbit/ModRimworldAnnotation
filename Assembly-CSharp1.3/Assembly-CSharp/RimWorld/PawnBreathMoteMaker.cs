using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000D8C RID: 3468
	public class PawnBreathMoteMaker
	{
		// Token: 0x06005073 RID: 20595 RVA: 0x001AE1A7 File Offset: 0x001AC3A7
		public PawnBreathMoteMaker(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x06005074 RID: 20596 RVA: 0x001AE1B8 File Offset: 0x001AC3B8
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

		// Token: 0x06005075 RID: 20597 RVA: 0x001AE250 File Offset: 0x001AC450
		private void TryMakeBreathMote()
		{
			Vector3 loc = this.pawn.Drawer.DrawPos + this.pawn.Drawer.renderer.BaseHeadOffsetAt(this.pawn.Rotation) + this.pawn.Rotation.FacingCell.ToVector3() * 0.21f + PawnBreathMoteMaker.BreathOffset;
			Vector3 lastTickTweenedVelocity = this.pawn.Drawer.tweener.LastTickTweenedVelocity;
			FleckMaker.ThrowBreathPuff(loc, this.pawn.Map, this.pawn.Rotation.AsAngle, lastTickTweenedVelocity);
		}

		// Token: 0x04002FE5 RID: 12261
		private Pawn pawn;

		// Token: 0x04002FE6 RID: 12262
		private bool doThisBreath;

		// Token: 0x04002FE7 RID: 12263
		private const int BreathDuration = 80;

		// Token: 0x04002FE8 RID: 12264
		private const int BreathInterval = 320;

		// Token: 0x04002FE9 RID: 12265
		private const int MoteInterval = 8;

		// Token: 0x04002FEA RID: 12266
		private const float MaxBreathTemperature = 0f;

		// Token: 0x04002FEB RID: 12267
		private static readonly Vector3 BreathOffset = new Vector3(0f, 0f, -0.04f);

		// Token: 0x04002FEC RID: 12268
		private const float BreathRotationOffsetDist = 0.21f;
	}
}
