using System;

namespace Verse.Noise
{
	// Token: 0x02000521 RID: 1313
	public class Filter : ModuleBase
	{
		// Token: 0x060027A9 RID: 10153 RVA: 0x000F17D4 File Offset: 0x000EF9D4
		public Filter() : base(1)
		{
		}

		// Token: 0x060027AA RID: 10154 RVA: 0x000F3515 File Offset: 0x000F1715
		public Filter(ModuleBase module, float from, float to) : base(1)
		{
			this.modules[0] = module;
			this.from = from;
			this.to = to;
		}

		// Token: 0x060027AB RID: 10155 RVA: 0x000F3538 File Offset: 0x000F1738
		public override double GetValue(double x, double y, double z)
		{
			double value = this.modules[0].GetValue(x, y, z);
			if (value >= (double)this.from && value <= (double)this.to)
			{
				return 1.0;
			}
			return 0.0;
		}

		// Token: 0x0400188E RID: 6286
		private float from;

		// Token: 0x0400188F RID: 6287
		private float to;
	}
}
