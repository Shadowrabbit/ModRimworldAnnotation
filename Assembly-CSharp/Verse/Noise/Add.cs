using System;

namespace Verse.Noise
{
	// Token: 0x020008D8 RID: 2264
	public class Add : ModuleBase
	{
		// Token: 0x06003852 RID: 14418 RVA: 0x0002B7CC File Offset: 0x000299CC
		public Add() : base(2)
		{
		}

		// Token: 0x06003853 RID: 14419 RVA: 0x0002B7D5 File Offset: 0x000299D5
		public Add(ModuleBase lhs, ModuleBase rhs) : base(2)
		{
			this.modules[0] = lhs;
			this.modules[1] = rhs;
		}

		// Token: 0x06003854 RID: 14420 RVA: 0x0002B7F0 File Offset: 0x000299F0
		public override double GetValue(double x, double y, double z)
		{
			return this.modules[0].GetValue(x, y, z) + this.modules[1].GetValue(x, y, z);
		}
	}
}
