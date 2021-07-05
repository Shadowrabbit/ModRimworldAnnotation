using System;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020010A4 RID: 4260
	public class QuestPart_AssaultColony : QuestPart_MakeLord
	{
		// Token: 0x06005CE7 RID: 23783 RVA: 0x00040741 File Offset: 0x0003E941
		protected override Lord MakeLord()
		{
			return LordMaker.MakeNewLord(this.faction, new LordJob_AssaultColony(this.faction, true, true, false, false, true), base.Map, null);
		}
	}
}
