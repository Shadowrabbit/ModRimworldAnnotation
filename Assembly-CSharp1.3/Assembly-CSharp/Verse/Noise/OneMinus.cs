using System;

namespace Verse.Noise
{
	// Token: 0x02000527 RID: 1319
	public class OneMinus : ModuleBase
	{
		// Token: 0x060027BB RID: 10171 RVA: 0x000F17D4 File Offset: 0x000EF9D4
		public OneMinus() : base(1)
		{
		}

		// Token: 0x060027BC RID: 10172 RVA: 0x000F2E95 File Offset: 0x000F1095
		public OneMinus(ModuleBase module) : base(1)
		{
			this.modules[0] = module;
		}

		// Token: 0x060027BD RID: 10173 RVA: 0x000F36A3 File Offset: 0x000F18A3
		public override double GetValue(double x, double y, double z)
		{
			return 1.0 - this.modules[0].GetValue(x, y, z);
		}
	}
}
