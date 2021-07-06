using System;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001F12 RID: 7954
	public class QuestNode_GetAnimalToHunt : QuestNode
	{
		// Token: 0x0600AA38 RID: 43576 RVA: 0x0006F943 File Offset: 0x0006DB43
		protected override bool TestRunInt(Slate slate)
		{
			return this.DoWork(slate);
		}

		// Token: 0x0600AA39 RID: 43577 RVA: 0x0006F94C File Offset: 0x0006DB4C
		protected override void RunInt()
		{
			this.DoWork(QuestGen.slate);
		}

		// Token: 0x0600AA3A RID: 43578 RVA: 0x0031B124 File Offset: 0x00319324
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

		// Token: 0x0600AA3B RID: 43579 RVA: 0x0031B2B0 File Offset: 0x003194B0
		private float AnimalCommonalityByDifficulty(PawnKindDef animalKind, float animalDifficultyFromPoints)
		{
			float num = Mathf.Abs(animalKind.GetAnimalPointsToHuntOrSlaughter() - animalDifficultyFromPoints);
			return 1f / num;
		}

		// Token: 0x0400739A RID: 29594
		[NoTranslate]
		public SlateRef<string> storeAnimalToHuntAs;

		// Token: 0x0400739B RID: 29595
		[NoTranslate]
		public SlateRef<string> storeCountToHuntAs;

		// Token: 0x0400739C RID: 29596
		public SlateRef<SimpleCurve> pointsToAnimalsToHuntCountCurve;

		// Token: 0x0400739D RID: 29597
		public SlateRef<SimpleCurve> pointsToAnimalDifficultyCurve;

		// Token: 0x0400739E RID: 29598
		public SlateRef<FloatRange?> animalsToHuntCountRandomFactorRange;
	}
}
