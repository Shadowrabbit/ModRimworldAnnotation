using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020013EC RID: 5100
	public struct ThoughtToAddToAll
	{
		// Token: 0x06006E4D RID: 28237 RVA: 0x0004AC37 File Offset: 0x00048E37
		public ThoughtToAddToAll(ThoughtDef thoughtDef, Pawn otherPawn = null)
		{
			if (thoughtDef == null)
			{
				throw new NullReferenceException("Thought def cant be null!");
			}
			this.thoughtDef = thoughtDef;
			this.otherPawn = otherPawn;
		}

		// Token: 0x06006E4E RID: 28238 RVA: 0x0021C26C File Offset: 0x0021A46C
		public void Add(Pawn to)
		{
			if (to.needs == null || to.needs.mood == null)
			{
				return;
			}
			if (this.otherPawn == null)
			{
				to.needs.mood.thoughts.memories.TryGainMemory(this.thoughtDef, null);
				return;
			}
			to.needs.mood.thoughts.memories.TryGainMemory(this.thoughtDef, this.otherPawn);
		}

		// Token: 0x040048BC RID: 18620
		public ThoughtDef thoughtDef;

		// Token: 0x040048BD RID: 18621
		public Pawn otherPawn;
	}
}
