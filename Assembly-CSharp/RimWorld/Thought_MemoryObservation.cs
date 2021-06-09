using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001566 RID: 5478
	public class Thought_MemoryObservation : Thought_Memory
	{
		// Token: 0x17001267 RID: 4711
		// (set) Token: 0x060076D5 RID: 30421 RVA: 0x000502FD File Offset: 0x0004E4FD
		public Thing Target
		{
			set
			{
				this.targetThingID = value.thingIDNumber;
			}
		}

		// Token: 0x060076D6 RID: 30422 RVA: 0x0005030B File Offset: 0x0004E50B
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.targetThingID, "targetThingID", 0, false);
		}

		// Token: 0x060076D7 RID: 30423 RVA: 0x00242AF8 File Offset: 0x00240CF8
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
				showBubble = (thought_MemoryObservation.age > thought_MemoryObservation.def.DurationTicks / 2);
				thought_MemoryObservation.Renew();
				return true;
			}
			showBubble = true;
			return false;
		}

		// Token: 0x04004E64 RID: 20068
		private int targetThingID;
	}
}
