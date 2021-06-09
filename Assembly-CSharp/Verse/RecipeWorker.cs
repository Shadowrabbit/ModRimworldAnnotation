using System;
using System.Collections.Generic;
using RimWorld;
using RimWorld.Planet;

namespace Verse
{
	// Token: 0x02000119 RID: 281
	public class RecipeWorker
	{
		// Token: 0x060007AE RID: 1966 RVA: 0x0000A2A7 File Offset: 0x000084A7
		public virtual bool AvailableOnNow(Thing thing)
		{
			return true;
		}

		// Token: 0x060007AF RID: 1967 RVA: 0x0000C294 File Offset: 0x0000A494
		public virtual IEnumerable<BodyPartRecord> GetPartsToApplyOn(Pawn pawn, RecipeDef recipe)
		{
			yield break;
		}

		// Token: 0x060007B0 RID: 1968 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void ApplyOnPawn(Pawn pawn, BodyPartRecord part, Pawn billDoer, List<Thing> ingredients, Bill bill)
		{
		}

		// Token: 0x060007B1 RID: 1969 RVA: 0x0000C29D File Offset: 0x0000A49D
		public virtual bool IsViolationOnPawn(Pawn pawn, BodyPartRecord part, Faction billDoerFaction)
		{
			return (pawn.Faction != billDoerFaction || pawn.IsQuestLodger()) && this.recipe.isViolation;
		}

		// Token: 0x060007B2 RID: 1970 RVA: 0x0000C2BD File Offset: 0x0000A4BD
		public virtual string GetLabelWhenUsedOn(Pawn pawn, BodyPartRecord part)
		{
			return this.recipe.label;
		}

		// Token: 0x060007B3 RID: 1971 RVA: 0x0000C2CA File Offset: 0x0000A4CA
		public virtual void ConsumeIngredient(Thing ingredient, RecipeDef recipe, Map map)
		{
			ingredient.Destroy(DestroyMode.Vanish);
		}

		// Token: 0x060007B4 RID: 1972 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void Notify_IterationCompleted(Pawn billDoer, List<Thing> ingredients)
		{
		}

		// Token: 0x060007B5 RID: 1973 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void CheckForWarnings(Pawn billDoer)
		{
		}

		// Token: 0x060007B6 RID: 1974 RVA: 0x000937F4 File Offset: 0x000919F4
		protected void ReportViolation(Pawn pawn, Pawn billDoer, Faction factionToInform, int goodwillImpact, string reason)
		{
			if (factionToInform != null && billDoer != null && billDoer.Faction != null)
			{
				Faction faction = billDoer.Faction;
				bool canSendMessage = true;
				GlobalTargetInfo? lookTarget = new GlobalTargetInfo?(pawn);
				factionToInform.TryAffectGoodwillWith(faction, goodwillImpact, canSendMessage, !factionToInform.temporary, reason, lookTarget);
				QuestUtility.SendQuestTargetSignals(pawn.questTags, "SurgeryViolation", pawn.Named("SUBJECT"));
			}
		}

		// Token: 0x0400054A RID: 1354
		public RecipeDef recipe;
	}
}
