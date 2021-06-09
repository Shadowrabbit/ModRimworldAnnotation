using System;

namespace Verse
{
	// Token: 0x020007F0 RID: 2032
	public struct ThingDefCountRange : IEquatable<ThingDefCountRange>, IExposable
	{
		// Token: 0x170007CC RID: 1996
		// (get) Token: 0x0600336A RID: 13162 RVA: 0x000285AF File Offset: 0x000267AF
		public ThingDef ThingDef
		{
			get
			{
				return this.thingDef;
			}
		}

		// Token: 0x170007CD RID: 1997
		// (get) Token: 0x0600336B RID: 13163 RVA: 0x000285B7 File Offset: 0x000267B7
		public IntRange CountRange
		{
			get
			{
				return this.countRange;
			}
		}

		// Token: 0x170007CE RID: 1998
		// (get) Token: 0x0600336C RID: 13164 RVA: 0x000285BF File Offset: 0x000267BF
		public int Min
		{
			get
			{
				return this.countRange.min;
			}
		}

		// Token: 0x170007CF RID: 1999
		// (get) Token: 0x0600336D RID: 13165 RVA: 0x000285CC File Offset: 0x000267CC
		public int Max
		{
			get
			{
				return this.countRange.max;
			}
		}

		// Token: 0x170007D0 RID: 2000
		// (get) Token: 0x0600336E RID: 13166 RVA: 0x000285D9 File Offset: 0x000267D9
		public int TrueMin
		{
			get
			{
				return this.countRange.TrueMin;
			}
		}

		// Token: 0x170007D1 RID: 2001
		// (get) Token: 0x0600336F RID: 13167 RVA: 0x000285E6 File Offset: 0x000267E6
		public int TrueMax
		{
			get
			{
				return this.countRange.TrueMax;
			}
		}

		// Token: 0x06003370 RID: 13168 RVA: 0x000285F3 File Offset: 0x000267F3
		public ThingDefCountRange(ThingDef thingDef, int min, int max)
		{
			this = new ThingDefCountRange(thingDef, new IntRange(min, max));
		}

		// Token: 0x06003371 RID: 13169 RVA: 0x00028603 File Offset: 0x00026803
		public ThingDefCountRange(ThingDef thingDef, IntRange countRange)
		{
			this.thingDef = thingDef;
			this.countRange = countRange;
		}

		// Token: 0x06003372 RID: 13170 RVA: 0x0014F970 File Offset: 0x0014DB70
		public void ExposeData()
		{
			Scribe_Defs.Look<ThingDef>(ref this.thingDef, "thingDef");
			Scribe_Values.Look<IntRange>(ref this.countRange, "countRange", default(IntRange), false);
		}

		// Token: 0x06003373 RID: 13171 RVA: 0x00028613 File Offset: 0x00026813
		public ThingDefCountRange WithCountRange(IntRange newCountRange)
		{
			return new ThingDefCountRange(this.thingDef, newCountRange);
		}

		// Token: 0x06003374 RID: 13172 RVA: 0x00028621 File Offset: 0x00026821
		public ThingDefCountRange WithCountRange(int newMin, int newMax)
		{
			return new ThingDefCountRange(this.thingDef, newMin, newMax);
		}

		// Token: 0x06003375 RID: 13173 RVA: 0x00028630 File Offset: 0x00026830
		public override bool Equals(object obj)
		{
			return obj is ThingDefCountRange && this.Equals((ThingDefCountRange)obj);
		}

		// Token: 0x06003376 RID: 13174 RVA: 0x00028648 File Offset: 0x00026848
		public bool Equals(ThingDefCountRange other)
		{
			return this == other;
		}

		// Token: 0x06003377 RID: 13175 RVA: 0x00028656 File Offset: 0x00026856
		public static bool operator ==(ThingDefCountRange a, ThingDefCountRange b)
		{
			return a.thingDef == b.thingDef && a.countRange == b.countRange;
		}

		// Token: 0x06003378 RID: 13176 RVA: 0x00028679 File Offset: 0x00026879
		public static bool operator !=(ThingDefCountRange a, ThingDefCountRange b)
		{
			return !(a == b);
		}

		// Token: 0x06003379 RID: 13177 RVA: 0x00028685 File Offset: 0x00026885
		public override int GetHashCode()
		{
			return Gen.HashCombine<ThingDef>(this.countRange.GetHashCode(), this.thingDef);
		}

		// Token: 0x0600337A RID: 13178 RVA: 0x0014F9A8 File Offset: 0x0014DBA8
		public override string ToString()
		{
			return string.Concat(new object[]
			{
				"(",
				this.countRange,
				"x ",
				(this.thingDef != null) ? this.thingDef.defName : "null",
				")"
			});
		}

		// Token: 0x0600337B RID: 13179 RVA: 0x000286A3 File Offset: 0x000268A3
		public static implicit operator ThingDefCountRange(ThingDefCountRangeClass t)
		{
			return new ThingDefCountRange(t.thingDef, t.countRange);
		}

		// Token: 0x0600337C RID: 13180 RVA: 0x000286B6 File Offset: 0x000268B6
		public static explicit operator ThingDefCountRange(ThingDefCount t)
		{
			return new ThingDefCountRange(t.ThingDef, t.Count, t.Count);
		}

		// Token: 0x0600337D RID: 13181 RVA: 0x000286D2 File Offset: 0x000268D2
		public static explicit operator ThingDefCountRange(ThingDefCountClass t)
		{
			return new ThingDefCountRange(t.thingDef, t.count, t.count);
		}

		// Token: 0x04002345 RID: 9029
		private ThingDef thingDef;

		// Token: 0x04002346 RID: 9030
		private IntRange countRange;
	}
}
