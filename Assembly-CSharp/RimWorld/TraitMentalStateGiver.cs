using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001006 RID: 4102
	public class TraitMentalStateGiver
	{
		// Token: 0x06005985 RID: 22917 RVA: 0x001D2904 File Offset: 0x001D0B04
		public virtual bool CheckGive(Pawn pawn, int checkInterval)
		{
			if (this.traitDegreeData.randomMentalState == null)
			{
				return false;
			}
			float curMood = pawn.mindState.mentalBreaker.CurMood;
			return Rand.MTBEventOccurs(this.traitDegreeData.randomMentalStateMtbDaysMoodCurve.Evaluate(curMood), 60000f, (float)checkInterval) && this.traitDegreeData.randomMentalState.Worker.StateCanOccur(pawn) && pawn.mindState.mentalStateHandler.TryStartMentalState(this.traitDegreeData.randomMentalState, "MentalStateReason_Trait".Translate(this.traitDegreeData.GetLabelFor(pawn)), false, false, null, false);
		}

		// Token: 0x04003C29 RID: 15401
		public TraitDegreeData traitDegreeData;
	}
}
