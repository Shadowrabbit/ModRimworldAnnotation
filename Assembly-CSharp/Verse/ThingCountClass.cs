using System;

namespace Verse
{
	// Token: 0x020007ED RID: 2029
	public sealed class ThingCountClass : IExposable
	{
		// Token: 0x170007C4 RID: 1988
		// (get) Token: 0x0600334B RID: 13131 RVA: 0x000283C3 File Offset: 0x000265C3
		// (set) Token: 0x0600334C RID: 13132 RVA: 0x0014F68C File Offset: 0x0014D88C
		public int Count
		{
			get
			{
				return this.countInt;
			}
			set
			{
				if (value < 0)
				{
					Log.Warning(string.Concat(new object[]
					{
						"Tried to set ThingCountClass stack count to ",
						value,
						". thing=",
						this.thing
					}), false);
					this.countInt = 0;
					return;
				}
				if (this.thing != null && value > this.thing.stackCount)
				{
					Log.Warning(string.Concat(new object[]
					{
						"Tried to set ThingCountClass stack count to ",
						value,
						", but thing's stack count is only ",
						this.thing.stackCount,
						". thing=",
						this.thing
					}), false);
					this.countInt = this.thing.stackCount;
					return;
				}
				this.countInt = value;
			}
		}

		// Token: 0x0600334D RID: 13133 RVA: 0x00006B8B File Offset: 0x00004D8B
		public ThingCountClass()
		{
		}

		// Token: 0x0600334E RID: 13134 RVA: 0x000283CB File Offset: 0x000265CB
		public ThingCountClass(Thing thing, int count)
		{
			this.thing = thing;
			this.Count = count;
		}

		// Token: 0x0600334F RID: 13135 RVA: 0x000283E1 File Offset: 0x000265E1
		public void ExposeData()
		{
			Scribe_References.Look<Thing>(ref this.thing, "thing", false);
			Scribe_Values.Look<int>(ref this.countInt, "count", 1, false);
		}

		// Token: 0x06003350 RID: 13136 RVA: 0x0014F758 File Offset: 0x0014D958
		public override string ToString()
		{
			return string.Concat(new object[]
			{
				"(",
				this.Count,
				"x ",
				(this.thing != null) ? this.thing.LabelShort : "null",
				")"
			});
		}

		// Token: 0x06003351 RID: 13137 RVA: 0x00028406 File Offset: 0x00026606
		public static implicit operator ThingCountClass(ThingCount t)
		{
			return new ThingCountClass(t.Thing, t.Count);
		}

		// Token: 0x0400233F RID: 9023
		public Thing thing;

		// Token: 0x04002340 RID: 9024
		private int countInt;
	}
}
