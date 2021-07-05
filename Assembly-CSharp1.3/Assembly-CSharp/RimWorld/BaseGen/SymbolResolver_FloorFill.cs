using System;
using UnityEngine;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x020015BF RID: 5567
	public class SymbolResolver_FloorFill : SymbolResolver
	{
		// Token: 0x06008329 RID: 33577 RVA: 0x002EB534 File Offset: 0x002E9734
		public override void Resolve(ResolveParams rp)
		{
			Map map = BaseGen.globalSettings.map;
			TerrainGrid terrainGrid = map.terrainGrid;
			TerrainDef terrainDef = rp.floorDef ?? BaseGenUtility.RandomBasicFloorDef(rp.faction, false);
			bool flag = rp.floorOnlyIfTerrainSupports ?? false;
			bool flag2 = rp.allowBridgeOnAnyImpassableTerrain ?? false;
			foreach (IntVec3 c in rp.rect)
			{
				if ((rp.chanceToSkipFloor == null || !Rand.Chance(rp.chanceToSkipFloor.Value)) && (!flag || GenConstruct.CanBuildOnTerrain(terrainDef, c, map, Rot4.North, null, null) || (flag2 && c.GetTerrain(map).passability == Traversability.Impassable)))
				{
					terrainGrid.SetTerrain(c, terrainDef);
					if (rp.filthDef != null)
					{
						FilthMaker.TryMakeFilth(c, map, rp.filthDef, (rp.filthDensity != null) ? Mathf.RoundToInt(rp.filthDensity.Value.RandomInRange) : 1, FilthSourceFlags.None);
					}
				}
			}
		}
	}
}
