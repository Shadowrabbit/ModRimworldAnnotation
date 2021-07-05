using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000262 RID: 610
	public class PawnDownedWiggler
	{
		// Token: 0x17000363 RID: 867
		// (get) Token: 0x06001148 RID: 4424 RVA: 0x00062224 File Offset: 0x00060424
		private static float RandomDownedAngle
		{
			get
			{
				float num = Rand.Range(45f, 135f);
				if (Rand.Value < 0.5f)
				{
					num += 180f;
				}
				return num;
			}
		}

		// Token: 0x06001149 RID: 4425 RVA: 0x00062256 File Offset: 0x00060456
		public PawnDownedWiggler(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x0600114A RID: 4426 RVA: 0x00062270 File Offset: 0x00060470
		public void WigglerTick()
		{
			if (this.pawn.Downed && this.pawn.Spawned && !this.pawn.InBed())
			{
				this.ticksToIncapIcon--;
				if (this.ticksToIncapIcon <= 0)
				{
					FleckMaker.ThrowMetaIcon(this.pawn.Position, this.pawn.Map, FleckDefOf.IncapIcon, 0.42f);
					this.ticksToIncapIcon = 200;
				}
				if (this.pawn.Awake())
				{
					int num = Find.TickManager.TicksGame % 300 * 2;
					if (num < 90)
					{
						this.downedAngle += 0.35f;
						return;
					}
					if (num < 390 && num >= 300)
					{
						this.downedAngle -= 0.35f;
					}
				}
			}
		}

		// Token: 0x0600114B RID: 4427 RVA: 0x0006234E File Offset: 0x0006054E
		public void SetToCustomRotation(float rot)
		{
			this.downedAngle = rot;
			this.usingCustomRotation = true;
		}

		// Token: 0x0600114C RID: 4428 RVA: 0x00062360 File Offset: 0x00060560
		public void Notify_DamageApplied(DamageInfo dam)
		{
			if ((this.pawn.Downed || this.pawn.Dead) && dam.Def.hasForcefulImpact)
			{
				this.downedAngle += 10f * Rand.Range(-1f, 1f);
				if (!this.usingCustomRotation)
				{
					if (this.downedAngle > 315f)
					{
						this.downedAngle = 315f;
					}
					if (this.downedAngle < 45f)
					{
						this.downedAngle = 45f;
					}
					if (this.downedAngle > 135f && this.downedAngle < 225f)
					{
						if (this.downedAngle > 180f)
						{
							this.downedAngle = 225f;
							return;
						}
						this.downedAngle = 135f;
						return;
					}
				}
				else
				{
					if (this.downedAngle >= 360f)
					{
						this.downedAngle -= 360f;
					}
					if (this.downedAngle < 0f)
					{
						this.downedAngle += 360f;
					}
				}
			}
		}

		// Token: 0x04000D29 RID: 3369
		private Pawn pawn;

		// Token: 0x04000D2A RID: 3370
		public float downedAngle = PawnDownedWiggler.RandomDownedAngle;

		// Token: 0x04000D2B RID: 3371
		public int ticksToIncapIcon;

		// Token: 0x04000D2C RID: 3372
		private bool usingCustomRotation;

		// Token: 0x04000D2D RID: 3373
		private const float DownedAngleWidth = 45f;

		// Token: 0x04000D2E RID: 3374
		private const float DamageTakenDownedAngleShift = 10f;

		// Token: 0x04000D2F RID: 3375
		private const int IncapWigglePeriod = 300;

		// Token: 0x04000D30 RID: 3376
		private const int IncapWiggleLength = 90;

		// Token: 0x04000D31 RID: 3377
		private const float IncapWiggleSpeed = 0.35f;

		// Token: 0x04000D32 RID: 3378
		private const int TicksBetweenIncapIcons = 200;
	}
}
