using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000D97 RID: 3479
	public class Recipe_ChangeImplantLevel : Recipe_Surgery
	{
		// Token: 0x060050B8 RID: 20664 RVA: 0x001B0348 File Offset: 0x001AE548
		private bool Operable(Hediff target, RecipeDef recipe)
		{
			int hediffLevelOffset = recipe.hediffLevelOffset;
			if (hediffLevelOffset == 0)
			{
				return false;
			}
			Hediff_Level hediff_Level = target as Hediff_Level;
			if (hediff_Level == null)
			{
				return false;
			}
			int level = hediff_Level.level;
			if (hediff_Level.def != recipe.changesHediffLevel)
			{
				return false;
			}
			if (hediffLevelOffset <= 0)
			{
				return level > 0;
			}
			return (float)level < hediff_Level.def.maxSeverity;
		}

		// Token: 0x060050B9 RID: 20665 RVA: 0x001B039C File Offset: 0x001AE59C
		public override IEnumerable<BodyPartRecord> GetPartsToApplyOn(Pawn pawn, RecipeDef recipe)
		{
			return MedicalRecipesUtility.GetFixedPartsToApplyOn(recipe, pawn, (BodyPartRecord record) => pawn.health.hediffSet.hediffs.Any((Hediff x) => x.Part == record && this.Operable(x, recipe)));
		}

		// Token: 0x060050BA RID: 20666 RVA: 0x001B03E4 File Offset: 0x001AE5E4
		public override void ApplyOnPawn(Pawn pawn, BodyPartRecord part, Pawn billDoer, List<Thing> ingredients, Bill bill)
		{
			if (billDoer != null)
			{
				TaleRecorder.RecordTale(TaleDefOf.DidSurgery, new object[]
				{
					billDoer,
					pawn
				});
			}
			Hediff_Level hediff_Level = (Hediff_Level)pawn.health.hediffSet.hediffs.FirstOrDefault((Hediff h) => this.Operable(h, this.recipe) && h.Part == part);
			if (hediff_Level != null)
			{
				if (this.IsViolationOnPawn(pawn, part, Faction.OfPlayer))
				{
					base.ReportViolation(pawn, billDoer, pawn.HomeFaction, -70);
				}
				hediff_Level.ChangeLevel(this.recipe.hediffLevelOffset);
			}
		}
	}
}
