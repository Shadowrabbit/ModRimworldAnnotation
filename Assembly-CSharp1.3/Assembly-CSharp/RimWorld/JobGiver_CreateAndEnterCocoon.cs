using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200081F RID: 2079
	public class JobGiver_CreateAndEnterCocoon : ThinkNode_JobGiver
	{
		// Token: 0x06003752 RID: 14162 RVA: 0x00138668 File Offset: 0x00136868
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (!ModsConfig.IdeologyActive)
			{
				return null;
			}
			if (pawn.connections == null || pawn.connections.ConnectedThings.NullOrEmpty<Thing>())
			{
				return null;
			}
			Predicate<IntVec3> <>9__0;
			foreach (Thing thing in pawn.connections.ConnectedThings)
			{
				CompTreeConnection compTreeConnection = thing.TryGetComp<CompTreeConnection>();
				if (compTreeConnection != null && compTreeConnection.DryadKind != pawn.kindDef && !thing.IsForbidden(pawn) && pawn.CanReach(thing, PathEndMode.Touch, Danger.Deadly, false, false, TraverseMode.ByPawn))
				{
					IntVec3 position = thing.Position;
					Map map = pawn.Map;
					int squareRadius = 4;
					Predicate<IntVec3> validator;
					if ((validator = <>9__0) == null)
					{
						validator = (<>9__0 = ((IntVec3 cell) => cell.Standable(pawn.Map)));
					}
					IntVec3 intVec;
					if (CellFinder.TryFindRandomCellNear(position, map, squareRadius, validator, out intVec, -1))
					{
						return JobMaker.MakeJob(JobDefOf.CreateAndEnterCocoon, thing);
					}
				}
			}
			return null;
		}

		// Token: 0x04001F0F RID: 7951
		public const int SquareRadius = 4;
	}
}
