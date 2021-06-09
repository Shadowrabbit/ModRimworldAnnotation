using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000C85 RID: 3205
	public class JobGiver_BingeDrug : JobGiver_Binge
	{
		// Token: 0x06004AD9 RID: 19161 RVA: 0x001A3460 File Offset: 0x001A1660
		protected override int IngestInterval(Pawn pawn)
		{
			ChemicalDef chemical = this.GetChemical(pawn);
			int num = 600;
			if (chemical == ChemicalDefOf.Alcohol)
			{
				Hediff firstHediffOfDef = pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.AlcoholHigh, false);
				if (firstHediffOfDef != null)
				{
					num = (int)((float)num * JobGiver_BingeDrug.IngestIntervalFactorCurve_Drunkness.Evaluate(firstHediffOfDef.Severity));
				}
			}
			else
			{
				Hediff firstHediffOfDef2 = pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.DrugOverdose, false);
				if (firstHediffOfDef2 != null)
				{
					num = (int)((float)num * JobGiver_BingeDrug.IngestIntervalFactorCurve_DrugOverdose.Evaluate(firstHediffOfDef2.Severity));
				}
			}
			return num;
		}

		// Token: 0x06004ADA RID: 19162 RVA: 0x001A34E4 File Offset: 0x001A16E4
		protected override Thing BestIngestTarget(Pawn pawn)
		{
			ChemicalDef chemical = this.GetChemical(pawn);
			DrugCategory drugCategory = this.GetDrugCategory(pawn);
			if (chemical == null)
			{
				Log.ErrorOnce("Tried to binge on null chemical.", 1393746152, false);
				return null;
			}
			Hediff overdose = pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.DrugOverdose, false);
			Predicate<Thing> validator = delegate(Thing t)
			{
				if (!this.IgnoreForbid(pawn) && t.IsForbidden(pawn))
				{
					return false;
				}
				if (!pawn.CanReserve(t, 1, -1, null, false))
				{
					return false;
				}
				CompDrug compDrug = t.TryGetComp<CompDrug>();
				return compDrug.Props.chemical == chemical && (overdose == null || !compDrug.Props.CanCauseOverdose || overdose.Severity + compDrug.Props.overdoseSeverityOffset.max < 0.786f) && (pawn.Position.InHorDistOf(t.Position, 60f) || t.Position.Roofed(t.Map) || pawn.Map.areaManager.Home[t.Position] || t.GetSlotGroup() != null) && t.def.ingestible.drugCategory.IncludedIn(drugCategory);
			};
			return GenClosest.ClosestThingReachable(pawn.Position, pawn.Map, ThingRequest.ForGroup(ThingRequestGroup.Drug), PathEndMode.OnCell, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false), 9999f, validator, null, 0, -1, false, RegionType.Set_Passable, false);
		}

		// Token: 0x06004ADB RID: 19163 RVA: 0x000357B7 File Offset: 0x000339B7
		private ChemicalDef GetChemical(Pawn pawn)
		{
			return ((MentalState_BingingDrug)pawn.MentalState).chemical;
		}

		// Token: 0x06004ADC RID: 19164 RVA: 0x000357C9 File Offset: 0x000339C9
		private DrugCategory GetDrugCategory(Pawn pawn)
		{
			return ((MentalState_BingingDrug)pawn.MentalState).drugCategory;
		}

		// Token: 0x0400319D RID: 12701
		private const int BaseIngestInterval = 600;

		// Token: 0x0400319E RID: 12702
		private const float OverdoseSeverityToAvoid = 0.786f;

		// Token: 0x0400319F RID: 12703
		private static readonly SimpleCurve IngestIntervalFactorCurve_Drunkness = new SimpleCurve
		{
			{
				new CurvePoint(0f, 1f),
				true
			},
			{
				new CurvePoint(1f, 4f),
				true
			}
		};

		// Token: 0x040031A0 RID: 12704
		private static readonly SimpleCurve IngestIntervalFactorCurve_DrugOverdose = new SimpleCurve
		{
			{
				new CurvePoint(0f, 1f),
				true
			},
			{
				new CurvePoint(1f, 5f),
				true
			}
		};
	}
}
