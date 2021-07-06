using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020013C3 RID: 5059
	public class Recipe_ChangeImplantLevel : Recipe_Surgery
	{
		// Token: 0x06006DB3 RID: 28083 RVA: 0x00219A70 File Offset: 0x00217C70
		private bool Operable(Hediff target, RecipeDef recipe)
		{
			int hediffLevelOffset = recipe.hediffLevelOffset;
			if (hediffLevelOffset == 0)
			{
				return false;
			}
			Hediff_ImplantWithLevel hediff_ImplantWithLevel = target as Hediff_ImplantWithLevel;
			if (hediff_ImplantWithLevel == null)
			{
				return false;
			}
			int level = hediff_ImplantWithLevel.level;
			if (hediff_ImplantWithLevel.def != recipe.changesHediffLevel)
			{
				return false;
			}
			if (hediffLevelOffset <= 0)
			{
				return level > 0;
			}
			return (float)level < hediff_ImplantWithLevel.def.maxSeverity;
		}

		// Token: 0x06006DB4 RID: 28084 RVA: 0x00219AC4 File Offset: 0x00217CC4
		public override IEnumerable<BodyPartRecord> GetPartsToApplyOn(Pawn pawn, RecipeDef recipe)
		{
			return MedicalRecipesUtility.GetFixedPartsToApplyOn(recipe, pawn, (BodyPartRecord record) => pawn.health.hediffSet.hediffs.Any((Hediff x) => x.Part == record && this.Operable(x, recipe)));
		}

		// Token: 0x06006DB5 RID: 28085 RVA: 0x00219B0C File Offset: 0x00217D0C
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
			Hediff_ImplantWithLevel hediff_ImplantWithLevel = (Hediff_ImplantWithLevel)pawn.health.hediffSet.hediffs.FirstOrDefault((Hediff h) => this.Operable(h, this.recipe) && h.Part == part);
			if (hediff_ImplantWithLevel != null)
			{
				if (this.IsViolationOnPawn(pawn, part, Faction.OfPlayer))
				{
					base.ReportViolation(pawn, billDoer, pawn.FactionOrExtraMiniOrHomeFaction, -70, "GoodwillChangedReason_DowngradedImplant".Translate(hediff_ImplantWithLevel.Label));
				}
				hediff_ImplantWithLevel.ChangeLevel(this.recipe.hediffLevelOffset);
			}
		}
	}
}
