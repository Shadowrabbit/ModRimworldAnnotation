using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200089E RID: 2206
	public class LordToil_PrepareCaravan_GatherAnimals : LordToil_PrepareCaravan_RopeAnimals
	{
		// Token: 0x06003A72 RID: 14962 RVA: 0x001473A8 File Offset: 0x001455A8
		public LordToil_PrepareCaravan_GatherAnimals(IntVec3 destinationPoint) : base(destinationPoint, null)
		{
		}

		// Token: 0x06003A73 RID: 14963 RVA: 0x001473C5 File Offset: 0x001455C5
		protected override PawnDuty MakeRopeDuty()
		{
			return new PawnDuty(DutyDefOf.PrepareCaravan_GatherAnimals, this.destinationPoint, -1f);
		}

		// Token: 0x06003A74 RID: 14964 RVA: 0x001473E4 File Offset: 0x001455E4
		public override void LordToilTick()
		{
			if (Find.TickManager.TicksGame % 100 == 0)
			{
				GatherAnimalsAndSlavesForCaravanUtility.CheckArrived(this.lord, this.lord.ownedPawns, this.destinationPoint, "AllAnimalsGathered", new Predicate<Pawn>(AnimalPenUtility.NeedsToBeManagedByRope), (Pawn x) => x.roping.RopedToSpot == this.destinationPoint);
			}
		}
	}
}
