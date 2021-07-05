using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000583 RID: 1411
	public class JobDriver_EmancipateSlave : JobDriver
	{
		// Token: 0x1700082D RID: 2093
		// (get) Token: 0x06002961 RID: 10593 RVA: 0x000FA5B8 File Offset: 0x000F87B8
		private Pawn Slave
		{
			get
			{
				return (Pawn)this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x1700082E RID: 2094
		// (get) Token: 0x06002962 RID: 10594 RVA: 0x000FA5E0 File Offset: 0x000F87E0
		private Building_Bed Bed
		{
			get
			{
				return (Building_Bed)this.job.GetTarget(TargetIndex.B).Thing;
			}
		}

		// Token: 0x06002963 RID: 10595 RVA: 0x000FA606 File Offset: 0x000F8806
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.Slave, this.job, 1, -1, null, errorOnFailed);
		}

		// Token: 0x06002964 RID: 10596 RVA: 0x000FA628 File Offset: 0x000F8828
		protected override IEnumerable<Toil> MakeNewToils()
		{
			if (!ModLister.CheckIdeology("Emancipate slave"))
			{
				yield break;
			}
			this.FailOnDestroyedOrNull(TargetIndex.A);
			this.FailOn(() => this.Slave.guest.slaveInteractionMode != SlaveInteractionModeDefOf.Emancipate);
			this.FailOnDowned(TargetIndex.A);
			this.FailOnAggroMentalState(TargetIndex.A);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.ClosestTouch).FailOn(() => !this.Slave.IsSlaveOfColony || !this.Slave.guest.SlaveIsSecure).FailOnSomeonePhysicallyInteracting(TargetIndex.A);
			yield return new Toil
			{
				initAction = delegate()
				{
					GenGuest.EmancipateSlave(this.pawn, this.Slave);
				}
			};
			yield break;
		}

		// Token: 0x040019A4 RID: 6564
		private const TargetIndex SlaveInd = TargetIndex.A;

		// Token: 0x040019A5 RID: 6565
		private const TargetIndex BedInd = TargetIndex.B;
	}
}
