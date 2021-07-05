using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020009E3 RID: 2531
	public class ThoughtWorker_NeedIndoors : ThoughtWorker
	{
		// Token: 0x06003E74 RID: 15988 RVA: 0x001555D0 File Offset: 0x001537D0
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			if (p.needs.indoors == null)
			{
				return ThoughtState.Inactive;
			}
			if (p.HostFaction != null)
			{
				return ThoughtState.Inactive;
			}
			switch (p.needs.indoors.CurCategory)
			{
			case IndoorsCategory.ComfortablyIndoors:
				return ThoughtState.Inactive;
			case IndoorsCategory.JustOutdoors:
				return ThoughtState.ActiveAtStage(0);
			case IndoorsCategory.Outdoors:
				return ThoughtState.ActiveAtStage(1);
			case IndoorsCategory.LongOutdoors:
				return ThoughtState.ActiveAtStage(2);
			case IndoorsCategory.VeryLongOutdoors:
				return ThoughtState.ActiveAtStage(3);
			case IndoorsCategory.BrutalOutdoors:
				return ThoughtState.ActiveAtStage(4);
			default:
				throw new InvalidOperationException("Unknown IndoorsCategory");
			}
		}
	}
}
