using System;
using System.Collections.Generic;
using RimWorld.SketchGen;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C9A RID: 3226
	public class GenStep_ScatterAncientUtilityBuilding : GenStep_Scatterer
	{
		// Token: 0x17000CFA RID: 3322
		// (get) Token: 0x06004B4A RID: 19274 RVA: 0x0018F624 File Offset: 0x0018D824
		public override int SeedPart
		{
			get
			{
				return 1872954345;
			}
		}

		// Token: 0x06004B4B RID: 19275 RVA: 0x0018FE58 File Offset: 0x0018E058
		public override void Generate(Map map, GenStepParams parms)
		{
			if (!ModLister.CheckIdeology("Scatter ancient outdoor building"))
			{
				return;
			}
			this.count = 1;
			this.allowInWaterBiome = false;
			this.randomSize = Mathf.RoundToInt(Rand.ByCurve(GenStep_ScatterAncientUtilityBuilding.SizeChanceCurve));
			base.Generate(map, parms);
		}

		// Token: 0x06004B4C RID: 19276 RVA: 0x0018FE94 File Offset: 0x0018E094
		protected override bool CanScatterAt(IntVec3 loc, Map map)
		{
			if (!base.CanScatterAt(loc, map))
			{
				return false;
			}
			if (!loc.SupportsStructureType(map, TerrainAffordanceDefOf.Heavy))
			{
				return false;
			}
			CellRect rect = new CellRect(loc.x, loc.z, this.randomSize, this.randomSize);
			return this.CanPlaceAt(rect, map);
		}

		// Token: 0x06004B4D RID: 19277 RVA: 0x0018FEEC File Offset: 0x0018E0EC
		private bool CanPlaceAt(CellRect rect, Map map)
		{
			foreach (IntVec3 c in rect.Cells)
			{
				if (!c.InBounds(map))
				{
					return false;
				}
				TerrainDef terrainDef = map.terrainGrid.TerrainAt(c);
				if (terrainDef.IsWater || terrainDef.IsRoad)
				{
					return false;
				}
				if (c.GetEdifice(map) != null)
				{
					return false;
				}
				List<Thing> thingList = c.GetThingList(map);
				for (int i = 0; i < thingList.Count; i++)
				{
					if (!thingList[i].def.destroyable)
					{
						return false;
					}
				}
				if (!c.SupportsStructureType(map, TerrainAffordanceDefOf.Heavy))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06004B4E RID: 19278 RVA: 0x0018FFC8 File Offset: 0x0018E1C8
		protected override void ScatterAt(IntVec3 loc, Map map, GenStepParams parms, int count = 1)
		{
			ResolveParams parms2 = default(ResolveParams);
			parms2.utilityBuildingSize = new IntVec2?(new IntVec2(this.randomSize, this.randomSize));
			parms2.sketch = new Sketch();
			SketchGen.Generate(SketchResolverDefOf.AncientUtilityBuilding, parms2).Spawn(map, loc, null, Sketch.SpawnPosType.Unchanged, Sketch.SpawnMode.Normal, false, false, null, false, false, null, null);
		}

		// Token: 0x04002DA1 RID: 11681
		private static readonly SimpleCurve SizeChanceCurve = new SimpleCurve
		{
			{
				new CurvePoint(8f, 0f),
				true
			},
			{
				new CurvePoint(12f, 4f),
				true
			},
			{
				new CurvePoint(18f, 0f),
				true
			}
		};

		// Token: 0x04002DA2 RID: 11682
		private int randomSize;
	}
}
