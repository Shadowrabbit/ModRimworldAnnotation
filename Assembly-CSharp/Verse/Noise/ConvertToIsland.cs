using System;
using UnityEngine;

namespace Verse.Noise
{
	// Token: 0x020008CC RID: 2252
	public class ConvertToIsland : ModuleBase
	{
		// Token: 0x060037F1 RID: 14321 RVA: 0x0002B39E File Offset: 0x0002959E
		public ConvertToIsland() : base(1)
		{
		}

		// Token: 0x060037F2 RID: 14322 RVA: 0x0002B3A7 File Offset: 0x000295A7
		public ConvertToIsland(Vector3 viewCenter, float viewAngle, ModuleBase input) : base(1)
		{
			this.viewCenter = viewCenter;
			this.viewAngle = viewAngle;
			this.modules[0] = input;
		}

		// Token: 0x060037F3 RID: 14323 RVA: 0x00162164 File Offset: 0x00160364
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

		// Token: 0x040026C7 RID: 9927
		public Vector3 viewCenter;

		// Token: 0x040026C8 RID: 9928
		public float viewAngle;

		// Token: 0x040026C9 RID: 9929
		private const float WaterLevel = -0.12f;
	}
}
