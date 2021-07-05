using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020007A0 RID: 1952
	public class JobGiver_MarryAdjacentPawn : ThinkNode_JobGiver
	{
		// Token: 0x06003544 RID: 13636 RVA: 0x0012D66C File Offset: 0x0012B86C
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (!pawn.RaceProps.IsFlesh)
			{
				return null;
			}
			Predicate<Thing> <>9__0;
			for (int i = 0; i < 4; i++)
			{
				IntVec3 c = pawn.Position + GenAdj.CardinalDirections[i];
				if (c.InBounds(pawn.Map))
				{
					List<Thing> thingList = c.GetThingList(pawn.Map);
					Predicate<Thing> match;
					if ((match = <>9__0) == null)
					{
						match = (<>9__0 = ((Thing x) => x is Pawn && this.CanMarry(pawn, (Pawn)x)));
					}
					Thing thing = thingList.Find(match);
					if (thing != null)
					{
						return JobMaker.MakeJob(JobDefOf.MarryAdjacentPawn, thing);
					}
				}
			}
			return null;
		}

		// Token: 0x06003545 RID: 13637 RVA: 0x0012D72C File Offset: 0x0012B92C
		private bool CanMarry(Pawn pawn, Pawn toMarry)
		{
			return !toMarry.Drafted && pawn.relations.DirectRelationExists(PawnRelationDefOf.Fiance, toMarry);
		}
	}
}
