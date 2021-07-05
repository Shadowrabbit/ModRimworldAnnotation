using System;
using System.Collections.Generic;

namespace Verse.AI
{
	// Token: 0x020005F7 RID: 1527
	public static class MurderousRageMentalStateUtility
	{
		// Token: 0x06002BEB RID: 11243 RVA: 0x00104F9C File Offset: 0x0010319C
		public static Pawn FindPawnToKill(Pawn pawn)
		{
			if (!pawn.Spawned)
			{
				return null;
			}
			MurderousRageMentalStateUtility.tmpTargets.Clear();
			List<Pawn> allPawnsSpawned = pawn.Map.mapPawns.AllPawnsSpawned;
			for (int i = 0; i < allPawnsSpawned.Count; i++)
			{
				Pawn pawn2 = allPawnsSpawned[i];
				if ((pawn2.Faction == pawn.Faction || (pawn2.IsPrisoner && pawn2.HostFaction == pawn.Faction)) && pawn2.RaceProps.Humanlike && pawn2 != pawn && pawn.CanReach(pawn2, PathEndMode.Touch, Danger.Deadly, false, false, TraverseMode.ByPawn) && (pawn2.CurJob == null || !pawn2.CurJob.exitMapOnArrival))
				{
					MurderousRageMentalStateUtility.tmpTargets.Add(pawn2);
				}
			}
			if (!MurderousRageMentalStateUtility.tmpTargets.Any<Pawn>())
			{
				return null;
			}
			Pawn result = MurderousRageMentalStateUtility.tmpTargets.RandomElement<Pawn>();
			MurderousRageMentalStateUtility.tmpTargets.Clear();
			return result;
		}

		// Token: 0x04001AAA RID: 6826
		private static List<Pawn> tmpTargets = new List<Pawn>();
	}
}
