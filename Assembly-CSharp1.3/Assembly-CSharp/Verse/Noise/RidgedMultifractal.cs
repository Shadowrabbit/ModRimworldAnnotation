using System;
using UnityEngine;

namespace Verse.Noise
{
	// Token: 0x02000513 RID: 1299
	public class RidgedMultifractal : ModuleBase
	{
		// Token: 0x170007AA RID: 1962
		// (get) Token: 0x06002739 RID: 10041 RVA: 0x000F1BD5 File Offset: 0x000EFDD5
		// (set) Token: 0x0600273A RID: 10042 RVA: 0x000F1BDD File Offset: 0x000EFDDD
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

		// Token: 0x170007AB RID: 1963
		// (get) Token: 0x0600273B RID: 10043 RVA: 0x000F1BE6 File Offset: 0x000EFDE6
		// (set) Token: 0x0600273C RID: 10044 RVA: 0x000F1BEE File Offset: 0x000EFDEE
		public double Lacunarity
		{
			get
			{
				return this.lacunarity;
			}
			set
			{
				this.lacunarity = value;
				this.UpdateWeights();
			}
		}

		// Token: 0x170007AC RID: 1964
		// (get) Token: 0x0600273D RID: 10045 RVA: 0x000F1BFD File Offset: 0x000EFDFD
		// (set) Token: 0x0600273E RID: 10046 RVA: 0x000F1C05 File Offset: 0x000EFE05
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

		// Token: 0x170007AD RID: 1965
		// (get) Token: 0x0600273F RID: 10047 RVA: 0x000F1C0E File Offset: 0x000EFE0E
		// (set) Token: 0x06002740 RID: 10048 RVA: 0x000F1C16 File Offset: 0x000EFE16
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

		// Token: 0x170007AE RID: 1966
		// (get) Token: 0x06002741 RID: 10049 RVA: 0x000F1C27 File Offset: 0x000EFE27
		// (set) Token: 0x06002742 RID: 10050 RVA: 0x000F1C2F File Offset: 0x000EFE2F
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

		// Token: 0x06002743 RID: 10051 RVA: 0x000F1C38 File Offset: 0x000EFE38
		public RidgedMultifractal() : base(0)
		{
			this.UpdateWeights();
		}

		// Token: 0x06002744 RID: 10052 RVA: 0x000F1C8C File Offset: 0x000EFE8C
		public RidgedMultifractal(double frequency, double lacunarity, int octaves, int seed, QualityMode quality) : base(0)
		{
			this.Frequency = frequency;
			this.Lacunarity = lacunarity;
			this.OctaveCount = octaves;
			this.Seed = seed;
			this.Quality = quality;
		}

		// Token: 0x06002745 RID: 10053 RVA: 0x000F1D00 File Offset: 0x000EFF00
		private void UpdateWeights()
		{
			double num = 1.0;
			for (int i = 0; i < 30; i++)
			{
				this.weights[i] = Math.Pow(num, -1.0);
				num *= this.lacunarity;
			}
		}

		// Token: 0x06002746 RID: 10054 RVA: 0x000F1D44 File Offset: 0x000EFF44
		public override double GetValue(double x, double y, double z)
		{
			x *= this.frequency;
			y *= this.frequency;
			z *= this.frequency;
			double num = 0.0;
			double num2 = 1.0;
			double num3 = 1.0;
			double num4 = 2.0;
			for (int i = 0; i < this.octaveCount; i++)
			{
				double x2 = Utils.MakeInt32Range(x);
				double y2 = Utils.MakeInt32Range(y);
				double z2 = Utils.MakeInt32Range(z);
				long num5 = (long)(this.seed + i & int.MaxValue);
				double num6 = Utils.GradientCoherentNoise3D(x2, y2, z2, num5, this.quality);
				num6 = Math.Abs(num6);
				num6 = num3 - num6;
				num6 *= num6;
				num6 *= num2;
				num2 = num6 * num4;
				if (num2 > 1.0)
				{
					num2 = 1.0;
				}
				if (num2 < 0.0)
				{
					num2 = 0.0;
				}
				num += num6 * this.weights[i];
				x *= this.lacunarity;
				y *= this.lacunarity;
				z *= this.lacunarity;
			}
			return num * 1.25 - 1.0;
		}

		// Token: 0x0400185F RID: 6239
		private double frequency = 1.0;

		// Token: 0x04001860 RID: 6240
		private double lacunarity = 2.0;

		// Token: 0x04001861 RID: 6241
		private QualityMode quality = QualityMode.Medium;

		// Token: 0x04001862 RID: 6242
		private int octaveCount = 6;

		// Token: 0x04001863 RID: 6243
		private int seed;

		// Token: 0x04001864 RID: 6244
		private double[] weights = new double[30];
	}
}
