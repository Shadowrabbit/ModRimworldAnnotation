using System;

namespace Verse.Noise
{
	// Token: 0x020008E2 RID: 2274
	public class InverseLerp : ModuleBase
	{
		// Token: 0x06003887 RID: 14471 RVA: 0x0002B39E File Offset: 0x0002959E
		public InverseLerp() : base(1)
		{
		}

		// Token: 0x06003888 RID: 14472 RVA: 0x0002BABE File Offset: 0x00029CBE
		public InverseLerp(ModuleBase module, float from, float to) : base(1)
		{
			this.modules[0] = module;
			this.from = from;
			this.to = to;
		}

		// Token: 0x06003889 RID: 14473 RVA: 0x00163828 File Offset: 0x00161A28
		public override double GetValue(double x, double y, double z)
		{
			double num = (this.modules[0].GetValue(x, y, z) - (double)this.from) / (double)(this.to - this.from);
			if (num < 0.0)
			{
				return 0.0;
			}
			if (num > 1.0)
			{
				return 1.0;
			}
			return num;
		}

		// Token: 0x04002709 RID: 9993
		private float from;

		// Token: 0x0400270A RID: 9994
		private float to;
	}
}
