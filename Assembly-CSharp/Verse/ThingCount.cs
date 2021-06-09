using System;

namespace Verse
{
	// Token: 0x020007EB RID: 2027
	public struct ThingCount : IEquatable<ThingCount>, IExposable
	{
		// Token: 0x170007C2 RID: 1986
		// (get) Token: 0x0600333D RID: 13117 RVA: 0x000282FD File Offset: 0x000264FD
		public Thing Thing
		{
			get
			{
				return this.thing;
			}
		}

		// Token: 0x170007C3 RID: 1987
		// (get) Token: 0x0600333E RID: 13118 RVA: 0x00028305 File Offset: 0x00026505
		public int Count
		{
			get
			{
				return this.count;
			}
		}

		// Token: 0x0600333F RID: 13119 RVA: 0x0014F4D8 File Offset: 0x0014D6D8
		public ThingCount(Thing thing, int count)
		{
			if (count < 0)
			{
				Log.Warning(string.Concat(new object[]
				{
					"Tried to set ThingCount stack count to ",
					count,
					". thing=",
					thing
				}), false);
				count = 0;
			}
			if (count > thing.stackCount)
			{
				Log.Warning(string.Concat(new object[]
				{
					"Tried to set ThingCount stack count to ",
					count,
					", but thing's stack count is only ",
					thing.stackCount,
					". thing=",
					thing
				}), false);
				count = thing.stackCount;
			}
			this.thing = thing;
			this.count = count;
		}

		// Token: 0x06003340 RID: 13120 RVA: 0x0002830D File Offset: 0x0002650D
		public void ExposeData()
		{
			Scribe_References.Look<Thing>(ref this.thing, "thing", false);
			Scribe_Values.Look<int>(ref this.count, "count", 1, false);
		}

		// Token: 0x06003341 RID: 13121 RVA: 0x00028332 File Offset: 0x00026532
		public ThingCount WithCount(int newCount)
		{
			return new ThingCount(this.thing, newCount);
		}

		// Token: 0x06003342 RID: 13122 RVA: 0x00028340 File Offset: 0x00026540
		public override bool Equals(object obj)
		{
			return obj is ThingCount && this.Equals((ThingCount)obj);
		}

		// Token: 0x06003343 RID: 13123 RVA: 0x00028358 File Offset: 0x00026558
		public bool Equals(ThingCount other)
		{
			return this == other;
		}

		// Token: 0x06003344 RID: 13124 RVA: 0x00028366 File Offset: 0x00026566
		public static bool operator ==(ThingCount a, ThingCount b)
		{
			return a.thing == b.thing && a.count == b.count;
		}

		// Token: 0x06003345 RID: 13125 RVA: 0x00028386 File Offset: 0x00026586
		public static bool operator !=(ThingCount a, ThingCount b)
		{
			return !(a == b);
		}

		// Token: 0x06003346 RID: 13126 RVA: 0x00028392 File Offset: 0x00026592
		public override int GetHashCode()
		{
			return Gen.HashCombine<Thing>(this.count, this.thing);
		}

		// Token: 0x06003347 RID: 13127 RVA: 0x0014F580 File Offset: 0x0014D780
		public override string ToString()
		{
			return string.Concat(new object[]
			{
				"(",
				this.count,
				"x ",
				(this.thing != null) ? this.thing.LabelShort : "null",
				")"
			});
		}

		// Token: 0x06003348 RID: 13128 RVA: 0x000283A5 File Offset: 0x000265A5
		public static implicit operator ThingCount(ThingCountClass t)
		{
			if (t == null)
			{
				return new ThingCount(null, 0);
			}
			return new ThingCount(t.thing, t.Count);
		}

		// Token: 0x0400233D RID: 9021
		private Thing thing;

		// Token: 0x0400233E RID: 9022
		private int count;
	}
}
