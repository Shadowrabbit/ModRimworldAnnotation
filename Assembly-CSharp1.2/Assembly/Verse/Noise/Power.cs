using System;

namespace Verse.Noise
{
	// Token: 0x020008E8 RID: 2280
	public class Power : ModuleBase
	{
		// Token: 0x06003899 RID: 14489 RVA: 0x0002B7CC File Offset: 0x000299CC
		public Power() : base(2)
		{
		}

		// Token: 0x0600389A RID: 14490 RVA: 0x0002B7D5 File Offset: 0x000299D5
		public Power(ModuleBase lhs, ModuleBase rhs) : base(2)
		{
			this.modules[0] = lhs;
			this.modules[1] = rhs;
		}

		// Token: 0x0600389B RID: 14491 RVA: 0x0002BB30 File Offset: 0x00029D30
		public override double GetValue(double x, double y, double z)
		{
			return Math.Pow(this.modules[0].GetValue(x, y, z), this.modules[1].GetValue(x, y, z));
		}
	}
}
