using System;
using UnityEngine;

namespace Verse.Noise
{
	// Token: 0x0200050A RID: 1290
	public class AbsLatitudeCurve : ModuleBase
	{
		// Token: 0x06002712 RID: 10002 RVA: 0x000F177C File Offset: 0x000EF97C
		public AbsLatitudeCurve() : base(0)
		{
		}

		// Token: 0x06002713 RID: 10003 RVA: 0x000F1785 File Offset: 0x000EF985
		public AbsLatitudeCurve(SimpleCurve curve, float planetRadius) : base(0)
		{
			this.curve = curve;
			this.planetRadius = planetRadius;
		}

		// Token: 0x06002714 RID: 10004 RVA: 0x000F179C File Offset: 0x000EF99C
		public override double GetValue(double x, double y, double z)
		{
			float f = Mathf.Asin((float)(y / (double)this.planetRadius)) * 57.29578f;
			return (double)this.curve.Evaluate(Mathf.Abs(f));
		}

		// Token: 0x0400184E RID: 6222
		public SimpleCurve curve;

		// Token: 0x0400184F RID: 6223
		public float planetRadius;
	}
}
