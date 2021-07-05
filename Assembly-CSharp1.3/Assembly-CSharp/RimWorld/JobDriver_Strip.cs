using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000733 RID: 1843
	public class JobDriver_Strip : JobDriver
	{
		// Token: 0x06003320 RID: 13088 RVA: 0x000FC76A File Offset: 0x000FA96A
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.job.targetA, this.job, 1, -1, null, errorOnFailed);
		}

		// Token: 0x06003321 RID: 13089 RVA: 0x00124482 File Offset: 0x00122682
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedOrNull(TargetIndex.A);
			this.FailOnAggroMentalState(TargetIndex.A);
			this.FailOn(() => !StrippableUtility.CanBeStrippedByColony(base.TargetThingA));
			Toil toil = new Toil();
			toil.initAction = delegate()
			{
				this.pawn.pather.StartPath(base.TargetThingA, PathEndMode.ClosestTouch);
			};
			toil.defaultCompleteMode = ToilCompleteMode.PatherArrival;
			toil.FailOnDespawnedNullOrForbidden(TargetIndex.A);
			yield return toil;
			yield return Toils_General.Wait(60, TargetIndex.None).WithProgressBarToilDelay(TargetIndex.A, false, -0.5f);
			yield return new Toil
			{
				initAction = delegate()
				{
					Thing thing = this.job.targetA.Thing;
					Designation designation = base.Map.designationManager.DesignationOn(thing, DesignationDefOf.Strip);
					if (designation != null)
					{
						designation.Delete();
					}
					IStrippable strippable = thing as IStrippable;
					if (strippable != null)
					{
						strippable.Strip();
					}
					this.pawn.records.Increment(RecordDefOf.BodiesStripped);
				},
				defaultCompleteMode = ToilCompleteMode.Instant
			};
			yield break;
		}

		// Token: 0x06003322 RID: 13090 RVA: 0x00124494 File Offset: 0x00122694
		public override object[] TaleParameters()
		{
			Corpse corpse = base.TargetA.Thing as Corpse;
			return new object[]
			{
				this.pawn,
				(corpse != null) ? corpse.InnerPawn : base.TargetA.Thing
			};
		}

		// Token: 0x04001DF4 RID: 7668
		private const int StripTicks = 60;
	}
}
