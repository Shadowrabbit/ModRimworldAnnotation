using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x020016A6 RID: 5798
	public class QuestNode_IsMechanoid : QuestNode_RaceProperty
	{
		// Token: 0x060086A0 RID: 34464 RVA: 0x00304010 File Offset: 0x00302210
		protected override bool Matches(RaceProperties raceProperties)
		{
			return raceProperties.IsMechanoid;
		}
	}
}
