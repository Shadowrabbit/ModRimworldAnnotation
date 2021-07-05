using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F5C RID: 3932
	public class RitualOutcomeComp_RoomStat : RitualOutcomeComp_Quality
	{
		// Token: 0x17001027 RID: 4135
		// (get) Token: 0x06005D49 RID: 23881 RVA: 0x0001276E File Offset: 0x0001096E
		public override bool DataRequired
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06005D4A RID: 23882 RVA: 0x001FFDFC File Offset: 0x001FDFFC
		private float PostProcessedRoomStat(Room room)
		{
			return GenMath.RoundTo(Math.Min(room.GetStat(this.statDef), this.curve.Points[this.curve.PointsCount - 1].x), 0.1f);
		}

		// Token: 0x06005D4B RID: 23883 RVA: 0x001FFE49 File Offset: 0x001FE049
		public override bool Applies(LordJob_Ritual ritual)
		{
			return ritual.Spot.GetRoom(ritual.Map) != null;
		}

		// Token: 0x06005D4C RID: 23884 RVA: 0x001FFE60 File Offset: 0x001FE060
		public override float Count(LordJob_Ritual ritual, RitualOutcomeComp_Data data)
		{
			Room room = ritual.Spot.GetRoom(ritual.Map);
			if (room != null)
			{
				return this.PostProcessedRoomStat(room);
			}
			return 0f;
		}

		// Token: 0x06005D4D RID: 23885 RVA: 0x001FFE90 File Offset: 0x001FE090
		public override ExpectedOutcomeDesc GetExpectedOutcomeDesc(Precept_Ritual ritual, TargetInfo ritualTarget, RitualObligation obligation, RitualRoleAssignments assignments)
		{
			Room room = ritualTarget.Cell.GetRoom(ritualTarget.Map);
			float num = (room == null) ? 0f : this.PostProcessedRoomStat(room);
			float quality = this.curve.Evaluate(num);
			return new ExpectedOutcomeDesc
			{
				label = this.label.CapitalizeFirst(),
				count = num + " / " + base.MaxValue,
				effect = this.ExpectedOffsetDesc(true, quality),
				quality = quality,
				positive = (room != null && !room.PsychologicallyOutdoors),
				priority = 0f
			};
		}

		// Token: 0x040035FB RID: 13819
		public RoomStatDef statDef;
	}
}
