using System;

namespace Verse.Noise
{
	// Token: 0x0200051A RID: 1306
	public class Arbitrary : ModuleBase
	{
		// Token: 0x0600277D RID: 10109 RVA: 0x000F17D4 File Offset: 0x000EF9D4
		public Arbitrary() : base(1)
		{
		}

		// Token: 0x0600277E RID: 10110 RVA: 0x000F2F05 File Offset: 0x000F1105
		public Arbitrary(ModuleBase source, Func<double, double> processor) : base(1)
		{
			this.modules[0] = source;
			this.processor = processor;
		}

		// Token: 0x0600277F RID: 10111 RVA: 0x000F2F1E File Offset: 0x000F111E
		public override double GetValue(double x, double y, double z)
		{
			return this.processor(this.modules[0].GetValue(x, y, z));
		}

		// Token: 0x04001884 RID: 6276
		private Func<double, double> processor;
	}
}
