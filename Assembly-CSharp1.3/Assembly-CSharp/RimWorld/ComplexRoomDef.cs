using System;
using System.Collections.Generic;
using RimWorld.SketchGen;
using Verse;

namespace RimWorld
{
	// Token: 0x02000A5A RID: 2650
	public class ComplexRoomDef : Def
	{
		// Token: 0x06003FBE RID: 16318 RVA: 0x00159F80 File Offset: 0x00158180
		public bool CanResolve(ComplexRoomParams parms)
		{
			int area = parms.room.Area;
			return area >= this.minArea && area <= this.maxArea && (!this.requiresSingleRectRoom || parms.room.rects.Count == 1);
		}

		// Token: 0x06003FBF RID: 16319 RVA: 0x00159FCC File Offset: 0x001581CC
		public void ResolveSketch(ComplexRoomParams parms)
		{
			ResolveParams parms2 = default(ResolveParams);
			TerrainDef terrainDef = (!this.floorTypes.NullOrEmpty<TerrainDef>()) ? this.floorTypes.RandomElement<TerrainDef>() : TerrainDefOf.Concrete;
			foreach (CellRect value in parms.room.rects)
			{
				if (terrainDef != null)
				{
					foreach (IntVec3 pos in value)
					{
						parms.sketch.AddTerrain(terrainDef, pos, true);
					}
				}
				parms2.rect = new CellRect?(value);
				parms2.sketch = parms.sketch;
				this.sketchResolverDef.Resolve(parms2);
			}
		}

		// Token: 0x0400236F RID: 9071
		public SketchResolverDef sketchResolverDef;

		// Token: 0x04002370 RID: 9072
		public float selectionWeight = 1f;

		// Token: 0x04002371 RID: 9073
		public int maxCount = int.MaxValue;

		// Token: 0x04002372 RID: 9074
		public int minArea = 25;

		// Token: 0x04002373 RID: 9075
		public int maxArea = int.MaxValue;

		// Token: 0x04002374 RID: 9076
		public bool requiresSingleRectRoom;

		// Token: 0x04002375 RID: 9077
		public List<TerrainDef> floorTypes;
	}
}
