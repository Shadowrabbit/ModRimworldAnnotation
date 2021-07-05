using System;

namespace Verse.Noise
{
	// Token: 0x02000524 RID: 1316
	public class Max : ModuleBase
	{
		// Token: 0x060027B2 RID: 10162 RVA: 0x000F2EBE File Offset: 0x000F10BE
		public Max() : base(2)
		{
		}

		// Token: 0x060027B3 RID: 10163 RVA: 0x000F2EC7 File Offset: 0x000F10C7
		public Max(ModuleBase lhs, ModuleBase rhs) : base(2)
		{
			this.modules[0] = lhs;
			this.modules[1] = rhs;
		}

		// Token: 0x060027B4 RID: 10164 RVA: 0x000F3618 File Offset: 0x000F1818
		public override double GetValue(double x, double y, double z)
		{
			double value = this.modules[0].GetValue(x, y, z);
			double value2 = this.modules[1].GetValue(x, y, z);
			return Math.Max(value, value2);
		}
	}
}
