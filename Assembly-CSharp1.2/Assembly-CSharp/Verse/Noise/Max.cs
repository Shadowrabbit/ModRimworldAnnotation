using System;

namespace Verse.Noise
{
	// Token: 0x020008E4 RID: 2276
	public class Max : ModuleBase
	{
		// Token: 0x0600388D RID: 14477 RVA: 0x0002B7CC File Offset: 0x000299CC
		public Max() : base(2)
		{
		}

		// Token: 0x0600388E RID: 14478 RVA: 0x0002B7D5 File Offset: 0x000299D5
		public Max(ModuleBase lhs, ModuleBase rhs) : base(2)
		{
			this.modules[0] = lhs;
			this.modules[1] = rhs;
		}

		// Token: 0x0600388F RID: 14479 RVA: 0x0016388C File Offset: 0x00161A8C
		public override double GetValue(double x, double y, double z)
		{
			double value = this.modules[0].GetValue(x, y, z);
			double value2 = this.modules[1].GetValue(x, y, z);
			return Math.Max(value, value2);
		}
	}
}
