using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000EBA RID: 3770
	public class ThoughtWorker_Pain : ThoughtWorker
	{
		// Token: 0x060053C5 RID: 21445 RVA: 0x001C1B00 File Offset: 0x001BFD00
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			float painTotal = p.health.hediffSet.PainTotal;
			if (painTotal < 0.0001f)
			{
				return ThoughtState.Inactive;
			}
			if (painTotal < 0.15f)
			{
				return ThoughtState.ActiveAtStage(0);
			}
			if (painTotal < 0.4f)
			{
				return ThoughtState.ActiveAtStage(1);
			}
			if (painTotal < 0.8f)
			{
				return ThoughtState.ActiveAtStage(2);
			}
			return ThoughtState.ActiveAtStage(3);
		}
	}
}
