using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020006CA RID: 1738
	public class JobDriver_PrepareCaravan_GatherAnimals : JobDriver_RopeToDestination
	{
		// Token: 0x06003080 RID: 12416 RVA: 0x0011DD4C File Offset: 0x0011BF4C
		protected override bool HasRopeeArrived(Pawn ropee, bool roperWaitingAtDest)
		{
			PawnDuty duty = this.pawn.mindState.duty;
			IntVec3 intVec = (duty != null) ? duty.focus.Cell : IntVec3.Invalid;
			if (!intVec.IsValid)
			{
				return false;
			}
			if (!this.pawn.Position.InHorDistOf(intVec, 2f))
			{
				return false;
			}
			District district = intVec.GetDistrict(this.pawn.Map, RegionType.Set_Passable);
			return district == this.pawn.GetDistrict(RegionType.Set_Passable) && district == ropee.GetDistrict(RegionType.Set_Passable);
		}

		// Token: 0x06003081 RID: 12417 RVA: 0x0011DDDC File Offset: 0x0011BFDC
		protected override void ProcessArrivedRopee(Pawn ropee)
		{
			PawnDuty duty = ropee.mindState.duty;
			IntVec3 spot = (duty != null) ? duty.focus.Cell : IntVec3.Invalid;
			if (spot.IsValid)
			{
				ropee.roping.RopeToSpot(spot);
			}
		}

		// Token: 0x06003082 RID: 12418 RVA: 0x0011DE1F File Offset: 0x0011C01F
		protected override bool ShouldOpportunisticallyRopeAnimal(Pawn animal)
		{
			return JobGiver_PrepareCaravan_GatherPawns.DoesAnimalNeedGathering(this.pawn, animal);
		}
	}
}
