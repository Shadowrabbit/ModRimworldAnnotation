using System;

namespace Verse.Noise
{
	// Token: 0x02000510 RID: 1296
	public class CurveSimple : ModuleBase
	{
		// Token: 0x06002723 RID: 10019 RVA: 0x000F197E File Offset: 0x000EFB7E
		public CurveSimple(ModuleBase input, SimpleCurve curve) : base(1)
		{
			this.modules[0] = input;
			this.curve = curve;
		}

		// Token: 0x06002724 RID: 10020 RVA: 0x000F1997 File Offset: 0x000EFB97
		public override double GetValue(double x, double y, double z)
		{
			return (double)this.curve.Evaluate((float)this.modules[0].GetValue(x, y, z));
		}

		// Token: 0x04001857 RID: 6231
		private SimpleCurve curve;
	}
}
