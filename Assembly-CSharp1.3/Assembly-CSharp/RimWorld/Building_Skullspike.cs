using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001085 RID: 4229
	public class Building_Skullspike : Building, IObservedThoughtGiver
	{
		// Token: 0x060064A7 RID: 25767 RVA: 0x0021E928 File Offset: 0x0021CB28
		public Thought_Memory GiveObservedThought(Pawn observer)
		{
			if ((observer.Ideo != null && observer.Ideo.IdeoApprovesOfSlavery()) || !observer.IsSlave)
			{
				return null;
			}
			Thought_MemoryObservation thought_MemoryObservation = (Thought_MemoryObservation)ThoughtMaker.MakeThought(ThoughtDefOf.ObservedSkullspike);
			thought_MemoryObservation.Target = this;
			return thought_MemoryObservation;
		}

		// Token: 0x060064A8 RID: 25768 RVA: 0x00002688 File Offset: 0x00000888
		public HistoryEventDef GiveObservedHistoryEvent(Pawn observer)
		{
			return null;
		}
	}
}
