using System;

namespace Verse.Noise
{
	// Token: 0x02000523 RID: 1315
	public class Invert : ModuleBase
	{
		// Token: 0x060027AF RID: 10159 RVA: 0x000F17D4 File Offset: 0x000EF9D4
		public Invert() : base(1)
		{
		}

		// Token: 0x060027B0 RID: 10160 RVA: 0x000F2E95 File Offset: 0x000F1095
		public Invert(ModuleBase input) : base(1)
		{
			this.modules[0] = input;
		}

		// Token: 0x060027B1 RID: 10161 RVA: 0x000F3602 File Offset: 0x000F1802
		public override double GetValue(double x, double y, double z)
		{
			return -this.modules[0].GetValue(x, y, z);
		}
	}
}
