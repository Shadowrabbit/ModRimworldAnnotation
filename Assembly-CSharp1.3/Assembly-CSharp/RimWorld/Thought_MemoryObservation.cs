using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E9C RID: 3740
	public class Thought_MemoryObservation : Thought_Memory
	{
		// Token: 0x17000F57 RID: 3927
		// (set) Token: 0x060057E7 RID: 22503 RVA: 0x001DE36A File Offset: 0x001DC56A
		public virtual Thing Target
		{
			set
			{
				this.targetThingID = value.thingIDNumber;
			}
		}

		// Token: 0x060057E8 RID: 22504 RVA: 0x001DE378 File Offset: 0x001DC578
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.targetThingID, "targetThingID", 0, false);
		}

		// Token: 0x060057E9 RID: 22505 RVA: 0x001DE394 File Offset: 0x001DC594
		public override bool TryMergeWithExistingMemory(out bool showBubble)
		{
			ThoughtHandler thoughts = this.pawn.needs.mood.thoughts;
			Thought_MemoryObservation thought_MemoryObservation = null;
			List<Thought_Memory> memories = thoughts.memories.Memories;
			for (int i = 0; i < memories.Count; i++)
			{
				Thought_MemoryObservation thought_MemoryObservation2 = memories[i] as Thought_MemoryObservation;
				if (thought_MemoryObservation2 != null && thought_MemoryObservation2.def == this.def && thought_MemoryObservation2.targetThingID == this.targetThingID && (thought_MemoryObservation == null || thought_MemoryObservation2.age > thought_MemoryObservation.age))
				{
					thought_MemoryObservation = thought_MemoryObservation2;
				}
			}
			if (thought_MemoryObservation != null)
			{
				showBubble = (thought_MemoryObservation.age > thought_MemoryObservation.DurationTicks / 2);
				thought_MemoryObservation.Renew();
				return true;
			}
			showBubble = true;
			return false;
		}

		// Token: 0x040033D5 RID: 13269
		protected int targetThingID;
	}
}
