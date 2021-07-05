using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200072F RID: 1839
	public class JobDriver_Reload : JobDriver
	{
		// Token: 0x17000985 RID: 2437
		// (get) Token: 0x0600330C RID: 13068 RVA: 0x0012424C File Offset: 0x0012244C
		private Thing Gear
		{
			get
			{
				return this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x0600330D RID: 13069 RVA: 0x0012426D File Offset: 0x0012246D
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			this.pawn.ReserveAsManyAsPossible(this.job.GetTargetQueue(TargetIndex.B), this.job, 1, -1, null);
			return true;
		}

		// Token: 0x0600330E RID: 13070 RVA: 0x00124290 File Offset: 0x00122490
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

		// Token: 0x0600330F RID: 13071 RVA: 0x001242A0 File Offset: 0x001224A0
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

		// Token: 0x04001DED RID: 7661
		private const TargetIndex GearInd = TargetIndex.A;

		// Token: 0x04001DEE RID: 7662
		private const TargetIndex AmmoInd = TargetIndex.B;
	}
}
