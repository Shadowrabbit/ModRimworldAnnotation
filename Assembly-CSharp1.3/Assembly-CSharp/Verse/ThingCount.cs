using System;

namespace Verse
{
	// Token: 0x02000488 RID: 1160
	public struct ThingCount : IEquatable<ThingCount>, IExposable
	{
		// Token: 0x170006AA RID: 1706
		// (get) Token: 0x06002364 RID: 9060 RVA: 0x000DD8F2 File Offset: 0x000DBAF2
		public Thing Thing
		{
			get
			{
				return this.thing;
			}
		}

		// Token: 0x170006AB RID: 1707
		// (get) Token: 0x06002365 RID: 9061 RVA: 0x000DD8FA File Offset: 0x000DBAFA
		public int Count
		{
			get
			{
				return this.count;
			}
		}

		// Token: 0x06002366 RID: 9062 RVA: 0x000DD904 File Offset: 0x000DBB04
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
				}));
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
				}));
				count = thing.stackCount;
			}
			this.thing = thing;
			this.count = count;
		}

		// Token: 0x06002367 RID: 9063 RVA: 0x000DD9A7 File Offset: 0x000DBBA7
		public void ExposeData()
		{
			Scribe_References.Look<Thing>(ref this.thing, "thing", false);
			Scribe_Values.Look<int>(ref this.count, "count", 1, false);
		}

		// Token: 0x06002368 RID: 9064 RVA: 0x000DD9CC File Offset: 0x000DBBCC
		public ThingCount WithCount(int newCount)
		{
			return new ThingCount(this.thing, newCount);
		}

		// Token: 0x06002369 RID: 9065 RVA: 0x000DD9DA File Offset: 0x000DBBDA
		public override bool Equals(object obj)
		{
			return obj is ThingCount && this.Equals((ThingCount)obj);
		}

		// Token: 0x0600236A RID: 9066 RVA: 0x000DD9F2 File Offset: 0x000DBBF2
		public bool Equals(ThingCount other)
		{
			return this == other;
		}

		// Token: 0x0600236B RID: 9067 RVA: 0x000DDA00 File Offset: 0x000DBC00
		public static bool operator ==(ThingCount a, ThingCount b)
		{
			return a.thing == b.thing && a.count == b.count;
		}

		// Token: 0x0600236C RID: 9068 RVA: 0x000DDA20 File Offset: 0x000DBC20
		public static bool operator !=(ThingCount a, ThingCount b)
		{
			return !(a == b);
		}

		// Token: 0x0600236D RID: 9069 RVA: 0x000DDA2C File Offset: 0x000DBC2C
		public override int GetHashCode()
		{
			return Gen.HashCombine<Thing>(this.count, this.thing);
		}

		// Token: 0x0600236E RID: 9070 RVA: 0x000DDA40 File Offset: 0x000DBC40
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

		// Token: 0x0600236F RID: 9071 RVA: 0x000DDA9B File Offset: 0x000DBC9B
		public static implicit operator ThingCount(ThingCountClass t)
		{
			if (t == null)
			{
				return new ThingCount(null, 0);
			}
			return new ThingCount(t.thing, t.Count);
		}

		// Token: 0x04001609 RID: 5641
		private Thing thing;

		// Token: 0x0400160A RID: 5642
		private int count;
	}
}
