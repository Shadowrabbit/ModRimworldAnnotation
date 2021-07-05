using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000709 RID: 1801
	public class JobDriver_CarryDownedPawn : JobDriver
	{
		// Token: 0x17000951 RID: 2385
		// (get) Token: 0x06003202 RID: 12802 RVA: 0x00121BF0 File Offset: 0x0011FDF0
		protected Pawn Takee
		{
			get
			{
				return (Pawn)this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x06003203 RID: 12803 RVA: 0x00121C16 File Offset: 0x0011FE16
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.Takee, this.job, 1, -1, null, errorOnFailed);
		}

		// Token: 0x06003204 RID: 12804 RVA: 0x00121C38 File Offset: 0x0011FE38
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDestroyedOrNull(TargetIndex.A);
			this.FailOnAggroMentalStateAndHostile(TargetIndex.A);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.ClosestTouch).FailOnDespawnedNullOrForbidden(TargetIndex.A).FailOn(() => !this.Takee.Downed).FailOnSomeonePhysicallyInteracting(TargetIndex.A);
			Toil toil = Toils_Haul.StartCarryThing(TargetIndex.A, false, false, false);
			toil.AddPreInitAction(new Action(this.CheckMakeTakeeGuest));
			yield return toil;
			yield break;
		}

		// Token: 0x06003205 RID: 12805 RVA: 0x00121C48 File Offset: 0x0011FE48
		private void CheckMakeTakeeGuest()
		{
			if (!this.job.def.makeTargetPrisoner && this.Takee.Faction != Faction.OfPlayer && this.Takee.HostFaction != Faction.OfPlayer && this.Takee.guest != null && !this.Takee.IsWildMan())
			{
				this.Takee.guest.SetGuestStatus(Faction.OfPlayer, GuestStatus.Guest);
			}
		}

		// Token: 0x04001DAB RID: 7595
		private const TargetIndex TakeeIndex = TargetIndex.A;
	}
}
