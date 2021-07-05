using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000DA1 RID: 3489
	public class Recipe_Surgery : RecipeWorker
	{
		// Token: 0x060050E2 RID: 20706 RVA: 0x001B0D84 File Offset: 0x001AEF84
		protected bool CheckSurgeryFail(Pawn surgeon, Pawn patient, List<Thing> ingredients, BodyPartRecord part, Bill bill)
		{
			if (bill.recipe.surgerySuccessChanceFactor >= 99999f)
			{
				return false;
			}
			float num = 1f;
			if (!patient.RaceProps.IsMechanoid)
			{
				num *= surgeon.GetStatValue(StatDefOf.MedicalSurgerySuccessChance, true);
			}
			if (!this.recipe.surgeryIgnoreEnvironment && patient.InBed())
			{
				num *= patient.CurrentBed().GetStatValue(StatDefOf.SurgerySuccessChanceFactor, true);
			}
			num *= Recipe_Surgery.MedicineMedicalPotencyToSurgeryChanceFactor.Evaluate(this.GetAverageMedicalPotency(ingredients, bill));
			num *= this.recipe.surgerySuccessChanceFactor;
			if (surgeon.InspirationDef == InspirationDefOf.Inspired_Surgery && !patient.RaceProps.IsMechanoid)
			{
				num *= 2f;
				surgeon.mindState.inspirationHandler.EndInspiration(InspirationDefOf.Inspired_Surgery);
			}
			num = Mathf.Min(num, 0.98f);
			if (!Rand.Chance(num))
			{
				if (Rand.Chance(this.recipe.deathOnFailedSurgeryChance))
				{
					HealthUtility.GiveInjuriesOperationFailureCatastrophic(patient, part);
					if (!patient.Dead)
					{
						patient.Kill(null, null);
					}
					Find.LetterStack.ReceiveLetter("LetterLabelSurgeryFailed".Translate(patient.Named("PATIENT")), "MessageMedicalOperationFailureFatal".Translate(surgeon.LabelShort, patient.LabelShort, this.recipe.LabelCap, surgeon.Named("SURGEON"), patient.Named("PATIENT")), LetterDefOf.NegativeEvent, patient, null, null, null, null);
				}
				else if (Rand.Chance(0.5f))
				{
					if (Rand.Chance(0.1f))
					{
						Find.LetterStack.ReceiveLetter("LetterLabelSurgeryFailed".Translate(patient.Named("PATIENT")), "MessageMedicalOperationFailureRidiculous".Translate(surgeon.LabelShort, patient.LabelShort, surgeon.Named("SURGEON"), patient.Named("PATIENT"), this.recipe.Named("RECIPE")), LetterDefOf.NegativeEvent, patient, null, null, null, null);
						HealthUtility.GiveInjuriesOperationFailureRidiculous(patient);
					}
					else
					{
						Find.LetterStack.ReceiveLetter("LetterLabelSurgeryFailed".Translate(patient.Named("PATIENT")), "MessageMedicalOperationFailureCatastrophic".Translate(surgeon.LabelShort, patient.LabelShort, surgeon.Named("SURGEON"), patient.Named("PATIENT"), this.recipe.Named("RECIPE")), LetterDefOf.NegativeEvent, patient, null, null, null, null);
						HealthUtility.GiveInjuriesOperationFailureCatastrophic(patient, part);
					}
				}
				else
				{
					Find.LetterStack.ReceiveLetter("LetterLabelSurgeryFailed".Translate(patient.Named("PATIENT")), "MessageMedicalOperationFailureMinor".Translate(surgeon.LabelShort, patient.LabelShort, surgeon.Named("SURGEON"), patient.Named("PATIENT"), this.recipe.Named("RECIPE")), LetterDefOf.NegativeEvent, patient, null, null, null, null);
					HealthUtility.GiveInjuriesOperationFailureMinor(patient, part);
				}
				if (!patient.Dead)
				{
					this.TryGainBotchedSurgeryThought(patient, surgeon);
				}
				return true;
			}
			return false;
		}

		// Token: 0x060050E3 RID: 20707 RVA: 0x001B10B8 File Offset: 0x001AF2B8
		private void TryGainBotchedSurgeryThought(Pawn patient, Pawn surgeon)
		{
			if (!patient.RaceProps.Humanlike || patient.needs.mood == null)
			{
				return;
			}
			patient.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.BotchedMySurgery, surgeon, null);
		}

		// Token: 0x060050E4 RID: 20708 RVA: 0x001B10F8 File Offset: 0x001AF2F8
		private float GetAverageMedicalPotency(List<Thing> ingredients, Bill bill)
		{
			Bill_Medical bill_Medical = bill as Bill_Medical;
			ThingDef thingDef;
			if (bill_Medical != null)
			{
				thingDef = bill_Medical.consumedInitialMedicineDef;
			}
			else
			{
				thingDef = null;
			}
			int num = 0;
			float num2 = 0f;
			if (thingDef != null)
			{
				num++;
				num2 += thingDef.GetStatValueAbstract(StatDefOf.MedicalPotency, null);
			}
			for (int i = 0; i < ingredients.Count; i++)
			{
				Medicine medicine = ingredients[i] as Medicine;
				if (medicine != null)
				{
					num += medicine.stackCount;
					num2 += medicine.GetStatValue(StatDefOf.MedicalPotency, true) * (float)medicine.stackCount;
				}
			}
			if (num == 0)
			{
				return 1f;
			}
			return num2 / (float)num;
		}

		// Token: 0x04002FFE RID: 12286
		private const float MaxSuccessChance = 0.98f;

		// Token: 0x04002FFF RID: 12287
		private const float CatastrophicFailChance = 0.5f;

		// Token: 0x04003000 RID: 12288
		private const float RidiculousFailChanceFromCatastrophic = 0.1f;

		// Token: 0x04003001 RID: 12289
		private const float InspiredSurgerySuccessChanceFactor = 2f;

		// Token: 0x04003002 RID: 12290
		private static readonly SimpleCurve MedicineMedicalPotencyToSurgeryChanceFactor = new SimpleCurve
		{
			{
				new CurvePoint(0f, 0.7f),
				true
			},
			{
				new CurvePoint(1f, 1f),
				true
			},
			{
				new CurvePoint(2f, 1.3f),
				true
			}
		};
	}
}
