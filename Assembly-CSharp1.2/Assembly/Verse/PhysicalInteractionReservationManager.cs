using System;
using System.Collections.Generic;
using Verse.AI;

namespace Verse
{
	// Token: 0x020000B0 RID: 176
	public class PhysicalInteractionReservationManager : IExposable
	{
		// Token: 0x06000599 RID: 1433 RVA: 0x0008BEB0 File Offset: 0x0008A0B0
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

		// Token: 0x0600059A RID: 1434 RVA: 0x0008BEF0 File Offset: 0x0008A0F0
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
			}), false);
		}

		// Token: 0x0600059B RID: 1435 RVA: 0x0008BF7C File Offset: 0x0008A17C
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

		// Token: 0x0600059C RID: 1436 RVA: 0x0000AC2E File Offset: 0x00008E2E
		public bool IsReserved(LocalTargetInfo target)
		{
			return this.FirstReserverOf(target) != null;
		}

		// Token: 0x0600059D RID: 1437 RVA: 0x0008BFC8 File Offset: 0x0008A1C8
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

		// Token: 0x0600059E RID: 1438 RVA: 0x0008C010 File Offset: 0x0008A210
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

		// Token: 0x0600059F RID: 1439 RVA: 0x0008C060 File Offset: 0x0008A260
		public void ReleaseAllForTarget(LocalTargetInfo target)
		{
			this.reservations.RemoveAll((PhysicalInteractionReservationManager.PhysicalInteractionReservation x) => x.target == target);
		}

		// Token: 0x060005A0 RID: 1440 RVA: 0x0008C094 File Offset: 0x0008A294
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

		// Token: 0x060005A1 RID: 1441 RVA: 0x0008C0F0 File Offset: 0x0008A2F0
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

		// Token: 0x060005A2 RID: 1442 RVA: 0x0008C138 File Offset: 0x0008A338
		public void ExposeData()
		{
			Scribe_Collections.Look<PhysicalInteractionReservationManager.PhysicalInteractionReservation>(ref this.reservations, "reservations", LookMode.Deep, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (this.reservations.RemoveAll((PhysicalInteractionReservationManager.PhysicalInteractionReservation x) => x.claimant.DestroyedOrNull()) != 0)
				{
					Log.Warning("Some physical interaction reservations had null or destroyed claimant.", false);
				}
			}
		}

		// Token: 0x040002BB RID: 699
		private List<PhysicalInteractionReservationManager.PhysicalInteractionReservation> reservations = new List<PhysicalInteractionReservationManager.PhysicalInteractionReservation>();

		// Token: 0x020000B1 RID: 177
		public class PhysicalInteractionReservation : IExposable
		{
			// Token: 0x060005A4 RID: 1444 RVA: 0x0000AC4D File Offset: 0x00008E4D
			public void ExposeData()
			{
				Scribe_TargetInfo.Look(ref this.target, "target");
				Scribe_References.Look<Pawn>(ref this.claimant, "claimant", false);
				Scribe_References.Look<Job>(ref this.job, "job", false);
			}

			// Token: 0x040002BC RID: 700
			public LocalTargetInfo target;

			// Token: 0x040002BD RID: 701
			public Pawn claimant;

			// Token: 0x040002BE RID: 702
			public Job job;
		}
	}
}
