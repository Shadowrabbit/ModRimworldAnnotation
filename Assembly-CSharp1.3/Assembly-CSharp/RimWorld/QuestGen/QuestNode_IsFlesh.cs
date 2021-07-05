using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x020016A4 RID: 5796
	public class QuestNode_IsFlesh : QuestNode_RaceProperty
	{
		// Token: 0x0600869C RID: 34460 RVA: 0x00304000 File Offset: 0x00302200
		protected override bool Matches(RaceProperties raceProperties)
		{
			return raceProperties.IsFlesh;
		}
	}
}
