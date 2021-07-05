using System;
using UnityEngine;

namespace Verse.Noise
{
	// Token: 0x0200050F RID: 1295
	public class DistanceFromPlanetViewCenter : ModuleBase
	{
		// Token: 0x0600271F RID: 10015 RVA: 0x000F177C File Offset: 0x000EF97C
		public DistanceFromPlanetViewCenter() : base(0)
		{
		}

		// Token: 0x06002720 RID: 10016 RVA: 0x000F18F9 File Offset: 0x000EFAF9
		public DistanceFromPlanetViewCenter(Vector3 viewCenter, float viewAngle, bool invert = false) : base(0)
		{
			this.viewCenter = viewCenter;
			this.viewAngle = viewAngle;
			this.invert = invert;
		}

		// Token: 0x06002721 RID: 10017 RVA: 0x000F1918 File Offset: 0x000EFB18
		public override double GetValue(double x, double y, double z)
		{
			float valueInt = this.GetValueInt(x, y, z);
			if (this.invert)
			{
				return (double)(1f - valueInt);
			}
			return (double)valueInt;
		}

		// Token: 0x06002722 RID: 10018 RVA: 0x000F1942 File Offset: 0x000EFB42
		private float GetValueInt(double x, double y, double z)
		{
			if (this.viewAngle >= 180f)
			{
				return 0f;
			}
			return Mathf.Min(Vector3.Angle(this.viewCenter, new Vector3((float)x, (float)y, (float)z)) / this.viewAngle, 1f);
		}

		// Token: 0x04001854 RID: 6228
		public Vector3 viewCenter;

		// Token: 0x04001855 RID: 6229
		public float viewAngle;

		// Token: 0x04001856 RID: 6230
		public bool invert;
	}
}
