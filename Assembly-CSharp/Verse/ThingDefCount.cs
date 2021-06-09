using System;
using RimWorld;

namespace Verse
{
	// Token: 0x020007EE RID: 2030
	public struct ThingDefCount : IEquatable<ThingDefCount>, IExposable
	{
		// Token: 0x170007C5 RID: 1989
		// (get) Token: 0x06003352 RID: 13138 RVA: 0x0002841B File Offset: 0x0002661B
		public ThingDef ThingDef
		{
			get
			{
				return this.thingDef;
			}
		}

		// Token: 0x170007C6 RID: 1990
		// (get) Token: 0x06003353 RID: 13139 RVA: 0x00028423 File Offset: 0x00026623
		public int Count
		{
			get
			{
				return this.count;
			}
		}

		// Token: 0x170007C7 RID: 1991
		// (get) Token: 0x06003354 RID: 13140 RVA: 0x0002842B File Offset: 0x0002662B
		public string Label
		{
			get
			{
				return GenLabel.ThingLabel(this.thingDef, null, this.count);
			}
		}

		// Token: 0x170007C8 RID: 1992
		// (get) Token: 0x06003355 RID: 13141 RVA: 0x0002843F File Offset: 0x0002663F
		public string LabelCap
		{
			get
			{
				return this.Label.CapitalizeFirst(this.thingDef);
			}
		}

		// Token: 0x06003356 RID: 13142 RVA: 0x0014F7B4 File Offset: 0x0014D9B4
		public ThingDefCount(ThingDef thingDef, int count)
		{
			if (count < 0)
			{
				Log.Warning(string.Concat(new object[]
				{
					"Tried to set ThingDefCount count to ",
					count,
					". thingDef=",
					thingDef
				}), false);
				count = 0;
			}
			this.thingDef = thingDef;
			this.count = count;
		}

		// Token: 0x06003357 RID: 13143 RVA: 0x00028452 File Offset: 0x00026652
		public void ExposeData()
		{
			Scribe_Defs.Look<ThingDef>(ref this.thingDef, "thingDef");
			Scribe_Values.Look<int>(ref this.count, "count", 1, false);
		}

		// Token: 0x06003358 RID: 13144 RVA: 0x00028476 File Offset: 0x00026676
		public ThingDefCount WithCount(int newCount)
		{
			return new ThingDefCount(this.thingDef, newCount);
		}

		// Token: 0x06003359 RID: 13145 RVA: 0x00028484 File Offset: 0x00026684
		public override bool Equals(object obj)
		{
			return obj is ThingDefCount && this.Equals((ThingDefCount)obj);
		}

		// Token: 0x0600335A RID: 13146 RVA: 0x0002849C File Offset: 0x0002669C
		public bool Equals(ThingDefCount other)
		{
			return this == other;
		}

		// Token: 0x0600335B RID: 13147 RVA: 0x000284AA File Offset: 0x000266AA
		public static bool operator ==(ThingDefCount a, ThingDefCount b)
		{
			return a.thingDef == b.thingDef && a.count == b.count;
		}

		// Token: 0x0600335C RID: 13148 RVA: 0x000284CA File Offset: 0x000266CA
		public static bool operator !=(ThingDefCount a, ThingDefCount b)
		{
			return !(a == b);
		}

		// Token: 0x0600335D RID: 13149 RVA: 0x000284D6 File Offset: 0x000266D6
		public override int GetHashCode()
		{
			return Gen.HashCombine<ThingDef>(this.count, this.thingDef);
		}

		// Token: 0x0600335E RID: 13150 RVA: 0x0014F804 File Offset: 0x0014DA04
		public override string ToString()
		{
			return string.Concat(new object[]
			{
				"(",
				this.count,
				"x ",
				(this.thingDef != null) ? this.thingDef.defName : "null",
				")"
			});
		}

		// Token: 0x0600335F RID: 13151 RVA: 0x000284E9 File Offset: 0x000266E9
		public static implicit operator ThingDefCount(ThingDefCountClass t)
		{
			if (t == null)
			{
				return new ThingDefCount(null, 0);
			}
			return new ThingDefCount(t.thingDef, t.count);
		}

		// Token: 0x04002341 RID: 9025
		private ThingDef thingDef;

		// Token: 0x04002342 RID: 9026
		private int count;
	}
}
