using System;

namespace Verse.Noise
{
	// Token: 0x02000520 RID: 1312
	public class Exponent : ModuleBase
	{
		// Token: 0x060027A3 RID: 10147 RVA: 0x000F344E File Offset: 0x000F164E
		public Exponent() : base(1)
		{
		}

		// Token: 0x060027A4 RID: 10148 RVA: 0x000F3466 File Offset: 0x000F1666
		public Exponent(ModuleBase input) : base(1)
		{
			this.modules[0] = input;
		}

		// Token: 0x060027A5 RID: 10149 RVA: 0x000F3487 File Offset: 0x000F1687
		public Exponent(double exponent, ModuleBase input) : base(1)
		{
			this.modules[0] = input;
			this.Value = exponent;
		}

		// Token: 0x170007C6 RID: 1990
		// (get) Token: 0x060027A6 RID: 10150 RVA: 0x000F34AF File Offset: 0x000F16AF
		// (set) Token: 0x060027A7 RID: 10151 RVA: 0x000F34B7 File Offset: 0x000F16B7
		public double Value
		{
			get
			{
				return this.m_exponent;
			}
			set
			{
				this.m_exponent = value;
			}
		}

		// Token: 0x060027A8 RID: 10152 RVA: 0x000F34C0 File Offset: 0x000F16C0
		public override double GetValue(double x, double y, double z)
		{
			return Math.Pow(Math.Abs((this.modules[0].GetValue(x, y, z) + 1.0) / 2.0), this.m_exponent) * 2.0 - 1.0;
		}

		// Token: 0x0400188D RID: 6285
		private double m_exponent = 1.0;
	}
}
