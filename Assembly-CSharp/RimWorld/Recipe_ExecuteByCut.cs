using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020013D0 RID: 5072
	public class Recipe_ExecuteByCut : RecipeWorker
	{
		// Token: 0x06006DD3 RID: 28115 RVA: 0x0021A158 File Offset: 0x00218358
		public override void ApplyOnPawn(Pawn pawn, BodyPartRecord part, Pawn billDoer, List<Thing> ingredients, Bill bill)
		{
			if (this.IsViolationOnPawn(pawn, part, Faction.OfPlayer))
			{
				base.ReportViolation(pawn, billDoer, pawn.FactionOrExtraMiniOrHomeFaction, -100, "GoodwillChangedReason_EuthanizedPawn".Translate(pawn.Named("PAWN")));
			}
			ExecutionUtility.DoExecutionByCut(billDoer, pawn);
			ThoughtUtility.GiveThoughtsForPawnExecuted(pawn, PawnExecutionKind.GenericHumane);
		}
	}
}
