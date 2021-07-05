using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02001526 RID: 5414
	public class RoyalTitleInheritanceWorker
	{
		// Token: 0x060080D2 RID: 32978 RVA: 0x002D9D58 File Offset: 0x002D7F58
		public Pawn FindHeir(Faction faction, Pawn pawn, RoyalTitleDef title)
		{
			RoyalTitleInheritanceWorker.<>c__DisplayClass1_0 CS$<>8__locals1 = new RoyalTitleInheritanceWorker.<>c__DisplayClass1_0();
			CS$<>8__locals1.faction = faction;
			CS$<>8__locals1.pawn = pawn;
			CS$<>8__locals1.relatedPawns = new List<Pawn>();
			foreach (Pawn pawn2 in CS$<>8__locals1.pawn.relations.RelatedPawns)
			{
				if (!pawn2.Dead)
				{
					CS$<>8__locals1.relatedPawns.Add(pawn2);
				}
			}
			Pawn pawn3 = CS$<>8__locals1.<FindHeir>g__GetClosestFamilyPawn|0(false);
			if (pawn3 != null)
			{
				return pawn3;
			}
			Pawn pawn4 = (from p in PawnsFinder.AllMapsAndWorld_Alive
			where p != CS$<>8__locals1.pawn && p.Faction == CS$<>8__locals1.pawn.Faction && p.RaceProps.Humanlike
			select p).MaxByWithFallback((Pawn p) => CS$<>8__locals1.pawn.relations.OpinionOf(p), null);
			if (pawn4 != null)
			{
				return pawn4;
			}
			pawn3 = CS$<>8__locals1.<FindHeir>g__GetClosestFamilyPawn|0(true);
			if (pawn3 != null)
			{
				return pawn3;
			}
			return null;
		}

		// Token: 0x04005050 RID: 20560
		private static List<Pawn> tmpPawns = new List<Pawn>();
	}
}
