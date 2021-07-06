using System;
using RimWorld;
using Verse.AI;

namespace Verse
{
	// Token: 0x020003EB RID: 1003
	public class HediffComp_ReactOnDamage : HediffComp
	{
		// Token: 0x17000480 RID: 1152
		// (get) Token: 0x06001884 RID: 6276 RVA: 0x00017425 File Offset: 0x00015625
		public HediffCompProperties_ReactOnDamage Props
		{
			get
			{
				return (HediffCompProperties_ReactOnDamage)this.props;
			}
		}

		// Token: 0x06001885 RID: 6277 RVA: 0x00017432 File Offset: 0x00015632
		public override void Notify_PawnPostApplyDamage(DamageInfo dinfo, float totalDamageDealt)
		{
			if (this.Props.damageDefIncoming == dinfo.Def)
			{
				this.React();
			}
		}

		// Token: 0x06001886 RID: 6278 RVA: 0x000DF8C0 File Offset: 0x000DDAC0
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
