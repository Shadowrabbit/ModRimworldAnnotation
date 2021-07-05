using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E0D RID: 3597
	public class PawnRelationWorker_GranduncleOrGrandaunt : PawnRelationWorker
	{
		// Token: 0x0600533D RID: 21309 RVA: 0x001C2D40 File Offset: 0x001C0F40
		public override bool InRelation(Pawn me, Pawn other)
		{
			if (me == other)
			{
				return false;
			}
			if (PawnRelationDefOf.Grandparent.Worker.InRelation(me, other))
			{
				return false;
			}
			PawnRelationWorker worker = PawnRelationDefOf.GreatGrandparent.Worker;
			return (other.GetMother() != null && worker.InRelation(me, other.GetMother())) || (other.GetFather() != null && worker.InRelation(me, other.GetFather()));
		}
	}
}
