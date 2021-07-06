using System;

namespace Verse.Noise
{
	// Token: 0x020008CA RID: 2250
	public class AxisAsValueX : ModuleBase
	{
		// Token: 0x060037ED RID: 14317 RVA: 0x0002B37B File Offset: 0x0002957B
		public AxisAsValueX() : base(0)
		{
		}

		// Token: 0x060037EE RID: 14318 RVA: 0x0001037D File Offset: 0x0000E57D
		public override double GetValue(double x, double y, double z)
		{
			return x;
		}
	}
}
