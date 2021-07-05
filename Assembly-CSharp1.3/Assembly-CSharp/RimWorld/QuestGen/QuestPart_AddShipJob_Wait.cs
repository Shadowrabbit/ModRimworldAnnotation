using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x0200165E RID: 5726
	public abstract class QuestPart_AddShipJob_Wait : QuestPart_AddShipJob
	{
		// Token: 0x06008588 RID: 34184 RVA: 0x002FEB94 File Offset: 0x002FCD94
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<bool>(ref this.leaveImmediatelyWhenSatisfied, "leaveImmediatelyWhenSatisfied", false, false);
			Scribe_Values.Look<bool>(ref this.showGizmos, "showGizmos", false, false);
			Scribe_Collections.Look<Thing>(ref this.sendAwayIfAllDespawned, "sendAwayIfAllDespawned", LookMode.Reference, Array.Empty<object>());
			Scribe_Collections.Look<Thing>(ref this.sendAwayIfAnyDespawnedDownedOrDead, "sendAwayIfAnyDespawnedDownedOrDead", LookMode.Reference, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				List<Thing> list = this.sendAwayIfAllDespawned;
				if (list != null)
				{
					list.RemoveAll((Thing x) => x == null);
				}
				List<Thing> list2 = this.sendAwayIfAnyDespawnedDownedOrDead;
				if (list2 == null)
				{
					return;
				}
				list2.RemoveAll((Thing x) => x == null);
			}
		}

		// Token: 0x04005365 RID: 21349
		public bool leaveImmediatelyWhenSatisfied;

		// Token: 0x04005366 RID: 21350
		public bool showGizmos = true;

		// Token: 0x04005367 RID: 21351
		public List<Thing> sendAwayIfAllDespawned;

		// Token: 0x04005368 RID: 21352
		public List<Thing> sendAwayIfAnyDespawnedDownedOrDead;
	}
}
