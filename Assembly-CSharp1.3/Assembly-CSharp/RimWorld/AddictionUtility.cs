using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000D92 RID: 3474
	public static class AddictionUtility
	{
		// Token: 0x0600509C RID: 20636 RVA: 0x001AFA21 File Offset: 0x001ADC21
		public static bool IsAddicted(Pawn pawn, Thing drug)
		{
			return AddictionUtility.FindAddictionHediff(pawn, drug) != null;
		}

		// Token: 0x0600509D RID: 20637 RVA: 0x001AFA2D File Offset: 0x001ADC2D
		public static bool IsAddicted(Pawn pawn, ChemicalDef chemical)
		{
			return AddictionUtility.FindAddictionHediff(pawn, chemical) != null;
		}

		// Token: 0x0600509E RID: 20638 RVA: 0x001AFA3C File Offset: 0x001ADC3C
		public static Hediff_Addiction FindAddictionHediff(Pawn pawn, Thing drug)
		{
			if (!drug.def.IsDrug)
			{
				return null;
			}
			CompDrug compDrug = drug.TryGetComp<CompDrug>();
			if (!compDrug.Props.Addictive)
			{
				return null;
			}
			return AddictionUtility.FindAddictionHediff(pawn, compDrug.Props.chemical);
		}

		// Token: 0x0600509F RID: 20639 RVA: 0x001AFA80 File Offset: 0x001ADC80
		public static Hediff_Addiction FindAddictionHediff(Pawn pawn, ChemicalDef chemical)
		{
			return (Hediff_Addiction)pawn.health.hediffSet.hediffs.Find((Hediff x) => x.def == chemical.addictionHediff);
		}

		// Token: 0x060050A0 RID: 20640 RVA: 0x001AFAC0 File Offset: 0x001ADCC0
		public static Hediff FindToleranceHediff(Pawn pawn, ChemicalDef chemical)
		{
			if (chemical.toleranceHediff == null)
			{
				return null;
			}
			return pawn.health.hediffSet.hediffs.Find((Hediff x) => x.def == chemical.toleranceHediff);
		}

		// Token: 0x060050A1 RID: 20641 RVA: 0x001AFB0C File Offset: 0x001ADD0C
		public static void ModifyChemicalEffectForToleranceAndBodySize(Pawn pawn, ChemicalDef chemicalDef, ref float effect)
		{
			if (chemicalDef != null)
			{
				List<Hediff> hediffs = pawn.health.hediffSet.hediffs;
				for (int i = 0; i < hediffs.Count; i++)
				{
					hediffs[i].ModifyChemicalEffect(chemicalDef, ref effect);
				}
			}
			effect /= pawn.BodySize;
		}

		// Token: 0x060050A2 RID: 20642 RVA: 0x001AFB58 File Offset: 0x001ADD58
		public static void CheckDrugAddictionTeachOpportunity(Pawn pawn)
		{
			if (!pawn.RaceProps.IsFlesh || !pawn.Spawned)
			{
				return;
			}
			if (pawn.Faction != Faction.OfPlayer && pawn.HostFaction != Faction.OfPlayer)
			{
				return;
			}
			if (!AddictionUtility.AddictedToAnything(pawn))
			{
				return;
			}
			LessonAutoActivator.TeachOpportunity(ConceptDefOf.DrugAddiction, pawn, OpportunityType.Important);
		}

		// Token: 0x060050A3 RID: 20643 RVA: 0x001AFBAC File Offset: 0x001ADDAC
		public static bool AddictedToAnything(Pawn pawn)
		{
			List<Hediff> hediffs = pawn.health.hediffSet.hediffs;
			for (int i = 0; i < hediffs.Count; i++)
			{
				if (hediffs[i] is Hediff_Addiction)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060050A4 RID: 20644 RVA: 0x001AFBEC File Offset: 0x001ADDEC
		public static bool CanBingeOnNow(Pawn pawn, ChemicalDef chemical, DrugCategory drugCategory)
		{
			if (!chemical.canBinge)
			{
				return false;
			}
			if (!pawn.Spawned)
			{
				return false;
			}
			List<Thing> list = pawn.Map.listerThings.ThingsInGroup(ThingRequestGroup.Drug);
			for (int i = 0; i < list.Count; i++)
			{
				if (!list[i].Position.Fogged(list[i].Map) && (drugCategory == DrugCategory.Any || list[i].def.ingestible.drugCategory == drugCategory) && list[i].TryGetComp<CompDrug>().Props.chemical == chemical && (list[i].Position.Roofed(list[i].Map) || list[i].Position.InHorDistOf(pawn.Position, 45f)) && pawn.CanReach(list[i], PathEndMode.ClosestTouch, Danger.Deadly, false, false, TraverseMode.ByPawn))
				{
					return true;
				}
			}
			return false;
		}
	}
}
