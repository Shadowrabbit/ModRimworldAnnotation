using System;

namespace Verse.Noise
{
	// Token: 0x02000519 RID: 1305
	public class Add : ModuleBase
	{
		// Token: 0x0600277A RID: 10106 RVA: 0x000F2EBE File Offset: 0x000F10BE
		public Add() : base(2)
		{
		}

		// Token: 0x0600277B RID: 10107 RVA: 0x000F2EC7 File Offset: 0x000F10C7
		public Add(ModuleBase lhs, ModuleBase rhs) : base(2)
		{
			this.modules[0] = lhs;
			this.modules[1] = rhs;
		}

		// Token: 0x0600277C RID: 10108 RVA: 0x000F2EE2 File Offset: 0x000F10E2
		public override double GetValue(double x, double y, double z)
		{
			return this.modules[0].GetValue(x, y, z) + this.modules[1].GetValue(x, y, z);
		}
	}
}
