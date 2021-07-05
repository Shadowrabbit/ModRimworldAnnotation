using System;

namespace Verse
{
	// Token: 0x020003E7 RID: 999
	public class HediffComp_KillAfterDays : HediffComp
	{
		// Token: 0x1700047C RID: 1148
		// (get) Token: 0x06001876 RID: 6262 RVA: 0x00017366 File Offset: 0x00015566
		public HediffCompProperties_KillAfterDays Props
		{
			get
			{
				return (HediffCompProperties_KillAfterDays)this.props;
			}
		}

		// Token: 0x06001877 RID: 6263 RVA: 0x00017373 File Offset: 0x00015573
		public override void CompPostPostAdd(DamageInfo? dinfo)
		{
			this.addedTick = Find.TickManager.TicksGame;
		}

		// Token: 0x06001878 RID: 6264 RVA: 0x000DF6D8 File Offset: 0x000DD8D8
		public override void CompPostTick(ref float severityAdjustment)
		{
			if (Find.TickManager.TicksGame - this.addedTick >= 60000 * this.Props.days)
			{
				base.Pawn.Kill(null, this.parent);
			}
		}

		// Token: 0x06001879 RID: 6265 RVA: 0x00017385 File Offset: 0x00015585
		public override void CompExposeData()
		{
			Scribe_Values.Look<int>(ref this.addedTick, "addedTick", 0, false);
		}

		// Token: 0x04001286 RID: 4742
		private int addedTick;
	}
}
