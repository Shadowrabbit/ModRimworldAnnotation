using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000BC6 RID: 3014
	public static class PawnsArriveQuestPartUtility
	{
		// Token: 0x06004699 RID: 18073 RVA: 0x00175775 File Offset: 0x00173975
		public static IEnumerable<Pawn> GetQuestLookTargets(IEnumerable<Pawn> pawns)
		{
			if (pawns.Count<Pawn>() == 1)
			{
				yield return pawns.First<Pawn>();
				yield break;
			}
			foreach (Pawn p in pawns)
			{
				if (p.Faction == Faction.OfPlayer || p.HostFaction == Faction.OfPlayer)
				{
					yield return p;
				}
				if (p.Faction == null && p.Downed)
				{
					yield return p;
				}
				p = null;
			}
			IEnumerator<Pawn> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x0600469A RID: 18074 RVA: 0x00175788 File Offset: 0x00173988
		public static bool IncreasesPopulation(IEnumerable<Pawn> pawns, bool joinPlayer, bool makePrisoners)
		{
			foreach (Pawn pawn in pawns)
			{
				if (pawn.RaceProps.Humanlike && !pawn.Destroyed && (pawn.Faction == Faction.OfPlayer || pawn.IsPrisonerOfColony || pawn.Downed || joinPlayer || makePrisoners))
				{
					return true;
				}
			}
			return false;
		}
	}
}
