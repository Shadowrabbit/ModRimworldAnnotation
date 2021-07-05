using System;
using UnityEngine;

namespace Verse.Noise
{
	// Token: 0x020008D1 RID: 2257
	public class Perlin : ModuleBase
	{
		// Token: 0x170008CC RID: 2252
		// (get) Token: 0x06003802 RID: 14338 RVA: 0x0002B49A File Offset: 0x0002969A
		// (set) Token: 0x06003803 RID: 14339 RVA: 0x0002B4A2 File Offset: 0x000296A2
		public double Frequency
		{
			get
			{
				return this.frequency;
			}
			set
			{
				this.frequency = value;
			}
		}

		// Token: 0x170008CD RID: 2253
		// (get) Token: 0x06003804 RID: 14340 RVA: 0x0002B4AB File Offset: 0x000296AB
		// (set) Token: 0x06003805 RID: 14341 RVA: 0x0002B4B3 File Offset: 0x000296B3
		public double Lacunarity
		{
			get
			{
				return this.lacunarity;
			}
			set
			{
				this.lacunarity = value;
			}
		}

		// Token: 0x170008CE RID: 2254
		// (get) Token: 0x06003806 RID: 14342 RVA: 0x0002B4BC File Offset: 0x000296BC
		// (set) Token: 0x06003807 RID: 14343 RVA: 0x0002B4C4 File Offset: 0x000296C4
		public QualityMode Quality
		{
			get
			{
				return this.quality;
			}
			set
			{
				this.quality = value;
			}
		}

		// Token: 0x170008CF RID: 2255
		// (get) Token: 0x06003808 RID: 14344 RVA: 0x0002B4CD File Offset: 0x000296CD
		// (set) Token: 0x06003809 RID: 14345 RVA: 0x0002B4D5 File Offset: 0x000296D5
		public int OctaveCount
		{
			get
			{
				return this.octaveCount;
			}
			set
			{
				this.octaveCount = Mathf.Clamp(value, 1, 30);
			}
		}

		// Token: 0x170008D0 RID: 2256
		// (get) Token: 0x0600380A RID: 14346 RVA: 0x0002B4E6 File Offset: 0x000296E6
		// (set) Token: 0x0600380B RID: 14347 RVA: 0x0002B4EE File Offset: 0x000296EE
		public double Persistence
		{
			get
			{
				return this.persistence;
			}
			set
			{
				this.persistence = value;
			}
		}

		// Token: 0x170008D1 RID: 2257
		// (get) Token: 0x0600380C RID: 14348 RVA: 0x0002B4F7 File Offset: 0x000296F7
		// (set) Token: 0x0600380D RID: 14349 RVA: 0x0002B4FF File Offset: 0x000296FF
		public int Seed
		{
			get
			{
				return this.seed;
			}
			set
			{
				this.seed = value;
			}
		}

		// Token: 0x0600380E RID: 14350 RVA: 0x0016226C File Offset: 0x0016046C
		public Perlin() : base(0)
		{
		}

		// Token: 0x0600380F RID: 14351 RVA: 0x001622BC File Offset: 0x001604BC
		public Perlin(double frequency, double lacunarity, double persistence, int octaves, int seed, QualityMode quality) : base(0)
		{
			this.Frequency = frequency;
			this.Lacunarity = lacunarity;
			this.OctaveCount = octaves;
			this.Persistence = persistence;
			this.Seed = seed;
			this.Quality = quality;
		}

		// Token: 0x06003810 RID: 14352 RVA: 0x00162338 File Offset: 0x00160538
		public override double GetValue(double x, double y, double z)
		{
			double num = 0.0;
			double num2 = 1.0;
			x *= this.frequency;
			y *= this.frequency;
			z *= this.frequency;
			for (int i = 0; i < this.octaveCount; i++)
			{
				double x2 = Utils.MakeInt32Range(x);
				double y2 = Utils.MakeInt32Range(y);
				double z2 = Utils.MakeInt32Range(z);
				long num3 = (long)(this.seed + i) & (long)((ulong)-1);
				double num4 = Utils.GradientCoherentNoise3D(x2, y2, z2, num3, this.quality);
				num += num4 * num2;
				x *= this.lacunarity;
				y *= this.lacunarity;
				z *= this.lacunarity;
				num2 *= this.persistence;
			}
			return num;
		}

		// Token: 0x040026D0 RID: 9936
		private double frequency = 1.0;

		// Token: 0x040026D1 RID: 9937
		private double lacunarity = 2.0;

		// Token: 0x040026D2 RID: 9938
		private QualityMode quality = QualityMode.Medium;

		// Token: 0x040026D3 RID: 9939
		private int octaveCount = 6;

		// Token: 0x040026D4 RID: 9940
		private double persistence = 0.5;

		// Token: 0x040026D5 RID: 9941
		private int seed;
	}
}
