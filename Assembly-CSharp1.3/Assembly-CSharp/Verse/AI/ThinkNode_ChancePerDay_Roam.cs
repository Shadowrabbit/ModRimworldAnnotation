using System;

namespace Verse.AI
{
	// Token: 0x02000616 RID: 1558
	public class ThinkNode_ChancePerDay_Roam : ThinkNode_ChancePerHour
	{
		// Token: 0x06002D12 RID: 11538 RVA: 0x0010F0AA File Offset: 0x0010D2AA
		protected override float MtbHours(Pawn pawn)
		{
			return pawn.RaceProps.roamMtbDays.GetValueOrDefault(0f) * 24f;
		}
	}
}
