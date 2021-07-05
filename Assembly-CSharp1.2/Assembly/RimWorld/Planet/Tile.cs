using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02002072 RID: 8306
	public class Tile
	{
		// Token: 0x17001A13 RID: 6675
		// (get) Token: 0x0600B022 RID: 45090 RVA: 0x00072737 File Offset: 0x00070937
		public bool WaterCovered
		{
			get
			{
				return this.elevation <= 0f;
			}
		}

		// Token: 0x17001A14 RID: 6676
		// (get) Token: 0x0600B023 RID: 45091 RVA: 0x00072749 File Offset: 0x00070949
		public List<Tile.RoadLink> Roads
		{
			get
			{
				if (!this.biome.allowRoads)
				{
					return null;
				}
				return this.potentialRoads;
			}
		}

		// Token: 0x17001A15 RID: 6677
		// (get) Token: 0x0600B024 RID: 45092 RVA: 0x00072760 File Offset: 0x00070960
		public List<Tile.RiverLink> Rivers
		{
			get
			{
				if (!this.biome.allowRivers)
				{
					return null;
				}
				return this.potentialRivers;
			}
		}

		// Token: 0x0600B025 RID: 45093 RVA: 0x003329F8 File Offset: 0x00330BF8
		public override string ToString()
		{
			return string.Concat(new object[]
			{
				"(",
				this.biome,
				" elev=",
				this.elevation,
				"m hill=",
				this.hilliness,
				" temp=",
				this.temperature,
				"°C rain=",
				this.rainfall,
				"mm swampiness=",
				this.swampiness.ToStringPercent(),
				" potentialRoads=",
				(this.potentialRoads == null) ? 0 : this.potentialRoads.Count,
				" (allowed=",
				this.biome.allowRoads.ToString(),
				") potentialRivers=",
				(this.potentialRivers == null) ? 0 : this.potentialRivers.Count,
				" (allowed=",
				this.biome.allowRivers.ToString(),
				"))"
			});
		}

		// Token: 0x0400792C RID: 31020
		public const int Invalid = -1;

		// Token: 0x0400792D RID: 31021
		public BiomeDef biome;

		// Token: 0x0400792E RID: 31022
		public float elevation = 100f;

		// Token: 0x0400792F RID: 31023
		public Hilliness hilliness;

		// Token: 0x04007930 RID: 31024
		public float temperature = 20f;

		// Token: 0x04007931 RID: 31025
		public float rainfall;

		// Token: 0x04007932 RID: 31026
		public float swampiness;

		// Token: 0x04007933 RID: 31027
		public WorldFeature feature;

		// Token: 0x04007934 RID: 31028
		public List<Tile.RoadLink> potentialRoads;

		// Token: 0x04007935 RID: 31029
		public List<Tile.RiverLink> potentialRivers;

		// Token: 0x02002073 RID: 8307
		public struct RoadLink
		{
			// Token: 0x04007936 RID: 31030
			public int neighbor;

			// Token: 0x04007937 RID: 31031
			public RoadDef road;
		}

		// Token: 0x02002074 RID: 8308
		public struct RiverLink
		{
			// Token: 0x04007938 RID: 31032
			public int neighbor;

			// Token: 0x04007939 RID: 31033
			public RiverDef river;
		}
	}
}
