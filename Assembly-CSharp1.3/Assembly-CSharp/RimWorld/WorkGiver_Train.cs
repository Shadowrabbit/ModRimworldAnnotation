using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200080F RID: 2063
	public class WorkGiver_Train : WorkGiver_InteractAnimal
	{
		// Token: 0x060036FA RID: 14074 RVA: 0x00136C3C File Offset: 0x00134E3C
		public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
		{
			return pawn.Map.mapPawns.SpawnedPawnsInFaction(pawn.Faction);
		}

		// Token: 0x060036FB RID: 14075 RVA: 0x001374AC File Offset: 0x001356AC
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Pawn pawn2 = t as Pawn;
			if (pawn2 == null || !pawn2.RaceProps.Animal)
			{
				return null;
			}
			if (pawn2.RaceProps.animalType == AnimalType.Dryad)
			{
				return null;
			}
			if (pawn2.Faction != pawn.Faction)
			{
				return null;
			}
			if (TrainableUtility.TrainedTooRecently(pawn2))
			{
				JobFailReason.Is(WorkGiver_InteractAnimal.AnimalInteractedTooRecentlyTrans, null);
				return null;
			}
			if (pawn2.training == null)
			{
				return null;
			}
			if (pawn2.training.NextTrainableToTrain() == null)
			{
				return null;
			}
			if (!this.CanInteractWithAnimal(pawn, pawn2, forced))
			{
				return null;
			}
			if (pawn2.RaceProps.EatsFood && !base.HasFoodToInteractAnimal(pawn, pawn2))
			{
				Job job = base.TakeFoodForAnimalInteractJob(pawn, pawn2);
				if (job == null)
				{
					JobFailReason.Is(WorkGiver_InteractAnimal.NoUsableFoodTrans, null);
				}
				return job;
			}
			return JobMaker.MakeJob(JobDefOf.Train, t);
		}
	}
}
