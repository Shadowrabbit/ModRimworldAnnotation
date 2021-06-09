using System;

namespace Verse.Noise
{
	// Token: 0x020008CE RID: 2254
	public class DistFromAxis : ModuleBase
	{
		// Token: 0x060037F8 RID: 14328 RVA: 0x0002B37B File Offset: 0x0002957B
		public DistFromAxis() : base(0)
		{
		}

		// Token: 0x060037F9 RID: 14329 RVA: 0x0002B421 File Offset: 0x00029621
		public DistFromAxis(float span) : base(0)
		{
			this.span = span;
		}

		// Token: 0x060037FA RID: 14330 RVA: 0x0002B431 File Offset: 0x00029631
		public override double GetValue(double x, double y, double z)
		{
			return Math.Abs(x) / (double)this.span;
		}

		// Token: 0x040026CD RID: 9933
		public float span;
	}
}
