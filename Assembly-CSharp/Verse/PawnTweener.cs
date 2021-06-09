using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000381 RID: 897
	public class PawnTweener
	{
		// Token: 0x1700041C RID: 1052
		// (get) Token: 0x06001681 RID: 5761 RVA: 0x00015F16 File Offset: 0x00014116
		public Vector3 TweenedPos
		{
			get
			{
				return this.tweenedPos;
			}
		}

		// Token: 0x1700041D RID: 1053
		// (get) Token: 0x06001682 RID: 5762 RVA: 0x00015F1E File Offset: 0x0001411E
		public Vector3 LastTickTweenedVelocity
		{
			get
			{
				return this.TweenedPos - this.lastTickSpringPos;
			}
		}

		// Token: 0x06001683 RID: 5763 RVA: 0x00015F31 File Offset: 0x00014131
		public PawnTweener(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x06001684 RID: 5764 RVA: 0x000D6A58 File Offset: 0x000D4C58
		public void PreDrawPosCalculation()
		{
			if (this.lastDrawFrame == RealTime.frameCount)
			{
				return;
			}
			if (this.lastDrawFrame < RealTime.frameCount - 1)
			{
				this.ResetTweenedPosToRoot();
			}
			else
			{
				this.lastTickSpringPos = this.tweenedPos;
				float tickRateMultiplier = Find.TickManager.TickRateMultiplier;
				if (tickRateMultiplier < 5f)
				{
					Vector3 a = this.TweenedPosRoot() - this.tweenedPos;
					float num = 0.09f * (RealTime.deltaTime * 60f * tickRateMultiplier);
					if (RealTime.deltaTime > 0.05f)
					{
						num = Mathf.Min(num, 1f);
					}
					this.tweenedPos += a * num;
				}
				else
				{
					this.tweenedPos = this.TweenedPosRoot();
				}
			}
			this.lastDrawFrame = RealTime.frameCount;
		}

		// Token: 0x06001685 RID: 5765 RVA: 0x00015F61 File Offset: 0x00014161
		public void ResetTweenedPosToRoot()
		{
			this.tweenedPos = this.TweenedPosRoot();
			this.lastTickSpringPos = this.tweenedPos;
		}

		// Token: 0x06001686 RID: 5766 RVA: 0x000D6B1C File Offset: 0x000D4D1C
		private Vector3 TweenedPosRoot()
		{
			if (!this.pawn.Spawned)
			{
				return this.pawn.Position.ToVector3Shifted();
			}
			float num = this.MovedPercent();
			return this.pawn.pather.nextCell.ToVector3Shifted() * num + this.pawn.Position.ToVector3Shifted() * (1f - num) + PawnCollisionTweenerUtility.PawnCollisionPosOffsetFor(this.pawn);
		}

		// Token: 0x06001687 RID: 5767 RVA: 0x000D6BA0 File Offset: 0x000D4DA0
		private float MovedPercent()
		{
			if (!this.pawn.pather.Moving)
			{
				return 0f;
			}
			if (this.pawn.stances.FullBodyBusy)
			{
				return 0f;
			}
			if (this.pawn.pather.BuildingBlockingNextPathCell() != null)
			{
				return 0f;
			}
			if (this.pawn.pather.NextCellDoorToWaitForOrManuallyOpen() != null)
			{
				return 0f;
			}
			if (this.pawn.pather.WillCollideWithPawnOnNextPathCell())
			{
				return 0f;
			}
			return 1f - this.pawn.pather.nextCellCostLeft / this.pawn.pather.nextCellCostTotal;
		}

		// Token: 0x0400114E RID: 4430
		private Pawn pawn;

		// Token: 0x0400114F RID: 4431
		private Vector3 tweenedPos = new Vector3(0f, 0f, 0f);

		// Token: 0x04001150 RID: 4432
		private int lastDrawFrame = -1;

		// Token: 0x04001151 RID: 4433
		private Vector3 lastTickSpringPos;

		// Token: 0x04001152 RID: 4434
		private const float SpringTightness = 0.09f;
	}
}
