using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000CBD RID: 3261
	public class GenStep_PreciousLump : GenStep_ScatterLumpsMineable
	{
		// Token: 0x17000D1A RID: 3354
		// (get) Token: 0x06004BEE RID: 19438 RVA: 0x00194CF6 File Offset: 0x00192EF6
		public override int SeedPart
		{
			get
			{
				return 1634184421;
			}
		}

		// Token: 0x06004BEF RID: 19439 RVA: 0x00194D00 File Offset: 0x00192F00
		public override void Generate(Map map, GenStepParams parms)
		{
			if (parms.sitePart != null && parms.sitePart.parms.preciousLumpResources != null)
			{
				this.forcedDefToScatter = parms.sitePart.parms.preciousLumpResources;
			}
			else
			{
				this.forcedDefToScatter = this.mineables.RandomElement<ThingDef>();
			}
			this.count = 1;
			float randomInRange = this.totalValueRange.RandomInRange;
			float baseMarketValue = this.forcedDefToScatter.building.mineableThing.BaseMarketValue;
			this.forcedLumpSize = Mathf.Max(Mathf.RoundToInt(randomInRange / ((float)this.forcedDefToScatter.building.mineableYield * baseMarketValue)), 1);
			base.Generate(map, parms);
		}

		// Token: 0x06004BF0 RID: 19440 RVA: 0x00194DA8 File Offset: 0x00192FA8
		protected override bool CanScatterAt(IntVec3 c, Map map)
		{
			List<CellRect> list;
			return (!MapGenerator.TryGetVar<List<CellRect>>("UsedRects", out list) || !list.Any((CellRect x) => x.Contains(c))) && map.reachability.CanReachMapEdge(c, TraverseParms.For(TraverseMode.PassDoors, Danger.Deadly, false, false, false));
		}

		// Token: 0x06004BF1 RID: 19441 RVA: 0x00194E04 File Offset: 0x00193004
		protected override void ScatterAt(IntVec3 c, Map map, GenStepParams parms, int stackCount = 1)
		{
			base.ScatterAt(c, map, parms, stackCount);
			int minX = this.recentLumpCells.Min((IntVec3 x) => x.x);
			int minZ = this.recentLumpCells.Min((IntVec3 x) => x.z);
			int maxX = this.recentLumpCells.Max((IntVec3 x) => x.x);
			int maxZ = this.recentLumpCells.Max((IntVec3 x) => x.z);
			CellRect var = CellRect.FromLimits(minX, minZ, maxX, maxZ);
			MapGenerator.SetVar<CellRect>("RectOfInterest", var);
		}

		// Token: 0x04002DF7 RID: 11767
		public List<ThingDef> mineables;

		// Token: 0x04002DF8 RID: 11768
		public FloatRange totalValueRange = new FloatRange(1000f, 2000f);
	}
}
