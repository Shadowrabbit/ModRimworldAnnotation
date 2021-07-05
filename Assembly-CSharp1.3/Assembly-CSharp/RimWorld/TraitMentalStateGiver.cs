using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000AE1 RID: 2785
	public class TraitMentalStateGiver
	{
		// Token: 0x060041A5 RID: 16805 RVA: 0x001601BC File Offset: 0x0015E3BC
		public virtual bool CheckGive(Pawn pawn, int checkInterval)
		{
			if (this.traitDegreeData.forcedMentalState != null)
			{
				float forcedMentalStateMtbDays = this.traitDegreeData.forcedMentalStateMtbDays;
				if (forcedMentalStateMtbDays > 0f && Rand.MTBEventOccurs(forcedMentalStateMtbDays, 60000f, (float)checkInterval) && this.traitDegreeData.forcedMentalState.Worker.StateCanOccur(pawn))
				{
					return pawn.mindState.mentalStateHandler.TryStartMentalState(this.traitDegreeData.forcedMentalState, "MentalStateReason_Trait".Translate(this.traitDegreeData.label), false, false, null, false, false, false);
				}
			}
			if (this.traitDegreeData.randomMentalState == null)
			{
				return false;
			}
			float curMood = pawn.mindState.mentalBreaker.CurMood;
			return Rand.MTBEventOccurs(this.traitDegreeData.randomMentalStateMtbDaysMoodCurve.Evaluate(curMood), 60000f, (float)checkInterval) && this.traitDegreeData.randomMentalState.Worker.StateCanOccur(pawn) && pawn.mindState.mentalStateHandler.TryStartMentalState(this.traitDegreeData.randomMentalState, "MentalStateReason_Trait".Translate(this.traitDegreeData.GetLabelFor(pawn)), false, false, null, false, false, false);
		}

		// Token: 0x040027D6 RID: 10198
		public TraitDegreeData traitDegreeData;
	}
}
