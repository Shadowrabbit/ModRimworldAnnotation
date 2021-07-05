using System;

namespace Verse.Noise
{
	// Token: 0x0200050B RID: 1291
	public class AxisAsValueX : ModuleBase
	{
		// Token: 0x06002715 RID: 10005 RVA: 0x000F177C File Offset: 0x000EF97C
		public AxisAsValueX() : base(0)
		{
		}

		// Token: 0x06002716 RID: 10006 RVA: 0x000210E7 File Offset: 0x0001F2E7
		public override double GetValue(double x, double y, double z)
		{
			return x;
		}
	}
}
