using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001F78 RID: 8056
	public class QuestNode_IsFlesh : QuestNode_RaceProperty
	{
		// Token: 0x0600AB9E RID: 43934 RVA: 0x00070690 File Offset: 0x0006E890
		protected override bool Matches(RaceProperties raceProperties)
		{
			return raceProperties.IsFlesh;
		}
	}
}
