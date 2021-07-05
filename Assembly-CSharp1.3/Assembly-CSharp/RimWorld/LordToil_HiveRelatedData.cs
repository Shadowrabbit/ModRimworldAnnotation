using System;
using System.Collections.Generic;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020008B5 RID: 2229
	public class LordToil_HiveRelatedData : LordToilData
	{
		// Token: 0x06003AE2 RID: 15074 RVA: 0x0014931C File Offset: 0x0014751C
		public override void ExposeData()
		{
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				this.assignedHives.RemoveAll((KeyValuePair<Pawn, Hive> x) => x.Key.Destroyed);
			}
			Scribe_Collections.Look<Pawn, Hive>(ref this.assignedHives, "assignedHives", LookMode.Reference, LookMode.Reference);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.assignedHives.RemoveAll((KeyValuePair<Pawn, Hive> x) => x.Value == null);
			}
		}

		// Token: 0x04002021 RID: 8225
		public Dictionary<Pawn, Hive> assignedHives = new Dictionary<Pawn, Hive>();
	}
}
