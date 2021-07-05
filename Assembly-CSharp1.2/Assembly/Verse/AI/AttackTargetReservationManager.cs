using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000A6B RID: 2667
	public class AttackTargetReservationManager : IExposable
	{
		// Token: 0x06003FAC RID: 16300 RVA: 0x0002FBAA File Offset: 0x0002DDAA
		public AttackTargetReservationManager(Map map)
		{
			this.map = map;
		}

		// Token: 0x06003FAD RID: 16301 RVA: 0x00180660 File Offset: 0x0017E860
		public void Reserve(Pawn claimant, Job job, IAttackTarget target)
		{
			if (target == null)
			{
				Log.Warning(claimant + " tried to reserve null attack target.", false);
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

		// Token: 0x06003FAE RID: 16302 RVA: 0x001806B4 File Offset: 0x0017E8B4
		public void Release(Pawn claimant, Job job, IAttackTarget target)
		{
			if (target == null)
			{
				Log.Warning(claimant + " tried to release reservation on null attack target.", false);
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
			}), false);
		}

		// Token: 0x06003FAF RID: 16303 RVA: 0x00180758 File Offset: 0x0017E958
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

		// Token: 0x06003FB0 RID: 16304 RVA: 0x0018078C File Offset: 0x0017E98C
		public bool IsReservedBy(Pawn claimant, IAttackTarget target)
		{
			for (int i = 0; i < this.reservations.Count; i++)
			{
				AttackTargetReservation attackTargetReservation = this.reservations[i];
				if (attackTargetReservation.target == target && attackTargetReservation.claimant == claimant)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06003FB1 RID: 16305 RVA: 0x001807D4 File Offset: 0x0017E9D4
		public void ReleaseAllForTarget(IAttackTarget target)
		{
			this.reservations.RemoveAll((AttackTargetReservationManager.AttackTargetReservation x) => x.target == target);
		}

		// Token: 0x06003FB2 RID: 16306 RVA: 0x00180808 File Offset: 0x0017EA08
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

		// Token: 0x06003FB3 RID: 16307 RVA: 0x00180864 File Offset: 0x0017EA64
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

		// Token: 0x06003FB4 RID: 16308 RVA: 0x001808AC File Offset: 0x0017EAAC
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

		// Token: 0x06003FB5 RID: 16309 RVA: 0x001808F8 File Offset: 0x0017EAF8
		public void ExposeData()
		{
			Scribe_Collections.Look<AttackTargetReservationManager.AttackTargetReservation>(ref this.reservations, "reservations", LookMode.Deep, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.reservations.RemoveAll((AttackTargetReservationManager.AttackTargetReservation x) => x.target == null);
				if (this.reservations.RemoveAll((AttackTargetReservationManager.AttackTargetReservation x) => x.claimant.DestroyedOrNull()) != 0)
				{
					Log.Warning("Some attack target reservations had null or destroyed claimant.", false);
				}
			}
		}

		// Token: 0x06003FB6 RID: 16310 RVA: 0x00180988 File Offset: 0x0017EB88
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

		// Token: 0x06003FB7 RID: 16311 RVA: 0x001809D8 File Offset: 0x0017EBD8
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

		// Token: 0x04002C0B RID: 11275
		private Map map;

		// Token: 0x04002C0C RID: 11276
		private List<AttackTargetReservationManager.AttackTargetReservation> reservations = new List<AttackTargetReservationManager.AttackTargetReservation>();

		// Token: 0x02000A6C RID: 2668
		public class AttackTargetReservation : IExposable
		{
			// Token: 0x06003FB8 RID: 16312 RVA: 0x0002FBC4 File Offset: 0x0002DDC4
			public void ExposeData()
			{
				Scribe_References.Look<IAttackTarget>(ref this.target, "target", false);
				Scribe_References.Look<Pawn>(ref this.claimant, "claimant", false);
				Scribe_References.Look<Job>(ref this.job, "job", false);
			}

			// Token: 0x04002C0D RID: 11277
			public IAttackTarget target;

			// Token: 0x04002C0E RID: 11278
			public Pawn claimant;

			// Token: 0x04002C0F RID: 11279
			public Job job;
		}
	}
}
