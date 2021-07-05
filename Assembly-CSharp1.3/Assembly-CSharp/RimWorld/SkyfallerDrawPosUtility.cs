using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020010D2 RID: 4306
	public static class SkyfallerDrawPosUtility
	{
		// Token: 0x0600671A RID: 26394 RVA: 0x0022D97C File Offset: 0x0022BB7C
		public static Vector3 DrawPos_Accelerate(Vector3 center, int ticksToImpact, float angle, float speed, CompSkyfallerRandomizeDirection offsetComp = null)
		{
			ticksToImpact = Mathf.Max(ticksToImpact, 0);
			float dist = Mathf.Pow((float)ticksToImpact, 0.95f) * 1.7f * speed;
			return SkyfallerDrawPosUtility.PosAtDist(center, dist, angle, offsetComp);
		}

		// Token: 0x0600671B RID: 26395 RVA: 0x0022D9B4 File Offset: 0x0022BBB4
		public static Vector3 DrawPos_ConstantSpeed(Vector3 center, int ticksToImpact, float angle, float speed, CompSkyfallerRandomizeDirection offsetComp = null)
		{
			ticksToImpact = Mathf.Max(ticksToImpact, 0);
			float dist = (float)ticksToImpact * speed;
			return SkyfallerDrawPosUtility.PosAtDist(center, dist, angle, offsetComp);
		}

		// Token: 0x0600671C RID: 26396 RVA: 0x0022D9DC File Offset: 0x0022BBDC
		public static Vector3 DrawPos_Decelerate(Vector3 center, int ticksToImpact, float angle, float speed, CompSkyfallerRandomizeDirection offsetComp = null)
		{
			ticksToImpact = Mathf.Max(ticksToImpact, 0);
			float dist = (float)(ticksToImpact * ticksToImpact) * 0.00721f * speed;
			return SkyfallerDrawPosUtility.PosAtDist(center, dist, angle, offsetComp);
		}

		// Token: 0x0600671D RID: 26397 RVA: 0x0022DA09 File Offset: 0x0022BC09
		private static Vector3 PosAtDist(Vector3 center, float dist, float angle, CompSkyfallerRandomizeDirection offsetComp = null)
		{
			return center + Vector3Utility.FromAngleFlat(angle - 90f) * dist + ((offsetComp != null) ? offsetComp.Offset : Vector3.zero);
		}
	}
}
