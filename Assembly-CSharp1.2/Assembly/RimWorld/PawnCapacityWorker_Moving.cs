using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020013E7 RID: 5095
	public class PawnCapacityWorker_Moving : PawnCapacityWorker
	{
		// Token: 0x06006E3D RID: 28221 RVA: 0x0021BC60 File Offset: 0x00219E60
		public override float CalculateCapacityLevel(HediffSet diffSet, List<PawnCapacityUtility.CapacityImpactor> impactors = null)
		{
			float num = 0f;
			float num2 = PawnCapacityUtility.CalculateLimbEfficiency(diffSet, BodyPartTagDefOf.MovingLimbCore, BodyPartTagDefOf.MovingLimbSegment, BodyPartTagDefOf.MovingLimbDigit, 0.4f, out num, impactors);
			if (num < 0.4999f)
			{
				return 0f;
			}
			num2 *= PawnCapacityUtility.CalculateTagEfficiency(diffSet, BodyPartTagDefOf.Pelvis, float.MaxValue, default(FloatRange), impactors, -1f);
			num2 *= PawnCapacityUtility.CalculateTagEfficiency(diffSet, BodyPartTagDefOf.Spine, float.MaxValue, default(FloatRange), impactors, -1f);
			num2 = Mathf.Lerp(num2, num2 * base.CalculateCapacityAndRecord(diffSet, PawnCapacityDefOf.Breathing, impactors), 0.2f);
			num2 = Mathf.Lerp(num2, num2 * base.CalculateCapacityAndRecord(diffSet, PawnCapacityDefOf.BloodPumping, impactors), 0.2f);
			return num2 * Mathf.Min(base.CalculateCapacityAndRecord(diffSet, PawnCapacityDefOf.Consciousness, impactors), 1f);
		}

		// Token: 0x06006E3E RID: 28222 RVA: 0x0004ABE0 File Offset: 0x00048DE0
		public override bool CanHaveCapacity(BodyDef body)
		{
			return body.HasPartWithTag(BodyPartTagDefOf.MovingLimbCore);
		}
	}
}
