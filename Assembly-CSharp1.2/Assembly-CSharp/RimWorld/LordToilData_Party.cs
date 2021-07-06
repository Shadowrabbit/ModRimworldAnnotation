using System;
using System.Collections.Generic;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000E1B RID: 3611
	public class LordToilData_Party : LordToilData
	{
		// Token: 0x060051FD RID: 20989 RVA: 0x001BD37C File Offset: 0x001BB57C
		public override void ExposeData()
		{
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				this.presentForTicks.RemoveAll((KeyValuePair<Pawn, int> x) => x.Key.Destroyed);
			}
			Scribe_Collections.Look<Pawn, int>(ref this.presentForTicks, "presentForTicks", LookMode.Reference, LookMode.Undefined);
		}

		// Token: 0x0400347D RID: 13437
		public Dictionary<Pawn, int> presentForTicks = new Dictionary<Pawn, int>();
	}
}
