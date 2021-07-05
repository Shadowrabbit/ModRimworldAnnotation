using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001738 RID: 5944
	public static class SkyfallerDrawPosUtility
	{
		// Token: 0x06008325 RID: 33573 RVA: 0x0026E494 File Offset: 0x0026C694
		public static Vector3 DrawPos_Accelerate(Vector3 center, int ticksToImpact, float angle, float speed)
		{
			ticksToImpact = Mathf.Max(ticksToImpact, 0);
			float dist = Mathf.Pow((float)ticksToImpact, 0.95f) * 1.7f * speed;
			return SkyfallerDrawPosUtility.PosAtDist(center, dist, angle);
		}

		// Token: 0x06008326 RID: 33574 RVA: 0x0026E4C8 File Offset: 0x0026C6C8
		public static Vector3 DrawPos_ConstantSpeed(Vector3 center, int ticksToImpact, float angle, float speed)
		{
			ticksToImpact = Mathf.Max(ticksToImpact, 0);
			float dist = (float)ticksToImpact * speed;
			return SkyfallerDrawPosUtility.PosAtDist(center, dist, angle);
		}

		// Token: 0x06008327 RID: 33575 RVA: 0x0026E4EC File Offset: 0x0026C6EC
		public static Vector3 DrawPos_Decelerate(Vector3 center, int ticksToImpact, float angle, float speed)
		{
			ticksToImpact = Mathf.Max(ticksToImpact, 0);
			float dist = (float)(ticksToImpact * ticksToImpact) * 0.00721f * speed;
			return SkyfallerDrawPosUtility.PosAtDist(center, dist, angle);
		}

		// Token: 0x06008328 RID: 33576 RVA: 0x000580E7 File Offset: 0x000562E7
		private static Vector3 PosAtDist(Vector3 center, float dist, float angle)
		{
			return center + Vector3Utility.FromAngleFlat(angle - 90f) * dist;
		}
	}
}
