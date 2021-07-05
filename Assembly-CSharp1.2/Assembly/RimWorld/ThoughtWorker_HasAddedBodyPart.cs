using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000EC5 RID: 3781
	public class ThoughtWorker_HasAddedBodyPart : ThoughtWorker
	{
		// Token: 0x060053DC RID: 21468 RVA: 0x001C1E4C File Offset: 0x001C004C
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			int num = p.health.hediffSet.CountAddedAndImplantedParts();
			if (num > 0)
			{
				return ThoughtState.ActiveAtStage(num - 1);
			}
			return false;
		}
	}
}
