using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02000707 RID: 1799
	public class JobDriver_ApplyTechprint : JobDriver
	{
		// Token: 0x1700094B RID: 2379
		// (get) Token: 0x060031F3 RID: 12787 RVA: 0x001219D0 File Offset: 0x0011FBD0
		protected Building_ResearchBench ResearchBench
		{
			get
			{
				return (Building_ResearchBench)this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x1700094C RID: 2380
		// (get) Token: 0x060031F4 RID: 12788 RVA: 0x001219F8 File Offset: 0x0011FBF8
		protected Thing Techprint
		{
			get
			{
				return this.job.GetTarget(TargetIndex.B).Thing;
			}
		}

		// Token: 0x1700094D RID: 2381
		// (get) Token: 0x060031F5 RID: 12789 RVA: 0x00121A19 File Offset: 0x0011FC19
		protected CompTechprint TechprintComp
		{
			get
			{
				return this.Techprint.TryGetComp<CompTechprint>();
			}
		}

		// Token: 0x060031F6 RID: 12790 RVA: 0x00121A28 File Offset: 0x0011FC28
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.ResearchBench, this.job, 1, -1, null, errorOnFailed) && this.pawn.Reserve(this.Techprint, this.job, 1, -1, null, errorOnFailed);
		}

		// Token: 0x060031F7 RID: 12791 RVA: 0x00121A79 File Offset: 0x0011FC79
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedNullOrForbidden(TargetIndex.A);
			this.FailOnBurningImmobile(TargetIndex.A);
			yield return Toils_General.DoAtomic(delegate
			{
				this.job.count = 1;
			});
			Toil toil = Toils_Reserve.Reserve(TargetIndex.B, 1, -1, null);
			yield return toil;
			yield return Toils_Goto.GotoThing(TargetIndex.B, PathEndMode.ClosestTouch).FailOnDespawnedNullOrForbidden(TargetIndex.B).FailOnSomeonePhysicallyInteracting(TargetIndex.B);
			yield return Toils_Haul.StartCarryThing(TargetIndex.B, false, true, false).FailOnDestroyedNullOrForbidden(TargetIndex.B);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.InteractionCell);
			yield return Toils_Haul.PlaceHauledThingInCell(TargetIndex.C, null, false, false);
			yield return Toils_General.Wait(600, TargetIndex.None).FailOnDestroyedNullOrForbidden(TargetIndex.B).FailOnDestroyedNullOrForbidden(TargetIndex.A).FailOnCannotTouch(TargetIndex.A, PathEndMode.InteractionCell).WithProgressBarToilDelay(TargetIndex.A, false, -0.5f);
			yield return new Toil
			{
				initAction = delegate()
				{
					Find.ResearchManager.ApplyTechprint(this.TechprintComp.Props.project, this.pawn);
					this.Techprint.Destroy(DestroyMode.Vanish);
					SoundDefOf.TechprintApplied.PlayOneShotOnCamera(null);
				},
				defaultCompleteMode = ToilCompleteMode.Instant
			};
			yield break;
		}

		// Token: 0x04001DA0 RID: 7584
		private const TargetIndex ResearchBenchInd = TargetIndex.A;

		// Token: 0x04001DA1 RID: 7585
		private const TargetIndex TechprintInd = TargetIndex.B;

		// Token: 0x04001DA2 RID: 7586
		private const TargetIndex HaulCell = TargetIndex.C;

		// Token: 0x04001DA3 RID: 7587
		private const int Duration = 600;
	}
}
