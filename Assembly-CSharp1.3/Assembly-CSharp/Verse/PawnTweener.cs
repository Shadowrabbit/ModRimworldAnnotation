using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200025C RID: 604
	public class PawnTweener
	{
		// Token: 0x1700035D RID: 861
		// (get) Token: 0x06001128 RID: 4392 RVA: 0x00061608 File Offset: 0x0005F808
		public Vector3 TweenedPos
		{
			get
			{
				return this.tweenedPos;
			}
		}

		// Token: 0x1700035E RID: 862
		// (get) Token: 0x06001129 RID: 4393 RVA: 0x00061610 File Offset: 0x0005F810
		public Vector3 LastTickTweenedVelocity
		{
			get
			{
				return this.TweenedPos - this.lastTickSpringPos;
			}
		}

		// Token: 0x0600112A RID: 4394 RVA: 0x00061623 File Offset: 0x0005F823
		public PawnTweener(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x0600112B RID: 4395 RVA: 0x00061654 File Offset: 0x0005F854
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

		// Token: 0x0600112C RID: 4396 RVA: 0x00061717 File Offset: 0x0005F917
		public void ResetTweenedPosToRoot()
		{
			this.tweenedPos = this.TweenedPosRoot();
			this.lastTickSpringPos = this.tweenedPos;
		}

		// Token: 0x0600112D RID: 4397 RVA: 0x00061734 File Offset: 0x0005F934
		private Vector3 TweenedPosRoot()
		{
			if (!this.pawn.Spawned)
			{
				return this.pawn.Position.ToVector3Shifted();
			}
			float num = this.MovedPercent();
			return this.pawn.pather.nextCell.ToVector3Shifted() * num + this.pawn.Position.ToVector3Shifted() * (1f - num) + PawnCollisionTweenerUtility.PawnCollisionPosOffsetFor(this.pawn);
		}

		// Token: 0x0600112E RID: 4398 RVA: 0x000617B8 File Offset: 0x0005F9B8
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

		// Token: 0x04000D11 RID: 3345
		private Pawn pawn;

		// Token: 0x04000D12 RID: 3346
		private Vector3 tweenedPos = new Vector3(0f, 0f, 0f);

		// Token: 0x04000D13 RID: 3347
		private int lastDrawFrame = -1;

		// Token: 0x04000D14 RID: 3348
		private Vector3 lastTickSpringPos;

		// Token: 0x04000D15 RID: 3349
		private const float SpringTightness = 0.09f;
	}
}
