using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F6F RID: 3951
	public class RitualOutcomeEffectWorker_GiveMemoryBelievers : RitualOutcomeEffectWorker
	{
		// Token: 0x06005DAE RID: 23982 RVA: 0x0020193C File Offset: 0x001FFB3C
		public RitualOutcomeEffectWorker_GiveMemoryBelievers()
		{
		}

		// Token: 0x06005DAF RID: 23983 RVA: 0x00201944 File Offset: 0x001FFB44
		public RitualOutcomeEffectWorker_GiveMemoryBelievers(RitualOutcomeEffectDef def) : base(def)
		{
		}

		// Token: 0x06005DB0 RID: 23984 RVA: 0x00201B9C File Offset: 0x001FFD9C
		public override void Apply(float progress, Dictionary<Pawn, int> totalPresence, LordJob_Ritual jobRitual)
		{
			foreach (Pawn pawn in PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_Colonists)
			{
				if (pawn.Ideo == jobRitual.Ritual.ideo)
				{
					pawn.needs.mood.thoughts.memories.TryGainMemory(base.MakeMemory(pawn, jobRitual, null), null);
				}
			}
		}
	}
}
