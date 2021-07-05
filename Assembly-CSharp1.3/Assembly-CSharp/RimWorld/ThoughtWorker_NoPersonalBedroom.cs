using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020009C8 RID: 2504
	public class ThoughtWorker_NoPersonalBedroom : ThoughtWorker
	{
		// Token: 0x06003E2C RID: 15916 RVA: 0x00154910 File Offset: 0x00152B10
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			if (p.royalty == null || p.MapHeld == null || !p.MapHeld.IsPlayerHome || p.royalty.HighestTitleWithBedroomRequirements() == null)
			{
				return false;
			}
			return !p.royalty.HasPersonalBedroom();
		}

		// Token: 0x06003E2D RID: 15917 RVA: 0x00154964 File Offset: 0x00152B64
		public override string PostProcessDescription(Pawn p, string description)
		{
			return description.Formatted(p.royalty.HighestTitleWithBedroomRequirements().Named("TITLE")).CapitalizeFirst();
		}
	}
}
