using System;
using RimWorld;

namespace Verse
{
	// Token: 0x0200048B RID: 1163
	public struct ThingDefCount : IEquatable<ThingDefCount>, IExposable
	{
		// Token: 0x170006AD RID: 1709
		// (get) Token: 0x06002379 RID: 9081 RVA: 0x000DDCE8 File Offset: 0x000DBEE8
		public ThingDef ThingDef
		{
			get
			{
				return this.thingDef;
			}
		}

		// Token: 0x170006AE RID: 1710
		// (get) Token: 0x0600237A RID: 9082 RVA: 0x000DDCF0 File Offset: 0x000DBEF0
		public int Count
		{
			get
			{
				return this.count;
			}
		}

		// Token: 0x170006AF RID: 1711
		// (get) Token: 0x0600237B RID: 9083 RVA: 0x000DDCF8 File Offset: 0x000DBEF8
		public string Label
		{
			get
			{
				return GenLabel.ThingLabel(this.thingDef, null, this.count);
			}
		}

		// Token: 0x170006B0 RID: 1712
		// (get) Token: 0x0600237C RID: 9084 RVA: 0x000DDD0C File Offset: 0x000DBF0C
		public string LabelCap
		{
			get
			{
				return this.Label.CapitalizeFirst(this.thingDef);
			}
		}

		// Token: 0x0600237D RID: 9085 RVA: 0x000DDD20 File Offset: 0x000DBF20
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
				}));
				count = 0;
			}
			this.thingDef = thingDef;
			this.count = count;
		}

		// Token: 0x0600237E RID: 9086 RVA: 0x000DDD6F File Offset: 0x000DBF6F
		public void ExposeData()
		{
			Scribe_Defs.Look<ThingDef>(ref this.thingDef, "thingDef");
			Scribe_Values.Look<int>(ref this.count, "count", 1, false);
		}

		// Token: 0x0600237F RID: 9087 RVA: 0x000DDD93 File Offset: 0x000DBF93
		public ThingDefCount WithCount(int newCount)
		{
			return new ThingDefCount(this.thingDef, newCount);
		}

		// Token: 0x06002380 RID: 9088 RVA: 0x000DDDA1 File Offset: 0x000DBFA1
		public override bool Equals(object obj)
		{
			return obj is ThingDefCount && this.Equals((ThingDefCount)obj);
		}

		// Token: 0x06002381 RID: 9089 RVA: 0x000DDDB9 File Offset: 0x000DBFB9
		public bool Equals(ThingDefCount other)
		{
			return this == other;
		}

		// Token: 0x06002382 RID: 9090 RVA: 0x000DDDC7 File Offset: 0x000DBFC7
		public static bool operator ==(ThingDefCount a, ThingDefCount b)
		{
			return a.thingDef == b.thingDef && a.count == b.count;
		}

		// Token: 0x06002383 RID: 9091 RVA: 0x000DDDE7 File Offset: 0x000DBFE7
		public static bool operator !=(ThingDefCount a, ThingDefCount b)
		{
			return !(a == b);
		}

		// Token: 0x06002384 RID: 9092 RVA: 0x000DDDF3 File Offset: 0x000DBFF3
		public override int GetHashCode()
		{
			return Gen.HashCombine<ThingDef>(this.count, this.thingDef);
		}

		// Token: 0x06002385 RID: 9093 RVA: 0x000DDE08 File Offset: 0x000DC008
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

		// Token: 0x06002386 RID: 9094 RVA: 0x000DDE63 File Offset: 0x000DC063
		public static implicit operator ThingDefCount(ThingDefCountClass t)
		{
			if (t == null)
			{
				return new ThingDefCount(null, 0);
			}
			return new ThingDefCount(t.thingDef, t.count);
		}

		// Token: 0x0400160D RID: 5645
		private ThingDef thingDef;

		// Token: 0x0400160E RID: 5646
		private int count;
	}
}
