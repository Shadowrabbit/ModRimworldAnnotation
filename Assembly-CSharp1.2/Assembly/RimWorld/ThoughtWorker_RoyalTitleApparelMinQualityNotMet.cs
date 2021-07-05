using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000ECF RID: 3791
	public class ThoughtWorker_RoyalTitleApparelMinQualityNotMet : ThoughtWorker
	{
		// Token: 0x06005406 RID: 21510 RVA: 0x0003A639 File Offset: 0x00038839
		[Obsolete("Will be removed in the future")]
		private static RoyalTitleDef Validate(Pawn p, out QualityCategory minQuality)
		{
			minQuality = QualityCategory.Awful;
			return null;
		}

		// Token: 0x06005407 RID: 21511 RVA: 0x001C2928 File Offset: 0x001C0B28
		private RoyalTitle Validate_NewTemp(Pawn p, out QualityCategory minQuality)
		{
			minQuality = QualityCategory.Awful;
			foreach (RoyalTitle royalTitle in p.royalty.AllTitlesInEffectForReading)
			{
				List<Apparel> wornApparel = p.apparel.WornApparel;
				minQuality = royalTitle.def.requiredMinimumApparelQuality;
				for (int i = 0; i < wornApparel.Count; i++)
				{
					QualityCategory qualityCategory;
					if (wornApparel[i].TryGetQuality(out qualityCategory) && qualityCategory < royalTitle.def.requiredMinimumApparelQuality)
					{
						return royalTitle;
					}
				}
			}
			return null;
		}

		// Token: 0x06005408 RID: 21512 RVA: 0x001C29D4 File Offset: 0x001C0BD4
		public override string PostProcessLabel(Pawn p, string label)
		{
			QualityCategory qualityCategory;
			RoyalTitle royalTitle = this.Validate_NewTemp(p, out qualityCategory);
			if (royalTitle == null)
			{
				return string.Empty;
			}
			return label.Formatted(royalTitle.Named("TITLE"), p.Named("PAWN"));
		}

		// Token: 0x06005409 RID: 21513 RVA: 0x001C2A18 File Offset: 0x001C0C18
		public override string PostProcessDescription(Pawn p, string description)
		{
			QualityCategory cat;
			RoyalTitle royalTitle = this.Validate_NewTemp(p, out cat);
			if (royalTitle == null)
			{
				return string.Empty;
			}
			return description.Formatted(royalTitle.Named("TITLE"), cat.GetLabel().Named("QUALITY"), p.Named("PAWN")).CapitalizeFirst();
		}

		// Token: 0x0600540A RID: 21514 RVA: 0x001C2A74 File Offset: 0x001C0C74
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			if (p.royalty == null)
			{
				return false;
			}
			QualityCategory qualityCategory;
			if (this.Validate_NewTemp(p, out qualityCategory) == null)
			{
				return ThoughtState.Inactive;
			}
			return ThoughtState.ActiveAtStage(0);
		}
	}
}
