using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x020016A3 RID: 5795
	public class QuestNode_IsAnimal : QuestNode_RaceProperty
	{
		// Token: 0x0600869A RID: 34458 RVA: 0x00303FF0 File Offset: 0x003021F0
		protected override bool Matches(RaceProperties raceProperties)
		{
			return raceProperties.Animal;
		}
	}
}
