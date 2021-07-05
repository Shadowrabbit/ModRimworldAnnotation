using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001158 RID: 4440
	public class FocusStrengthOffset_Lit : FocusStrengthOffset
	{
		// Token: 0x06006AB3 RID: 27315 RVA: 0x0023D70C File Offset: 0x0023B90C
		public override string GetExplanation(Thing parent)
		{
			if (this.CanApply(parent, null))
			{
				return "StatsReport_Lit".Translate() + ": " + this.GetOffset(parent, null).ToStringWithSign("0%");
			}
			return this.GetExplanationAbstract(null);
		}

		// Token: 0x06006AB4 RID: 27316 RVA: 0x0023D75B File Offset: 0x0023B95B
		public override string GetExplanationAbstract(ThingDef def = null)
		{
			return "StatsReport_Lit".Translate() + ": " + this.offset.ToStringWithSign("0%");
		}

		// Token: 0x06006AB5 RID: 27317 RVA: 0x0023D32B File Offset: 0x0023B52B
		public override float GetOffset(Thing parent, Pawn user = null)
		{
			return this.offset;
		}

		// Token: 0x06006AB6 RID: 27318 RVA: 0x0023D78C File Offset: 0x0023B98C
		public override bool CanApply(Thing parent, Pawn user = null)
		{
			CompGlower compGlower = parent.TryGetComp<CompGlower>();
			return compGlower != null && compGlower.Glows;
		}
	}
}
