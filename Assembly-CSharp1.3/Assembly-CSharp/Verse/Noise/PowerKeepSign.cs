using System;

namespace Verse.Noise
{
	// Token: 0x02000529 RID: 1321
	public class PowerKeepSign : ModuleBase
	{
		// Token: 0x060027C1 RID: 10177 RVA: 0x000F2EBE File Offset: 0x000F10BE
		public PowerKeepSign() : base(2)
		{
		}

		// Token: 0x060027C2 RID: 10178 RVA: 0x000F2EC7 File Offset: 0x000F10C7
		public PowerKeepSign(ModuleBase lhs, ModuleBase rhs) : base(2)
		{
			this.modules[0] = lhs;
			this.modules[1] = rhs;
		}

		// Token: 0x060027C3 RID: 10179 RVA: 0x000F36E8 File Offset: 0x000F18E8
		public override double GetValue(double x, double y, double z)
		{
			double value = this.modules[0].GetValue(x, y, z);
			return (double)Math.Sign(value) * Math.Pow(Math.Abs(value), this.modules[1].GetValue(x, y, z));
		}
	}
}
