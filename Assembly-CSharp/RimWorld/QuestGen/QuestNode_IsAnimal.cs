using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001F77 RID: 8055
	public class QuestNode_IsAnimal : QuestNode_RaceProperty
	{
		// Token: 0x0600AB9C RID: 43932 RVA: 0x00070680 File Offset: 0x0006E880
		protected override bool Matches(RaceProperties raceProperties)
		{
			return raceProperties.Animal;
		}
	}
}
