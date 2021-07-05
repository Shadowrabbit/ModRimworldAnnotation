using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200075B RID: 1883
	public class JobDriver_SuppressSlave : JobDriver
	{
		// Token: 0x170009AD RID: 2477
		// (get) Token: 0x06003425 RID: 13349 RVA: 0x0011D55B File Offset: 0x0011B75B
		protected Pawn Slave
		{
			get
			{
				return (Pawn)this.job.targetA.Thing;
			}
		}

		// Token: 0x06003426 RID: 13350 RVA: 0x000FC76A File Offset: 0x000FA96A
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.job.targetA, this.job, 1, -1, null, errorOnFailed);
		}

		// Token: 0x06003427 RID: 13351 RVA: 0x001280C9 File Offset: 0x001262C9
		protected override IEnumerable<Toil> MakeNewToils()
		{
			if (!ModLister.CheckIdeology("Suppress slave"))
			{
				yield break;
			}
			this.FailOnDespawnedOrNull(TargetIndex.A);
			this.FailOnMentalState(TargetIndex.A);
			this.FailOnNotAwake(TargetIndex.A);
			this.FailOn(() => !this.Slave.IsSlaveOfColony);
			yield return Toils_Interpersonal.GotoSlave(this.pawn, this.Slave);
			yield return Toils_Interpersonal.WaitToBeAbleToInteract(this.pawn);
			yield return Toils_Interpersonal.GotoInteractablePosition(TargetIndex.A);
			yield return this.SetLastSuppressionTime(TargetIndex.A);
			yield return this.TrySuppress(TargetIndex.A);
			yield break;
		}

		// Token: 0x06003428 RID: 13352 RVA: 0x001280D9 File Offset: 0x001262D9
		private Toil TrySuppress(TargetIndex slaveInd)
		{
			return new Toil
			{
				initAction = delegate()
				{
					this.pawn.interactions.TryInteractWith(this.Slave, InteractionDefOf.Suppress);
					PawnUtility.ForceWait(this.Slave, this.SuppressDuration, this.pawn, false);
				},
				socialMode = RandomSocialMode.Off,
				defaultCompleteMode = ToilCompleteMode.Delay,
				defaultDuration = this.SuppressDuration
			};
		}

		// Token: 0x06003429 RID: 13353 RVA: 0x0012810C File Offset: 0x0012630C
		private Toil SetLastSuppressionTime(TargetIndex targetInd)
		{
			Toil toil = new Toil();
			toil.initAction = delegate()
			{
				((Pawn)toil.actor.jobs.curJob.GetTarget(targetInd).Thing).mindState.lastSlaveSuppressedTick = Find.TickManager.TicksGame;
			};
			toil.defaultCompleteMode = ToilCompleteMode.Instant;
			return toil;
		}

		// Token: 0x04001E38 RID: 7736
		private int SuppressDuration = 180;
	}
}
