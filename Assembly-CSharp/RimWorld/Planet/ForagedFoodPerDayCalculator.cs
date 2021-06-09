using System;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x0200210F RID: 8463
	public static class ForagedFoodPerDayCalculator
	{
		// Token: 0x0600B3BA RID: 46010 RVA: 0x00342CB0 File Offset: 0x00340EB0
		public static Pair<ThingDef, float> ForagedFoodPerDay(List<Pawn> pawns, BiomeDef biome, Faction faction, bool caravanMovingNow, bool caravanNightResting, StringBuilder explanation = null)
		{
			float foragedFoodCountPerInterval = ForagedFoodPerDayCalculator.GetForagedFoodCountPerInterval(pawns, biome, faction, explanation);
			float progressPerTick = ForagedFoodPerDayCalculator.GetProgressPerTick(caravanMovingNow, caravanNightResting, explanation);
			float num = foragedFoodCountPerInterval * progressPerTick * 60000f;
			float num2;
			if (num != 0f)
			{
				num2 = num * biome.foragedFood.GetStatValueAbstract(StatDefOf.Nutrition, null);
			}
			else
			{
				num2 = 0f;
			}
			if (explanation != null)
			{
				explanation.AppendLine();
				explanation.AppendLine();
				TaggedString taggedString = "TotalNutrition".Translate() + ": " + num2.ToString("0.##") + " / " + "day".Translate();
				if (num2 > 0f)
				{
					taggedString += "\n= " + biome.LabelCap + ": " + biome.foragedFood.LabelCap + " x" + num.ToString("0.##") + " / " + "day".Translate();
				}
				explanation.Append(taggedString);
			}
			return new Pair<ThingDef, float>(biome.foragedFood, num);
		}

		// Token: 0x0600B3BB RID: 46011 RVA: 0x00342DDC File Offset: 0x00340FDC
		public static float GetProgressPerTick(bool caravanMovingNow, bool caravanNightResting, StringBuilder explanation = null)
		{
			float num = 0.0001f;
			if (!caravanMovingNow && !caravanNightResting)
			{
				num *= 2f;
				if (explanation != null)
				{
					explanation.AppendLine();
					explanation.Append("CaravanNotMoving".Translate() + ": " + 2f.ToStringPercent());
				}
			}
			return num;
		}

		// Token: 0x0600B3BC RID: 46012 RVA: 0x00342E38 File Offset: 0x00341038
		public static float GetForagedFoodCountPerInterval(List<Pawn> pawns, BiomeDef biome, Faction faction, StringBuilder explanation = null)
		{
			float num = (biome.foragedFood != null) ? biome.forageability : 0f;
			if (explanation != null)
			{
				explanation.Append("ForagedNutritionPerDay".Translate() + ":");
			}
			float num2 = 0f;
			bool flag = false;
			int i = 0;
			int count = pawns.Count;
			while (i < count)
			{
				Pawn pawn = pawns[i];
				bool flag2;
				float baseForagedNutritionPerDay = ForagedFoodPerDayCalculator.GetBaseForagedNutritionPerDay(pawn, out flag2);
				if (!flag2)
				{
					num2 += baseForagedNutritionPerDay;
					flag = true;
					if (explanation != null)
					{
						explanation.AppendLine();
						explanation.Append("  - " + pawn.LabelShortCap + ": +" + baseForagedNutritionPerDay.ToString("0.##"));
					}
				}
				i++;
			}
			float num3 = num2;
			num2 /= 6f;
			if (explanation != null)
			{
				explanation.AppendLine();
				if (flag)
				{
					explanation.Append("  = " + num3.ToString("0.##"));
				}
				else
				{
					explanation.Append("  (" + "NoneCapable".Translate().ToLower() + ")");
				}
				explanation.AppendLine();
				explanation.AppendLine();
				explanation.Append("Biome".Translate() + ": x" + num.ToStringPercent() + " (" + biome.label + ")");
				if (faction.def.forageabilityFactor != 1f)
				{
					explanation.AppendLine();
					explanation.Append("  " + "FactionType".Translate() + ": " + faction.def.forageabilityFactor.ToStringPercent());
				}
			}
			num2 *= num;
			num2 *= faction.def.forageabilityFactor;
			if (biome.foragedFood != null)
			{
				return num2 / biome.foragedFood.ingestible.CachedNutrition;
			}
			return num2;
		}

		// Token: 0x0600B3BD RID: 46013 RVA: 0x0034303C File Offset: 0x0034123C
		public static float GetBaseForagedNutritionPerDay(Pawn p, out bool skip)
		{
			if (!p.IsFreeColonist || p.InMentalState || p.Downed || p.CarriedByCaravan())
			{
				skip = true;
				return 0f;
			}
			skip = false;
			if (!StatDefOf.ForagedNutritionPerDay.Worker.IsDisabledFor(p))
			{
				return p.GetStatValue(StatDefOf.ForagedNutritionPerDay, true);
			}
			return 0f;
		}

		// Token: 0x0600B3BE RID: 46014 RVA: 0x00074C4E File Offset: 0x00072E4E
		public static Pair<ThingDef, float> ForagedFoodPerDay(Caravan caravan, StringBuilder explanation = null)
		{
			return ForagedFoodPerDayCalculator.ForagedFoodPerDay(caravan.PawnsListForReading, caravan.Biome, caravan.Faction, caravan.pather.MovingNow, caravan.NightResting, explanation);
		}

		// Token: 0x0600B3BF RID: 46015 RVA: 0x00074C79 File Offset: 0x00072E79
		public static float GetProgressPerTick(Caravan caravan, StringBuilder explanation = null)
		{
			return ForagedFoodPerDayCalculator.GetProgressPerTick(caravan.pather.MovingNow, caravan.NightResting, explanation);
		}

		// Token: 0x0600B3C0 RID: 46016 RVA: 0x00074C92 File Offset: 0x00072E92
		public static float GetForagedFoodCountPerInterval(Caravan caravan, StringBuilder explanation = null)
		{
			return ForagedFoodPerDayCalculator.GetForagedFoodCountPerInterval(caravan.PawnsListForReading, caravan.Biome, caravan.Faction, explanation);
		}

		// Token: 0x0600B3C1 RID: 46017 RVA: 0x0034309C File Offset: 0x0034129C
		public static Pair<ThingDef, float> ForagedFoodPerDay(List<TransferableOneWay> transferables, BiomeDef biome, Faction faction, StringBuilder explanation = null)
		{
			ForagedFoodPerDayCalculator.tmpPawns.Clear();
			for (int i = 0; i < transferables.Count; i++)
			{
				TransferableOneWay transferableOneWay = transferables[i];
				if (transferableOneWay.HasAnyThing && transferableOneWay.AnyThing is Pawn)
				{
					for (int j = 0; j < transferableOneWay.CountToTransfer; j++)
					{
						ForagedFoodPerDayCalculator.tmpPawns.Add((Pawn)transferableOneWay.things[j]);
					}
				}
			}
			Pair<ThingDef, float> result = ForagedFoodPerDayCalculator.ForagedFoodPerDay(ForagedFoodPerDayCalculator.tmpPawns, biome, faction, true, false, explanation);
			ForagedFoodPerDayCalculator.tmpPawns.Clear();
			return result;
		}

		// Token: 0x0600B3C2 RID: 46018 RVA: 0x00343128 File Offset: 0x00341328
		public static Pair<ThingDef, float> ForagedFoodPerDayLeftAfterTransfer(List<TransferableOneWay> transferables, BiomeDef biome, Faction faction, StringBuilder explanation = null)
		{
			ForagedFoodPerDayCalculator.tmpPawns.Clear();
			for (int i = 0; i < transferables.Count; i++)
			{
				TransferableOneWay transferableOneWay = transferables[i];
				if (transferableOneWay.HasAnyThing && transferableOneWay.AnyThing is Pawn)
				{
					for (int j = transferableOneWay.things.Count - 1; j >= transferableOneWay.CountToTransfer; j--)
					{
						ForagedFoodPerDayCalculator.tmpPawns.Add((Pawn)transferableOneWay.things[j]);
					}
				}
			}
			Pair<ThingDef, float> result = ForagedFoodPerDayCalculator.ForagedFoodPerDay(ForagedFoodPerDayCalculator.tmpPawns, biome, faction, true, false, explanation);
			ForagedFoodPerDayCalculator.tmpPawns.Clear();
			return result;
		}

		// Token: 0x0600B3C3 RID: 46019 RVA: 0x00074CAC File Offset: 0x00072EAC
		public static Pair<ThingDef, float> ForagedFoodPerDayLeftAfterTradeableTransfer(List<Thing> allCurrentThings, List<Tradeable> tradeables, BiomeDef biome, Faction faction, StringBuilder explanation = null)
		{
			ForagedFoodPerDayCalculator.tmpThingCounts.Clear();
			TransferableUtility.SimulateTradeableTransfer(allCurrentThings, tradeables, ForagedFoodPerDayCalculator.tmpThingCounts);
			Pair<ThingDef, float> result = ForagedFoodPerDayCalculator.ForagedFoodPerDay(ForagedFoodPerDayCalculator.tmpThingCounts, biome, faction, explanation);
			ForagedFoodPerDayCalculator.tmpThingCounts.Clear();
			return result;
		}

		// Token: 0x0600B3C4 RID: 46020 RVA: 0x003431C0 File Offset: 0x003413C0
		public static Pair<ThingDef, float> ForagedFoodPerDay(List<ThingCount> thingCounts, BiomeDef biome, Faction faction, StringBuilder explanation = null)
		{
			ForagedFoodPerDayCalculator.tmpPawns.Clear();
			for (int i = 0; i < thingCounts.Count; i++)
			{
				if (thingCounts[i].Count > 0)
				{
					Pawn pawn = thingCounts[i].Thing as Pawn;
					if (pawn != null)
					{
						ForagedFoodPerDayCalculator.tmpPawns.Add(pawn);
					}
				}
			}
			Pair<ThingDef, float> result = ForagedFoodPerDayCalculator.ForagedFoodPerDay(ForagedFoodPerDayCalculator.tmpPawns, biome, faction, true, false, explanation);
			ForagedFoodPerDayCalculator.tmpPawns.Clear();
			return result;
		}

		// Token: 0x04007B8D RID: 31629
		private static List<Pawn> tmpPawns = new List<Pawn>();

		// Token: 0x04007B8E RID: 31630
		private static List<ThingCount> tmpThingCounts = new List<ThingCount>();

		// Token: 0x04007B8F RID: 31631
		private const float BaseProgressPerTick = 0.0001f;

		// Token: 0x04007B90 RID: 31632
		public const float NotMovingProgressFactor = 2f;
	}
}
