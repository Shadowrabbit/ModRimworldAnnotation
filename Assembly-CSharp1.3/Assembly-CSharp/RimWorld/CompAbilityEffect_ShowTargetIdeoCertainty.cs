using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000D5D RID: 3421
	public class CompAbilityEffect_ShowTargetIdeoCertainty : CompAbilityEffect
	{
		// Token: 0x17000DC4 RID: 3524
		// (get) Token: 0x06004F99 RID: 20377 RVA: 0x000126F5 File Offset: 0x000108F5
		public override bool HideTargetPawnTooltip
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06004F9A RID: 20378 RVA: 0x001AA844 File Offset: 0x001A8A44
		public override string ExtraLabelMouseAttachment(LocalTargetInfo target)
		{
			if (target.Pawn != null)
			{
				Pawn pawn = target.Pawn;
				if (pawn.Ideo != null)
				{
					return "IdeoCertaintyTooltip".Translate(pawn.Named("PAWN"), pawn.Ideo.Named("IDEO"), pawn.ideo.Certainty.ToStringPercent().Named("PERCENTAGE"));
				}
			}
			return null;
		}
	}
}
