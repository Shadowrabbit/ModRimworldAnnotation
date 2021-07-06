using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000ECB RID: 3787
	public class ThoughtWorker_PassionateWork : ThoughtWorker
	{
		// Token: 0x060053E9 RID: 21481 RVA: 0x001C21D8 File Offset: 0x001C03D8
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			JobDriver curDriver = p.jobs.curDriver;
			if (curDriver == null)
			{
				return ThoughtState.Inactive;
			}
			if (p.skills == null)
			{
				return ThoughtState.Inactive;
			}
			if (curDriver.ActiveSkill == null)
			{
				return ThoughtState.Inactive;
			}
			SkillRecord skill = p.skills.GetSkill(curDriver.ActiveSkill);
			if (skill == null)
			{
				return ThoughtState.Inactive;
			}
			if (skill.passion == Passion.Minor)
			{
				return ThoughtState.ActiveAtStage(0);
			}
			if (skill.passion == Passion.Major)
			{
				return ThoughtState.ActiveAtStage(1);
			}
			return ThoughtState.Inactive;
		}
	}
}
