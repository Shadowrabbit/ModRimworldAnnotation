using System;

namespace Verse.Noise
{
	// Token: 0x020008EE RID: 2286
	public class Subtract : ModuleBase
	{
		// Token: 0x060038C9 RID: 14537 RVA: 0x0002B7CC File Offset: 0x000299CC
		public Subtract() : base(2)
		{
		}

		// Token: 0x060038CA RID: 14538 RVA: 0x0002B7D5 File Offset: 0x000299D5
		public Subtract(ModuleBase lhs, ModuleBase rhs) : base(2)
		{
			this.modules[0] = lhs;
			this.modules[1] = rhs;
		}

		// Token: 0x060038CB RID: 14539 RVA: 0x0002BE0C File Offset: 0x0002A00C
		public override double GetValue(double x, double y, double z)
		{
			return this.modules[0].GetValue(x, y, z) - this.modules[1].GetValue(x, y, z);
		}
	}
}
