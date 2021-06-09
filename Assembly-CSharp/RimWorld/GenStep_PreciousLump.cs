using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020012CE RID: 4814
	public class GenStep_PreciousLump : GenStep_ScatterLumpsMineable
	{
		// Token: 0x1700100F RID: 4111
		// (get) Token: 0x06006844 RID: 26692 RVA: 0x00046FC3 File Offset: 0x000451C3
		public override int SeedPart
		{
			get
			{
				return 1634184421;
			}
		}

		// Token: 0x06006845 RID: 26693 RVA: 0x00202B64 File Offset: 0x00200D64
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

		// Token: 0x06006846 RID: 26694 RVA: 0x00202C0C File Offset: 0x00200E0C
		protected override bool CanScatterAt(IntVec3 c, Map map)
		{
			List<CellRect> list;
			return (!MapGenerator.TryGetVar<List<CellRect>>("UsedRects", out list) || !list.Any((CellRect x) => x.Contains(c))) && map.reachability.CanReachMapEdge(c, TraverseParms.For(TraverseMode.PassDoors, Danger.Deadly, false));
		}

		// Token: 0x06006847 RID: 26695 RVA: 0x00202C64 File Offset: 0x00200E64
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

		// Token: 0x0400456B RID: 17771
		public List<ThingDef> mineables;

		// Token: 0x0400456C RID: 17772
		public FloatRange totalValueRange = new FloatRange(1000f, 2000f);
	}
}
