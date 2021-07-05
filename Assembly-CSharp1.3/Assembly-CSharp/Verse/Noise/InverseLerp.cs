using System;

namespace Verse.Noise
{
	// Token: 0x02000522 RID: 1314
	public class InverseLerp : ModuleBase
	{
		// Token: 0x060027AC RID: 10156 RVA: 0x000F17D4 File Offset: 0x000EF9D4
		public InverseLerp() : base(1)
		{
		}

		// Token: 0x060027AD RID: 10157 RVA: 0x000F357D File Offset: 0x000F177D
		public InverseLerp(ModuleBase module, float from, float to) : base(1)
		{
			this.modules[0] = module;
			this.from = from;
			this.to = to;
		}

		// Token: 0x060027AE RID: 10158 RVA: 0x000F35A0 File Offset: 0x000F17A0
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

		// Token: 0x04001890 RID: 6288
		private float from;

		// Token: 0x04001891 RID: 6289
		private float to;
	}
}
