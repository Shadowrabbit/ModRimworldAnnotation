using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E7E RID: 3710
	public class ThoughtWorker_NeedRest : ThoughtWorker
	{
		// Token: 0x06005345 RID: 21317 RVA: 0x001C02C0 File Offset: 0x001BE4C0
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			if (p.needs.rest == null)
			{
				return ThoughtState.Inactive;
			}
			switch (p.needs.rest.CurCategory)
			{
			case RestCategory.Rested:
				return ThoughtState.Inactive;
			case RestCategory.Tired:
				return ThoughtState.ActiveAtStage(0);
			case RestCategory.VeryTired:
				return ThoughtState.ActiveAtStage(1);
			case RestCategory.Exhausted:
				return ThoughtState.ActiveAtStage(2);
			default:
				throw new NotImplementedException();
			}
		}
	}
}
