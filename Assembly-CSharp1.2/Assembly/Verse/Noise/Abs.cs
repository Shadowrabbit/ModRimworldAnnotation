using System;

namespace Verse.Noise
{
	// Token: 0x020008D7 RID: 2263
	public class Abs : ModuleBase
	{
		// Token: 0x0600384F RID: 14415 RVA: 0x0002B39E File Offset: 0x0002959E
		public Abs() : base(1)
		{
		}

		// Token: 0x06003850 RID: 14416 RVA: 0x0002B7A3 File Offset: 0x000299A3
		public Abs(ModuleBase input) : base(1)
		{
			this.modules[0] = input;
		}

		// Token: 0x06003851 RID: 14417 RVA: 0x0002B7B5 File Offset: 0x000299B5
		public override double GetValue(double x, double y, double z)
		{
			return Math.Abs(this.modules[0].GetValue(x, y, z));
		}
	}
}
