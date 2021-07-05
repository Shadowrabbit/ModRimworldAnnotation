using System;

namespace Verse.Noise
{
	// Token: 0x02000525 RID: 1317
	public class Min : ModuleBase
	{
		// Token: 0x060027B5 RID: 10165 RVA: 0x000F2EBE File Offset: 0x000F10BE
		public Min() : base(2)
		{
		}

		// Token: 0x060027B6 RID: 10166 RVA: 0x000F2EC7 File Offset: 0x000F10C7
		public Min(ModuleBase lhs, ModuleBase rhs) : base(2)
		{
			this.modules[0] = lhs;
			this.modules[1] = rhs;
		}

		// Token: 0x060027B7 RID: 10167 RVA: 0x000F364C File Offset: 0x000F184C
		public override double GetValue(double x, double y, double z)
		{
			double value = this.modules[0].GetValue(x, y, z);
			double value2 = this.modules[1].GetValue(x, y, z);
			return Math.Min(value, value2);
		}
	}
}
