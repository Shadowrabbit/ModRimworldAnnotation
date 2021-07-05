using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200073C RID: 1852
	public class JobDriver_UseItem : JobDriver
	{
		// Token: 0x0600335D RID: 13149 RVA: 0x0012500D File Offset: 0x0012320D
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.useDuration, "useDuration", 0, false);
		}

		// Token: 0x0600335E RID: 13150 RVA: 0x00125028 File Offset: 0x00123228
		public override void Notify_Starting()
		{
			base.Notify_Starting();
			this.useDuration = this.job.GetTarget(TargetIndex.A).Thing.TryGetComp<CompUsable>().Props.useDuration;
		}

		// Token: 0x0600335F RID: 13151 RVA: 0x000FC76A File Offset: 0x000FA96A
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.job.targetA, this.job, 1, -1, null, errorOnFailed);
		}

		// Token: 0x06003360 RID: 13152 RVA: 0x00125064 File Offset: 0x00123264
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

		// Token: 0x04001E05 RID: 7685
		private int useDuration = -1;
	}
}
