using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x0200078C RID: 1932
	public class JobGiver_PrepareCaravan_GatherPawns : JobGiver_PrepareCaravan_RopePawns
	{
		// Token: 0x170009B5 RID: 2485
		// (get) Token: 0x06003504 RID: 13572 RVA: 0x0012C3D4 File Offset: 0x0012A5D4
		protected override JobDef RopeJobDef
		{
			get
			{
				return JobDefOf.PrepareCaravan_GatherAnimals;
			}
		}

		// Token: 0x06003505 RID: 13573 RVA: 0x0012C3DB File Offset: 0x0012A5DB
		protected override bool AnimalNeedsGathering(Pawn pawn, Pawn animal)
		{
			return JobGiver_PrepareCaravan_GatherPawns.DoesAnimalNeedGathering(pawn, animal);
		}

		// Token: 0x06003506 RID: 13574 RVA: 0x0012C3E4 File Offset: 0x0012A5E4
		public static bool DoesAnimalNeedGathering(Pawn pawn, Pawn animal)
		{
			IntVec3 cell = pawn.mindState.duty.focus.Cell;
			return AnimalPenUtility.NeedsToBeManagedByRope(animal) && !animal.roping.IsRopedByPawn && (!animal.roping.IsRopedToSpot || !(animal.roping.RopedToSpot == cell)) && pawn.GetLord() == animal.GetLord() && pawn.CanReserveAndReach(animal, PathEndMode.Touch, Danger.Deadly, 1, -1, null, false);
		}
	}
}
