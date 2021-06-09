using System;
using System.Collections.Generic;
using Verse;
using Verse.Noise;

namespace RimWorld
{
	// Token: 0x020012B2 RID: 4786
	public class TerrainPatchMaker
	{
		// Token: 0x060067E9 RID: 26601 RVA: 0x00200F9C File Offset: 0x001FF19C
		private void Init(Map map)
		{
			this.noise = new Perlin((double)this.perlinFrequency, (double)this.perlinLacunarity, (double)this.perlinPersistence, this.perlinOctaves, Rand.Range(0, int.MaxValue), QualityMode.Medium);
			NoiseDebugUI.RenderSize = new IntVec2(map.Size.x, map.Size.z);
			NoiseDebugUI.StoreNoiseRender(this.noise, "TerrainPatchMaker " + this.thresholds[0].terrain.defName);
			this.currentlyInitializedForMap = map;
		}

		// Token: 0x060067EA RID: 26602 RVA: 0x00046CD4 File Offset: 0x00044ED4
		public void Cleanup()
		{
			this.noise = null;
			this.currentlyInitializedForMap = null;
		}

		// Token: 0x060067EB RID: 26603 RVA: 0x00201030 File Offset: 0x001FF230
		public TerrainDef TerrainAt(IntVec3 c, Map map, float fertility)
		{
			if (fertility < this.minFertility || fertility > this.maxFertility)
			{
				return null;
			}
			if (this.noise != null && map != this.currentlyInitializedForMap)
			{
				this.Cleanup();
			}
			if (this.noise == null)
			{
				this.Init(map);
			}
			if (this.minSize > 0)
			{
				int count = 0;
				map.floodFiller.FloodFill(c, (IntVec3 x) => TerrainThreshold.TerrainAtValue(this.thresholds, this.noise.GetValue(x)) != null, delegate(IntVec3 x)
				{
					int count = count;
					count++;
					return count >= this.minSize;
				}, int.MaxValue, false, null);
				if (count < this.minSize)
				{
					return null;
				}
			}
			return TerrainThreshold.TerrainAtValue(this.thresholds, this.noise.GetValue(c));
		}

		// Token: 0x0400452A RID: 17706
		private Map currentlyInitializedForMap;

		// Token: 0x0400452B RID: 17707
		public List<TerrainThreshold> thresholds = new List<TerrainThreshold>();

		// Token: 0x0400452C RID: 17708
		public float perlinFrequency = 0.01f;

		// Token: 0x0400452D RID: 17709
		public float perlinLacunarity = 2f;

		// Token: 0x0400452E RID: 17710
		public float perlinPersistence = 0.5f;

		// Token: 0x0400452F RID: 17711
		public int perlinOctaves = 6;

		// Token: 0x04004530 RID: 17712
		public float minFertility = -999f;

		// Token: 0x04004531 RID: 17713
		public float maxFertility = 999f;

		// Token: 0x04004532 RID: 17714
		public int minSize;

		// Token: 0x04004533 RID: 17715
		[Unsaved(false)]
		private ModuleBase noise;
	}
}
