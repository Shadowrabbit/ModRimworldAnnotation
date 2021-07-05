using System;

namespace Verse.Noise
{
	// Token: 0x020008CF RID: 2255
	public class CurveSimple : ModuleBase
	{
		// Token: 0x060037FB RID: 14331 RVA: 0x0002B441 File Offset: 0x00029641
		public CurveSimple(ModuleBase input, SimpleCurve curve) : base(1)
		{
			this.modules[0] = input;
			this.curve = curve;
		}

		// Token: 0x060037FC RID: 14332 RVA: 0x0002B45A File Offset: 0x0002965A
		public override double GetValue(double x, double y, double z)
		{
			return (double)this.curve.Evaluate((float)this.modules[0].GetValue(x, y, z));
		}

		// Token: 0x040026CE RID: 9934
		private SimpleCurve curve;
	}
}
