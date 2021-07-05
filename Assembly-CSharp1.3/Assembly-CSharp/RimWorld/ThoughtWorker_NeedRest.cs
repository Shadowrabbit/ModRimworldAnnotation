using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020009DD RID: 2525
	public class ThoughtWorker_NeedRest : ThoughtWorker
	{
		// Token: 0x06003E68 RID: 15976 RVA: 0x00155298 File Offset: 0x00153498
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
