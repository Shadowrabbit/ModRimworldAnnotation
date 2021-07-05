using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020009BB RID: 2491
	public class ThoughtWorker_HasAddedBodyPart : ThoughtWorker
	{
		// Token: 0x06003E06 RID: 15878 RVA: 0x00154074 File Offset: 0x00152274
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
