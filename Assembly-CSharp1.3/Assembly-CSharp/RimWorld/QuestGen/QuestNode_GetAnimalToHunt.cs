using System;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001668 RID: 5736
	public class QuestNode_GetAnimalToHunt : QuestNode
	{
		// Token: 0x060085A4 RID: 34212 RVA: 0x002FF0ED File Offset: 0x002FD2ED
		protected override bool TestRunInt(Slate slate)
		{
			return this.DoWork(slate);
		}

		// Token: 0x060085A5 RID: 34213 RVA: 0x002FF0F6 File Offset: 0x002FD2F6
		protected override void RunInt()
		{
			this.DoWork(QuestGen.slate);
		}

		// Token: 0x060085A6 RID: 34214 RVA: 0x002FF104 File Offset: 0x002FD304
		private bool DoWork(Slate slate)
		{
			Map map = slate.Get<Map>("map", null, false);
			if (map == null)
			{
				return false;
			}
			float x2 = slate.Get<float>("points", 0f, false);
			float animalDifficultyFromPoints = this.pointsToAnimalDifficultyCurve.GetValue(slate).Evaluate(x2);
			PawnKindDef pawnKindDef;
			if (!map.Biome.AllWildAnimals.Where(delegate(PawnKindDef x)
			{
				if (map.mapTemperature.SeasonAndOutdoorTemperatureAcceptableFor(x.race))
				{
					return map.listerThings.ThingsOfDef(x.race).Any((Thing p) => p.Faction == null);
				}
				return false;
			}).TryRandomElementByWeight((PawnKindDef x) => this.AnimalCommonalityByDifficulty(x, animalDifficultyFromPoints), out pawnKindDef))
			{
				return false;
			}
			int num = 0;
			for (int i = 0; i < map.mapPawns.AllPawnsSpawned.Count; i++)
			{
				Pawn pawn = map.mapPawns.AllPawnsSpawned[i];
				if (pawn.def == pawnKindDef.race && !pawn.IsQuestLodger() && pawn.Faction == null)
				{
					num++;
				}
			}
			SimpleCurve value = this.pointsToAnimalsToHuntCountCurve.GetValue(slate);
			float randomInRange = (this.animalsToHuntCountRandomFactorRange.GetValue(slate) ?? FloatRange.One).RandomInRange;
			int num2 = Mathf.RoundToInt(value.Evaluate(x2) * randomInRange);
			num2 = Mathf.Min(num2, num);
			num2 = Mathf.Max(num2, 1);
			slate.Set<ThingDef>(this.storeAnimalToHuntAs.GetValue(slate), pawnKindDef.race, false);
			slate.Set<int>(this.storeCountToHuntAs.GetValue(slate), num2, false);
			return true;
		}

		// Token: 0x060085A7 RID: 34215 RVA: 0x002FF290 File Offset: 0x002FD490
		private float AnimalCommonalityByDifficulty(PawnKindDef animalKind, float animalDifficultyFromPoints)
		{
			float num = Mathf.Abs(animalKind.GetAnimalPointsToHuntOrSlaughter() - animalDifficultyFromPoints);
			return 1f / num;
		}

		// Token: 0x04005378 RID: 21368
		[NoTranslate]
		public SlateRef<string> storeAnimalToHuntAs;

		// Token: 0x04005379 RID: 21369
		[NoTranslate]
		public SlateRef<string> storeCountToHuntAs;

		// Token: 0x0400537A RID: 21370
		public SlateRef<SimpleCurve> pointsToAnimalsToHuntCountCurve;

		// Token: 0x0400537B RID: 21371
		public SlateRef<SimpleCurve> pointsToAnimalDifficultyCurve;

		// Token: 0x0400537C RID: 21372
		public SlateRef<FloatRange?> animalsToHuntCountRandomFactorRange;
	}
}
