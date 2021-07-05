using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000977 RID: 2423
	public class ThoughtWorker_NeedNeuralSupercharge : ThoughtWorker_Precept
	{
		// Token: 0x06003D6D RID: 15725 RVA: 0x001521C8 File Offset: 0x001503C8
		protected override ThoughtState ShouldHaveThought(Pawn p)
		{
			if (p.health == null)
			{
				return false;
			}
			if (!ResearchProjectDefOf.MicroelectronicsBasics.IsFinished)
			{
				return false;
			}
			int lastReceivedNeuralSuperchargeTick = p.health.lastReceivedNeuralSuperchargeTick;
			return Find.TickManager.TicksGame - lastReceivedNeuralSuperchargeTick >= 30000 || lastReceivedNeuralSuperchargeTick == -1;
		}

		// Token: 0x040020DB RID: 8411
		public const int TicksUntilNeed = 30000;
	}
}
