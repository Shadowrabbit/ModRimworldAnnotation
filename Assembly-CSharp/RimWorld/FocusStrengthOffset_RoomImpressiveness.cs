using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020017E8 RID: 6120
	public class FocusStrengthOffset_RoomImpressiveness : FocusStrengthOffset_Curve
	{
		// Token: 0x0600877D RID: 34685 RVA: 0x0027BCCC File Offset: 0x00279ECC
		protected override float SourceValue(Thing parent)
		{
			Room room = parent.GetRoom(RegionType.Set_Passable);
			if (room == null)
			{
				return 0f;
			}
			return room.GetStat(RoomStatDefOf.Impressiveness);
		}

		// Token: 0x17001519 RID: 5401
		// (get) Token: 0x0600877E RID: 34686 RVA: 0x0005AEE4 File Offset: 0x000590E4
		protected override string ExplanationKey
		{
			get
			{
				return "StatsReport_RoomImpressiveness";
			}
		}
	}
}
