using System;

namespace Verse.Noise
{
	// Token: 0x02000531 RID: 1329
	public class Turbulence : ModuleBase
	{
		// Token: 0x06002806 RID: 10246 RVA: 0x000F4126 File Offset: 0x000F2326
		public Turbulence() : base(1)
		{
			this.m_xDistort = new Perlin();
			this.m_yDistort = new Perlin();
			this.m_zDistort = new Perlin();
		}

		// Token: 0x06002807 RID: 10247 RVA: 0x000F4160 File Offset: 0x000F2360
		public Turbulence(ModuleBase input) : base(1)
		{
			this.m_xDistort = new Perlin();
			this.m_yDistort = new Perlin();
			this.m_zDistort = new Perlin();
			this.modules[0] = input;
		}

		// Token: 0x06002808 RID: 10248 RVA: 0x000F41AD File Offset: 0x000F23AD
		public Turbulence(double power, ModuleBase input) : this(new Perlin(), new Perlin(), new Perlin(), power, input)
		{
		}

		// Token: 0x06002809 RID: 10249 RVA: 0x000F41C6 File Offset: 0x000F23C6
		public Turbulence(Perlin x, Perlin y, Perlin z, double power, ModuleBase input) : base(1)
		{
			this.m_xDistort = x;
			this.m_yDistort = y;
			this.m_zDistort = z;
			this.modules[0] = input;
			this.Power = power;
		}

		// Token: 0x170007D9 RID: 2009
		// (get) Token: 0x0600280A RID: 10250 RVA: 0x000F4205 File Offset: 0x000F2405
		// (set) Token: 0x0600280B RID: 10251 RVA: 0x000F4212 File Offset: 0x000F2412
		public double Frequency
		{
			get
			{
				return this.m_xDistort.Frequency;
			}
			set
			{
				this.m_xDistort.Frequency = value;
				this.m_yDistort.Frequency = value;
				this.m_zDistort.Frequency = value;
			}
		}

		// Token: 0x170007DA RID: 2010
		// (get) Token: 0x0600280C RID: 10252 RVA: 0x000F4238 File Offset: 0x000F2438
		// (set) Token: 0x0600280D RID: 10253 RVA: 0x000F4240 File Offset: 0x000F2440
		public double Power
		{
			get
			{
				return this.m_power;
			}
			set
			{
				this.m_power = value;
			}
		}

		// Token: 0x170007DB RID: 2011
		// (get) Token: 0x0600280E RID: 10254 RVA: 0x000F4249 File Offset: 0x000F2449
		// (set) Token: 0x0600280F RID: 10255 RVA: 0x000F4256 File Offset: 0x000F2456
		public int Roughness
		{
			get
			{
				return this.m_xDistort.OctaveCount;
			}
			set
			{
				this.m_xDistort.OctaveCount = value;
				this.m_yDistort.OctaveCount = value;
				this.m_zDistort.OctaveCount = value;
			}
		}

		// Token: 0x170007DC RID: 2012
		// (get) Token: 0x06002810 RID: 10256 RVA: 0x000F427C File Offset: 0x000F247C
		// (set) Token: 0x06002811 RID: 10257 RVA: 0x000F4289 File Offset: 0x000F2489
		public int Seed
		{
			get
			{
				return this.m_xDistort.Seed;
			}
			set
			{
				this.m_xDistort.Seed = value;
				this.m_yDistort.Seed = value + 1;
				this.m_zDistort.Seed = value + 2;
			}
		}

		// Token: 0x06002812 RID: 10258 RVA: 0x000F42B4 File Offset: 0x000F24B4
		public override double GetValue(double x, double y, double z)
		{
			double x2 = x + this.m_xDistort.GetValue(x + 0.189422607421875, y + 0.99371337890625, z + 0.4781646728515625) * this.m_power;
			double y2 = y + this.m_yDistort.GetValue(x + 0.4046478271484375, y + 0.276611328125, z + 0.9230499267578125) * this.m_power;
			double z2 = z + this.m_zDistort.GetValue(x + 0.82122802734375, y + 0.1710968017578125, z + 0.6842803955078125) * this.m_power;
			return this.modules[0].GetValue(x2, y2, z2);
		}

		// Token: 0x040018AC RID: 6316
		private const double X0 = 0.189422607421875;

		// Token: 0x040018AD RID: 6317
		private const double Y0 = 0.99371337890625;

		// Token: 0x040018AE RID: 6318
		private const double Z0 = 0.4781646728515625;

		// Token: 0x040018AF RID: 6319
		private const double X1 = 0.4046478271484375;

		// Token: 0x040018B0 RID: 6320
		private const double Y1 = 0.276611328125;

		// Token: 0x040018B1 RID: 6321
		private const double Z1 = 0.9230499267578125;

		// Token: 0x040018B2 RID: 6322
		private const double X2 = 0.82122802734375;

		// Token: 0x040018B3 RID: 6323
		private const double Y2 = 0.1710968017578125;

		// Token: 0x040018B4 RID: 6324
		private const double Z2 = 0.6842803955078125;

		// Token: 0x040018B5 RID: 6325
		private double m_power = 1.0;

		// Token: 0x040018B6 RID: 6326
		private Perlin m_xDistort;

		// Token: 0x040018B7 RID: 6327
		private Perlin m_yDistort;

		// Token: 0x040018B8 RID: 6328
		private Perlin m_zDistort;
	}
}
