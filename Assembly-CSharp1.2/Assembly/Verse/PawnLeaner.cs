using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200037D RID: 893
	public class PawnLeaner
	{
		// Token: 0x1700041A RID: 1050
		// (get) Token: 0x06001670 RID: 5744 RVA: 0x00015DE7 File Offset: 0x00013FE7
		public Vector3 LeanOffset
		{
			get
			{
				return this.shootSourceOffset.ToVector3() * 0.5f * this.leanOffsetCurPct;
			}
		}

		// Token: 0x06001671 RID: 5745 RVA: 0x00015E09 File Offset: 0x00014009
		public PawnLeaner(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x06001672 RID: 5746 RVA: 0x000D6564 File Offset: 0x000D4764
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

		// Token: 0x06001673 RID: 5747 RVA: 0x00015E26 File Offset: 0x00014026
		public bool ShouldLean()
		{
			return this.pawn.stances.curStance is Stance_Busy && !(this.shootSourceOffset == new IntVec3(0, 0, 0));
		}

		// Token: 0x06001674 RID: 5748 RVA: 0x00015E59 File Offset: 0x00014059
		public void Notify_WarmingCastAlongLine(ShootLine newShootLine, IntVec3 ShootPosition)
		{
			this.shootSourceOffset = newShootLine.Source - this.pawn.Position;
		}

		// Token: 0x0400113A RID: 4410
		private Pawn pawn;

		// Token: 0x0400113B RID: 4411
		private IntVec3 shootSourceOffset = new IntVec3(0, 0, 0);

		// Token: 0x0400113C RID: 4412
		private float leanOffsetCurPct;

		// Token: 0x0400113D RID: 4413
		private const float LeanOffsetPctChangeRate = 0.075f;

		// Token: 0x0400113E RID: 4414
		private const float LeanOffsetDistanceMultiplier = 0.5f;
	}
}
