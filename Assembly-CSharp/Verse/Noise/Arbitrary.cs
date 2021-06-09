using System;

namespace Verse.Noise
{
	// Token: 0x020008D9 RID: 2265
	public class Arbitrary : ModuleBase
	{
		// Token: 0x06003855 RID: 14421 RVA: 0x0002B39E File Offset: 0x0002959E
		public Arbitrary() : base(1)
		{
		}

		// Token: 0x06003856 RID: 14422 RVA: 0x0002B813 File Offset: 0x00029A13
		public Arbitrary(ModuleBase source, Func<double, double> processor) : base(1)
		{
			this.modules[0] = source;
			this.processor = processor;
		}

		// Token: 0x06003857 RID: 14423 RVA: 0x0002B82C File Offset: 0x00029A2C
		public override double GetValue(double x, double y, double z)
		{
			return this.processor(this.modules[0].GetValue(x, y, z));
		}

		// Token: 0x040026FB RID: 9979
		private Func<double, double> processor;
	}
}
