using System;
using System.Collections.Generic;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020008D0 RID: 2256
	public class LordToilData_Gathering : LordToilData
	{
		// Token: 0x06003B54 RID: 15188 RVA: 0x0014B86C File Offset: 0x00149A6C
		public override void ExposeData()
		{
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				this.presentForTicks.RemoveAll((KeyValuePair<Pawn, int> x) => x.Key.Destroyed);
			}
			Scribe_Collections.Look<Pawn, int>(ref this.presentForTicks, "presentForTicks", LookMode.Reference, LookMode.Value, ref this.tmpPresentPawns, ref this.tmpPresentPawnsTicks);
		}

		// Token: 0x04002055 RID: 8277
		public Dictionary<Pawn, int> presentForTicks = new Dictionary<Pawn, int>();

		// Token: 0x04002056 RID: 8278
		private List<Pawn> tmpPresentPawns;

		// Token: 0x04002057 RID: 8279
		private List<int> tmpPresentPawnsTicks;
	}
}
