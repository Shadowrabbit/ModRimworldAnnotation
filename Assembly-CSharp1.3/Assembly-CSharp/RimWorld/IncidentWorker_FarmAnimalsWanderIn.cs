using System;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C05 RID: 3077
	public class IncidentWorker_FarmAnimalsWanderIn : IncidentWorker
	{
		// Token: 0x17000CA3 RID: 3235
		// (get) Token: 0x06004860 RID: 18528 RVA: 0x0017ED37 File Offset: 0x0017CF37
		public override float BaseChanceThisGame
		{
			get
			{
				if (!ModsConfig.IdeologyActive || !IdeoUtility.AnyColonistWithRanchingIssue())
				{
					return base.BaseChanceThisGame;
				}
				return base.BaseChanceThisGame * IncidentWorker_FarmAnimalsWanderIn.BaseChanceFactorByAnimalBodySizePerCapitaCurve.Evaluate(PawnUtility.PlayerAnimalBodySizePerCapita());
			}
		}

		// Token: 0x06004861 RID: 18529 RVA: 0x0017ED64 File Offset: 0x0017CF64
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			if (!base.CanFireNowSub(parms))
			{
				return false;
			}
			Map map = (Map)parms.target;
			IntVec3 intVec;
			PawnKindDef pawnKindDef;
			return RCellFinder.TryFindRandomPawnEntryCell(out intVec, map, CellFinder.EdgeRoadChance_Animal, false, null) && this.TryFindRandomPawnKind(map, out pawnKindDef);
		}

		// Token: 0x06004862 RID: 18530 RVA: 0x0017EDA4 File Offset: 0x0017CFA4
		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			IntVec3 intVec;
			if (!RCellFinder.TryFindRandomPawnEntryCell(out intVec, map, CellFinder.EdgeRoadChance_Animal, false, null))
			{
				return false;
			}
			PawnKindDef pawnKindDef;
			if (!this.TryFindRandomPawnKind(map, out pawnKindDef))
			{
				return false;
			}
			int num = Mathf.Clamp(GenMath.RoundRandom(((parms.totalBodySize > 0f) ? parms.totalBodySize : 2.5f) / pawnKindDef.RaceProps.baseBodySize), 2, 10);
			if (num >= 2)
			{
				this.SpawnAnimal(intVec, map, pawnKindDef, new Gender?(Gender.Female));
				this.SpawnAnimal(intVec, map, pawnKindDef, new Gender?(Gender.Male));
				num -= 2;
			}
			for (int i = 0; i < num; i++)
			{
				this.SpawnAnimal(intVec, map, pawnKindDef, null);
			}
			base.SendStandardLetter(parms.customLetterLabel ?? "LetterLabelFarmAnimalsWanderIn".Translate(pawnKindDef.GetLabelPlural(-1)).CapitalizeFirst(), parms.customLetterText ?? "LetterFarmAnimalsWanderIn".Translate(pawnKindDef.GetLabelPlural(-1)), LetterDefOf.PositiveEvent, parms, new TargetInfo(intVec, map, false), Array.Empty<NamedArgument>());
			return true;
		}

		// Token: 0x06004863 RID: 18531 RVA: 0x0017EED8 File Offset: 0x0017D0D8
		private void SpawnAnimal(IntVec3 location, Map map, PawnKindDef pawnKind, Gender? gender = null)
		{
			IntVec3 loc = CellFinder.RandomClosewalkCellNear(location, map, 12, null);
			Pawn pawn = PawnGenerator.GeneratePawn(new PawnGenerationRequest(pawnKind, null, PawnGenerationContext.NonPlayer, -1, false, false, false, false, true, false, 1f, false, true, true, true, false, false, false, false, 0f, 0f, null, 1f, null, null, null, null, null, null, null, gender, null, null, null, null, null, false, false));
			GenSpawn.Spawn(pawn, loc, map, Rot4.Random, WipeMode.Vanish, false);
			pawn.SetFaction(Faction.OfPlayer, null);
		}

		// Token: 0x06004864 RID: 18532 RVA: 0x0017EF6C File Offset: 0x0017D16C
		private bool TryFindRandomPawnKind(Map map, out PawnKindDef kind)
		{
			return (from x in DefDatabase<PawnKindDef>.AllDefs
			where x.RaceProps.Animal && x.RaceProps.wildness < 0.35f && map.mapTemperature.SeasonAndOutdoorTemperatureAcceptableFor(x.race) && x.race.tradeTags.Contains("AnimalFarm")
			select x).TryRandomElementByWeight((PawnKindDef k) => this.SelectionChance(k), out kind);
		}

		// Token: 0x06004865 RID: 18533 RVA: 0x0017EFB8 File Offset: 0x0017D1B8
		private float SelectionChance(PawnKindDef pawnKind)
		{
			float num = 0.42000002f - pawnKind.RaceProps.wildness;
			if (PawnUtility.PlayerHasReproductivePair(pawnKind))
			{
				num *= 0.5f;
			}
			return num;
		}

		// Token: 0x04002C5C RID: 11356
		private const float MaxWildness = 0.35f;

		// Token: 0x04002C5D RID: 11357
		private const float TotalBodySizeToSpawn = 2.5f;

		// Token: 0x04002C5E RID: 11358
		private static SimpleCurve BaseChanceFactorByAnimalBodySizePerCapitaCurve = new SimpleCurve
		{
			{
				new CurvePoint(0f, 1.5f),
				true
			},
			{
				new CurvePoint(4f, 1f),
				true
			}
		};

		// Token: 0x04002C5F RID: 11359
		private const float SelectionChanceFactorIfExistingMatingPair = 0.5f;

		// Token: 0x04002C60 RID: 11360
		private const string FarmAnimalTradeTag = "AnimalFarm";
	}
}
