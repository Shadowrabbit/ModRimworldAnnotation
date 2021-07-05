using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x020018EC RID: 6380
	public class StockGenerator_Animals : StockGenerator
	{
		// Token: 0x06008D52 RID: 36178 RVA: 0x0005EAE0 File Offset: 0x0005CCE0
		public override IEnumerable<Thing> GenerateThings(int forTile, Faction faction = null)
		{
			int randomInRange = this.kindCountRange.RandomInRange;
			int count = this.countRange.RandomInRange;
			List<PawnKindDef> kinds = new List<PawnKindDef>();
			Func<PawnKindDef, bool> <>9__0;
			Func<PawnKindDef, float> <>9__1;
			for (int j = 0; j < randomInRange; j++)
			{
				IEnumerable<PawnKindDef> allDefs = DefDatabase<PawnKindDef>.AllDefs;
				Func<PawnKindDef, bool> predicate;
				if ((predicate = <>9__0) == null)
				{
					predicate = (<>9__0 = ((PawnKindDef k) => !kinds.Contains(k) && this.PawnKindAllowed(k, forTile)));
				}
				IEnumerable<PawnKindDef> source = allDefs.Where(predicate);
				Func<PawnKindDef, float> weightSelector;
				if ((weightSelector = <>9__1) == null)
				{
					weightSelector = (<>9__1 = ((PawnKindDef k) => this.SelectionChance(k)));
				}
				PawnKindDef item;
				if (!source.TryRandomElementByWeight(weightSelector, out item))
				{
					break;
				}
				kinds.Add(item);
			}
			int num;
			for (int i = 0; i < count; i = num + 1)
			{
				PawnKindDef kind;
				if (!kinds.TryRandomElement(out kind))
				{
					yield break;
				}
				PawnGenerationRequest request = new PawnGenerationRequest(kind, null, PawnGenerationContext.NonPlayer, forTile, false, false, false, false, true, false, 1f, false, true, true, true, false, false, false, false, 0f, null, 1f, null, null, null, null, null, null, null, null, null, null, null, null);
				yield return PawnGenerator.GeneratePawn(request);
				num = i;
			}
			yield break;
		}

		// Token: 0x06008D53 RID: 36179 RVA: 0x0005EAF7 File Offset: 0x0005CCF7
		private float SelectionChance(PawnKindDef k)
		{
			return StockGenerator_Animals.SelectionChanceFromWildnessCurve.Evaluate(k.RaceProps.wildness);
		}

		// Token: 0x06008D54 RID: 36180 RVA: 0x0028EE00 File Offset: 0x0028D000
		public override bool HandlesThingDef(ThingDef thingDef)
		{
			return thingDef.category == ThingCategory.Pawn && thingDef.race.Animal && thingDef.tradeability != Tradeability.None && (this.tradeTagsSell.Any((string tag) => thingDef.tradeTags != null && thingDef.tradeTags.Contains(tag)) || this.tradeTagsBuy.Any((string tag) => thingDef.tradeTags != null && thingDef.tradeTags.Contains(tag)));
		}

		// Token: 0x06008D55 RID: 36181 RVA: 0x0028EE7C File Offset: 0x0028D07C
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

		// Token: 0x06008D56 RID: 36182 RVA: 0x0028EF80 File Offset: 0x0028D180
		public void LogAnimalChances()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (PawnKindDef pawnKindDef in DefDatabase<PawnKindDef>.AllDefs)
			{
				stringBuilder.AppendLine(pawnKindDef.defName + ": " + this.SelectionChance(pawnKindDef).ToString("F2"));
			}
			Log.Message(stringBuilder.ToString(), false);
		}

		// Token: 0x06008D57 RID: 36183 RVA: 0x0005EB0E File Offset: 0x0005CD0E
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

		// Token: 0x04005A2E RID: 23086
		[NoTranslate]
		private List<string> tradeTagsSell = new List<string>();

		// Token: 0x04005A2F RID: 23087
		[NoTranslate]
		private List<string> tradeTagsBuy = new List<string>();

		// Token: 0x04005A30 RID: 23088
		private IntRange kindCountRange = new IntRange(1, 1);

		// Token: 0x04005A31 RID: 23089
		private float minWildness;

		// Token: 0x04005A32 RID: 23090
		private float maxWildness = 1f;

		// Token: 0x04005A33 RID: 23091
		private bool checkTemperature = true;

		// Token: 0x04005A34 RID: 23092
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
	}
}
