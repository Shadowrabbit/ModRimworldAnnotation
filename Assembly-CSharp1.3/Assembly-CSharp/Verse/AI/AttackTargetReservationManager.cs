using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000610 RID: 1552
	public class AttackTargetReservationManager : IExposable
	{
		// Token: 0x06002CDD RID: 11485 RVA: 0x0010D8E3 File Offset: 0x0010BAE3
		public AttackTargetReservationManager(Map map)
		{
			this.map = map;
		}

		// Token: 0x06002CDE RID: 11486 RVA: 0x0010D900 File Offset: 0x0010BB00
		public void Reserve(Pawn claimant, Job job, IAttackTarget target)
		{
			if (target == null)
			{
				Log.Warning(claimant + " tried to reserve null attack target.");
				return;
			}
			if (this.IsReservedBy(claimant, target))
			{
				return;
			}
			AttackTargetReservationManager.AttackTargetReservation attackTargetReservation = new AttackTargetReservationManager.AttackTargetReservation();
			attackTargetReservation.target = target;
			attackTargetReservation.claimant = claimant;
			attackTargetReservation.job = job;
			this.reservations.Add(attackTargetReservation);
		}

		// Token: 0x06002CDF RID: 11487 RVA: 0x0010D954 File Offset: 0x0010BB54
		public void Release(Pawn claimant, Job job, IAttackTarget target)
		{
			if (target == null)
			{
				Log.Warning(claimant + " tried to release reservation on null attack target.");
				return;
			}
			for (int i = 0; i < this.reservations.Count; i++)
			{
				AttackTargetReservationManager.AttackTargetReservation attackTargetReservation = this.reservations[i];
				if (attackTargetReservation.target == target && attackTargetReservation.claimant == claimant && attackTargetReservation.job == job)
				{
					this.reservations.RemoveAt(i);
					return;
				}
			}
			Log.Warning(string.Concat(new object[]
			{
				claimant,
				" with job ",
				job,
				" tried to release reservation on target ",
				target,
				", but it's not reserved by him."
			}));
		}

		// Token: 0x06002CE0 RID: 11488 RVA: 0x0010D9F4 File Offset: 0x0010BBF4
		public bool CanReserve(Pawn claimant, IAttackTarget target)
		{
			if (this.IsReservedBy(claimant, target))
			{
				return true;
			}
			int reservationsCount = this.GetReservationsCount(target, claimant.Faction);
			int maxPreferredReservationsCount = this.GetMaxPreferredReservationsCount(target);
			return reservationsCount < maxPreferredReservationsCount;
		}

		// Token: 0x06002CE1 RID: 11489 RVA: 0x0010DA28 File Offset: 0x0010BC28
		public bool IsReservedBy(Pawn claimant, IAttackTarget target)
		{
			for (int i = 0; i < this.reservations.Count; i++)
			{
				AttackTargetReservationManager.AttackTargetReservation attackTargetReservation = this.reservations[i];
				if (attackTargetReservation.target == target && attackTargetReservation.claimant == claimant)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06002CE2 RID: 11490 RVA: 0x0010DA70 File Offset: 0x0010BC70
		public void ReleaseAllForTarget(IAttackTarget target)
		{
			this.reservations.RemoveAll((AttackTargetReservationManager.AttackTargetReservation x) => x.target == target);
		}

		// Token: 0x06002CE3 RID: 11491 RVA: 0x0010DAA4 File Offset: 0x0010BCA4
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

		// Token: 0x06002CE4 RID: 11492 RVA: 0x0010DB00 File Offset: 0x0010BD00
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

		// Token: 0x06002CE5 RID: 11493 RVA: 0x0010DB48 File Offset: 0x0010BD48
		public IAttackTarget FirstReservationFor(Pawn claimant)
		{
			for (int i = this.reservations.Count - 1; i >= 0; i--)
			{
				if (this.reservations[i].claimant == claimant)
				{
					return this.reservations[i].target;
				}
			}
			return null;
		}

		// Token: 0x06002CE6 RID: 11494 RVA: 0x0010DB94 File Offset: 0x0010BD94
		public void ExposeData()
		{
			Scribe_Collections.Look<AttackTargetReservationManager.AttackTargetReservation>(ref this.reservations, "reservations", LookMode.Deep, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.reservations.RemoveAll((AttackTargetReservationManager.AttackTargetReservation x) => x.target == null);
				if (this.reservations.RemoveAll((AttackTargetReservationManager.AttackTargetReservation x) => x.claimant.DestroyedOrNull()) != 0)
				{
					Log.Warning("Some attack target reservations had null or destroyed claimant.");
				}
			}
		}

		// Token: 0x06002CE7 RID: 11495 RVA: 0x0010DC20 File Offset: 0x0010BE20
		private int GetReservationsCount(IAttackTarget target, Faction faction)
		{
			int num = 0;
			for (int i = 0; i < this.reservations.Count; i++)
			{
				AttackTargetReservationManager.AttackTargetReservation attackTargetReservation = this.reservations[i];
				if (attackTargetReservation.target == target && attackTargetReservation.claimant.Faction == faction)
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x06002CE8 RID: 11496 RVA: 0x0010DC70 File Offset: 0x0010BE70
		private int GetMaxPreferredReservationsCount(IAttackTarget target)
		{
			int num = 0;
			CellRect cellRect = target.Thing.OccupiedRect();
			foreach (IntVec3 c in cellRect.ExpandedBy(1))
			{
				if (!cellRect.Contains(c) && c.InBounds(this.map) && c.Standable(this.map))
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x04001BA3 RID: 7075
		private Map map;

		// Token: 0x04001BA4 RID: 7076
		private List<AttackTargetReservationManager.AttackTargetReservation> reservations = new List<AttackTargetReservationManager.AttackTargetReservation>();

		// Token: 0x02001DB7 RID: 7607
		public class AttackTargetReservation : IExposable
		{
			// Token: 0x0600AB88 RID: 43912 RVA: 0x0039159B File Offset: 0x0038F79B
			public void ExposeData()
			{
				Scribe_References.Look<IAttackTarget>(ref this.target, "target", false);
				Scribe_References.Look<Pawn>(ref this.claimant, "claimant", false);
				Scribe_References.Look<Job>(ref this.job, "job", false);
			}

			// Token: 0x0400722D RID: 29229
			public IAttackTarget target;

			// Token: 0x0400722E RID: 29230
			public Pawn claimant;

			// Token: 0x0400722F RID: 29231
			public Job job;
		}
	}
}
