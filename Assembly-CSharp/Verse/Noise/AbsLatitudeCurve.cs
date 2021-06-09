using System;
using UnityEngine;

namespace Verse.Noise
{
	// Token: 0x020008C9 RID: 2249
	public class AbsLatitudeCurve : ModuleBase
	{
		// Token: 0x060037EA RID: 14314 RVA: 0x0002B37B File Offset: 0x0002957B
		public AbsLatitudeCurve() : base(0)
		{
		}

		// Token: 0x060037EB RID: 14315 RVA: 0x0002B384 File Offset: 0x00029584
		public AbsLatitudeCurve(SimpleCurve curve, float planetRadius) : base(0)
		{
			this.curve = curve;
			this.planetRadius = planetRadius;
		}

		// Token: 0x060037EC RID: 14316 RVA: 0x0016212C File Offset: 0x0016032C
		public override double GetValue(double x, double y, double z)
		{
			float f = Mathf.Asin((float)(y / (double)this.planetRadius)) * 57.29578f;
			return (double)this.curve.Evaluate(Mathf.Abs(f));
		}

		// Token: 0x040026C5 RID: 9925
		public SimpleCurve curve;

		// Token: 0x040026C6 RID: 9926
		public float planetRadius;
	}
}
