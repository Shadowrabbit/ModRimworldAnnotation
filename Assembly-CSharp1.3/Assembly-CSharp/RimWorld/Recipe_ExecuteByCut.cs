using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000D9B RID: 3483
	public class Recipe_ExecuteByCut : RecipeWorker
	{
		// Token: 0x060050C6 RID: 20678 RVA: 0x001B0777 File Offset: 0x001AE977
		public override void ApplyOnPawn(Pawn pawn, BodyPartRecord part, Pawn billDoer, List<Thing> ingredients, Bill bill)
		{
			if (this.IsViolationOnPawn(pawn, part, Faction.OfPlayer))
			{
				base.ReportViolation(pawn, billDoer, pawn.HomeFaction, -100);
			}
			ExecutionUtility.DoExecutionByCut(billDoer, pawn, 8, true);
			ThoughtUtility.GiveThoughtsForPawnExecuted(pawn, billDoer, PawnExecutionKind.GenericHumane);
		}
	}
}
