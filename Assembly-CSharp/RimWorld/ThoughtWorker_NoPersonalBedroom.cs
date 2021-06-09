using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000ED4 RID: 3796
	public class ThoughtWorker_NoPersonalBedroom : ThoughtWorker
	{
		// Token: 0x06005418 RID: 21528 RVA: 0x001C2B48 File Offset: 0x001C0D48
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			if (p.royalty == null || p.MapHeld == null || !p.MapHeld.IsPlayerHome || p.royalty.HighestTitleWithBedroomRequirements() == null)
			{
				return false;
			}
			return !p.royalty.HasPersonalBedroom();
		}

		// Token: 0x06005419 RID: 21529 RVA: 0x001C2B9C File Offset: 0x001C0D9C
		public override string PostProcessDescription(Pawn p, string description)
		{
			return description.Formatted(p.royalty.HighestTitleWithBedroomRequirements().Named("TITLE")).CapitalizeFirst();
		}
	}
}
