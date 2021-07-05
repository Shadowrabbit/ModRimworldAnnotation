using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020006E4 RID: 1764
	public class JobDriver_BeatFire : JobDriver
	{
		// Token: 0x1700092E RID: 2350
		// (get) Token: 0x0600312A RID: 12586 RVA: 0x0011F3FD File Offset: 0x0011D5FD
		protected Fire TargetFire
		{
			get
			{
				return (Fire)this.job.targetA.Thing;
			}
		}

		// Token: 0x0600312B RID: 12587 RVA: 0x000126F5 File Offset: 0x000108F5
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return true;
		}

		// Token: 0x0600312C RID: 12588 RVA: 0x0011F414 File Offset: 0x0011D614
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedOrNull(TargetIndex.A);
			Toil beat = new Toil();
			Toil approach = new Toil();
			approach.initAction = delegate()
			{
				if (this.Map.reservationManager.CanReserve(this.pawn, this.TargetFire, 1, -1, null, false))
				{
					this.pawn.Reserve(this.TargetFire, this.job, 1, -1, null, true);
				}
				this.pawn.pather.StartPath(this.TargetFire, PathEndMode.Touch);
			};
			approach.tickAction = delegate()
			{
				if (this.pawn.pather.Moving && this.pawn.pather.nextCell != this.TargetFire.Position)
				{
					this.StartBeatingFireIfAnyAt(this.pawn.pather.nextCell, beat);
				}
				if (this.pawn.Position != this.TargetFire.Position)
				{
					this.StartBeatingFireIfAnyAt(this.pawn.Position, beat);
				}
			};
			approach.FailOnDespawnedOrNull(TargetIndex.A);
			approach.defaultCompleteMode = ToilCompleteMode.PatherArrival;
			approach.atomicWithPrevious = true;
			yield return approach;
			beat.tickAction = delegate()
			{
				if (!this.pawn.CanReachImmediate(this.TargetFire, PathEndMode.Touch))
				{
					this.JumpToToil(approach);
					return;
				}
				if (this.pawn.Position != this.TargetFire.Position && this.StartBeatingFireIfAnyAt(this.pawn.Position, beat))
				{
					return;
				}
				this.pawn.natives.TryBeatFire(this.TargetFire);
				if (this.TargetFire.Destroyed)
				{
					this.pawn.records.Increment(RecordDefOf.FiresExtinguished);
					this.pawn.jobs.EndCurrentJob(JobCondition.Succeeded, true, true);
					return;
				}
			};
			beat.FailOnDespawnedOrNull(TargetIndex.A);
			beat.defaultCompleteMode = ToilCompleteMode.Never;
			yield return beat;
			yield break;
		}

		// Token: 0x0600312D RID: 12589 RVA: 0x0011F424 File Offset: 0x0011D624
		private bool StartBeatingFireIfAnyAt(IntVec3 cell, Toil nextToil)
		{
			List<Thing> thingList = cell.GetThingList(base.Map);
			for (int i = 0; i < thingList.Count; i++)
			{
				Fire fire = thingList[i] as Fire;
				if (fire != null && fire.parent == null)
				{
					this.job.targetA = fire;
					this.pawn.pather.StopDead();
					base.JumpToToil(nextToil);
					return true;
				}
			}
			return false;
		}
	}
}
