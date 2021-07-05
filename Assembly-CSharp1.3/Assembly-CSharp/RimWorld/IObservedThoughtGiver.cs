using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E5B RID: 3675
	public interface IObservedThoughtGiver
	{
		// Token: 0x0600550B RID: 21771
		Thought_Memory GiveObservedThought(Pawn observer);

		// Token: 0x0600550C RID: 21772
		HistoryEventDef GiveObservedHistoryEvent(Pawn observer);
	}
}
