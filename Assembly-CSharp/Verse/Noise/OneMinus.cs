using System;

namespace Verse.Noise
{
	// Token: 0x020008E7 RID: 2279
	public class OneMinus : ModuleBase
	{
		// Token: 0x06003896 RID: 14486 RVA: 0x0002B39E File Offset: 0x0002959E
		public OneMinus() : base(1)
		{
		}

		// Token: 0x06003897 RID: 14487 RVA: 0x0002B7A3 File Offset: 0x000299A3
		public OneMinus(ModuleBase module) : base(1)
		{
			this.modules[0] = module;
		}

		// Token: 0x06003898 RID: 14488 RVA: 0x0002BB14 File Offset: 0x00029D14
		public override double GetValue(double x, double y, double z)
		{
			return 1.0 - this.modules[0].GetValue(x, y, z);
		}
	}
}
