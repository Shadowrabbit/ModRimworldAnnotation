using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000BF9 RID: 3065
	internal class IncidentWorker_Alphabeavers : IncidentWorker
	{
		// Token: 0x0600482C RID: 18476 RVA: 0x0017D750 File Offset: 0x0017B950
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			if (!base.CanFireNowSub(parms))
			{
				return false;
			}
			Map map = (Map)parms.target;
			IntVec3 intVec;
			return Find.World.tileTemperatures.SeasonAndOutdoorTemperatureAcceptableFor(map.Tile, PawnKindDefOf.Alphabeaver.race) && RCellFinder.TryFindRandomPawnEntryCell(out intVec, map, CellFinder.EdgeRoadChance_Animal, false, null);
		}

		// Token: 0x0600482D RID: 18477 RVA: 0x0017D7A8 File Offset: 0x0017B9A8
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

		// Token: 0x04002C4A RID: 11338
		private static readonly FloatRange CountPerColonistRange = new FloatRange(1f, 1.5f);

		// Token: 0x04002C4B RID: 11339
		private const int MinCount = 1;

		// Token: 0x04002C4C RID: 11340
		private const int MaxCount = 10;
	}
}
