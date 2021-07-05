using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02001773 RID: 6003
	public class Tile
	{
		// Token: 0x17001697 RID: 5783
		// (get) Token: 0x06008A72 RID: 35442 RVA: 0x0031AF42 File Offset: 0x00319142
		public bool WaterCovered
		{
			get
			{
				return this.elevation <= 0f;
			}
		}

		// Token: 0x17001698 RID: 5784
		// (get) Token: 0x06008A73 RID: 35443 RVA: 0x0031AF54 File Offset: 0x00319154
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

		// Token: 0x17001699 RID: 5785
		// (get) Token: 0x06008A74 RID: 35444 RVA: 0x0031AF6B File Offset: 0x0031916B
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

		// Token: 0x06008A75 RID: 35445 RVA: 0x0031AF84 File Offset: 0x00319184
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

		// Token: 0x04005819 RID: 22553
		public const int Invalid = -1;

		// Token: 0x0400581A RID: 22554
		public BiomeDef biome;

		// Token: 0x0400581B RID: 22555
		public float elevation = 100f;

		// Token: 0x0400581C RID: 22556
		public Hilliness hilliness;

		// Token: 0x0400581D RID: 22557
		public float temperature = 20f;

		// Token: 0x0400581E RID: 22558
		public float rainfall;

		// Token: 0x0400581F RID: 22559
		public float swampiness;

		// Token: 0x04005820 RID: 22560
		public WorldFeature feature;

		// Token: 0x04005821 RID: 22561
		public List<Tile.RoadLink> potentialRoads;

		// Token: 0x04005822 RID: 22562
		public List<Tile.RiverLink> potentialRivers;

		// Token: 0x020029B4 RID: 10676
		public struct RoadLink
		{
			// Token: 0x04009CDE RID: 40158
			public int neighbor;

			// Token: 0x04009CDF RID: 40159
			public RoadDef road;
		}

		// Token: 0x020029B5 RID: 10677
		public struct RiverLink
		{
			// Token: 0x04009CE0 RID: 40160
			public int neighbor;

			// Token: 0x04009CE1 RID: 40161
			public RiverDef river;
		}
	}
}
