using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000C15 RID: 3093
	public class JobDriver_UseItem : JobDriver
	{
		// Token: 0x060048D1 RID: 18641 RVA: 0x00034A92 File Offset: 0x00032C92
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.useDuration, "useDuration", 0, false);
		}

		// Token: 0x060048D2 RID: 18642 RVA: 0x0019B6A8 File Offset: 0x001998A8
		public override void Notify_Starting()
		{
			base.Notify_Starting();
			this.useDuration = this.job.GetTarget(TargetIndex.A).Thing.TryGetComp<CompUsable>().Props.useDuration;
		}

		// Token: 0x060048D3 RID: 18643 RVA: 0x0002D6EB File Offset: 0x0002B8EB
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.job.targetA, this.job, 1, -1, null, errorOnFailed);
		}

		// Token: 0x060048D4 RID: 18644 RVA: 0x00034AAC File Offset: 0x00032CAC
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnIncapable(PawnCapacityDefOf.Manipulation);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch);
			Toil toil = Toils_General.Wait(this.useDuration, TargetIndex.None);
			toil.WithProgressBarToilDelay(TargetIndex.A, false, -0.5f);
			toil.FailOnDespawnedNullOrForbidden(TargetIndex.A);
			toil.FailOnCannotTouch(TargetIndex.A, PathEndMode.Touch);
			if (this.job.targetB.IsValid)
			{
				toil.FailOnDespawnedOrNull(TargetIndex.B);
				CompTargetable compTargetable = this.job.GetTarget(TargetIndex.A).Thing.TryGetComp<CompTargetable>();
				if (compTargetable != null && compTargetable.Props.nonDownedPawnOnly)
				{
					toil.FailOnDownedOrDead(TargetIndex.B);
				}
			}
			yield return toil;
			Toil use = new Toil();
			use.initAction = delegate()
			{
				Pawn actor = use.actor;
				actor.CurJob.targetA.Thing.TryGetComp<CompUsable>().UsedBy(actor);
			};
			use.defaultCompleteMode = ToilCompleteMode.Instant;
			yield return use;
			yield break;
		}

		// Token: 0x04003084 RID: 12420
		private int useDuration = -1;
	}
}
