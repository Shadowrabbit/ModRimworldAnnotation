using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02000BAB RID: 2987
	public class JobDriver_ApplyTechprint : JobDriver
	{
		// Token: 0x17000AFA RID: 2810
		// (get) Token: 0x0600462C RID: 17964 RVA: 0x00194890 File Offset: 0x00192A90
		protected Building_ResearchBench ResearchBench
		{
			get
			{
				return (Building_ResearchBench)this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x17000AFB RID: 2811
		// (get) Token: 0x0600462D RID: 17965 RVA: 0x00190280 File Offset: 0x0018E480
		protected Thing Techprint
		{
			get
			{
				return this.job.GetTarget(TargetIndex.B).Thing;
			}
		}

		// Token: 0x17000AFC RID: 2812
		// (get) Token: 0x0600462E RID: 17966 RVA: 0x00033580 File Offset: 0x00031780
		protected CompTechprint TechprintComp
		{
			get
			{
				return this.Techprint.TryGetComp<CompTechprint>();
			}
		}

		// Token: 0x0600462F RID: 17967 RVA: 0x001948B8 File Offset: 0x00192AB8
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.ResearchBench, this.job, 1, -1, null, errorOnFailed) && this.pawn.Reserve(this.Techprint, this.job, 1, -1, null, errorOnFailed);
		}

		// Token: 0x06004630 RID: 17968 RVA: 0x0003358D File Offset: 0x0003178D
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

		// Token: 0x04002F36 RID: 12086
		private const TargetIndex ResearchBenchInd = TargetIndex.A;

		// Token: 0x04002F37 RID: 12087
		private const TargetIndex TechprintInd = TargetIndex.B;

		// Token: 0x04002F38 RID: 12088
		private const TargetIndex HaulCell = TargetIndex.C;

		// Token: 0x04002F39 RID: 12089
		private const int Duration = 600;
	}
}
