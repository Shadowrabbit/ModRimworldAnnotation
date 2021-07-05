using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000D96 RID: 3478
	public class Recipe_AdministerUsableItem : Recipe_Surgery
	{
		// Token: 0x060050B5 RID: 20661 RVA: 0x001B0331 File Offset: 0x001AE531
		public override void ApplyOnPawn(Pawn pawn, BodyPartRecord part, Pawn billDoer, List<Thing> ingredients, Bill bill)
		{
			ingredients[0].TryGetComp<CompUsable>().UsedBy(pawn);
		}

		// Token: 0x060050B6 RID: 20662 RVA: 0x0000313F File Offset: 0x0000133F
		public override void ConsumeIngredient(Thing ingredient, RecipeDef recipe, Map map)
		{
		}
	}
}
