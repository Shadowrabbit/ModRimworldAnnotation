using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200144E RID: 5198
	public static class RecordsUtility
	{
		// Token: 0x0600705F RID: 28767 RVA: 0x0022681C File Offset: 0x00224A1C
		public static void Notify_PawnKilled(Pawn killed, Pawn killer)
		{
			killer.records.Increment(RecordDefOf.Kills);
			RaceProperties raceProps = killed.RaceProps;
			if (raceProps.Humanlike)
			{
				killer.records.Increment(RecordDefOf.KillsHumanlikes);
			}
			if (raceProps.Animal)
			{
				killer.records.Increment(RecordDefOf.KillsAnimals);
			}
			if (raceProps.IsMechanoid)
			{
				killer.records.Increment(RecordDefOf.KillsMechanoids);
			}
		}

		// Token: 0x06007060 RID: 28768 RVA: 0x00226888 File Offset: 0x00224A88
		public static void Notify_PawnDowned(Pawn downed, Pawn instigator)
		{
			instigator.records.Increment(RecordDefOf.PawnsDowned);
			RaceProperties raceProps = downed.RaceProps;
			if (raceProps.Humanlike)
			{
				instigator.records.Increment(RecordDefOf.PawnsDownedHumanlikes);
			}
			if (raceProps.Animal)
			{
				instigator.records.Increment(RecordDefOf.PawnsDownedAnimals);
			}
			if (raceProps.IsMechanoid)
			{
				instigator.records.Increment(RecordDefOf.PawnsDownedMechanoids);
			}
		}

		// Token: 0x06007061 RID: 28769 RVA: 0x002268F4 File Offset: 0x00224AF4
		public static void Notify_BillDone(Pawn billDoer, List<Thing> products)
		{
			for (int i = 0; i < products.Count; i++)
			{
				if (products[i].def.IsNutritionGivingIngestible && products[i].def.ingestible.preferability >= FoodPreferability.MealAwful)
				{
					billDoer.records.Increment(RecordDefOf.MealsCooked);
				}
				else if (RecordsUtility.ShouldIncrementThingsCrafted(products[i]))
				{
					billDoer.records.Increment(RecordDefOf.ThingsCrafted);
				}
			}
		}

		// Token: 0x06007062 RID: 28770 RVA: 0x00226970 File Offset: 0x00224B70
		private static bool ShouldIncrementThingsCrafted(Thing crafted)
		{
			return crafted.def.IsApparel || crafted.def.IsWeapon || crafted.def.HasComp(typeof(CompArt)) || crafted.def.HasComp(typeof(CompQuality));
		}
	}
}
