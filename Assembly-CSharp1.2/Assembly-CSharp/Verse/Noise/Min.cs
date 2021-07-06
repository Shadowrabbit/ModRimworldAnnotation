using System;

namespace Verse.Noise
{
	// Token: 0x020008E5 RID: 2277
	public class Min : ModuleBase
	{
		// Token: 0x06003890 RID: 14480 RVA: 0x0002B7CC File Offset: 0x000299CC
		public Min() : base(2)
		{
		}

		// Token: 0x06003891 RID: 14481 RVA: 0x0002B7D5 File Offset: 0x000299D5
		public Min(ModuleBase lhs, ModuleBase rhs) : base(2)
		{
			this.modules[0] = lhs;
			this.modules[1] = rhs;
		}

		// Token: 0x06003892 RID: 14482 RVA: 0x001638C0 File Offset: 0x00161AC0
		public override double GetValue(double x, double y, double z)
		{
			double value = this.modules[0].GetValue(x, y, z);
			double value2 = this.modules[1].GetValue(x, y, z);
			return Math.Min(value, value2);
		}
	}
}
