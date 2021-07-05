using System;

namespace Verse.Noise
{
	// Token: 0x02000518 RID: 1304
	public class Abs : ModuleBase
	{
		// Token: 0x06002777 RID: 10103 RVA: 0x000F17D4 File Offset: 0x000EF9D4
		public Abs() : base(1)
		{
		}

		// Token: 0x06002778 RID: 10104 RVA: 0x000F2E95 File Offset: 0x000F1095
		public Abs(ModuleBase input) : base(1)
		{
			this.modules[0] = input;
		}

		// Token: 0x06002779 RID: 10105 RVA: 0x000F2EA7 File Offset: 0x000F10A7
		public override double GetValue(double x, double y, double z)
		{
			return Math.Abs(this.modules[0].GetValue(x, y, z));
		}
	}
}
