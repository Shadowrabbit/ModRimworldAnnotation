using System;
using RimWorld;
using Verse.AI;

namespace Verse
{
	// Token: 0x020002AE RID: 686
	public class HediffComp_ReactOnDamage : HediffComp
	{
		// Token: 0x170003A7 RID: 935
		// (get) Token: 0x060012BA RID: 4794 RVA: 0x0006B7D4 File Offset: 0x000699D4
		public HediffCompProperties_ReactOnDamage Props
		{
			get
			{
				return (HediffCompProperties_ReactOnDamage)this.props;
			}
		}

		// Token: 0x060012BB RID: 4795 RVA: 0x0006B7E1 File Offset: 0x000699E1
		public override void Notify_PawnPostApplyDamage(DamageInfo dinfo, float totalDamageDealt)
		{
			if (this.Props.damageDefIncoming == dinfo.Def)
			{
				this.React();
			}
		}

		// Token: 0x060012BC RID: 4796 RVA: 0x0006B800 File Offset: 0x00069A00
		private void React()
		{
			if (this.Props.createHediff != null)
			{
				BodyPartRecord part = this.parent.Part;
				if (this.Props.createHediffOn != null)
				{
					part = this.parent.pawn.RaceProps.body.AllParts.FirstOrFallback((BodyPartRecord p) => p.def == this.Props.createHediffOn, null);
				}
				this.parent.pawn.health.AddHediff(this.Props.createHediff, part, null, null);
			}
			if (this.Props.vomit)
			{
				this.parent.pawn.jobs.StartJob(JobMaker.MakeJob(JobDefOf.Vomit), JobCondition.InterruptForced, null, true, true, null, null, false, false);
			}
		}
	}
}
