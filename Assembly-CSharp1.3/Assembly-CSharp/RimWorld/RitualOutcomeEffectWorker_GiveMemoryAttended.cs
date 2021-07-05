using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F6D RID: 3949
	public class RitualOutcomeEffectWorker_GiveMemoryAttended : RitualOutcomeEffectWorker
	{
		// Token: 0x06005DA6 RID: 23974 RVA: 0x0020193C File Offset: 0x001FFB3C
		public RitualOutcomeEffectWorker_GiveMemoryAttended()
		{
		}

		// Token: 0x06005DA7 RID: 23975 RVA: 0x00201944 File Offset: 0x001FFB44
		public RitualOutcomeEffectWorker_GiveMemoryAttended(RitualOutcomeEffectDef def) : base(def)
		{
		}

		// Token: 0x06005DA8 RID: 23976 RVA: 0x00201950 File Offset: 0x001FFB50
		public override void Apply(float progress, Dictionary<Pawn, int> totalPresence, LordJob_Ritual jobRitual)
		{
			if (progress < 1f)
			{
				return;
			}
			foreach (KeyValuePair<Pawn, int> keyValuePair in totalPresence)
			{
				keyValuePair.Key.needs.mood.thoughts.memories.TryGainMemory(base.MakeMemory(keyValuePair.Key, jobRitual, null), null);
			}
		}
	}
}
