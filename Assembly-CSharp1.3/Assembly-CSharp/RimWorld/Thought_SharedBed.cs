using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020009AE RID: 2478
	public class Thought_SharedBed : Thought_Situational
	{
		// Token: 0x17000ADB RID: 2779
		// (get) Token: 0x06003DEA RID: 15850 RVA: 0x00153BB4 File Offset: 0x00151DB4
		protected override float BaseMoodOffset
		{
			get
			{
				Pawn mostDislikedNonPartnerBedOwner = LovePartnerRelationUtility.GetMostDislikedNonPartnerBedOwner(this.pawn);
				if (mostDislikedNonPartnerBedOwner == null)
				{
					return 0f;
				}
				return Mathf.Min(0.05f * (float)this.pawn.relations.OpinionOf(mostDislikedNonPartnerBedOwner) - 5f, 0f);
			}
		}
	}
}
