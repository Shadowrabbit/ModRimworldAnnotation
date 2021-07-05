using System;

namespace Verse
{
	// Token: 0x0200048A RID: 1162
	public sealed class ThingCountClass : IExposable
	{
		// Token: 0x170006AC RID: 1708
		// (get) Token: 0x06002372 RID: 9074 RVA: 0x000DDB69 File Offset: 0x000DBD69
		// (set) Token: 0x06002373 RID: 9075 RVA: 0x000DDB74 File Offset: 0x000DBD74
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
					}));
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
					}));
					this.countInt = this.thing.stackCount;
					return;
				}
				this.countInt = value;
			}
		}

		// Token: 0x06002374 RID: 9076 RVA: 0x000033AC File Offset: 0x000015AC
		public ThingCountClass()
		{
		}

		// Token: 0x06002375 RID: 9077 RVA: 0x000DDC3B File Offset: 0x000DBE3B
		public ThingCountClass(Thing thing, int count)
		{
			this.thing = thing;
			this.Count = count;
		}

		// Token: 0x06002376 RID: 9078 RVA: 0x000DDC51 File Offset: 0x000DBE51
		public void ExposeData()
		{
			Scribe_References.Look<Thing>(ref this.thing, "thing", false);
			Scribe_Values.Look<int>(ref this.countInt, "count", 1, false);
		}

		// Token: 0x06002377 RID: 9079 RVA: 0x000DDC78 File Offset: 0x000DBE78
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

		// Token: 0x06002378 RID: 9080 RVA: 0x000DDCD3 File Offset: 0x000DBED3
		public static implicit operator ThingCountClass(ThingCount t)
		{
			return new ThingCountClass(t.Thing, t.Count);
		}

		// Token: 0x0400160B RID: 5643
		public Thing thing;

		// Token: 0x0400160C RID: 5644
		private int countInt;
	}
}
