using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000DB1 RID: 3505
	public struct ThoughtToAddToAll
	{
		// Token: 0x06005121 RID: 20769 RVA: 0x001B263B File Offset: 0x001B083B
		public ThoughtToAddToAll(ThoughtDef thoughtDef, Pawn otherPawn = null)
		{
			if (thoughtDef == null)
			{
				throw new NullReferenceException("Thought def cant be null!");
			}
			this.thoughtDef = thoughtDef;
			this.otherPawn = otherPawn;
		}

		// Token: 0x06005122 RID: 20770 RVA: 0x001B265C File Offset: 0x001B085C
		public void Add(Pawn to)
		{
			if (to.needs == null || to.needs.mood == null)
			{
				return;
			}
			if (this.otherPawn == null)
			{
				to.needs.mood.thoughts.memories.TryGainMemory(this.thoughtDef, null, null);
				return;
			}
			to.needs.mood.thoughts.memories.TryGainMemory(this.thoughtDef, this.otherPawn, null);
		}

		// Token: 0x0400300D RID: 12301
		public ThoughtDef thoughtDef;

		// Token: 0x0400300E RID: 12302
		public Pawn otherPawn;
	}
}
