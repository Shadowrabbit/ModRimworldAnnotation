using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001208 RID: 4616
	public class CompUseEffect_FinishRandomResearchProject : CompUseEffect
	{
		// Token: 0x06006EE9 RID: 28393 RVA: 0x00251570 File Offset: 0x0024F770
		public override void DoEffect(Pawn usedBy)
		{
			base.DoEffect(usedBy);
			ResearchProjectDef currentProj = Find.ResearchManager.currentProj;
			if (currentProj != null)
			{
				this.FinishInstantly(currentProj, usedBy);
			}
		}

		// Token: 0x06006EEA RID: 28394 RVA: 0x0025159A File Offset: 0x0024F79A
		public override bool CanBeUsedBy(Pawn p, out string failReason)
		{
			if (Find.ResearchManager.currentProj == null)
			{
				failReason = "NoActiveResearchProjectToFinish".Translate();
				return false;
			}
			failReason = null;
			return true;
		}

		// Token: 0x06006EEB RID: 28395 RVA: 0x002515BF File Offset: 0x0024F7BF
		private void FinishInstantly(ResearchProjectDef proj, Pawn usedBy)
		{
			Find.ResearchManager.FinishProject(proj, false, null);
			Messages.Message("MessageResearchProjectFinishedByItem".Translate(proj.LabelCap), usedBy, MessageTypeDefOf.PositiveEvent, true);
		}
	}
}
