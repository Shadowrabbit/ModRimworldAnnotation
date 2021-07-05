using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x020000B1 RID: 177
	public class RecipeWorker
	{
		// Token: 0x06000580 RID: 1408 RVA: 0x000126F5 File Offset: 0x000108F5
		public virtual bool AvailableOnNow(Thing thing, BodyPartRecord part = null)
		{
			return true;
		}

		// Token: 0x06000581 RID: 1409 RVA: 0x0001C706 File Offset: 0x0001A906
		public virtual IEnumerable<BodyPartRecord> GetPartsToApplyOn(Pawn pawn, RecipeDef recipe)
		{
			yield break;
		}

		// Token: 0x06000582 RID: 1410 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void ApplyOnPawn(Pawn pawn, BodyPartRecord part, Pawn billDoer, List<Thing> ingredients, Bill bill)
		{
		}

		// Token: 0x06000583 RID: 1411 RVA: 0x0001C70F File Offset: 0x0001A90F
		public virtual bool IsViolationOnPawn(Pawn pawn, BodyPartRecord part, Faction billDoerFaction)
		{
			return (pawn.Faction != billDoerFaction || pawn.IsQuestLodger()) && this.recipe.isViolation;
		}

		// Token: 0x06000584 RID: 1412 RVA: 0x0001C72F File Offset: 0x0001A92F
		public virtual string GetLabelWhenUsedOn(Pawn pawn, BodyPartRecord part)
		{
			return this.recipe.label;
		}

		// Token: 0x06000585 RID: 1413 RVA: 0x0001C73C File Offset: 0x0001A93C
		public virtual void ConsumeIngredient(Thing ingredient, RecipeDef recipe, Map map)
		{
			ingredient.Destroy(DestroyMode.Vanish);
		}

		// Token: 0x06000586 RID: 1414 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void Notify_IterationCompleted(Pawn billDoer, List<Thing> ingredients)
		{
		}

		// Token: 0x06000587 RID: 1415 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void CheckForWarnings(Pawn billDoer)
		{
		}

		// Token: 0x06000588 RID: 1416 RVA: 0x0001C748 File Offset: 0x0001A948
		protected void ReportViolation(Pawn pawn, Pawn billDoer, Faction factionToInform, int goodwillImpact)
		{
			if (factionToInform != null && billDoer != null && billDoer.Faction == Faction.OfPlayer)
			{
				Faction.OfPlayer.TryAffectGoodwillWith(factionToInform, goodwillImpact, true, !factionToInform.temporary, HistoryEventDefOf.PerformedHarmfulSurgery, null);
				QuestUtility.SendQuestTargetSignals(pawn.questTags, "SurgeryViolation", pawn.Named("SUBJECT"));
			}
		}

		// Token: 0x04000362 RID: 866
		public RecipeDef recipe;
	}
}
