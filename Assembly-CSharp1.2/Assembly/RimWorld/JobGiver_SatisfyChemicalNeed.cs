using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000C88 RID: 3208
	public class JobGiver_SatisfyChemicalNeed : ThinkNode_JobGiver
	{
		// Token: 0x06004AE4 RID: 19172 RVA: 0x000357EA File Offset: 0x000339EA
		public override float GetPriority(Pawn pawn)
		{
			if (pawn.needs.AllNeeds.Any((Need x) => this.ShouldSatisfy(x)))
			{
				return 9.25f;
			}
			return 0f;
		}

		// Token: 0x06004AE5 RID: 19173 RVA: 0x001A3774 File Offset: 0x001A1974
		protected override Job TryGiveJob(Pawn pawn)
		{
			JobGiver_SatisfyChemicalNeed.tmpChemicalNeeds.Clear();
			List<Need> allNeeds = pawn.needs.AllNeeds;
			for (int i = 0; i < allNeeds.Count; i++)
			{
				if (this.ShouldSatisfy(allNeeds[i]))
				{
					JobGiver_SatisfyChemicalNeed.tmpChemicalNeeds.Add((Need_Chemical)allNeeds[i]);
				}
			}
			if (!JobGiver_SatisfyChemicalNeed.tmpChemicalNeeds.Any<Need_Chemical>())
			{
				return null;
			}
			JobGiver_SatisfyChemicalNeed.tmpChemicalNeeds.SortBy((Need_Chemical x) => x.CurLevel);
			for (int j = 0; j < JobGiver_SatisfyChemicalNeed.tmpChemicalNeeds.Count; j++)
			{
				Thing thing = this.FindDrugFor(pawn, JobGiver_SatisfyChemicalNeed.tmpChemicalNeeds[j]);
				if (thing != null)
				{
					JobGiver_SatisfyChemicalNeed.tmpChemicalNeeds.Clear();
					return DrugAIUtility.IngestAndTakeToInventoryJob(thing, pawn, 1);
				}
			}
			JobGiver_SatisfyChemicalNeed.tmpChemicalNeeds.Clear();
			return null;
		}

		// Token: 0x06004AE6 RID: 19174 RVA: 0x001A3850 File Offset: 0x001A1A50
		private bool ShouldSatisfy(Need need)
		{
			Need_Chemical need_Chemical = need as Need_Chemical;
			return need_Chemical != null && need_Chemical.CurCategory <= DrugDesireCategory.Desire;
		}

		// Token: 0x06004AE7 RID: 19175 RVA: 0x001A3874 File Offset: 0x001A1A74
		private Thing FindDrugFor(Pawn pawn, Need_Chemical need)
		{
			Hediff_Addiction addictionHediff = need.AddictionHediff;
			if (addictionHediff == null)
			{
				return null;
			}
			ThingOwner<Thing> innerContainer = pawn.inventory.innerContainer;
			for (int i = 0; i < innerContainer.Count; i++)
			{
				if (this.DrugValidator(pawn, addictionHediff, innerContainer[i]))
				{
					return innerContainer[i];
				}
			}
			return GenClosest.ClosestThingReachable(pawn.Position, pawn.Map, ThingRequest.ForGroup(ThingRequestGroup.Drug), PathEndMode.ClosestTouch, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false), 9999f, (Thing x) => this.DrugValidator(pawn, addictionHediff, x), null, 0, -1, false, RegionType.Set_Passable, false);
		}

		// Token: 0x06004AE8 RID: 19176 RVA: 0x001A3938 File Offset: 0x001A1B38
		private bool DrugValidator(Pawn pawn, Hediff_Addiction addiction, Thing drug)
		{
			if (!drug.def.IsDrug)
			{
				return false;
			}
			if (drug.Spawned)
			{
				if (drug.IsForbidden(pawn))
				{
					return false;
				}
				if (!pawn.CanReserve(drug, 1, -1, null, false))
				{
					return false;
				}
				if (!drug.IsSociallyProper(pawn))
				{
					return false;
				}
			}
			CompDrug compDrug = drug.TryGetComp<CompDrug>();
			return compDrug != null && compDrug.Props.chemical != null && compDrug.Props.chemical.addictionHediff == addiction.def && (pawn.drugs == null || pawn.drugs.CurrentPolicy[drug.def].allowedForAddiction || pawn.story == null || pawn.story.traits.DegreeOfTrait(TraitDefOf.DrugDesire) > 0 || (pawn.InMentalState && pawn.MentalStateDef.ignoreDrugPolicy));
		}

		// Token: 0x040031A7 RID: 12711
		private static List<Need_Chemical> tmpChemicalNeeds = new List<Need_Chemical>();
	}
}
