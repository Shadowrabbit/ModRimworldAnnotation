using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020018D6 RID: 6358
	public class CompUseEffect_FinishRandomResearchProject : CompUseEffect
	{
		// Token: 0x06008CDB RID: 36059 RVA: 0x0028DB9C File Offset: 0x0028BD9C
		public override void DoEffect(Pawn usedBy)
		{
			base.DoEffect(usedBy);
			ResearchProjectDef currentProj = Find.ResearchManager.currentProj;
			if (currentProj != null)
			{
				this.FinishInstantly(currentProj, usedBy);
			}
		}

		// Token: 0x06008CDC RID: 36060 RVA: 0x0005E6CB File Offset: 0x0005C8CB
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

		// Token: 0x06008CDD RID: 36061 RVA: 0x0005E6F0 File Offset: 0x0005C8F0
		private void FinishInstantly(ResearchProjectDef proj, Pawn usedBy)
		{
			Find.ResearchManager.FinishProject(proj, false, null);
			Messages.Message("MessageResearchProjectFinishedByItem".Translate(proj.LabelCap), usedBy, MessageTypeDefOf.PositiveEvent, true);
		}
	}
}
