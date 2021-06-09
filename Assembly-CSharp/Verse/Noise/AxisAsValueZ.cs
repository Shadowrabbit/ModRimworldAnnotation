using System;

namespace Verse.Noise
{
	// Token: 0x020008CB RID: 2251
	public class AxisAsValueZ : ModuleBase
	{
		// Token: 0x060037EF RID: 14319 RVA: 0x0002B37B File Offset: 0x0002957B
		public AxisAsValueZ() : base(0)
		{
		}

		// Token: 0x060037F0 RID: 14320 RVA: 0x0002B39B File Offset: 0x0002959B
		public override double GetValue(double x, double y, double z)
		{
			return z;
		}
	}
}
