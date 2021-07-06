using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001DF3 RID: 7667
	public class Hediff_HeartAttack : HediffWithComps
	{
		// Token: 0x0600A629 RID: 42537 RVA: 0x0006DE5B File Offset: 0x0006C05B
		public override void PostMake()
		{
			base.PostMake();
			this.intervalFactor = Rand.Range(0.1f, 2f);
		}

		// Token: 0x0600A62A RID: 42538 RVA: 0x0006DE78 File Offset: 0x0006C078
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.intervalFactor, "intervalFactor", 0f, false);
		}

		// Token: 0x0600A62B RID: 42539 RVA: 0x0006DE96 File Offset: 0x0006C096
		public override void Tick()
		{
			base.Tick();
			if (this.pawn.IsHashIntervalTick((int)(5000f * this.intervalFactor)))
			{
				this.Severity += Rand.Range(-0.4f, 0.6f);
			}
		}

		// Token: 0x0600A62C RID: 42540 RVA: 0x00302EA8 File Offset: 0x003010A8
		public override void Tended_NewTemp(float quality, float maxQuality, int batchPosition = 0)
		{
			base.Tended_NewTemp(quality, maxQuality, 0);
			float num = 0.65f * quality;
			if (Rand.Value < num)
			{
				if (batchPosition == 0 && this.pawn.Spawned)
				{
					MoteMaker.ThrowText(this.pawn.DrawPos, this.pawn.Map, "TextMote_TreatSuccess".Translate(num.ToStringPercent()), 6.5f);
				}
				this.Severity -= 0.3f;
				return;
			}
			if (batchPosition == 0 && this.pawn.Spawned)
			{
				MoteMaker.ThrowText(this.pawn.DrawPos, this.pawn.Map, "TextMote_TreatFailed".Translate(num.ToStringPercent()), 6.5f);
			}
		}

		// Token: 0x040070A0 RID: 28832
		private float intervalFactor;

		// Token: 0x040070A1 RID: 28833
		private const int SeverityChangeInterval = 5000;

		// Token: 0x040070A2 RID: 28834
		private const float TendSuccessChanceFactor = 0.65f;

		// Token: 0x040070A3 RID: 28835
		private const float TendSeverityReduction = 0.3f;
	}
}
