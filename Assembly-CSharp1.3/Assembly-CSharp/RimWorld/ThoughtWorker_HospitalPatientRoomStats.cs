using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200097D RID: 2429
	public class ThoughtWorker_HospitalPatientRoomStats : ThoughtWorker
	{
		// Token: 0x06003D7D RID: 15741 RVA: 0x0015246C File Offset: 0x0015066C
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			Building_Bed building_Bed = p.CurrentBed();
			if (building_Bed == null || !building_Bed.Medical)
			{
				return ThoughtState.Inactive;
			}
			Room room = p.GetRoom(RegionType.Set_All);
			if (room == null || room.Role != RoomRoleDefOf.Hospital)
			{
				return ThoughtState.Inactive;
			}
			int scoreStageIndex = RoomStatDefOf.Impressiveness.GetScoreStageIndex(room.GetStat(RoomStatDefOf.Impressiveness));
			if (this.def.stages[scoreStageIndex] != null)
			{
				return ThoughtState.ActiveAtStage(scoreStageIndex);
			}
			return ThoughtState.Inactive;
		}
	}
}
