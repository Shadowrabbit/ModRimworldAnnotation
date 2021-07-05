using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000EB8 RID: 3768
	public class Thought_SharedBed : Thought_Situational
	{
		// Token: 0x17000CC5 RID: 3269
		// (get) Token: 0x060053C1 RID: 21441 RVA: 0x001C1A38 File Offset: 0x001BFC38
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
