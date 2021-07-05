using System;
using System.Collections.Generic;
using Verse.AI;

namespace Verse
{
	// Token: 0x02000066 RID: 102
	public class PhysicalInteractionReservationManager : IExposable
	{
		// Token: 0x06000430 RID: 1072 RVA: 0x00015FD4 File Offset: 0x000141D4
		public void Reserve(Pawn claimant, Job job, LocalTargetInfo target)
		{
			if (this.IsReservedBy(claimant, target))
			{
				return;
			}
			PhysicalInteractionReservationManager.PhysicalInteractionReservation physicalInteractionReservation = new PhysicalInteractionReservationManager.PhysicalInteractionReservation();
			physicalInteractionReservation.target = target;
			physicalInteractionReservation.claimant = claimant;
			physicalInteractionReservation.job = job;
			this.reservations.Add(physicalInteractionReservation);
		}

		// Token: 0x06000431 RID: 1073 RVA: 0x00016014 File Offset: 0x00014214
		public void Release(Pawn claimant, Job job, LocalTargetInfo target)
		{
			for (int i = 0; i < this.reservations.Count; i++)
			{
				PhysicalInteractionReservationManager.PhysicalInteractionReservation physicalInteractionReservation = this.reservations[i];
				if (physicalInteractionReservation.target == target && physicalInteractionReservation.claimant == claimant && physicalInteractionReservation.job == job)
				{
					this.reservations.RemoveAt(i);
					return;
				}
			}
			Log.Warning(string.Concat(new object[]
			{
				claimant,
				" tried to release reservation on target ",
				target,
				", but it's not reserved by him."
			}));
		}

		// Token: 0x06000432 RID: 1074 RVA: 0x000160A0 File Offset: 0x000142A0
		public bool IsReservedBy(Pawn claimant, LocalTargetInfo target)
		{
			for (int i = 0; i < this.reservations.Count; i++)
			{
				PhysicalInteractionReservationManager.PhysicalInteractionReservation physicalInteractionReservation = this.reservations[i];
				if (physicalInteractionReservation.target == target && physicalInteractionReservation.claimant == claimant)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000433 RID: 1075 RVA: 0x000160EA File Offset: 0x000142EA
		public bool IsReserved(LocalTargetInfo target)
		{
			return this.FirstReserverOf(target) != null;
		}

		// Token: 0x06000434 RID: 1076 RVA: 0x000160F8 File Offset: 0x000142F8
		public Pawn FirstReserverOf(LocalTargetInfo target)
		{
			for (int i = 0; i < this.reservations.Count; i++)
			{
				PhysicalInteractionReservationManager.PhysicalInteractionReservation physicalInteractionReservation = this.reservations[i];
				if (physicalInteractionReservation.target == target)
				{
					return physicalInteractionReservation.claimant;
				}
			}
			return null;
		}

		// Token: 0x06000435 RID: 1077 RVA: 0x00016140 File Offset: 0x00014340
		public LocalTargetInfo FirstReservationFor(Pawn claimant)
		{
			for (int i = this.reservations.Count - 1; i >= 0; i--)
			{
				if (this.reservations[i].claimant == claimant)
				{
					return this.reservations[i].target;
				}
			}
			return LocalTargetInfo.Invalid;
		}

		// Token: 0x06000436 RID: 1078 RVA: 0x00016190 File Offset: 0x00014390
		public void ReleaseAllForTarget(LocalTargetInfo target)
		{
			this.reservations.RemoveAll((PhysicalInteractionReservationManager.PhysicalInteractionReservation x) => x.target == target);
		}

		// Token: 0x06000437 RID: 1079 RVA: 0x000161C4 File Offset: 0x000143C4
		public void ReleaseClaimedBy(Pawn claimant, Job job)
		{
			for (int i = this.reservations.Count - 1; i >= 0; i--)
			{
				if (this.reservations[i].claimant == claimant && this.reservations[i].job == job)
				{
					this.reservations.RemoveAt(i);
				}
			}
		}

		// Token: 0x06000438 RID: 1080 RVA: 0x00016220 File Offset: 0x00014420
		public void ReleaseAllClaimedBy(Pawn claimant)
		{
			for (int i = this.reservations.Count - 1; i >= 0; i--)
			{
				if (this.reservations[i].claimant == claimant)
				{
					this.reservations.RemoveAt(i);
				}
			}
		}

		// Token: 0x06000439 RID: 1081 RVA: 0x00016268 File Offset: 0x00014468
		public void ExposeData()
		{
			Scribe_Collections.Look<PhysicalInteractionReservationManager.PhysicalInteractionReservation>(ref this.reservations, "reservations", LookMode.Deep, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (this.reservations.RemoveAll((PhysicalInteractionReservationManager.PhysicalInteractionReservation x) => x.claimant.DestroyedOrNull()) != 0)
				{
					Log.Warning("Some physical interaction reservations had null or destroyed claimant.");
				}
			}
		}

		// Token: 0x04000146 RID: 326
		private List<PhysicalInteractionReservationManager.PhysicalInteractionReservation> reservations = new List<PhysicalInteractionReservationManager.PhysicalInteractionReservation>();

		// Token: 0x020018AB RID: 6315
		public class PhysicalInteractionReservation : IExposable
		{
			// Token: 0x0600943E RID: 37950 RVA: 0x0034EB93 File Offset: 0x0034CD93
			public void ExposeData()
			{
				Scribe_TargetInfo.Look(ref this.target, "target");
				Scribe_References.Look<Pawn>(ref this.claimant, "claimant", false);
				Scribe_References.Look<Job>(ref this.job, "job", false);
			}

			// Token: 0x04005E51 RID: 24145
			public LocalTargetInfo target;

			// Token: 0x04005E52 RID: 24146
			public Pawn claimant;

			// Token: 0x04005E53 RID: 24147
			public Job job;
		}
	}
}
