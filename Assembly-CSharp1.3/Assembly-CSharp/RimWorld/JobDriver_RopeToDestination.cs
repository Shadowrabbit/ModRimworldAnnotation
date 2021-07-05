using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020006C7 RID: 1735
	public abstract class JobDriver_RopeToDestination : JobDriver
	{
		// Token: 0x0600305C RID: 12380
		protected abstract bool HasRopeeArrived(Pawn ropee, bool roperWaitingAtDest);

		// Token: 0x0600305D RID: 12381
		protected abstract void ProcessArrivedRopee(Pawn ropee);

		// Token: 0x0600305E RID: 12382
		protected abstract bool ShouldOpportunisticallyRopeAnimal(Pawn animal);

		// Token: 0x0600305F RID: 12383 RVA: 0x00002688 File Offset: 0x00000888
		protected virtual Thing FindDistantAnimalToRope()
		{
			return null;
		}

		// Token: 0x06003060 RID: 12384 RVA: 0x0001276E File Offset: 0x0001096E
		protected virtual bool UpdateDestination()
		{
			return false;
		}

		// Token: 0x06003061 RID: 12385 RVA: 0x0011D8BC File Offset: 0x0011BABC
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			this.UpdateDestination();
			this.pawn.Map.pawnDestinationReservationManager.Reserve(this.pawn, this.job, this.job.GetTarget(TargetIndex.B).Cell);
			return this.pawn.Reserve(this.job.GetTarget(TargetIndex.A), this.job, 1, -1, null, errorOnFailed);
		}

		// Token: 0x06003062 RID: 12386 RVA: 0x0011D926 File Offset: 0x0011BB26
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.UpdateDestination();
			base.AddFinishAction(delegate
			{
				Pawn pawn = this.pawn;
				if (pawn == null)
				{
					return;
				}
				Pawn_RopeTracker roping = pawn.roping;
				if (roping == null)
				{
					return;
				}
				roping.DropRopes();
			});
			Toil findAnotherAnimal = Toils_General.Label();
			Toil topOfLoop = Toils_General.Label();
			yield return topOfLoop;
			yield return Toils_Jump.JumpIf(findAnotherAnimal, delegate
			{
				Pawn pawn = this.job.GetTarget(TargetIndex.A).Thing as Pawn;
				return ((pawn != null) ? pawn.roping.RopedByPawn : null) == this.pawn;
			});
			yield return Toils_Reserve.Reserve(TargetIndex.A, 1, -1, null);
			Toil toil = Toils_Rope.GotoRopeAttachmentInteractionCell(TargetIndex.A);
			yield return toil;
			yield return Toils_Rope.RopePawn(TargetIndex.A);
			yield return findAnotherAnimal;
			yield return Toils_Jump.JumpIf(topOfLoop, new Func<bool>(this.FindAnotherAnimalToRope));
			findAnotherAnimal = null;
			topOfLoop = null;
			topOfLoop = Toils_General.Label();
			yield return topOfLoop;
			Toil toil2 = Toils_Goto.Goto(TargetIndex.B, PathEndMode.OnCell);
			this.MatchLocomotionUrgency(toil2);
			toil2.AddPreTickAction(delegate
			{
				this.ProcessRopeesThatHaveArrived(false);
			});
			toil2.FailOn(() => !this.pawn.roping.IsRopingOthers);
			yield return toil2;
			yield return Toils_Jump.JumpIf(topOfLoop, new Func<bool>(this.UpdateDestination));
			topOfLoop = null;
			topOfLoop = Toils_General.Wait(60, TargetIndex.A);
			topOfLoop.AddPreTickAction(delegate
			{
				this.ProcessRopeesThatHaveArrived(true);
			});
			yield return topOfLoop;
			yield return Toils_Jump.JumpIf(topOfLoop, () => this.pawn.roping.IsRopingOthers);
			topOfLoop = null;
			yield break;
		}

		// Token: 0x06003063 RID: 12387 RVA: 0x0011D938 File Offset: 0x0011BB38
		private void ProcessRopeesThatHaveArrived(bool roperWaitingAtDest)
		{
			JobDriver_RopeToDestination.tmpRopees.Clear();
			JobDriver_RopeToDestination.tmpRopees.AddRange(this.pawn.roping.Ropees);
			foreach (Pawn pawn in JobDriver_RopeToDestination.tmpRopees)
			{
				if (this.HasRopeeArrived(pawn, roperWaitingAtDest))
				{
					this.pawn.roping.DropRope(pawn);
					if (pawn.jobs != null && pawn.CurJob != null && pawn.jobs.curDriver is JobDriver_FollowRoper)
					{
						pawn.jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
					}
					this.ProcessArrivedRopee(pawn);
				}
			}
		}

		// Token: 0x06003064 RID: 12388 RVA: 0x0011D9FC File Offset: 0x0011BBFC
		private void MatchLocomotionUrgency(Toil toil)
		{
			toil.AddPreInitAction(delegate
			{
				this.locomotionUrgencySameAs = this.SlowestRopee();
			});
			toil.AddFinishAction(delegate
			{
				this.locomotionUrgencySameAs = null;
			});
		}

		// Token: 0x06003065 RID: 12389 RVA: 0x0011DA24 File Offset: 0x0011BC24
		private Pawn SlowestRopee()
		{
			Pawn result;
			if (!this.pawn.roping.Ropees.TryMaxBy((Pawn p) => p.TicksPerMoveCardinal, out result))
			{
				return null;
			}
			return result;
		}

		// Token: 0x06003066 RID: 12390 RVA: 0x0011DA6C File Offset: 0x0011BC6C
		private bool FindAnotherAnimalToRope()
		{
			Pawn_MindState mindState = this.pawn.mindState;
			int? num;
			if (mindState == null)
			{
				num = null;
			}
			else
			{
				PawnDuty duty = mindState.duty;
				num = ((duty != null) ? duty.ropeeLimit : null);
			}
			int num2 = num ?? 10;
			if (this.pawn.roping.Ropees.Count >= num2)
			{
				return false;
			}
			Thing thing = GenClosest.ClosestThingReachable(this.pawn.Position, this.pawn.Map, ThingRequest.ForGroup(ThingRequestGroup.Pawn), PathEndMode.Touch, TraverseParms.For(this.pawn, Danger.Deadly, TraverseMode.ByPawn, false, false, false), 10f, new Predicate<Thing>(this.<FindAnotherAnimalToRope>g__AnimalValidator|15_0), null, 0, -1, false, RegionType.Set_Passable, false);
			if (thing == null)
			{
				thing = this.FindDistantAnimalToRope();
			}
			if (thing != null)
			{
				this.job.SetTarget(TargetIndex.A, thing);
				return true;
			}
			return false;
		}

		// Token: 0x06003071 RID: 12401 RVA: 0x0011DC04 File Offset: 0x0011BE04
		[CompilerGenerated]
		private bool <FindAnotherAnimalToRope>g__AnimalValidator|15_0(Thing thing)
		{
			Pawn pawn = thing as Pawn;
			return pawn != null && this.ShouldOpportunisticallyRopeAnimal(pawn);
		}

		// Token: 0x04001D4A RID: 7498
		private const TargetIndex AnimalInd = TargetIndex.A;

		// Token: 0x04001D4B RID: 7499
		public const TargetIndex DestCellInd = TargetIndex.B;

		// Token: 0x04001D4C RID: 7500
		private const int MaxAnimalsToRope = 10;

		// Token: 0x04001D4D RID: 7501
		private const int RopeMoreAnimalsScanRadius = 10;

		// Token: 0x04001D4E RID: 7502
		private static List<Pawn> tmpRopees = new List<Pawn>();
	}
}
