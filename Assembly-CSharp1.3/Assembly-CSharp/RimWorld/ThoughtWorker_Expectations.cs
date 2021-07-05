using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020009A0 RID: 2464
	public class ThoughtWorker_Expectations : ThoughtWorker
	{
		// Token: 0x06003DC9 RID: 15817 RVA: 0x001535A0 File Offset: 0x001517A0
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			if (p.IsSlave)
			{
				return ThoughtState.Inactive;
			}
			ExpectationDef expectationDef = ExpectationsUtility.CurrentExpectationFor(p);
			if (expectationDef == null)
			{
				return ThoughtState.Inactive;
			}
			return ThoughtState.ActiveAtStage(expectationDef.thoughtStage);
		}

		// Token: 0x06003DCA RID: 15818 RVA: 0x001535D8 File Offset: 0x001517D8
		public override string PostProcessDescription(Pawn p, string description)
		{
			string text = base.PostProcessDescription(p, description);
			if (ModsConfig.IdeologyActive)
			{
				Ideo ideo = p.Ideo;
				Precept_Role precept_Role = (ideo != null) ? ideo.GetRole(p) : null;
				if (precept_Role != null && ExpectationsUtility.OffsetByRole(p))
				{
					return text + "\n\n" + "RoleRaisedExpectation".Translate(precept_Role.LabelCap);
				}
			}
			return text;
		}
	}
}
