using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020012DD RID: 4829
	internal struct StrikeRecord : IExposable
	{
		// Token: 0x17001013 RID: 4115
		// (get) Token: 0x06006872 RID: 26738 RVA: 0x000471B7 File Offset: 0x000453B7
		public bool Expired
		{
			get
			{
				return Find.TickManager.TicksGame > this.ticksGame + 900000;
			}
		}

		// Token: 0x06006873 RID: 26739 RVA: 0x00203698 File Offset: 0x00201898
		public void ExposeData()
		{
			Scribe_Values.Look<IntVec3>(ref this.cell, "cell", default(IntVec3), false);
			Scribe_Values.Look<int>(ref this.ticksGame, "ticksGame", 0, false);
			Scribe_Defs.Look<ThingDef>(ref this.def, "def");
		}

		// Token: 0x06006874 RID: 26740 RVA: 0x002036E4 File Offset: 0x002018E4
		public override string ToString()
		{
			return string.Concat(new object[]
			{
				"(",
				this.cell,
				", ",
				this.def,
				", ",
				this.ticksGame,
				")"
			});
		}

		// Token: 0x0400458E RID: 17806
		public IntVec3 cell;

		// Token: 0x0400458F RID: 17807
		public int ticksGame;

		// Token: 0x04004590 RID: 17808
		public ThingDef def;

		// Token: 0x04004591 RID: 17809
		private const int StrikeRecordExpiryDays = 15;
	}
}
