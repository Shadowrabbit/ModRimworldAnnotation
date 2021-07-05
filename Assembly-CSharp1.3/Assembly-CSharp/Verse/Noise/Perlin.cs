using System;
using UnityEngine;

namespace Verse.Noise
{
	// Token: 0x02000512 RID: 1298
	public class Perlin : ModuleBase
	{
		// Token: 0x170007A4 RID: 1956
		// (get) Token: 0x0600272A RID: 10026 RVA: 0x000F19D7 File Offset: 0x000EFBD7
		// (set) Token: 0x0600272B RID: 10027 RVA: 0x000F19DF File Offset: 0x000EFBDF
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

		// Token: 0x170007A5 RID: 1957
		// (get) Token: 0x0600272C RID: 10028 RVA: 0x000F19E8 File Offset: 0x000EFBE8
		// (set) Token: 0x0600272D RID: 10029 RVA: 0x000F19F0 File Offset: 0x000EFBF0
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

		// Token: 0x170007A6 RID: 1958
		// (get) Token: 0x0600272E RID: 10030 RVA: 0x000F19F9 File Offset: 0x000EFBF9
		// (set) Token: 0x0600272F RID: 10031 RVA: 0x000F1A01 File Offset: 0x000EFC01
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

		// Token: 0x170007A7 RID: 1959
		// (get) Token: 0x06002730 RID: 10032 RVA: 0x000F1A0A File Offset: 0x000EFC0A
		// (set) Token: 0x06002731 RID: 10033 RVA: 0x000F1A12 File Offset: 0x000EFC12
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

		// Token: 0x170007A8 RID: 1960
		// (get) Token: 0x06002732 RID: 10034 RVA: 0x000F1A23 File Offset: 0x000EFC23
		// (set) Token: 0x06002733 RID: 10035 RVA: 0x000F1A2B File Offset: 0x000EFC2B
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

		// Token: 0x170007A9 RID: 1961
		// (get) Token: 0x06002734 RID: 10036 RVA: 0x000F1A34 File Offset: 0x000EFC34
		// (set) Token: 0x06002735 RID: 10037 RVA: 0x000F1A3C File Offset: 0x000EFC3C
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

		// Token: 0x06002736 RID: 10038 RVA: 0x000F1A48 File Offset: 0x000EFC48
		public Perlin() : base(0)
		{
		}

		// Token: 0x06002737 RID: 10039 RVA: 0x000F1A98 File Offset: 0x000EFC98
		public Perlin(double frequency, double lacunarity, double persistence, int octaves, int seed, QualityMode quality) : base(0)
		{
			this.Frequency = frequency;
			this.Lacunarity = lacunarity;
			this.OctaveCount = octaves;
			this.Persistence = persistence;
			this.Seed = seed;
			this.Quality = quality;
		}

		// Token: 0x06002738 RID: 10040 RVA: 0x000F1B14 File Offset: 0x000EFD14
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

		// Token: 0x04001859 RID: 6233
		private double frequency = 1.0;

		// Token: 0x0400185A RID: 6234
		private double lacunarity = 2.0;

		// Token: 0x0400185B RID: 6235
		private QualityMode quality = QualityMode.Medium;

		// Token: 0x0400185C RID: 6236
		private int octaveCount = 6;

		// Token: 0x0400185D RID: 6237
		private double persistence = 0.5;

		// Token: 0x0400185E RID: 6238
		private int seed;
	}
}
