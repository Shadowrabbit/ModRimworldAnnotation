using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000BFB RID: 3067
	public class IncidentWorker_AnimaTreeSpawn : IncidentWorker
	{
		// Token: 0x06004836 RID: 18486 RVA: 0x0017DABC File Offset: 0x0017BCBC
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			if (!base.CanFireNowSub(parms))
			{
				return false;
			}
			Map map = (Map)parms.target;
			if (map.Biome.isExtremeBiome)
			{
				return false;
			}
			int num = GenStep_AnimaTrees.DesiredTreeCountForMap(map);
			IntVec3 intVec;
			return map.listerThings.ThingsOfDef(ThingDefOf.Plant_TreeAnima).Count < num && this.TryFindRootCell(map, out intVec);
		}

		// Token: 0x06004837 RID: 18487 RVA: 0x0017DB1C File Offset: 0x0017BD1C
		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			IntVec3 cell;
			if (!this.TryFindRootCell(map, out cell))
			{
				return false;
			}
			Thing t;
			if (!GenStep_AnimaTrees.TrySpawnAt(cell, map, 0.05f, out t))
			{
				return false;
			}
			if (PawnsFinder.HomeMaps_FreeColonistsSpawned.Any((Pawn c) => c.HasPsylink && MeditationFocusDefOf.Natural.CanPawnUse(c)))
			{
				base.SendStandardLetter(parms, t, Array.Empty<NamedArgument>());
			}
			return true;
		}

		// Token: 0x06004838 RID: 18488 RVA: 0x0017DB94 File Offset: 0x0017BD94
		private bool TryFindRootCell(Map map, out IntVec3 cell)
		{
			return CellFinderLoose.TryFindRandomNotEdgeCellWith(10, (IntVec3 x) => GenStep_AnimaTrees.CanSpawnAt(x, map, 40, 0, 22, 10), map, out cell) || CellFinderLoose.TryFindRandomNotEdgeCellWith(10, (IntVec3 x) => GenStep_AnimaTrees.CanSpawnAt(x, map, 10, 0, 18, 20), map, out cell);
		}
	}
}
