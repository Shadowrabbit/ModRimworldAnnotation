using System;
using UnityEngine;

namespace Verse.Noise
{
	// Token: 0x020008CD RID: 2253
	public class DistanceFromPlanetViewCenter : ModuleBase
	{
		// Token: 0x060037F4 RID: 14324 RVA: 0x0002B37B File Offset: 0x0002957B
		public DistanceFromPlanetViewCenter() : base(0)
		{
		}

		// Token: 0x060037F5 RID: 14325 RVA: 0x0002B3C7 File Offset: 0x000295C7
		public DistanceFromPlanetViewCenter(Vector3 viewCenter, float viewAngle, bool invert = false) : base(0)
		{
			this.viewCenter = viewCenter;
			this.viewAngle = viewAngle;
			this.invert = invert;
		}

		// Token: 0x060037F6 RID: 14326 RVA: 0x00162240 File Offset: 0x00160440
		public override double GetValue(double x, double y, double z)
		{
			float valueInt = this.GetValueInt(x, y, z);
			if (this.invert)
			{
				return (double)(1f - valueInt);
			}
			return (double)valueInt;
		}

		// Token: 0x060037F7 RID: 14327 RVA: 0x0002B3E5 File Offset: 0x000295E5
		private float GetValueInt(double x, double y, double z)
		{
			if (this.viewAngle >= 180f)
			{
				return 0f;
			}
			return Mathf.Min(Vector3.Angle(this.viewCenter, new Vector3((float)x, (float)y, (float)z)) / this.viewAngle, 1f);
		}

		// Token: 0x040026CA RID: 9930
		public Vector3 viewCenter;

		// Token: 0x040026CB RID: 9931
		public float viewAngle;

		// Token: 0x040026CC RID: 9932
		public bool invert;
	}
}
