using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200075D RID: 1885
	public class JobDriver_RemoveApparel : JobDriver
	{
		// Token: 0x170009AF RID: 2479
		// (get) Token: 0x06003433 RID: 13363 RVA: 0x00128220 File Offset: 0x00126420
		private Apparel Apparel
		{
			get
			{
				return (Apparel)this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x06003434 RID: 13364 RVA: 0x00128246 File Offset: 0x00126446
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.duration, "duration", 0, false);
		}

		// Token: 0x06003435 RID: 13365 RVA: 0x000126F5 File Offset: 0x000108F5
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return true;
		}

		// Token: 0x06003436 RID: 13366 RVA: 0x00128260 File Offset: 0x00126460
		public override void Notify_Starting()
		{
			base.Notify_Starting();
			this.duration = (int)(this.Apparel.GetStatValue(StatDefOf.EquipDelay, true) * 60f);
		}

		// Token: 0x06003437 RID: 13367 RVA: 0x00128286 File Offset: 0x00126486
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDestroyedOrNull(TargetIndex.A);
			yield return Toils_General.Wait(this.duration, TargetIndex.None).WithProgressBarToilDelay(TargetIndex.A, false, -0.5f);
			yield return Toils_General.Do(delegate
			{
				if (!this.pawn.apparel.WornApparel.Contains(this.Apparel))
				{
					base.EndJobWith(JobCondition.Incompletable);
					return;
				}
				Apparel apparel;
				if (!this.pawn.apparel.TryDrop(this.Apparel, out apparel))
				{
					base.EndJobWith(JobCondition.Incompletable);
					return;
				}
				this.job.targetA = apparel;
				if (!this.job.haulDroppedApparel)
				{
					base.EndJobWith(JobCondition.Succeeded);
					return;
				}
				apparel.SetForbidden(false, false);
				StoragePriority currentPriority = StoreUtility.CurrentStoragePriorityOf(apparel);
				IntVec3 c;
				if (StoreUtility.TryFindBestBetterStoreCellFor(apparel, this.pawn, base.Map, currentPriority, this.pawn.Faction, out c, true))
				{
					this.job.count = apparel.stackCount;
					this.job.targetB = c;
					return;
				}
				base.EndJobWith(JobCondition.Incompletable);
			});
			if (this.job.haulDroppedApparel)
			{
				yield return Toils_Reserve.Reserve(TargetIndex.B, 1, -1, null);
				yield return Toils_Reserve.Reserve(TargetIndex.A, 1, -1, null);
				yield return Toils_Haul.StartCarryThing(TargetIndex.A, false, false, false).FailOn(() => !this.pawn.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation));
				Toil carryToCell = Toils_Haul.CarryHauledThingToCell(TargetIndex.B, PathEndMode.ClosestTouch);
				yield return carryToCell;
				yield return Toils_Haul.PlaceHauledThingInCell(TargetIndex.B, carryToCell, true, false);
				carryToCell = null;
			}
			yield break;
		}

		// Token: 0x04001E3A RID: 7738
		private int duration;

		// Token: 0x04001E3B RID: 7739
		private const TargetIndex ApparelInd = TargetIndex.A;
	}
}
