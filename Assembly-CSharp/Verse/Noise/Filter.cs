using System;

namespace Verse.Noise
{
	// Token: 0x020008E1 RID: 2273
	public class Filter : ModuleBase
	{
		// Token: 0x06003884 RID: 14468 RVA: 0x0002B39E File Offset: 0x0002959E
		public Filter() : base(1)
		{
		}

		// Token: 0x06003885 RID: 14469 RVA: 0x0002BA9E File Offset: 0x00029C9E
		public Filter(ModuleBase module, float from, float to) : base(1)
		{
			this.modules[0] = module;
			this.from = from;
			this.to = to;
		}

		// Token: 0x06003886 RID: 14470 RVA: 0x001637E0 File Offset: 0x001619E0
		public override double GetValue(double x, double y, double z)
		{
			double value = this.modules[0].GetValue(x, y, z);
			if (value >= (double)this.from && value <= (double)this.to)
			{
				return 1.0;
			}
			return 0.0;
		}

		// Token: 0x04002707 RID: 9991
		private float from;

		// Token: 0x04002708 RID: 9992
		private float to;
	}
}
