using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020009C1 RID: 2497
	public class ThoughtWorker_PassionateWork : ThoughtWorker
	{
		// Token: 0x06003E13 RID: 15891 RVA: 0x0015442C File Offset: 0x0015262C
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
