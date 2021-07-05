using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020006CB RID: 1739
	public class JobDriver_PrepareCaravan_CollectAnimals : JobDriver_RopeToDestination
	{
		// Token: 0x06003084 RID: 12420 RVA: 0x0001276E File Offset: 0x0001096E
		protected override bool HasRopeeArrived(Pawn ropee, bool roperWaitingAtDest)
		{
			return false;
		}

		// Token: 0x06003085 RID: 12421 RVA: 0x0000313F File Offset: 0x0000133F
		protected override void ProcessArrivedRopee(Pawn ropee)
		{
		}

		// Token: 0x06003086 RID: 12422 RVA: 0x0011DE2D File Offset: 0x0011C02D
		protected override bool ShouldOpportunisticallyRopeAnimal(Pawn animal)
		{
			return JobGiver_PrepareCaravan_CollectPawns.DoesAnimalNeedGathering(this.pawn, animal);
		}

		// Token: 0x06003087 RID: 12423 RVA: 0x0011DE3C File Offset: 0x0011C03C
		protected override Thing FindDistantAnimalToRope()
		{
			Lord lord = this.pawn.GetLord();
			if (lord == null)
			{
				return null;
			}
			return GenClosest.ClosestThing_Global(this.pawn.Position, lord.ownedPawns, 99999f, (Thing t) => this.ShouldOpportunisticallyRopeAnimal(t as Pawn), null);
		}
	}
}
