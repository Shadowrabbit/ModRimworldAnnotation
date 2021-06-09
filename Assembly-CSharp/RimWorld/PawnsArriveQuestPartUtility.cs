using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x0200114C RID: 4428
	public static class PawnsArriveQuestPartUtility
	{
		// Token: 0x06006130 RID: 24880 RVA: 0x00042F75 File Offset: 0x00041175
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

		// Token: 0x06006131 RID: 24881 RVA: 0x001E7104 File Offset: 0x001E5304
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
