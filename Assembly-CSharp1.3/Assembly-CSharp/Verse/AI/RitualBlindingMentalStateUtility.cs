using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x020005F8 RID: 1528
	public static class RitualBlindingMentalStateUtility
	{
		// Token: 0x06002BED RID: 11245 RVA: 0x00105080 File Offset: 0x00103280
		public static Pawn FindPawnToBlind(Pawn pawn)
		{
			if (!pawn.Spawned)
			{
				return null;
			}
			RitualBlindingMentalStateUtility.tmpTargets.Clear();
			List<Pawn> allPawnsSpawned = pawn.Map.mapPawns.AllPawnsSpawned;
			for (int i = 0; i < allPawnsSpawned.Count; i++)
			{
				Pawn pawn2 = allPawnsSpawned[i];
				if (pawn2 != pawn && (pawn2.Faction == pawn.Faction || (pawn2.IsPrisoner && pawn2.HostFaction == pawn.Faction)) && pawn2.RaceProps.Humanlike && pawn2 != pawn && pawn.CanReach(pawn2, PathEndMode.Touch, Danger.Deadly, false, false, TraverseMode.ByPawn) && (pawn2.CurJob == null || !pawn2.CurJob.exitMapOnArrival))
				{
					if (pawn2.health.hediffSet.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined, null, null).Any((BodyPartRecord x) => x.def == BodyPartDefOf.Eye))
					{
						RitualBlindingMentalStateUtility.tmpTargets.Add(pawn2);
					}
				}
			}
			Pawn result = null;
			IEnumerable<Pawn> source = from x in RitualBlindingMentalStateUtility.tmpTargets
			where x.IsPrisoner
			select x;
			if (source.Any<Pawn>())
			{
				result = source.RandomElement<Pawn>();
			}
			else if (RitualBlindingMentalStateUtility.tmpTargets.Any<Pawn>())
			{
				result = RitualBlindingMentalStateUtility.tmpTargets.RandomElement<Pawn>();
			}
			RitualBlindingMentalStateUtility.tmpTargets.Clear();
			return result;
		}

		// Token: 0x04001AAB RID: 6827
		private static List<Pawn> tmpTargets = new List<Pawn>();
	}
}
