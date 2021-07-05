using System;

namespace Verse.Noise
{
	// Token: 0x02000528 RID: 1320
	public class Power : ModuleBase
	{
		// Token: 0x060027BE RID: 10174 RVA: 0x000F2EBE File Offset: 0x000F10BE
		public Power() : base(2)
		{
		}

		// Token: 0x060027BF RID: 10175 RVA: 0x000F2EC7 File Offset: 0x000F10C7
		public Power(ModuleBase lhs, ModuleBase rhs) : base(2)
		{
			this.modules[0] = lhs;
			this.modules[1] = rhs;
		}

		// Token: 0x060027C0 RID: 10176 RVA: 0x000F36BF File Offset: 0x000F18BF
		public override double GetValue(double x, double y, double z)
		{
			return Math.Pow(this.modules[0].GetValue(x, y, z), this.modules[1].GetValue(x, y, z));
		}
	}
}
