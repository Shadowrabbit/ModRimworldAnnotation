using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F20 RID: 3872
	public class RitualStage_WaitingForParticipants : RitualStage
	{
		// Token: 0x06005C1B RID: 23579 RVA: 0x001FCB34 File Offset: 0x001FAD34
		public override float ProgressPerTick(LordJob_Ritual ritual)
		{
			foreach (Pawn p in ritual.assignments.SpectatorsForReading)
			{
				if (!ritual.IsParticipating(p))
				{
					return 0f;
				}
			}
			return base.ProgressPerTick(ritual);
		}
	}
}
