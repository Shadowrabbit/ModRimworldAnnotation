using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x0200078D RID: 1933
	public class JobGiver_PrepareCaravan_CollectPawns : JobGiver_PrepareCaravan_RopePawns
	{
		// Token: 0x170009B6 RID: 2486
		// (get) Token: 0x06003508 RID: 13576 RVA: 0x0012C466 File Offset: 0x0012A666
		protected override JobDef RopeJobDef
		{
			get
			{
				return JobDefOf.PrepareCaravan_CollectAnimals;
			}
		}

		// Token: 0x06003509 RID: 13577 RVA: 0x0012C46D File Offset: 0x0012A66D
		protected override bool AnimalNeedsGathering(Pawn pawn, Pawn animal)
		{
			return JobGiver_PrepareCaravan_CollectPawns.DoesAnimalNeedGathering(pawn, animal);
		}

		// Token: 0x0600350A RID: 13578 RVA: 0x0012C476 File Offset: 0x0012A676
		public static bool DoesAnimalNeedGathering(Pawn pawn, Pawn animal)
		{
			return AnimalPenUtility.NeedsToBeManagedByRope(animal) && !GatherAnimalsAndSlavesForCaravanUtility.IsRopedByCaravanPawn(animal) && pawn.GetLord() == animal.GetLord() && pawn.CanReserveAndReach(animal, PathEndMode.Touch, Danger.Deadly, 1, -1, null, false);
		}
	}
}
