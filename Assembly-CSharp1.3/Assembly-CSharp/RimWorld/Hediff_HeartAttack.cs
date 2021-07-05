using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001575 RID: 5493
	public class Hediff_HeartAttack : HediffWithComps
	{
		// Token: 0x060081E7 RID: 33255 RVA: 0x002DE8DD File Offset: 0x002DCADD
		public override void PostMake()
		{
			base.PostMake();
			this.intervalFactor = Rand.Range(0.1f, 2f);
		}

		// Token: 0x060081E8 RID: 33256 RVA: 0x002DE8FA File Offset: 0x002DCAFA
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.intervalFactor, "intervalFactor", 0f, false);
		}

		// Token: 0x060081E9 RID: 33257 RVA: 0x002DE918 File Offset: 0x002DCB18
		public override void Tick()
		{
			base.Tick();
			if (this.pawn.IsHashIntervalTick((int)(5000f * this.intervalFactor)))
			{
				this.Severity += Rand.Range(-0.4f, 0.6f);
			}
		}

		// Token: 0x060081EA RID: 33258 RVA: 0x002DE958 File Offset: 0x002DCB58
		public override void Tended(float quality, float maxQuality, int batchPosition = 0)
		{
			base.Tended(quality, maxQuality, 0);
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

		// Token: 0x040050CF RID: 20687
		private float intervalFactor;

		// Token: 0x040050D0 RID: 20688
		private const int SeverityChangeInterval = 5000;

		// Token: 0x040050D1 RID: 20689
		private const float TendSuccessChanceFactor = 0.65f;

		// Token: 0x040050D2 RID: 20690
		private const float TendSeverityReduction = 0.3f;
	}
}
