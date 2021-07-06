using System;

namespace Verse.Noise
{
	// Token: 0x020008E3 RID: 2275
	public class Invert : ModuleBase
	{
		// Token: 0x0600388A RID: 14474 RVA: 0x0002B39E File Offset: 0x0002959E
		public Invert() : base(1)
		{
		}

		// Token: 0x0600388B RID: 14475 RVA: 0x0002B7A3 File Offset: 0x000299A3
		public Invert(ModuleBase input) : base(1)
		{
			this.modules[0] = input;
		}

		// Token: 0x0600388C RID: 14476 RVA: 0x0002BADE File Offset: 0x00029CDE
		public override double GetValue(double x, double y, double z)
		{
			return -this.modules[0].GetValue(x, y, z);
		}
	}
}
