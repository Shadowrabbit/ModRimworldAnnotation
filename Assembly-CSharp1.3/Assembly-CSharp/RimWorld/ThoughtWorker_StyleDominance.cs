using System;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000974 RID: 2420
	public class ThoughtWorker_StyleDominance : ThoughtWorker_Precept
	{
		// Token: 0x06003D60 RID: 15712 RVA: 0x00151E2C File Offset: 0x0015002C
		public override string PostProcessLabel(Pawn p, string label)
		{
			return label.Formatted(p.Ideo.adjective.Named("ADJECTIVE")).CapitalizeFirst();
		}

		// Token: 0x06003D61 RID: 15713 RVA: 0x00151E64 File Offset: 0x00150064
		public override string PostProcessDescription(Pawn p, string description)
		{
			return description.Formatted(p.Ideo.adjective.Named("ADJECTIVE"), (from s in p.Ideo.thingStyleCategories
			select s.category.LabelCap.Resolve()).ToCommaList(true, false).Named("STYLES")).CapitalizeFirst();
		}

		// Token: 0x06003D62 RID: 15714 RVA: 0x00151EDC File Offset: 0x001500DC
		protected override ThoughtState ShouldHaveThought(Pawn p)
		{
			int styleDominanceThoughtIndex = p.styleObserver.StyleDominanceThoughtIndex;
			if (styleDominanceThoughtIndex >= 0)
			{
				return ThoughtState.ActiveAtStage(styleDominanceThoughtIndex);
			}
			return false;
		}
	}
}
