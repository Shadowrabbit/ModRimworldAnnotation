using System;

namespace Verse.Noise
{
	// Token: 0x02000526 RID: 1318
	public class Multiply : ModuleBase
	{
		// Token: 0x060027B8 RID: 10168 RVA: 0x000F2EBE File Offset: 0x000F10BE
		public Multiply() : base(2)
		{
		}

		// Token: 0x060027B9 RID: 10169 RVA: 0x000F2EC7 File Offset: 0x000F10C7
		public Multiply(ModuleBase lhs, ModuleBase rhs) : base(2)
		{
			this.modules[0] = lhs;
			this.modules[1] = rhs;
		}

		// Token: 0x060027BA RID: 10170 RVA: 0x000F3680 File Offset: 0x000F1880
		public override double GetValue(double x, double y, double z)
		{
			return this.modules[0].GetValue(x, y, z) * this.modules[1].GetValue(x, y, z);
		}
	}
}
