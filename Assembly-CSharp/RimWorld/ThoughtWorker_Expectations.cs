using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000EAB RID: 3755
	public class ThoughtWorker_Expectations : ThoughtWorker
	{
		// Token: 0x060053A5 RID: 21413 RVA: 0x001C15E0 File Offset: 0x001BF7E0
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			ExpectationDef expectationDef = ExpectationsUtility.CurrentExpectationFor(p);
			if (expectationDef == null)
			{
				return ThoughtState.Inactive;
			}
			return ThoughtState.ActiveAtStage(expectationDef.thoughtStage);
		}
	}
}
