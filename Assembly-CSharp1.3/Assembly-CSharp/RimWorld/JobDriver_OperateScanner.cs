using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200072B RID: 1835
	public class JobDriver_OperateScanner : JobDriver
	{
		// Token: 0x060032F0 RID: 13040 RVA: 0x000FC76A File Offset: 0x000FA96A
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.job.targetA, this.job, 1, -1, null, errorOnFailed);
		}

		// Token: 0x060032F1 RID: 13041 RVA: 0x00123E41 File Offset: 0x00122041
		protected override IEnumerable<Toil> MakeNewToils()
		{
			CompScanner scannerComp = this.job.targetA.Thing.TryGetComp<CompScanner>();
			this.FailOnDespawnedNullOrForbidden(TargetIndex.A);
			this.FailOnBurningImmobile(TargetIndex.A);
			this.FailOn(() => !scannerComp.CanUseNow);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.InteractionCell);
			Toil work = new Toil();
			work.tickAction = delegate()
			{
				Pawn actor = work.actor;
				Building building = (Building)actor.CurJob.targetA.Thing;
				scannerComp.Used(actor);
				actor.skills.Learn(SkillDefOf.Intellectual, 0.035f, false);
				actor.GainComfortFromCellIfPossible(true);
			};
			work.PlaySustainerOrSound(scannerComp.Props.soundWorking, 1f);
			work.AddFailCondition(() => !scannerComp.CanUseNow);
			work.defaultCompleteMode = ToilCompleteMode.Never;
			work.FailOnCannotTouch(TargetIndex.A, PathEndMode.InteractionCell);
			work.activeSkill = (() => SkillDefOf.Intellectual);
			yield return work;
			yield break;
		}
	}
}
