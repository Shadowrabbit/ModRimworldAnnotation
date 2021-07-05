using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000DBB RID: 3515
	public static class PawnAddictionHediffsGenerator
	{
		// Token: 0x06005154 RID: 20820 RVA: 0x001B5648 File Offset: 0x001B3848
		public static void GenerateAddictionsAndTolerancesFor(Pawn pawn)
		{
			if (!pawn.RaceProps.IsFlesh || !pawn.RaceProps.Humanlike)
			{
				return;
			}
			if (pawn.IsTeetotaler())
			{
				return;
			}
			PawnAddictionHediffsGenerator.allDrugs.Clear();
			int num = 0;
			foreach (ChemicalDef chemicalDef in pawn.kindDef.forcedAddictions)
			{
				PawnAddictionHediffsGenerator.ApplyAddiction(pawn, chemicalDef);
				num++;
			}
			Func<ChemicalDef, bool> <>9__1;
			while (num < 3 && Rand.Value < pawn.kindDef.chemicalAddictionChance)
			{
				if (!PawnAddictionHediffsGenerator.allDrugs.Any<ThingDef>())
				{
					PawnAddictionHediffsGenerator.allDrugs.AddRange(from x in DefDatabase<ThingDef>.AllDefsListForReading
					where x.category == ThingCategory.Item && x.GetCompProperties<CompProperties_Drug>() != null
					select x);
				}
				IEnumerable<ChemicalDef> allDefsListForReading = DefDatabase<ChemicalDef>.AllDefsListForReading;
				Func<ChemicalDef, bool> predicate;
				if ((predicate = <>9__1) == null)
				{
					predicate = (<>9__1 = ((ChemicalDef x) => PawnAddictionHediffsGenerator.PossibleWithTechLevel(x, pawn.Faction) && !AddictionUtility.IsAddicted(pawn, x)));
				}
				ChemicalDef chemicalDef2;
				if (!allDefsListForReading.Where(predicate).TryRandomElement(out chemicalDef2))
				{
					break;
				}
				PawnAddictionHediffsGenerator.ApplyAddiction(pawn, chemicalDef2);
				num++;
			}
		}

		// Token: 0x06005155 RID: 20821 RVA: 0x001B57A4 File Offset: 0x001B39A4
		private static void ApplyAddiction(Pawn pawn, ChemicalDef chemicalDef)
		{
			Hediff hediff = HediffMaker.MakeHediff(chemicalDef.addictionHediff, pawn, null);
			hediff.Severity = PawnAddictionHediffsGenerator.GeneratedAddictionSeverityRange.RandomInRange;
			pawn.health.AddHediff(hediff, null, null, null);
			if (chemicalDef.toleranceHediff != null && Rand.Value < chemicalDef.onGeneratedAddictedToleranceChance)
			{
				Hediff hediff2 = HediffMaker.MakeHediff(chemicalDef.toleranceHediff, pawn, null);
				hediff2.Severity = PawnAddictionHediffsGenerator.GeneratedToleranceSeverityRange.RandomInRange;
				pawn.health.AddHediff(hediff2, null, null, null);
			}
			if (chemicalDef.onGeneratedAddictedEvents != null)
			{
				foreach (HediffGiver_Event hediffGiver_Event in chemicalDef.onGeneratedAddictedEvents)
				{
					hediffGiver_Event.EventOccurred(pawn);
				}
			}
			PawnAddictionHediffsGenerator.DoIngestionOutcomeDoers(pawn, chemicalDef);
		}

		// Token: 0x06005156 RID: 20822 RVA: 0x001B588C File Offset: 0x001B3A8C
		private static bool PossibleWithTechLevel(ChemicalDef chemical, Faction faction)
		{
			return faction == null || PawnAddictionHediffsGenerator.allDrugs.Any((ThingDef x) => x.GetCompProperties<CompProperties_Drug>().chemical == chemical && x.techLevel <= faction.def.techLevel);
		}

		// Token: 0x06005157 RID: 20823 RVA: 0x001B58D0 File Offset: 0x001B3AD0
		private static void DoIngestionOutcomeDoers(Pawn pawn, ChemicalDef chemical)
		{
			for (int i = 0; i < PawnAddictionHediffsGenerator.allDrugs.Count; i++)
			{
				if (PawnAddictionHediffsGenerator.allDrugs[i].GetCompProperties<CompProperties_Drug>().chemical == chemical)
				{
					List<IngestionOutcomeDoer> outcomeDoers = PawnAddictionHediffsGenerator.allDrugs[i].ingestible.outcomeDoers;
					for (int j = 0; j < outcomeDoers.Count; j++)
					{
						if (outcomeDoers[j].doToGeneratedPawnIfAddicted)
						{
							outcomeDoers[j].DoIngestionOutcome(pawn, null);
						}
					}
				}
			}
		}

		// Token: 0x0400302F RID: 12335
		private static List<ThingDef> allDrugs = new List<ThingDef>();

		// Token: 0x04003030 RID: 12336
		private const int MaxAddictions = 3;

		// Token: 0x04003031 RID: 12337
		private static readonly FloatRange GeneratedAddictionSeverityRange = new FloatRange(0.6f, 1f);

		// Token: 0x04003032 RID: 12338
		private static readonly FloatRange GeneratedToleranceSeverityRange = new FloatRange(0.1f, 0.9f);
	}
}
