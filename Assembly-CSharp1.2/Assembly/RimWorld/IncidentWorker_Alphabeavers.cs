using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200119E RID: 4510
	internal class IncidentWorker_Alphabeavers : IncidentWorker
	{
		// Token: 0x06006364 RID: 25444 RVA: 0x001EF2D4 File Offset: 0x001ED4D4
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			if (!base.CanFireNowSub(parms))
			{
				return false;
			}
			Map map = (Map)parms.target;
			IntVec3 intVec;
			return RCellFinder.TryFindRandomPawnEntryCell(out intVec, map, CellFinder.EdgeRoadChance_Animal, false, null);
		}

		// Token: 0x06006365 RID: 25445 RVA: 0x001EF308 File Offset: 0x001ED508
		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			PawnKindDef alphabeaver = PawnKindDefOf.Alphabeaver;
			IntVec3 intVec;
			if (!RCellFinder.TryFindRandomPawnEntryCell(out intVec, map, CellFinder.EdgeRoadChance_Animal, false, null))
			{
				return false;
			}
			float freeColonistsCount = (float)map.mapPawns.FreeColonistsCount;
			float randomInRange = IncidentWorker_Alphabeavers.CountPerColonistRange.RandomInRange;
			int num = Mathf.Clamp(GenMath.RoundRandom(freeColonistsCount * randomInRange), 1, 10);
			for (int i = 0; i < num; i++)
			{
				IntVec3 loc = CellFinder.RandomClosewalkCellNear(intVec, map, 10, null);
				((Pawn)GenSpawn.Spawn(PawnGenerator.GeneratePawn(alphabeaver, null), loc, map, WipeMode.Vanish)).needs.food.CurLevelPercentage = 1f;
			}
			base.SendStandardLetter("LetterLabelBeaversArrived".Translate(), "BeaversArrived".Translate(), LetterDefOf.ThreatSmall, parms, new TargetInfo(intVec, map, false), Array.Empty<NamedArgument>());
			return true;
		}

		// Token: 0x04004288 RID: 17032
		private static readonly FloatRange CountPerColonistRange = new FloatRange(1f, 1.5f);

		// Token: 0x04004289 RID: 17033
		private const int MinCount = 1;

		// Token: 0x0400428A RID: 17034
		private const int MaxCount = 10;
	}
}
