using System;

namespace Verse.Noise
{
	// Token: 0x020008E0 RID: 2272
	public class Exponent : ModuleBase
	{
		// Token: 0x0600387E RID: 14462 RVA: 0x0002BA2C File Offset: 0x00029C2C
		public Exponent() : base(1)
		{
		}

		// Token: 0x0600387F RID: 14463 RVA: 0x0002BA44 File Offset: 0x00029C44
		public Exponent(ModuleBase input) : base(1)
		{
			this.modules[0] = input;
		}

		// Token: 0x06003880 RID: 14464 RVA: 0x0002BA65 File Offset: 0x00029C65
		public Exponent(double exponent, ModuleBase input) : base(1)
		{
			this.modules[0] = input;
			this.Value = exponent;
		}

		// Token: 0x170008EE RID: 2286
		// (get) Token: 0x06003881 RID: 14465 RVA: 0x0002BA8D File Offset: 0x00029C8D
		// (set) Token: 0x06003882 RID: 14466 RVA: 0x0002BA95 File Offset: 0x00029C95
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

		// Token: 0x06003883 RID: 14467 RVA: 0x00163788 File Offset: 0x00161988
		public override double GetValue(double x, double y, double z)
		{
			return Math.Pow(Math.Abs((this.modules[0].GetValue(x, y, z) + 1.0) / 2.0), this.m_exponent) * 2.0 - 1.0;
		}

		// Token: 0x04002706 RID: 9990
		private double m_exponent = 1.0;
	}
}
