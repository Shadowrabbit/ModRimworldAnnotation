using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000CC5 RID: 3269
	internal struct StrikeRecord : IExposable
	{
		// Token: 0x17000D20 RID: 3360
		// (get) Token: 0x06004C0D RID: 19469 RVA: 0x00195C9D File Offset: 0x00193E9D
		public bool Expired
		{
			get
			{
				return Find.TickManager.TicksGame > this.ticksGame + 900000;
			}
		}

		// Token: 0x06004C0E RID: 19470 RVA: 0x00195CB8 File Offset: 0x00193EB8
		public void ExposeData()
		{
			Scribe_Values.Look<IntVec3>(ref this.cell, "cell", default(IntVec3), false);
			Scribe_Values.Look<int>(ref this.ticksGame, "ticksGame", 0, false);
			Scribe_Defs.Look<ThingDef>(ref this.def, "def");
		}

		// Token: 0x06004C0F RID: 19471 RVA: 0x00195D04 File Offset: 0x00193F04
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

		// Token: 0x04002E09 RID: 11785
		public IntVec3 cell;

		// Token: 0x04002E0A RID: 11786
		public int ticksGame;

		// Token: 0x04002E0B RID: 11787
		public ThingDef def;

		// Token: 0x04002E0C RID: 11788
		private const int StrikeRecordExpiryDays = 15;
	}
}
