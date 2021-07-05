using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001110 RID: 4368
	public class CompBiosculpterPod_PleasureCycle : CompBiosculpterPod_Cycle
	{
		// Token: 0x060068E1 RID: 26849 RVA: 0x002366C4 File Offset: 0x002348C4
		public override void CycleCompleted(Pawn pawn)
		{
			pawn.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.BiosculpterPleasure, null, null);
			Messages.Message("BiosculpterPleasureCompletedMessage".Translate(pawn.Named("PAWN")), pawn, MessageTypeDefOf.PositiveEvent, true);
		}
	}
}
