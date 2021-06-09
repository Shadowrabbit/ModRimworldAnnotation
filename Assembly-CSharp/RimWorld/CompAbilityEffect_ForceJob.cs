using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001379 RID: 4985
	public class CompAbilityEffect_ForceJob : CompAbilityEffect_WithDest
	{
		// Token: 0x170010BD RID: 4285
		// (get) Token: 0x06006C64 RID: 27748 RVA: 0x00049BB1 File Offset: 0x00047DB1
		public new CompProperties_AbilityForceJob Props
		{
			get
			{
				return (CompProperties_AbilityForceJob)this.props;
			}
		}

		// Token: 0x06006C65 RID: 27749 RVA: 0x00214FC8 File Offset: 0x002131C8
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
				job.expiryInterval = (this.parent.def.statBases.GetStatValueFromList(StatDefOf.Ability_Duration, 10f) * num).SecondsToTicks();
				job.mote = MoteMaker.MakeThoughtBubble(pawn, this.parent.def.iconPath, true);
				pawn.jobs.StopAll(false, true);
				pawn.jobs.StartJob(job, JobCondition.InterruptForced, null, false, true, null, null, false, false);
			}
		}
	}
}
