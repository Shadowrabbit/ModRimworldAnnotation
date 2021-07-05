using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F1F RID: 3871
	public class RitualStage_AnimaTreeLinking : RitualStage
	{
		// Token: 0x06005C17 RID: 23575 RVA: 0x001FCA38 File Offset: 0x001FAC38
		public override TargetInfo GetSecondFocus(LordJob_Ritual ritual)
		{
			return ritual.selectedTarget;
		}

		// Token: 0x06005C18 RID: 23576 RVA: 0x001FCA40 File Offset: 0x001FAC40
		public override float ProgressPerTick(LordJob_Ritual ritual)
		{
			int num = 1;
			foreach (Pawn p in ritual.assignments.SpectatorsForReading)
			{
				if (ritual.IsParticipating(p))
				{
					num++;
				}
			}
			return RitualStage_AnimaTreeLinking.ProgressPerParticipantCurve.Evaluate((float)num);
		}

		// Token: 0x040035A9 RID: 13737
		public static readonly SimpleCurve ProgressPerParticipantCurve = new SimpleCurve
		{
			{
				new CurvePoint(1f, 1f),
				true
			},
			{
				new CurvePoint(2f, 1.2f),
				true
			},
			{
				new CurvePoint(4f, 1.5f),
				true
			},
			{
				new CurvePoint(6f, 2f),
				true
			},
			{
				new CurvePoint(8f, 3f),
				true
			}
		};
	}
}
