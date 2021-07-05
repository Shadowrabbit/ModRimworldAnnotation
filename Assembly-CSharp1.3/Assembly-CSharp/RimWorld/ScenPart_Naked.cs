using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000FFE RID: 4094
	public class ScenPart_Naked : ScenPart_PawnModifier
	{
		// Token: 0x0600606F RID: 24687 RVA: 0x0020DB84 File Offset: 0x0020BD84
		public override string Summary(Scenario scen)
		{
			return "ScenPart_PawnsAreNaked".Translate(this.context.ToStringHuman(), this.chance.ToStringPercent()).CapitalizeFirst();
		}

		// Token: 0x06006070 RID: 24688 RVA: 0x0020DBC8 File Offset: 0x0020BDC8
		protected override void ModifyPawnPostGenerate(Pawn pawn, bool redressed)
		{
			if (pawn.apparel != null)
			{
				pawn.apparel.DestroyAll(DestroyMode.Vanish);
			}
		}

		// Token: 0x06006071 RID: 24689 RVA: 0x0020DBE0 File Offset: 0x0020BDE0
		public override void DoEditInterface(Listing_ScenEdit listing)
		{
			Rect scenPartRect = listing.GetScenPartRect(this, ScenPart.RowHeight * 2f);
			base.DoPawnModifierEditInterface(scenPartRect.BottomPartPixels(ScenPart.RowHeight * 2f));
		}
	}
}
