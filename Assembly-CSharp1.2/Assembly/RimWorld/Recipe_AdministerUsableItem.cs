using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020013C2 RID: 5058
	public class Recipe_AdministerUsableItem : Recipe_Surgery
	{
		// Token: 0x06006DB0 RID: 28080 RVA: 0x0004A866 File Offset: 0x00048A66
		public override void ApplyOnPawn(Pawn pawn, BodyPartRecord part, Pawn billDoer, List<Thing> ingredients, Bill bill)
		{
			ingredients[0].TryGetComp<CompUsable>().UsedBy(pawn);
		}

		// Token: 0x06006DB1 RID: 28081 RVA: 0x00006A05 File Offset: 0x00004C05
		public override void ConsumeIngredient(Thing ingredient, RecipeDef recipe, Map map)
		{
		}
	}
}
