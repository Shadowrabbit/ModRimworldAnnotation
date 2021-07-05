using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E11 RID: 3601
	public class PawnRelationWorker_Kin : PawnRelationWorker
	{
		// Token: 0x06005345 RID: 21317 RVA: 0x001C2E64 File Offset: 0x001C1064
		public override bool InRelation(Pawn me, Pawn other)
		{
			if (me == other || me.RaceProps.Animal != other.RaceProps.Animal)
			{
				return false;
			}
			IEnumerable<Pawn> familyByBlood = me.relations.FamilyByBlood;
			HashSet<Pawn> hashSet = familyByBlood as HashSet<Pawn>;
			if (hashSet == null)
			{
				return familyByBlood.Contains(other);
			}
			return hashSet.Contains(other);
		}
	}
}
