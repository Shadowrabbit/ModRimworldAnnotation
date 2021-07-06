using System;

namespace Verse.Noise
{
	// Token: 0x020008F2 RID: 2290
	public class Turbulence : ModuleBase
	{
		// Token: 0x060038E4 RID: 14564 RVA: 0x0002BF9C File Offset: 0x0002A19C
		public Turbulence() : base(1)
		{
			this.m_xDistort = new Perlin();
			this.m_yDistort = new Perlin();
			this.m_zDistort = new Perlin();
		}

		// Token: 0x060038E5 RID: 14565 RVA: 0x00163F04 File Offset: 0x00162104
		public Turbulence(ModuleBase input) : base(1)
		{
			this.m_xDistort = new Perlin();
			this.m_yDistort = new Perlin();
			this.m_zDistort = new Perlin();
			this.modules[0] = input;
		}

		// Token: 0x060038E6 RID: 14566 RVA: 0x0002BFD5 File Offset: 0x0002A1D5
		public Turbulence(double power, ModuleBase input) : this(new Perlin(), new Perlin(), new Perlin(), power, input)
		{
		}

		// Token: 0x060038E7 RID: 14567 RVA: 0x0002BFEE File Offset: 0x0002A1EE
		public Turbulence(Perlin x, Perlin y, Perlin z, double power, ModuleBase input) : base(1)
		{
			this.m_xDistort = x;
			this.m_yDistort = y;
			this.m_zDistort = z;
			this.modules[0] = input;
			this.Power = power;
		}

		// Token: 0x17000901 RID: 2305
		// (get) Token: 0x060038E8 RID: 14568 RVA: 0x0002C02D File Offset: 0x0002A22D
		// (set) Token: 0x060038E9 RID: 14569 RVA: 0x0002C03A File Offset: 0x0002A23A
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

		// Token: 0x17000902 RID: 2306
		// (get) Token: 0x060038EA RID: 14570 RVA: 0x0002C060 File Offset: 0x0002A260
		// (set) Token: 0x060038EB RID: 14571 RVA: 0x0002C068 File Offset: 0x0002A268
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

		// Token: 0x17000903 RID: 2307
		// (get) Token: 0x060038EC RID: 14572 RVA: 0x0002C071 File Offset: 0x0002A271
		// (set) Token: 0x060038ED RID: 14573 RVA: 0x0002C07E File Offset: 0x0002A27E
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

		// Token: 0x17000904 RID: 2308
		// (get) Token: 0x060038EE RID: 14574 RVA: 0x0002C0A4 File Offset: 0x0002A2A4
		// (set) Token: 0x060038EF RID: 14575 RVA: 0x0002C0B1 File Offset: 0x0002A2B1
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

		// Token: 0x060038F0 RID: 14576 RVA: 0x00163F54 File Offset: 0x00162154
		public override double GetValue(double x, double y, double z)
		{
			double x2 = x + this.m_xDistort.GetValue(x + 0.189422607421875, y + 0.99371337890625, z + 0.4781646728515625) * this.m_power;
			double y2 = y + this.m_yDistort.GetValue(x + 0.4046478271484375, y + 0.276611328125, z + 0.9230499267578125) * this.m_power;
			double z2 = z + this.m_zDistort.GetValue(x + 0.82122802734375, y + 0.1710968017578125, z + 0.6842803955078125) * this.m_power;
			return this.modules[0].GetValue(x2, y2, z2);
		}

		// Token: 0x04002727 RID: 10023
		private const double X0 = 0.189422607421875;

		// Token: 0x04002728 RID: 10024
		private const double Y0 = 0.99371337890625;

		// Token: 0x04002729 RID: 10025
		private const double Z0 = 0.4781646728515625;

		// Token: 0x0400272A RID: 10026
		private const double X1 = 0.4046478271484375;

		// Token: 0x0400272B RID: 10027
		private const double Y1 = 0.276611328125;

		// Token: 0x0400272C RID: 10028
		private const double Z1 = 0.9230499267578125;

		// Token: 0x0400272D RID: 10029
		private const double X2 = 0.82122802734375;

		// Token: 0x0400272E RID: 10030
		private const double Y2 = 0.1710968017578125;

		// Token: 0x0400272F RID: 10031
		private const double Z2 = 0.6842803955078125;

		// Token: 0x04002730 RID: 10032
		private double m_power = 1.0;

		// Token: 0x04002731 RID: 10033
		private Perlin m_xDistort;

		// Token: 0x04002732 RID: 10034
		private Perlin m_yDistort;

		// Token: 0x04002733 RID: 10035
		private Perlin m_zDistort;
	}
}
