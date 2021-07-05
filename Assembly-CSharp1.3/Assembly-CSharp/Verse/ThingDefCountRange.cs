using System;

namespace Verse
{
	// Token: 0x0200048D RID: 1165
	public struct ThingDefCountRange : IEquatable<ThingDefCountRange>, IExposable
	{
		// Token: 0x170006B4 RID: 1716
		// (get) Token: 0x06002391 RID: 9105 RVA: 0x000DE03B File Offset: 0x000DC23B
		public ThingDef ThingDef
		{
			get
			{
				return this.thingDef;
			}
		}

		// Token: 0x170006B5 RID: 1717
		// (get) Token: 0x06002392 RID: 9106 RVA: 0x000DE043 File Offset: 0x000DC243
		public IntRange CountRange
		{
			get
			{
				return this.countRange;
			}
		}

		// Token: 0x170006B6 RID: 1718
		// (get) Token: 0x06002393 RID: 9107 RVA: 0x000DE04B File Offset: 0x000DC24B
		public int Min
		{
			get
			{
				return this.countRange.min;
			}
		}

		// Token: 0x170006B7 RID: 1719
		// (get) Token: 0x06002394 RID: 9108 RVA: 0x000DE058 File Offset: 0x000DC258
		public int Max
		{
			get
			{
				return this.countRange.max;
			}
		}

		// Token: 0x170006B8 RID: 1720
		// (get) Token: 0x06002395 RID: 9109 RVA: 0x000DE065 File Offset: 0x000DC265
		public int TrueMin
		{
			get
			{
				return this.countRange.TrueMin;
			}
		}

		// Token: 0x170006B9 RID: 1721
		// (get) Token: 0x06002396 RID: 9110 RVA: 0x000DE072 File Offset: 0x000DC272
		public int TrueMax
		{
			get
			{
				return this.countRange.TrueMax;
			}
		}

		// Token: 0x06002397 RID: 9111 RVA: 0x000DE07F File Offset: 0x000DC27F
		public ThingDefCountRange(ThingDef thingDef, int min, int max)
		{
			this = new ThingDefCountRange(thingDef, new IntRange(min, max));
		}

		// Token: 0x06002398 RID: 9112 RVA: 0x000DE08F File Offset: 0x000DC28F
		public ThingDefCountRange(ThingDef thingDef, IntRange countRange)
		{
			this.thingDef = thingDef;
			this.countRange = countRange;
		}

		// Token: 0x06002399 RID: 9113 RVA: 0x000DE0A0 File Offset: 0x000DC2A0
		public void ExposeData()
		{
			Scribe_Defs.Look<ThingDef>(ref this.thingDef, "thingDef");
			Scribe_Values.Look<IntRange>(ref this.countRange, "countRange", default(IntRange), false);
		}

		// Token: 0x0600239A RID: 9114 RVA: 0x000DE0D7 File Offset: 0x000DC2D7
		public ThingDefCountRange WithCountRange(IntRange newCountRange)
		{
			return new ThingDefCountRange(this.thingDef, newCountRange);
		}

		// Token: 0x0600239B RID: 9115 RVA: 0x000DE0E5 File Offset: 0x000DC2E5
		public ThingDefCountRange WithCountRange(int newMin, int newMax)
		{
			return new ThingDefCountRange(this.thingDef, newMin, newMax);
		}

		// Token: 0x0600239C RID: 9116 RVA: 0x000DE0F4 File Offset: 0x000DC2F4
		public override bool Equals(object obj)
		{
			return obj is ThingDefCountRange && this.Equals((ThingDefCountRange)obj);
		}

		// Token: 0x0600239D RID: 9117 RVA: 0x000DE10C File Offset: 0x000DC30C
		public bool Equals(ThingDefCountRange other)
		{
			return this == other;
		}

		// Token: 0x0600239E RID: 9118 RVA: 0x000DE11A File Offset: 0x000DC31A
		public static bool operator ==(ThingDefCountRange a, ThingDefCountRange b)
		{
			return a.thingDef == b.thingDef && a.countRange == b.countRange;
		}

		// Token: 0x0600239F RID: 9119 RVA: 0x000DE13D File Offset: 0x000DC33D
		public static bool operator !=(ThingDefCountRange a, ThingDefCountRange b)
		{
			return !(a == b);
		}

		// Token: 0x060023A0 RID: 9120 RVA: 0x000DE149 File Offset: 0x000DC349
		public override int GetHashCode()
		{
			return Gen.HashCombine<ThingDef>(this.countRange.GetHashCode(), this.thingDef);
		}

		// Token: 0x060023A1 RID: 9121 RVA: 0x000DE168 File Offset: 0x000DC368
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

		// Token: 0x060023A2 RID: 9122 RVA: 0x000DE1C3 File Offset: 0x000DC3C3
		public static implicit operator ThingDefCountRange(ThingDefCountRangeClass t)
		{
			return new ThingDefCountRange(t.thingDef, t.countRange);
		}

		// Token: 0x060023A3 RID: 9123 RVA: 0x000DE1D6 File Offset: 0x000DC3D6
		public static explicit operator ThingDefCountRange(ThingDefCount t)
		{
			return new ThingDefCountRange(t.ThingDef, t.Count, t.Count);
		}

		// Token: 0x060023A4 RID: 9124 RVA: 0x000DE1F2 File Offset: 0x000DC3F2
		public static explicit operator ThingDefCountRange(ThingDefCountClass t)
		{
			return new ThingDefCountRange(t.thingDef, t.count, t.count);
		}

		// Token: 0x04001611 RID: 5649
		private ThingDef thingDef;

		// Token: 0x04001612 RID: 5650
		private IntRange countRange;
	}
}
