using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001F79 RID: 8057
	public class QuestNode_IsHumanlike : QuestNode_RaceProperty
	{
		// Token: 0x0600ABA0 RID: 43936 RVA: 0x00070698 File Offset: 0x0006E898
		protected override bool Matches(RaceProperties raceProperties)
		{
			return raceProperties.Humanlike;
		}
	}
}
