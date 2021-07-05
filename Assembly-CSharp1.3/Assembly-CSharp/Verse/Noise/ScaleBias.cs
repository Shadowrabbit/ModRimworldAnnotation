using System;

namespace Verse.Noise
{
	// Token: 0x0200052C RID: 1324
	public class ScaleBias : ModuleBase
	{
		// Token: 0x060027D9 RID: 10201 RVA: 0x000F3A5E File Offset: 0x000F1C5E
		public ScaleBias() : base(1)
		{
		}

		// Token: 0x060027DA RID: 10202 RVA: 0x000F3A76 File Offset: 0x000F1C76
		public ScaleBias(ModuleBase input) : base(1)
		{
			this.modules[0] = input;
		}

		// Token: 0x060027DB RID: 10203 RVA: 0x000F3A97 File Offset: 0x000F1C97
		public ScaleBias(double scale, double bias, ModuleBase input) : base(1)
		{
			this.modules[0] = input;
			this.Bias = bias;
			this.Scale = scale;
		}

		// Token: 0x170007CD RID: 1997
		// (get) Token: 0x060027DC RID: 10204 RVA: 0x000F3AC6 File Offset: 0x000F1CC6
		// (set) Token: 0x060027DD RID: 10205 RVA: 0x000F3ACE File Offset: 0x000F1CCE
		public double Bias
		{
			get
			{
				return this.bias;
			}
			set
			{
				this.bias = value;
			}
		}

		// Token: 0x170007CE RID: 1998
		// (get) Token: 0x060027DE RID: 10206 RVA: 0x000F3AD7 File Offset: 0x000F1CD7
		// (set) Token: 0x060027DF RID: 10207 RVA: 0x000F3ADF File Offset: 0x000F1CDF
		public double Scale
		{
			get
			{
				return this.scale;
			}
			set
			{
				this.scale = value;
			}
		}

		// Token: 0x060027E0 RID: 10208 RVA: 0x000F3AE8 File Offset: 0x000F1CE8
		public override double GetValue(double x, double y, double z)
		{
			return this.modules[0].GetValue(x, y, z) * this.scale + this.bias;
		}

		// Token: 0x040018A1 RID: 6305
		private double scale = 1.0;

		// Token: 0x040018A2 RID: 6306
		private double bias;
	}
}
