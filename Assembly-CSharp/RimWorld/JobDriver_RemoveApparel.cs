using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000C6C RID: 3180
	public class JobDriver_RemoveApparel : JobDriver
	{
		// Token: 0x17000BAF RID: 2991
		// (get) Token: 0x06004A7D RID: 19069 RVA: 0x001A1E80 File Offset: 0x001A0080
		private Apparel Apparel
		{
			get
			{
				return (Apparel)this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x06004A7E RID: 19070 RVA: 0x000355BE File Offset: 0x000337BE
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.duration, "duration", 0, false);
		}

		// Token: 0x06004A7F RID: 19071 RVA: 0x0000A2A7 File Offset: 0x000084A7
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return true;
		}

		// Token: 0x06004A80 RID: 19072 RVA: 0x000355D8 File Offset: 0x000337D8
		public override void Notify_Starting()
		{
			base.Notify_Starting();
			this.duration = (int)(this.Apparel.GetStatValue(StatDefOf.EquipDelay, true) * 60f);
		}

		// Token: 0x06004A81 RID: 19073 RVA: 0x000355FE File Offset: 0x000337FE
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
				Toil carryToCell = Toils_Haul.CarryHauledThingToCell(TargetIndex.B);
				yield return carryToCell;
				yield return Toils_Haul.PlaceHauledThingInCell(TargetIndex.B, carryToCell, true, false);
				carryToCell = null;
			}
			yield break;
		}

		// Token: 0x04003172 RID: 12658
		private int duration;

		// Token: 0x04003173 RID: 12659
		private const TargetIndex ApparelInd = TargetIndex.A;
	}
}
