using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020015E1 RID: 5601
	public class ScenPart_Naked : ScenPart_PawnModifier
	{
		// Token: 0x060079BC RID: 31164 RVA: 0x0024DA58 File Offset: 0x0024BC58
		public override string Summary(Scenario scen)
		{
			return "ScenPart_PawnsAreNaked".Translate(this.context.ToStringHuman()).CapitalizeFirst();
		}

		// Token: 0x060079BD RID: 31165 RVA: 0x00051EDB File Offset: 0x000500DB
		protected override void ModifyPawnPostGenerate(Pawn pawn, bool redressed)
		{
			if (pawn.apparel != null)
			{
				pawn.apparel.DestroyAll(DestroyMode.Vanish);
			}
		}

		// Token: 0x060079BE RID: 31166 RVA: 0x0024DA8C File Offset: 0x0024BC8C
		public override void DoEditInterface(Listing_ScenEdit listing)
		{
			Rect scenPartRect = listing.GetScenPartRect(this, ScenPart.RowHeight * 2f);
			base.DoPawnModifierEditInterface(scenPartRect.BottomPartPixels(ScenPart.RowHeight * 2f));
		}
	}
}
