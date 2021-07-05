using System;

namespace Verse.Noise
{
	// Token: 0x0200052E RID: 1326
	public class Subtract : ModuleBase
	{
		// Token: 0x060027EE RID: 10222 RVA: 0x000F2EBE File Offset: 0x000F10BE
		public Subtract() : base(2)
		{
		}

		// Token: 0x060027EF RID: 10223 RVA: 0x000F2EC7 File Offset: 0x000F10C7
		public Subtract(ModuleBase lhs, ModuleBase rhs) : base(2)
		{
			this.modules[0] = lhs;
			this.modules[1] = rhs;
		}

		// Token: 0x060027F0 RID: 10224 RVA: 0x000F3DC1 File Offset: 0x000F1FC1
		public override double GetValue(double x, double y, double z)
		{
			return this.modules[0].GetValue(x, y, z) - this.modules[1].GetValue(x, y, z);
		}
	}
}
