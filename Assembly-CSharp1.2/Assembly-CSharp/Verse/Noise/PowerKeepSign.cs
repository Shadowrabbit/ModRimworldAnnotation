using System;

namespace Verse.Noise
{
	// Token: 0x020008E9 RID: 2281
	public class PowerKeepSign : ModuleBase
	{
		// Token: 0x0600389C RID: 14492 RVA: 0x0002B7CC File Offset: 0x000299CC
		public PowerKeepSign() : base(2)
		{
		}

		// Token: 0x0600389D RID: 14493 RVA: 0x0002B7D5 File Offset: 0x000299D5
		public PowerKeepSign(ModuleBase lhs, ModuleBase rhs) : base(2)
		{
			this.modules[0] = lhs;
			this.modules[1] = rhs;
		}

		// Token: 0x0600389E RID: 14494 RVA: 0x001638F4 File Offset: 0x00161AF4
		public override double GetValue(double x, double y, double z)
		{
			double value = this.modules[0].GetValue(x, y, z);
			return (double)Math.Sign(value) * Math.Pow(Math.Abs(value), this.modules[1].GetValue(x, y, z));
		}
	}
}
