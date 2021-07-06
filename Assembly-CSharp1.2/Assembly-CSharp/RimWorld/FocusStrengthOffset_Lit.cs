using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020017EC RID: 6124
	public class FocusStrengthOffset_Lit : FocusStrengthOffset
	{
		// Token: 0x06008790 RID: 34704 RVA: 0x0027BE50 File Offset: 0x0027A050
		public override string GetExplanation(Thing parent)
		{
			if (this.CanApply(parent, null))
			{
				return "StatsReport_Lit".Translate() + ": " + this.GetOffset(parent, null).ToStringWithSign("0%");
			}
			return this.GetExplanationAbstract(null);
		}

		// Token: 0x06008791 RID: 34705 RVA: 0x0005AF84 File Offset: 0x00059184
		public override string GetExplanationAbstract(ThingDef def = null)
		{
			return "StatsReport_Lit".Translate() + ": " + this.offset.ToStringWithSign("0%");
		}

		// Token: 0x06008792 RID: 34706 RVA: 0x0005AE5B File Offset: 0x0005905B
		public override float GetOffset(Thing parent, Pawn user = null)
		{
			return this.offset;
		}

		// Token: 0x06008793 RID: 34707 RVA: 0x0027BEA0 File Offset: 0x0027A0A0
		public override bool CanApply(Thing parent, Pawn user = null)
		{
			CompGlower compGlower = parent.TryGetComp<CompGlower>();
			return compGlower != null && compGlower.Glows;
		}
	}
}
