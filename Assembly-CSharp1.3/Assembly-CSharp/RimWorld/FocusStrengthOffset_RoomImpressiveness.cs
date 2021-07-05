using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001154 RID: 4436
	public class FocusStrengthOffset_RoomImpressiveness : FocusStrengthOffset_Curve
	{
		// Token: 0x06006AA0 RID: 27296 RVA: 0x0023D4E8 File Offset: 0x0023B6E8
		protected override float SourceValue(Thing parent)
		{
			Room room = parent.GetRoom(RegionType.Set_All);
			if (room == null)
			{
				return 0f;
			}
			return room.GetStat(RoomStatDefOf.Impressiveness);
		}

		// Token: 0x17001256 RID: 4694
		// (get) Token: 0x06006AA1 RID: 27297 RVA: 0x0023D512 File Offset: 0x0023B712
		protected override string ExplanationKey
		{
			get
			{
				return "StatsReport_RoomImpressiveness";
			}
		}
	}
}
