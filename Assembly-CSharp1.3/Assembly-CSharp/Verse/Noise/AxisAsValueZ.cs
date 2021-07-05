using System;

namespace Verse.Noise
{
	// Token: 0x0200050C RID: 1292
	public class AxisAsValueZ : ModuleBase
	{
		// Token: 0x06002717 RID: 10007 RVA: 0x000F177C File Offset: 0x000EF97C
		public AxisAsValueZ() : base(0)
		{
		}

		// Token: 0x06002718 RID: 10008 RVA: 0x000F17D1 File Offset: 0x000EF9D1
		public override double GetValue(double x, double y, double z)
		{
			return z;
		}
	}
}
