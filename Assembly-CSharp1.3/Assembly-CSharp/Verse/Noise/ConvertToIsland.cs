using System;
using UnityEngine;

namespace Verse.Noise
{
	// Token: 0x0200050D RID: 1293
	public class ConvertToIsland : ModuleBase
	{
		// Token: 0x06002719 RID: 10009 RVA: 0x000F17D4 File Offset: 0x000EF9D4
		public ConvertToIsland() : base(1)
		{
		}

		// Token: 0x0600271A RID: 10010 RVA: 0x000F17DD File Offset: 0x000EF9DD
		public ConvertToIsland(Vector3 viewCenter, float viewAngle, ModuleBase input) : base(1)
		{
			this.viewCenter = viewCenter;
			this.viewAngle = viewAngle;
			this.modules[0] = input;
		}

		// Token: 0x0600271B RID: 10011 RVA: 0x000F1800 File Offset: 0x000EFA00
		public override double GetValue(double x, double y, double z)
		{
			float num = Vector3.Angle(this.viewCenter, new Vector3((float)x, (float)y, (float)z));
			double value = this.modules[0].GetValue(x, y, z);
			float num2 = Mathf.Max(2.5f, this.viewAngle * 0.25f);
			float num3 = Mathf.Max(0.8f, this.viewAngle * 0.1f);
			if (num < this.viewAngle - num2)
			{
				return value;
			}
			float num4 = GenMath.LerpDouble(this.viewAngle - num2, this.viewAngle - num3, 0f, 0.62f, num);
			if (value > -0.11999999731779099)
			{
				return (value - -0.11999999731779099) * (double)(1f - num4 * 0.7f) - (double)(num4 * 0.3f) + -0.11999999731779099;
			}
			return value - (double)(num4 * 0.3f);
		}

		// Token: 0x04001850 RID: 6224
		public Vector3 viewCenter;

		// Token: 0x04001851 RID: 6225
		public float viewAngle;

		// Token: 0x04001852 RID: 6226
		private const float WaterLevel = -0.12f;
	}
}
