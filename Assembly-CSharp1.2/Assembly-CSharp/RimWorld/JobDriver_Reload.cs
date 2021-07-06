using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000BF5 RID: 3061
	public class JobDriver_Reload : JobDriver
	{
		// Token: 0x17000B55 RID: 2901
		// (get) Token: 0x06004803 RID: 18435 RVA: 0x0018EA98 File Offset: 0x0018CC98
		private Thing Gear
		{
			get
			{
				return this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x06004804 RID: 18436 RVA: 0x000344A7 File Offset: 0x000326A7
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			this.pawn.ReserveAsManyAsPossible(this.job.GetTargetQueue(TargetIndex.B), this.job, 1, -1, null);
			return true;
		}

		// Token: 0x06004805 RID: 18437 RVA: 0x000344CA File Offset: 0x000326CA
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_Reload.<>c__DisplayClass5_0 CS$<>8__locals1 = new JobDriver_Reload.<>c__DisplayClass5_0();
			CS$<>8__locals1.<>4__this = this;
			JobDriver_Reload.<>c__DisplayClass5_0 CS$<>8__locals2 = CS$<>8__locals1;
			Thing gear = this.Gear;
			CS$<>8__locals2.comp = ((gear != null) ? gear.TryGetComp<CompReloadable>() : null);
			this.FailOn(() => CS$<>8__locals1.comp == null);
			this.FailOn(() => CS$<>8__locals1.comp.Wearer != CS$<>8__locals1.<>4__this.pawn);
			this.FailOn(() => !CS$<>8__locals1.comp.NeedsReload(true));
			this.FailOnDestroyedOrNull(TargetIndex.A);
			this.FailOnIncapable(PawnCapacityDefOf.Manipulation);
			Toil getNextIngredient = Toils_General.Label();
			yield return getNextIngredient;
			foreach (Toil toil in this.ReloadAsMuchAsPossible(CS$<>8__locals1.comp))
			{
				yield return toil;
			}
			IEnumerator<Toil> enumerator = null;
			yield return Toils_JobTransforms.ExtractNextTargetFromQueue(TargetIndex.B, true);
			yield return Toils_Goto.GotoThing(TargetIndex.B, PathEndMode.ClosestTouch).FailOnDespawnedNullOrForbidden(TargetIndex.B).FailOnSomeonePhysicallyInteracting(TargetIndex.B);
			yield return Toils_Haul.StartCarryThing(TargetIndex.B, false, true, false).FailOnDestroyedNullOrForbidden(TargetIndex.B);
			yield return Toils_Jump.JumpIf(getNextIngredient, () => !CS$<>8__locals1.<>4__this.job.GetTargetQueue(TargetIndex.B).NullOrEmpty<LocalTargetInfo>());
			foreach (Toil toil2 in this.ReloadAsMuchAsPossible(CS$<>8__locals1.comp))
			{
				yield return toil2;
			}
			enumerator = null;
			yield return new Toil
			{
				initAction = delegate()
				{
					Thing carriedThing = CS$<>8__locals1.<>4__this.pawn.carryTracker.CarriedThing;
					if (carriedThing != null && !carriedThing.Destroyed)
					{
						Thing thing;
						CS$<>8__locals1.<>4__this.pawn.carryTracker.TryDropCarriedThing(CS$<>8__locals1.<>4__this.pawn.Position, ThingPlaceMode.Near, out thing, null);
					}
				},
				defaultCompleteMode = ToilCompleteMode.Instant
			};
			yield break;
			yield break;
		}

		// Token: 0x06004806 RID: 18438 RVA: 0x000344DA File Offset: 0x000326DA
		private IEnumerable<Toil> ReloadAsMuchAsPossible(CompReloadable comp)
		{
			Toil done = Toils_General.Label();
			yield return Toils_Jump.JumpIf(done, () => this.pawn.carryTracker.CarriedThing == null || this.pawn.carryTracker.CarriedThing.stackCount < comp.MinAmmoNeeded(true));
			yield return Toils_General.Wait(comp.Props.baseReloadTicks, TargetIndex.None).WithProgressBarToilDelay(TargetIndex.A, false, -0.5f);
			yield return new Toil
			{
				initAction = delegate()
				{
					Thing carriedThing = this.pawn.carryTracker.CarriedThing;
					comp.ReloadFrom(carriedThing);
				},
				defaultCompleteMode = ToilCompleteMode.Instant
			};
			yield return done;
			yield break;
		}

		// Token: 0x0400301E RID: 12318
		private const TargetIndex GearInd = TargetIndex.A;

		// Token: 0x0400301F RID: 12319
		private const TargetIndex AmmoInd = TargetIndex.B;
	}
}
