using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020006C8 RID: 1736
	public class JobDriver_RopeToPen : JobDriver_RopeToDestination
	{
		// Token: 0x17000907 RID: 2311
		// (get) Token: 0x06003072 RID: 12402 RVA: 0x0011DC24 File Offset: 0x0011BE24
		private Thing DestinationThing
		{
			get
			{
				return this.job.targetC.Thing;
			}
		}

		// Token: 0x17000908 RID: 2312
		// (get) Token: 0x06003073 RID: 12403 RVA: 0x0011DC36 File Offset: 0x0011BE36
		protected CompAnimalPenMarker DestinationMarker
		{
			get
			{
				return this.DestinationThing.TryGetComp<CompAnimalPenMarker>();
			}
		}

		// Token: 0x17000909 RID: 2313
		// (get) Token: 0x06003074 RID: 12404 RVA: 0x0011DC43 File Offset: 0x0011BE43
		private District DestinationDistrict
		{
			get
			{
				return this.DestinationThing.GetDistrict(RegionType.Set_Passable);
			}
		}

		// Token: 0x06003075 RID: 12405 RVA: 0x0011DC52 File Offset: 0x0011BE52
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedNullOrForbidden(TargetIndex.C);
			return base.MakeNewToils();
		}

		// Token: 0x06003076 RID: 12406 RVA: 0x0011DC64 File Offset: 0x0011BE64
		protected override bool HasRopeeArrived(Pawn ropee, bool roperWaitingAtDest)
		{
			PenMarkerState penState = this.DestinationMarker.PenState;
			return !penState.Enclosed || penState.ContainsConnectedRegion(ropee.GetRegion(RegionType.Set_Passable));
		}

		// Token: 0x06003077 RID: 12407 RVA: 0x0000313F File Offset: 0x0000133F
		protected override void ProcessArrivedRopee(Pawn ropee)
		{
		}

		// Token: 0x06003078 RID: 12408 RVA: 0x0011DC98 File Offset: 0x0011BE98
		protected override bool ShouldOpportunisticallyRopeAnimal(Pawn animal)
		{
			if (animal.roping.RopedByPawn == this.pawn)
			{
				return false;
			}
			string text;
			CompAnimalPenMarker penAnimalShouldBeTakenTo = AnimalPenUtility.GetPenAnimalShouldBeTakenTo(this.pawn, animal, out text, false, true, this.job.ropeToUnenclosedPens, false, this.job.ropingPriority);
			return penAnimalShouldBeTakenTo != null && this.ShouldOpportunisticallyRopeAnimal(animal, penAnimalShouldBeTakenTo);
		}

		// Token: 0x06003079 RID: 12409 RVA: 0x0011DCEF File Offset: 0x0011BEEF
		protected virtual bool ShouldOpportunisticallyRopeAnimal(Pawn animal, CompAnimalPenMarker targetPenMarker)
		{
			return targetPenMarker.parent.GetDistrict(RegionType.Set_Passable) == this.DestinationDistrict;
		}

		// Token: 0x04001D4F RID: 7503
		public const TargetIndex DestMarkerInd = TargetIndex.C;
	}
}
