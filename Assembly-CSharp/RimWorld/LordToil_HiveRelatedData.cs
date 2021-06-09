using System;
using System.Collections.Generic;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000DFE RID: 3582
	public class LordToil_HiveRelatedData : LordToilData
	{
		// Token: 0x06005181 RID: 20865 RVA: 0x001BBAE4 File Offset: 0x001B9CE4
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

		// Token: 0x04003444 RID: 13380
		public Dictionary<Pawn, Hive> assignedHives = new Dictionary<Pawn, Hive>();
	}
}
