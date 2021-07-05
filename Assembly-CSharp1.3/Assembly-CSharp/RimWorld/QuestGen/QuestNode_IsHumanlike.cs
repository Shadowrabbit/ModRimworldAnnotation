using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x020016A5 RID: 5797
	public class QuestNode_IsHumanlike : QuestNode_RaceProperty
	{
		// Token: 0x0600869E RID: 34462 RVA: 0x00304008 File Offset: 0x00302208
		protected override bool Matches(RaceProperties raceProperties)
		{
			return raceProperties.Humanlike;
		}
	}
}
