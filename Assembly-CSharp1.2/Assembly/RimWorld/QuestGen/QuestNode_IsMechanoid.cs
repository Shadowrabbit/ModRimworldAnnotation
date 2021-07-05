using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001F7A RID: 8058
	public class QuestNode_IsMechanoid : QuestNode_RaceProperty
	{
		// Token: 0x0600ABA2 RID: 43938 RVA: 0x000706A0 File Offset: 0x0006E8A0
		protected override bool Matches(RaceProperties raceProperties)
		{
			return raceProperties.IsMechanoid;
		}
	}
}
