using System;

namespace Verse.Noise
{
	// Token: 0x020008E6 RID: 2278
	public class Multiply : ModuleBase
	{
		// Token: 0x06003893 RID: 14483 RVA: 0x0002B7CC File Offset: 0x000299CC
		public Multiply() : base(2)
		{
		}

		// Token: 0x06003894 RID: 14484 RVA: 0x0002B7D5 File Offset: 0x000299D5
		public Multiply(ModuleBase lhs, ModuleBase rhs) : base(2)
		{
			this.modules[0] = lhs;
			this.modules[1] = rhs;
		}

		// Token: 0x06003895 RID: 14485 RVA: 0x0002BAF1 File Offset: 0x00029CF1
		public override double GetValue(double x, double y, double z)
		{
			return this.modules[0].GetValue(x, y, z) * this.modules[1].GetValue(x, y, z);
		}
	}
}
