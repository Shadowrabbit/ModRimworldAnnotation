using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000259 RID: 601
	public class PawnLeaner
	{
		// Token: 0x1700035C RID: 860
		// (get) Token: 0x0600111D RID: 4381 RVA: 0x0006102D File Offset: 0x0005F22D
		public Vector3 LeanOffset
		{
			get
			{
				return this.shootSourceOffset.ToVector3() * 0.5f * this.leanOffsetCurPct;
			}
		}

		// Token: 0x0600111E RID: 4382 RVA: 0x0006104F File Offset: 0x0005F24F
		public PawnLeaner(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x0600111F RID: 4383 RVA: 0x0006106C File Offset: 0x0005F26C
		public void LeanerTick()
		{
			if (this.ShouldLean())
			{
				this.leanOffsetCurPct += 0.075f;
				if (this.leanOffsetCurPct > 1f)
				{
					this.leanOffsetCurPct = 1f;
					return;
				}
			}
			else
			{
				this.leanOffsetCurPct -= 0.075f;
				if (this.leanOffsetCurPct < 0f)
				{
					this.leanOffsetCurPct = 0f;
				}
			}
		}

		// Token: 0x06001120 RID: 4384 RVA: 0x000610D6 File Offset: 0x0005F2D6
		public bool ShouldLean()
		{
			return this.pawn.stances.curStance is Stance_Busy && !(this.shootSourceOffset == new IntVec3(0, 0, 0));
		}

		// Token: 0x06001121 RID: 4385 RVA: 0x00061109 File Offset: 0x0005F309
		public void Notify_WarmingCastAlongLine(ShootLine newShootLine, IntVec3 ShootPosition)
		{
			this.shootSourceOffset = newShootLine.Source - this.pawn.Position;
		}

		// Token: 0x04000CFF RID: 3327
		private Pawn pawn;

		// Token: 0x04000D00 RID: 3328
		private IntVec3 shootSourceOffset = new IntVec3(0, 0, 0);

		// Token: 0x04000D01 RID: 3329
		private float leanOffsetCurPct;

		// Token: 0x04000D02 RID: 3330
		private const float LeanOffsetPctChangeRate = 0.075f;

		// Token: 0x04000D03 RID: 3331
		private const float LeanOffsetDistanceMultiplier = 0.5f;
	}
}
