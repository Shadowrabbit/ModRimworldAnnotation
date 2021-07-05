using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000D3A RID: 3386
	public class CompAbilityEffect_ForceJob : CompAbilityEffect_WithDest
	{
		// Token: 0x17000DB3 RID: 3507
		// (get) Token: 0x06004F3C RID: 20284 RVA: 0x001A9155 File Offset: 0x001A7355
		public new CompProperties_AbilityForceJob Props
		{
			get
			{
				return (CompProperties_AbilityForceJob)this.props;
			}
		}

		// Token: 0x06004F3D RID: 20285 RVA: 0x001A9164 File Offset: 0x001A7364
		public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
		{
			base.Apply(target, dest);
			Pawn pawn = target.Thing as Pawn;
			if (pawn != null)
			{
				Job job = JobMaker.MakeJob(this.Props.jobDef, new LocalTargetInfo(base.GetDestination(target).Cell));
				float num = 1f;
				if (this.Props.durationMultiplier != null)
				{
					num = pawn.GetStatValue(this.Props.durationMultiplier, true);
				}
				job.expiryInterval = (this.parent.def.GetStatValueAbstract(StatDefOf.Ability_Duration, this.parent.pawn) * num).SecondsToTicks();
				job.mote = MoteMaker.MakeThoughtBubble(pawn, this.parent.def.iconPath, true);
				pawn.jobs.StopAll(false, true);
				pawn.jobs.StartJob(job, JobCondition.InterruptForced, null, false, true, null, null, false, false);
			}
		}
	}
}
