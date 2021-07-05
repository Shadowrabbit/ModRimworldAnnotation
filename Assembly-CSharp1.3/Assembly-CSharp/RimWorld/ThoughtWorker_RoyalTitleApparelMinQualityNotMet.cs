using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020009C3 RID: 2499
	public class ThoughtWorker_RoyalTitleApparelMinQualityNotMet : ThoughtWorker
	{
		// Token: 0x06003E1B RID: 15899 RVA: 0x0015464C File Offset: 0x0015284C
		private RoyalTitle Validate(Pawn p, out QualityCategory minQuality)
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

		// Token: 0x06003E1C RID: 15900 RVA: 0x001546F8 File Offset: 0x001528F8
		public override string PostProcessLabel(Pawn p, string label)
		{
			QualityCategory qualityCategory;
			RoyalTitle royalTitle = this.Validate(p, out qualityCategory);
			if (royalTitle == null)
			{
				return string.Empty;
			}
			return label.Formatted(royalTitle.Named("TITLE"), p.Named("PAWN"));
		}

		// Token: 0x06003E1D RID: 15901 RVA: 0x0015473C File Offset: 0x0015293C
		public override string PostProcessDescription(Pawn p, string description)
		{
			QualityCategory cat;
			RoyalTitle royalTitle = this.Validate(p, out cat);
			if (royalTitle == null)
			{
				return string.Empty;
			}
			return description.Formatted(royalTitle.Named("TITLE"), cat.GetLabel().Named("QUALITY"), p.Named("PAWN")).CapitalizeFirst();
		}

		// Token: 0x06003E1E RID: 15902 RVA: 0x00154798 File Offset: 0x00152998
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			if (p.royalty == null)
			{
				return false;
			}
			QualityCategory qualityCategory;
			if (this.Validate(p, out qualityCategory) == null)
			{
				return ThoughtState.Inactive;
			}
			return ThoughtState.ActiveAtStage(0);
		}
	}
}
