using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x0200157A RID: 5498
	public struct ThingStuffPair : IEquatable<ThingStuffPair>
	{
		// Token: 0x170015FB RID: 5627
		// (get) Token: 0x060081F7 RID: 33271 RVA: 0x002DEE01 File Offset: 0x002DD001
		public float Price
		{
			get
			{
				return this.cachedPrice;
			}
		}

		// Token: 0x170015FC RID: 5628
		// (get) Token: 0x060081F8 RID: 33272 RVA: 0x002DEE09 File Offset: 0x002DD009
		public float InsulationCold
		{
			get
			{
				return this.cachedInsulationCold;
			}
		}

		// Token: 0x170015FD RID: 5629
		// (get) Token: 0x060081F9 RID: 33273 RVA: 0x002DEE11 File Offset: 0x002DD011
		public float InsulationHeat
		{
			get
			{
				return this.cachedInsulationHeat;
			}
		}

		// Token: 0x170015FE RID: 5630
		// (get) Token: 0x060081FA RID: 33274 RVA: 0x002DEE1C File Offset: 0x002DD01C
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

		// Token: 0x060081FB RID: 33275 RVA: 0x002DEE88 File Offset: 0x002DD088
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
				}));
				stuff = null;
			}
			this.cachedPrice = thing.GetStatValueAbstract(StatDefOf.MarketValue, stuff);
			this.cachedInsulationCold = thing.GetStatValueAbstract(StatDefOf.Insulation_Cold, stuff);
			this.cachedInsulationHeat = thing.GetStatValueAbstract(StatDefOf.Insulation_Heat, stuff);
		}

		// Token: 0x060081FC RID: 33276 RVA: 0x002DEF20 File Offset: 0x002DD120
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

		// Token: 0x060081FD RID: 33277 RVA: 0x002DF068 File Offset: 0x002DD268
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

		// Token: 0x060081FE RID: 33278 RVA: 0x002DF105 File Offset: 0x002DD305
		public static bool operator ==(ThingStuffPair a, ThingStuffPair b)
		{
			return a.thing == b.thing && a.stuff == b.stuff && a.commonalityMultiplier == b.commonalityMultiplier;
		}

		// Token: 0x060081FF RID: 33279 RVA: 0x002DF133 File Offset: 0x002DD333
		public static bool operator !=(ThingStuffPair a, ThingStuffPair b)
		{
			return !(a == b);
		}

		// Token: 0x06008200 RID: 33280 RVA: 0x002DF13F File Offset: 0x002DD33F
		public override bool Equals(object obj)
		{
			return obj is ThingStuffPair && this.Equals((ThingStuffPair)obj);
		}

		// Token: 0x06008201 RID: 33281 RVA: 0x002DF157 File Offset: 0x002DD357
		public bool Equals(ThingStuffPair other)
		{
			return this == other;
		}

		// Token: 0x06008202 RID: 33282 RVA: 0x002DF165 File Offset: 0x002DD365
		public override int GetHashCode()
		{
			return Gen.HashCombineStruct<float>(Gen.HashCombine<ThingDef>(Gen.HashCombine<ThingDef>(0, this.thing), this.stuff), this.commonalityMultiplier);
		}

		// Token: 0x06008203 RID: 33283 RVA: 0x002DF189 File Offset: 0x002DD389
		public static explicit operator ThingStuffPair(ThingStuffPairWithQuality p)
		{
			return new ThingStuffPair(p.thing, p.stuff, 1f);
		}

		// Token: 0x040050DB RID: 20699
		public ThingDef thing;

		// Token: 0x040050DC RID: 20700
		public ThingDef stuff;

		// Token: 0x040050DD RID: 20701
		public float commonalityMultiplier;

		// Token: 0x040050DE RID: 20702
		private float cachedPrice;

		// Token: 0x040050DF RID: 20703
		private float cachedInsulationCold;

		// Token: 0x040050E0 RID: 20704
		private float cachedInsulationHeat;
	}
}
