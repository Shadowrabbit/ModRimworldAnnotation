using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000386 RID: 902
	public class PawnDownedWiggler
	{
		// Token: 0x17000421 RID: 1057
		// (get) Token: 0x06001696 RID: 5782 RVA: 0x000D7250 File Offset: 0x000D5450
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

		// Token: 0x06001697 RID: 5783 RVA: 0x00016018 File Offset: 0x00014218
		public PawnDownedWiggler(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x06001698 RID: 5784 RVA: 0x000D7284 File Offset: 0x000D5484
		public void WigglerTick()
		{
			if (this.pawn.Downed && this.pawn.Spawned && !this.pawn.InBed())
			{
				this.ticksToIncapIcon--;
				if (this.ticksToIncapIcon <= 0)
				{
					MoteMaker.ThrowMetaIcon(this.pawn.Position, this.pawn.Map, ThingDefOf.Mote_IncapIcon);
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

		// Token: 0x06001699 RID: 5785 RVA: 0x00016032 File Offset: 0x00014232
		public void SetToCustomRotation(float rot)
		{
			this.downedAngle = rot;
			this.usingCustomRotation = true;
		}

		// Token: 0x0600169A RID: 5786 RVA: 0x000D7360 File Offset: 0x000D5560
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

		// Token: 0x0400115D RID: 4445
		private Pawn pawn;

		// Token: 0x0400115E RID: 4446
		public float downedAngle = PawnDownedWiggler.RandomDownedAngle;

		// Token: 0x0400115F RID: 4447
		public int ticksToIncapIcon;

		// Token: 0x04001160 RID: 4448
		private bool usingCustomRotation;

		// Token: 0x04001161 RID: 4449
		private const float DownedAngleWidth = 45f;

		// Token: 0x04001162 RID: 4450
		private const float DamageTakenDownedAngleShift = 10f;

		// Token: 0x04001163 RID: 4451
		private const int IncapWigglePeriod = 300;

		// Token: 0x04001164 RID: 4452
		private const int IncapWiggleLength = 90;

		// Token: 0x04001165 RID: 4453
		private const float IncapWiggleSpeed = 0.35f;

		// Token: 0x04001166 RID: 4454
		private const int TicksBetweenIncapIcons = 200;
	}
}
