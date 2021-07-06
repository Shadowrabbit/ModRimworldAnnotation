using System;
using UnityEngine;

namespace Verse.Noise
{
	// Token: 0x020008D2 RID: 2258
	public class RidgedMultifractal : ModuleBase
	{
		// Token: 0x170008D2 RID: 2258
		// (get) Token: 0x06003811 RID: 14353 RVA: 0x0002B508 File Offset: 0x00029708
		// (set) Token: 0x06003812 RID: 14354 RVA: 0x0002B510 File Offset: 0x00029710
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

		// Token: 0x170008D3 RID: 2259
		// (get) Token: 0x06003813 RID: 14355 RVA: 0x0002B519 File Offset: 0x00029719
		// (set) Token: 0x06003814 RID: 14356 RVA: 0x0002B521 File Offset: 0x00029721
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

		// Token: 0x170008D4 RID: 2260
		// (get) Token: 0x06003815 RID: 14357 RVA: 0x0002B530 File Offset: 0x00029730
		// (set) Token: 0x06003816 RID: 14358 RVA: 0x0002B538 File Offset: 0x00029738
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

		// Token: 0x170008D5 RID: 2261
		// (get) Token: 0x06003817 RID: 14359 RVA: 0x0002B541 File Offset: 0x00029741
		// (set) Token: 0x06003818 RID: 14360 RVA: 0x0002B549 File Offset: 0x00029749
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

		// Token: 0x170008D6 RID: 2262
		// (get) Token: 0x06003819 RID: 14361 RVA: 0x0002B55A File Offset: 0x0002975A
		// (set) Token: 0x0600381A RID: 14362 RVA: 0x0002B562 File Offset: 0x00029762
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

		// Token: 0x0600381B RID: 14363 RVA: 0x001623FC File Offset: 0x001605FC
		public RidgedMultifractal() : base(0)
		{
			this.UpdateWeights();
		}

		// Token: 0x0600381C RID: 14364 RVA: 0x00162450 File Offset: 0x00160650
		public RidgedMultifractal(double frequency, double lacunarity, int octaves, int seed, QualityMode quality) : base(0)
		{
			this.Frequency = frequency;
			this.Lacunarity = lacunarity;
			this.OctaveCount = octaves;
			this.Seed = seed;
			this.Quality = quality;
		}

		// Token: 0x0600381D RID: 14365 RVA: 0x001624C4 File Offset: 0x001606C4
		private void UpdateWeights()
		{
			double num = 1.0;
			for (int i = 0; i < 30; i++)
			{
				this.weights[i] = Math.Pow(num, -1.0);
				num *= this.lacunarity;
			}
		}

		// Token: 0x0600381E RID: 14366 RVA: 0x00162508 File Offset: 0x00160708
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

		// Token: 0x040026D6 RID: 9942
		private double frequency = 1.0;

		// Token: 0x040026D7 RID: 9943
		private double lacunarity = 2.0;

		// Token: 0x040026D8 RID: 9944
		private QualityMode quality = QualityMode.Medium;

		// Token: 0x040026D9 RID: 9945
		private int octaveCount = 6;

		// Token: 0x040026DA RID: 9946
		private int seed;

		// Token: 0x040026DB RID: 9947
		private double[] weights = new double[30];
	}
}
