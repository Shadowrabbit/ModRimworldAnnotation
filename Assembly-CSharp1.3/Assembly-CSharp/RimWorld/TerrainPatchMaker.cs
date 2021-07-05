using System;
using System.Collections.Generic;
using Verse;
using Verse.Noise;

namespace RimWorld
{
	// Token: 0x02000CAB RID: 3243
	public class TerrainPatchMaker
	{
		// Token: 0x06004BA0 RID: 19360 RVA: 0x00192C28 File Offset: 0x00190E28
		private void Init(Map map)
		{
			this.noise = new Perlin((double)this.perlinFrequency, (double)this.perlinLacunarity, (double)this.perlinPersistence, this.perlinOctaves, Rand.Range(0, int.MaxValue), QualityMode.Medium);
			NoiseDebugUI.RenderSize = new IntVec2(map.Size.x, map.Size.z);
			NoiseDebugUI.StoreNoiseRender(this.noise, "TerrainPatchMaker " + this.thresholds[0].terrain.defName);
			this.currentlyInitializedForMap = map;
		}

		// Token: 0x06004BA1 RID: 19361 RVA: 0x00192CB9 File Offset: 0x00190EB9
		public void Cleanup()
		{
			this.noise = null;
			this.currentlyInitializedForMap = null;
		}

		// Token: 0x06004BA2 RID: 19362 RVA: 0x00192CCC File Offset: 0x00190ECC
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

		// Token: 0x04002DC4 RID: 11716
		private Map currentlyInitializedForMap;

		// Token: 0x04002DC5 RID: 11717
		public List<TerrainThreshold> thresholds = new List<TerrainThreshold>();

		// Token: 0x04002DC6 RID: 11718
		public float perlinFrequency = 0.01f;

		// Token: 0x04002DC7 RID: 11719
		public float perlinLacunarity = 2f;

		// Token: 0x04002DC8 RID: 11720
		public float perlinPersistence = 0.5f;

		// Token: 0x04002DC9 RID: 11721
		public int perlinOctaves = 6;

		// Token: 0x04002DCA RID: 11722
		public float minFertility = -999f;

		// Token: 0x04002DCB RID: 11723
		public float maxFertility = 999f;

		// Token: 0x04002DCC RID: 11724
		public int minSize;

		// Token: 0x04002DCD RID: 11725
		[Unsaved(false)]
		private ModuleBase noise;
	}
}
