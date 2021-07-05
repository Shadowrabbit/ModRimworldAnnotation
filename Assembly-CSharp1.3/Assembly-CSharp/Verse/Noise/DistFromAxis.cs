using System;

namespace Verse.Noise
{
	// Token: 0x0200050E RID: 1294
	public class DistFromAxis : ModuleBase
	{
		// Token: 0x0600271C RID: 10012 RVA: 0x000F177C File Offset: 0x000EF97C
		public DistFromAxis() : base(0)
		{
		}

		// Token: 0x0600271D RID: 10013 RVA: 0x000F18D9 File Offset: 0x000EFAD9
		public DistFromAxis(float span) : base(0)
		{
			this.span = span;
		}

		// Token: 0x0600271E RID: 10014 RVA: 0x000F18E9 File Offset: 0x000EFAE9
		public override double GetValue(double x, double y, double z)
		{
			return Math.Abs(x) / (double)this.span;
		}

		// Token: 0x04001853 RID: 6227
		public float span;
	}
}
