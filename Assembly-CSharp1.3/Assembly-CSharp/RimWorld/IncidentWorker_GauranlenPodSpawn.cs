using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C07 RID: 3079
	public class IncidentWorker_GauranlenPodSpawn : IncidentWorker
	{
		// Token: 0x0600486B RID: 18539 RVA: 0x0017F104 File Offset: 0x0017D304
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			if (!base.CanFireNowSub(parms))
			{
				return false;
			}
			if (!ModsConfig.IdeologyActive)
			{
				return false;
			}
			Map map = (Map)parms.target;
			if (!IncidentWorker_GauranlenPodSpawn.IsGoodBiome(map.Biome))
			{
				return false;
			}
			if (Faction.OfPlayer.ideos == null)
			{
				return false;
			}
			bool flag = false;
			using (IEnumerator<Ideo> enumerator = Faction.OfPlayer.ideos.AllIdeos.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.HasMeme(MemeDefOf.TreeConnection))
					{
						flag = true;
						break;
					}
				}
			}
			IntVec3 intVec;
			return flag && IncidentWorker_GauranlenPodSpawn.TryFindRootCell(map, out intVec);
		}

		// Token: 0x0600486C RID: 18540 RVA: 0x0017F1B0 File Offset: 0x0017D3B0
		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			IntVec3 cell;
			if (!IncidentWorker_GauranlenPodSpawn.TryFindRootCell(map, out cell))
			{
				return false;
			}
			Thing thing;
			if (!this.TrySpawnAt(cell, map, out thing))
			{
				return false;
			}
			((Plant)thing).Growth = 1f;
			base.SendStandardLetter(parms, thing, Array.Empty<NamedArgument>());
			return true;
		}

		// Token: 0x0600486D RID: 18541 RVA: 0x0017F206 File Offset: 0x0017D406
		public static bool IsGoodBiome(BiomeDef biomeDef)
		{
			return !biomeDef.isExtremeBiome && biomeDef != BiomeDefOf.Desert;
		}

		// Token: 0x0600486E RID: 18542 RVA: 0x0017F220 File Offset: 0x0017D420
		private static bool CanSpawnPodAt(IntVec3 c, Map map)
		{
			if (!c.Standable(map) || c.Fogged(map) || !c.GetRoom(map).PsychologicallyOutdoors || c.Roofed(map))
			{
				return false;
			}
			Plant plant = c.GetPlant(map);
			if (plant != null && plant.def.plant.growDays > 10f)
			{
				return false;
			}
			List<Thing> thingList = c.GetThingList(map);
			for (int i = 0; i < thingList.Count; i++)
			{
				if (thingList[i].def == ThingDefOf.Plant_PodGauranlen)
				{
					return false;
				}
			}
			if (!map.reachability.CanReachFactionBase(c, map.ParentFaction))
			{
				return false;
			}
			TerrainDef terrain = c.GetTerrain(map);
			return !terrain.avoidWander && terrain.fertility >= ThingDefOf.Plant_PodGauranlen.plant.fertilityMin;
		}

		// Token: 0x0600486F RID: 18543 RVA: 0x0017F2EC File Offset: 0x0017D4EC
		public static bool TryFindRootCell(Map map, out IntVec3 cell)
		{
			return CellFinderLoose.TryFindRandomNotEdgeCellWith(10, (IntVec3 x) => IncidentWorker_GauranlenPodSpawn.CanSpawnPodAt(x, map), map, out cell);
		}

		// Token: 0x06004870 RID: 18544 RVA: 0x0017F320 File Offset: 0x0017D520
		private bool TrySpawnAt(IntVec3 cell, Map map, out Thing plant)
		{
			Plant plant2 = cell.GetPlant(map);
			if (plant2 != null)
			{
				plant2.Destroy(DestroyMode.Vanish);
			}
			plant = GenSpawn.Spawn(ThingDefOf.Plant_PodGauranlen, cell, map, WipeMode.Vanish);
			return plant != null;
		}
	}
}
