using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02001DFE RID: 7678
	public struct ThingStuffPair : IEquatable<ThingStuffPair>
	{
		// Token: 0x17001967 RID: 6503
		// (get) Token: 0x0600A650 RID: 42576 RVA: 0x0006E02A File Offset: 0x0006C22A
		public float Price
		{
			get
			{
				return this.cachedPrice;
			}
		}

		// Token: 0x17001968 RID: 6504
		// (get) Token: 0x0600A651 RID: 42577 RVA: 0x0006E032 File Offset: 0x0006C232
		public float InsulationCold
		{
			get
			{
				return this.cachedInsulationCold;
			}
		}

		// Token: 0x17001969 RID: 6505
		// (get) Token: 0x0600A652 RID: 42578 RVA: 0x0006E03A File Offset: 0x0006C23A
		public float InsulationHeat
		{
			get
			{
				return this.cachedInsulationHeat;
			}
		}

		// Token: 0x1700196A RID: 6506
		// (get) Token: 0x0600A653 RID: 42579 RVA: 0x003037CC File Offset: 0x003019CC
		public float Commonality
		{
			get
			{
				float num = this.commonalityMultiplier;
				num *= this.thing.generateCommonality;
				if (this.stuff != null)
				{
					num *= this.stuff.stuffProps.commonality;
				}
				if (PawnWeaponGenerator.IsDerpWeapon(this.thing, this.stuff) || PawnApparelGenerator.IsDerpApparel(this.thing, this.stuff))
				{
					num *= 0.01f;
				}
				return num;
			}
		}

		// Token: 0x0600A654 RID: 42580 RVA: 0x00303838 File Offset: 0x00301A38
		public ThingStuffPair(ThingDef thing, ThingDef stuff, float commonalityMultiplier = 1f)
		{
			this.thing = thing;
			this.stuff = stuff;
			this.commonalityMultiplier = commonalityMultiplier;
			if (stuff != null && !thing.MadeFromStuff)
			{
				Log.Warning(string.Concat(new object[]
				{
					"Created ThingStuffPairWithQuality with stuff ",
					stuff,
					" but ",
					thing,
					" is not made from stuff."
				}), false);
				stuff = null;
			}
			this.cachedPrice = thing.GetStatValueAbstract(StatDefOf.MarketValue, stuff);
			this.cachedInsulationCold = thing.GetStatValueAbstract(StatDefOf.Insulation_Cold, stuff);
			this.cachedInsulationHeat = thing.GetStatValueAbstract(StatDefOf.Insulation_Heat, stuff);
		}

		// Token: 0x0600A655 RID: 42581 RVA: 0x003038D0 File Offset: 0x00301AD0
		public static List<ThingStuffPair> AllWith(Predicate<ThingDef> thingValidator)
		{
			List<ThingStuffPair> list = new List<ThingStuffPair>();
			List<ThingDef> allDefsListForReading = DefDatabase<ThingDef>.AllDefsListForReading;
			for (int i = 0; i < allDefsListForReading.Count; i++)
			{
				ThingDef thingDef = allDefsListForReading[i];
				if (thingValidator(thingDef))
				{
					if (!thingDef.MadeFromStuff)
					{
						list.Add(new ThingStuffPair(thingDef, null, 1f));
					}
					else
					{
						IEnumerable<ThingDef> enumerable = from st in DefDatabase<ThingDef>.AllDefs
						where st.IsStuff && st.stuffProps.CanMake(thingDef)
						select st;
						int num = enumerable.Count<ThingDef>();
						float num2 = enumerable.Average((ThingDef st) => st.stuffProps.commonality);
						float num3 = 1f / (float)num / num2;
						foreach (ThingDef thingDef2 in enumerable)
						{
							list.Add(new ThingStuffPair(thingDef, thingDef2, num3));
						}
					}
				}
			}
			return (from p in list
			orderby p.Price descending
			select p).ToList<ThingStuffPair>();
		}

		// Token: 0x0600A656 RID: 42582 RVA: 0x00303A18 File Offset: 0x00301C18
		public override string ToString()
		{
			if (this.thing == null)
			{
				return "(null)";
			}
			string text;
			if (this.stuff == null)
			{
				text = this.thing.label;
			}
			else
			{
				text = this.thing.label + " " + this.stuff.LabelAsStuff;
			}
			return string.Concat(new string[]
			{
				text,
				" $",
				this.Price.ToString("F0"),
				" c=",
				this.Commonality.ToString("F4")
			});
		}

		// Token: 0x0600A657 RID: 42583 RVA: 0x0006E042 File Offset: 0x0006C242
		public static bool operator ==(ThingStuffPair a, ThingStuffPair b)
		{
			return a.thing == b.thing && a.stuff == b.stuff && a.commonalityMultiplier == b.commonalityMultiplier;
		}

		// Token: 0x0600A658 RID: 42584 RVA: 0x0006E070 File Offset: 0x0006C270
		public static bool operator !=(ThingStuffPair a, ThingStuffPair b)
		{
			return !(a == b);
		}

		// Token: 0x0600A659 RID: 42585 RVA: 0x0006E07C File Offset: 0x0006C27C
		public override bool Equals(object obj)
		{
			return obj is ThingStuffPair && this.Equals((ThingStuffPair)obj);
		}

		// Token: 0x0600A65A RID: 42586 RVA: 0x0006E094 File Offset: 0x0006C294
		public bool Equals(ThingStuffPair other)
		{
			return this == other;
		}

		// Token: 0x0600A65B RID: 42587 RVA: 0x0006E0A2 File Offset: 0x0006C2A2
		public override int GetHashCode()
		{
			return Gen.HashCombineStruct<float>(Gen.HashCombine<ThingDef>(Gen.HashCombine<ThingDef>(0, this.thing), this.stuff), this.commonalityMultiplier);
		}

		// Token: 0x0600A65C RID: 42588 RVA: 0x0006E0C6 File Offset: 0x0006C2C6
		public static explicit operator ThingStuffPair(ThingStuffPairWithQuality p)
		{
			return new ThingStuffPair(p.thing, p.stuff, 1f);
		}

		// Token: 0x040070B5 RID: 28853
		public ThingDef thing;

		// Token: 0x040070B6 RID: 28854
		public ThingDef stuff;

		// Token: 0x040070B7 RID: 28855
		public float commonalityMultiplier;

		// Token: 0x040070B8 RID: 28856
		private float cachedPrice;

		// Token: 0x040070B9 RID: 28857
		private float cachedInsulationCold;

		// Token: 0x040070BA RID: 28858
		private float cachedInsulationHeat;
	}
}
