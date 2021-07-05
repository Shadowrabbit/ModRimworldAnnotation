using System;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F65 RID: 3941
	public class RitualOutcomeComp_NumParticipantsWithTag : RitualOutcomeComp_Quality
	{
		// Token: 0x1700102B RID: 4139
		// (get) Token: 0x06005D78 RID: 23928 RVA: 0x0001276E File Offset: 0x0001096E
		public override bool DataRequired
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06005D79 RID: 23929 RVA: 0x00200D04 File Offset: 0x001FEF04
		public override float Count(LordJob_Ritual ritual, RitualOutcomeComp_Data data)
		{
			int num = 0;
			foreach (Pawn p in ritual.assignments.Participants)
			{
				if (ritual.PawnTagSet(p, this.tag))
				{
					num++;
				}
			}
			return (float)num;
		}

		// Token: 0x06005D7A RID: 23930 RVA: 0x00200D6C File Offset: 0x001FEF6C
		public override ExpectedOutcomeDesc GetExpectedOutcomeDesc(Precept_Ritual ritual, TargetInfo ritualTarget, RitualObligation obligation, RitualRoleAssignments assignments)
		{
			int num = assignments.Participants.Count((Pawn p) => p.RaceProps.Humanlike);
			float quality = this.curve.Evaluate((float)num);
			return new ExpectedOutcomeDesc
			{
				label = "RitualPredictedOutcomeDescParticipantCount".Translate(),
				count = num + " / " + Mathf.Max(base.MaxValue, (float)num),
				effect = this.ExpectedOffsetDesc(num > 0, quality),
				quality = quality,
				positive = (num > 0),
				priority = 4f
			};
		}

		// Token: 0x04003604 RID: 13828
		public string tag;
	}
}
