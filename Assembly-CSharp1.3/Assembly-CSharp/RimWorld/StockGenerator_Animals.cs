using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02001217 RID: 4631
	public class StockGenerator_Animals : StockGenerator
	{
		// Token: 0x06006F35 RID: 28469 RVA: 0x0025215D File Offset: 0x0025035D
		public override IEnumerable<Thing> GenerateThings(int forTile, Faction faction = null)
		{
			StockGenerator_Animals.<>c__DisplayClass9_0 CS$<>8__locals1 = new StockGenerator_Animals.<>c__DisplayClass9_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.forTile = forTile;
			int numKinds = this.kindCountRange.RandomInRange;
			int count = this.countRange.RandomInRange;
			if (count > 1 && !this.createMatingPair.NullOrEmpty<string>())
			{
				StockGenerator_Animals.<>c__DisplayClass9_1 CS$<>8__locals2 = new StockGenerator_Animals.<>c__DisplayClass9_1();
				CS$<>8__locals2.CS$<>8__locals1 = CS$<>8__locals1;
				CS$<>8__locals2.CanCreateMatingPair = delegate(PawnKindDef k)
				{
					if (k.race.tradeTags == null || CS$<>8__locals2.CS$<>8__locals1.<>4__this.createMatingPair.NullOrEmpty<string>())
					{
						return false;
					}
					for (int l = 0; l < k.race.tradeTags.Count; l++)
					{
						if (CS$<>8__locals2.CS$<>8__locals1.<>4__this.createMatingPair.Contains(k.race.tradeTags[l]))
						{
							return true;
						}
					}
					return false;
				};
				PawnKindDef matingKind;
				(from k in DefDatabase<PawnKindDef>.AllDefs
				where CS$<>8__locals2.CS$<>8__locals1.<>4__this.PawnKindAllowed(k, CS$<>8__locals2.CS$<>8__locals1.forTile) && CS$<>8__locals2.CanCreateMatingPair(k)
				select k).TryRandomElementByWeight((PawnKindDef k) => (PawnUtility.PlayerHasReproductivePair(k) ? 0.5f : 1f) * CS$<>8__locals2.CS$<>8__locals1.<>4__this.SelectionChance(k), out matingKind);
				if (matingKind != null)
				{
					PawnGenerationRequest request = new PawnGenerationRequest(matingKind, null, PawnGenerationContext.NonPlayer, CS$<>8__locals2.CS$<>8__locals1.forTile, false, false, false, false, true, false, 1f, false, true, true, true, false, false, false, false, 0f, 0f, null, 1f, null, null, null, null, null, null, null, new Gender?(Gender.Female), null, null, null, null, null, false, false);
					yield return PawnGenerator.GeneratePawn(request);
					PawnGenerationRequest request2 = new PawnGenerationRequest(matingKind, null, PawnGenerationContext.NonPlayer, CS$<>8__locals2.CS$<>8__locals1.forTile, false, false, false, false, true, false, 1f, false, true, true, true, false, false, false, false, 0f, 0f, null, 1f, null, null, null, null, null, null, null, new Gender?(Gender.Male), null, null, null, null, null, false, false);
					yield return PawnGenerator.GeneratePawn(request2);
					count -= 2;
				}
				CS$<>8__locals2 = null;
				matingKind = null;
			}
			if (count <= 0)
			{
				yield break;
			}
			CS$<>8__locals1.kinds = new List<PawnKindDef>();
			for (int j = 0; j < numKinds; j++)
			{
				IEnumerable<PawnKindDef> allDefs = DefDatabase<PawnKindDef>.AllDefs;
				Func<PawnKindDef, bool> predicate;
				if ((predicate = CS$<>8__locals1.<>9__3) == null)
				{
					predicate = (CS$<>8__locals1.<>9__3 = ((PawnKindDef k) => !CS$<>8__locals1.kinds.Contains(k) && CS$<>8__locals1.<>4__this.PawnKindAllowed(k, CS$<>8__locals1.forTile)));
				}
				IEnumerable<PawnKindDef> source = allDefs.Where(predicate);
				Func<PawnKindDef, float> weightSelector;
				if ((weightSelector = CS$<>8__locals1.<>9__4) == null)
				{
					weightSelector = (CS$<>8__locals1.<>9__4 = ((PawnKindDef k) => CS$<>8__locals1.<>4__this.SelectionChance(k)));
				}
				PawnKindDef item;
				if (!source.TryRandomElementByWeight(weightSelector, out item))
				{
					break;
				}
				CS$<>8__locals1.kinds.Add(item);
			}
			int num;
			for (int i = 0; i < count; i = num + 1)
			{
				PawnKindDef kind;
				if (!CS$<>8__locals1.kinds.TryRandomElement(out kind))
				{
					yield break;
				}
				PawnGenerationRequest request3 = new PawnGenerationRequest(kind, null, PawnGenerationContext.NonPlayer, CS$<>8__locals1.forTile, false, false, false, false, true, false, 1f, false, true, true, true, false, false, false, false, 0f, 0f, null, 1f, null, null, null, null, null, null, null, null, null, null, null, null, null, false, false);
				yield return PawnGenerator.GeneratePawn(request3);
				num = i;
			}
			yield break;
		}

		// Token: 0x06006F36 RID: 28470 RVA: 0x00252174 File Offset: 0x00250374
		private float SelectionChance(PawnKindDef k)
		{
			return StockGenerator_Animals.SelectionChanceFromWildnessCurve.Evaluate(k.RaceProps.wildness);
		}

		// Token: 0x06006F37 RID: 28471 RVA: 0x0025218C File Offset: 0x0025038C
		public override bool HandlesThingDef(ThingDef thingDef)
		{
			return thingDef.category == ThingCategory.Pawn && thingDef.race.Animal && thingDef.tradeability != Tradeability.None && (this.tradeTagsSell.Any((string tag) => thingDef.tradeTags != null && thingDef.tradeTags.Contains(tag)) || this.tradeTagsBuy.Any((string tag) => thingDef.tradeTags != null && thingDef.tradeTags.Contains(tag)));
		}

		// Token: 0x06006F38 RID: 28472 RVA: 0x00252208 File Offset: 0x00250408
		private bool PawnKindAllowed(PawnKindDef kind, int forTile)
		{
			if (!kind.RaceProps.Animal || kind.RaceProps.wildness < this.minWildness || kind.RaceProps.wildness > this.maxWildness || kind.RaceProps.wildness > 1f)
			{
				return false;
			}
			if (this.checkTemperature)
			{
				int num = forTile;
				if (num == -1 && Find.AnyPlayerHomeMap != null)
				{
					num = Find.AnyPlayerHomeMap.Tile;
				}
				if (num != -1 && !Find.World.tileTemperatures.SeasonAndOutdoorTemperatureAcceptableFor(num, kind.race))
				{
					return false;
				}
			}
			return kind.race.tradeTags != null && this.tradeTagsSell.Any((string x) => kind.race.tradeTags.Contains(x)) && kind.race.tradeability.TraderCanSell();
		}

		// Token: 0x06006F39 RID: 28473 RVA: 0x0025230C File Offset: 0x0025050C
		public void LogAnimalChances()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (PawnKindDef pawnKindDef in DefDatabase<PawnKindDef>.AllDefs)
			{
				stringBuilder.AppendLine(pawnKindDef.defName + ": " + this.SelectionChance(pawnKindDef).ToString("F2"));
			}
			Log.Message(stringBuilder.ToString());
		}

		// Token: 0x06006F3A RID: 28474 RVA: 0x00252390 File Offset: 0x00250590
		[DebugOutput]
		private static void StockGenerationAnimals()
		{
			new StockGenerator_Animals
			{
				tradeTagsSell = new List<string>(),
				tradeTagsSell = 
				{
					"AnimalCommon",
					"AnimalUncommon"
				}
			}.LogAnimalChances();
		}

		// Token: 0x04003D73 RID: 15731
		[NoTranslate]
		private List<string> tradeTagsSell = new List<string>();

		// Token: 0x04003D74 RID: 15732
		[NoTranslate]
		private List<string> tradeTagsBuy = new List<string>();

		// Token: 0x04003D75 RID: 15733
		private IntRange kindCountRange = new IntRange(1, 1);

		// Token: 0x04003D76 RID: 15734
		private float minWildness;

		// Token: 0x04003D77 RID: 15735
		private float maxWildness = 1f;

		// Token: 0x04003D78 RID: 15736
		private bool checkTemperature = true;

		// Token: 0x04003D79 RID: 15737
		[NoTranslate]
		private List<string> createMatingPair = new List<string>();

		// Token: 0x04003D7A RID: 15738
		private static readonly SimpleCurve SelectionChanceFromWildnessCurve = new SimpleCurve
		{
			{
				new CurvePoint(0f, 100f),
				true
			},
			{
				new CurvePoint(0.25f, 60f),
				true
			},
			{
				new CurvePoint(0.5f, 30f),
				true
			},
			{
				new CurvePoint(0.75f, 12f),
				true
			},
			{
				new CurvePoint(1f, 2f),
				true
			}
		};

		// Token: 0x04003D7B RID: 15739
		private const float SelectionChanceFactorIfExistingMatingPair = 0.5f;
	}
}
