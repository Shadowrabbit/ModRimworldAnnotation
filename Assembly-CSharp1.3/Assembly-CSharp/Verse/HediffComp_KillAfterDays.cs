using System;
using RimWorld;

namespace Verse
{
	// Token: 0x020002AA RID: 682
	public class HediffComp_KillAfterDays : HediffComp
	{
		// Token: 0x170003A2 RID: 930
		// (get) Token: 0x060012AB RID: 4779 RVA: 0x0006B4A8 File Offset: 0x000696A8
		public HediffCompProperties_KillAfterDays Props
		{
			get
			{
				return (HediffCompProperties_KillAfterDays)this.props;
			}
		}

		// Token: 0x170003A3 RID: 931
		// (get) Token: 0x060012AC RID: 4780 RVA: 0x0006B4B8 File Offset: 0x000696B8
		public override string CompTipStringExtra
		{
			get
			{
				if (this.ticksLeft <= 0)
				{
					return null;
				}
				return "DeathIn".Translate(this.ticksLeft.ToStringTicksToPeriod(true, false, true, true).Colorize(ColoredText.DateTimeColor)).Resolve().CapitalizeFirst();
			}
		}

		// Token: 0x060012AD RID: 4781 RVA: 0x0006B505 File Offset: 0x00069705
		public override void CompPostPostAdd(DamageInfo? dinfo)
		{
			this.ticksLeft = 60000 * this.Props.days;
		}

		// Token: 0x060012AE RID: 4782 RVA: 0x0006B520 File Offset: 0x00069720
		public override void CompPostTick(ref float severityAdjustment)
		{
			this.ticksLeft--;
			if (this.ticksLeft <= 0)
			{
				base.Pawn.Kill(null, this.parent);
			}
		}

		// Token: 0x060012AF RID: 4783 RVA: 0x0006B55E File Offset: 0x0006975E
		public override void CompExposeData()
		{
			Scribe_Values.Look<int>(ref this.ticksLeft, "ticksLeft", 0, false);
		}

		// Token: 0x04000E1C RID: 3612
		private int ticksLeft;
	}
}
