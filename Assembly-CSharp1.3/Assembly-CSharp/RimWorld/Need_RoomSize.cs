using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E56 RID: 3670
	public class Need_RoomSize : Need_Seeker
	{
		// Token: 0x17000EA2 RID: 3746
		// (get) Token: 0x060054F2 RID: 21746 RVA: 0x001CC68D File Offset: 0x001CA88D
		public override float CurInstantLevel
		{
			get
			{
				return this.SpacePerceptibleNow();
			}
		}

		// Token: 0x17000EA3 RID: 3747
		// (get) Token: 0x060054F3 RID: 21747 RVA: 0x001CC695 File Offset: 0x001CA895
		public RoomSizeCategory CurCategory
		{
			get
			{
				if (this.CurLevel < 0.01f)
				{
					return RoomSizeCategory.VeryCramped;
				}
				if (this.CurLevel < 0.3f)
				{
					return RoomSizeCategory.Cramped;
				}
				if (this.CurLevel < 0.7f)
				{
					return RoomSizeCategory.Normal;
				}
				return RoomSizeCategory.Spacious;
			}
		}

		// Token: 0x060054F4 RID: 21748 RVA: 0x001CC6C5 File Offset: 0x001CA8C5
		public Need_RoomSize(Pawn pawn) : base(pawn)
		{
			this.threshPercents = new List<float>();
			this.threshPercents.Add(0.3f);
			this.threshPercents.Add(0.7f);
		}

		// Token: 0x060054F5 RID: 21749 RVA: 0x001CC6FC File Offset: 0x001CA8FC
		public float SpacePerceptibleNow()
		{
			if (!this.pawn.Spawned)
			{
				return 1f;
			}
			IntVec3 position = this.pawn.Position;
			Need_RoomSize.tempScanRooms.Clear();
			for (int i = 0; i < 5; i++)
			{
				Room room = (position + GenRadial.RadialPattern[i]).GetRoom(this.pawn.Map);
				if (room != null)
				{
					if (i == 0 && room.PsychologicallyOutdoors)
					{
						return 1f;
					}
					if ((i == 0 || !room.IsDoorway) && !Need_RoomSize.tempScanRooms.Contains(room))
					{
						Need_RoomSize.tempScanRooms.Add(room);
					}
				}
			}
			float num = 0f;
			for (int j = 0; j < Need_RoomSize.SampleNumCells; j++)
			{
				IntVec3 loc = position + GenRadial.RadialPattern[j];
				if (Need_RoomSize.tempScanRooms.Contains(loc.GetRoom(this.pawn.Map)))
				{
					num += 1f;
				}
			}
			Need_RoomSize.tempScanRooms.Clear();
			return Need_RoomSize.RoomCellCountSpaceCurve.Evaluate(num);
		}

		// Token: 0x04003256 RID: 12886
		private static List<Room> tempScanRooms = new List<Room>();

		// Token: 0x04003257 RID: 12887
		private const float MinCramped = 0.01f;

		// Token: 0x04003258 RID: 12888
		private const float MinNormal = 0.3f;

		// Token: 0x04003259 RID: 12889
		private const float MinSpacious = 0.7f;

		// Token: 0x0400325A RID: 12890
		public static readonly int SampleNumCells = GenRadial.NumCellsInRadius(7.9f);

		// Token: 0x0400325B RID: 12891
		private static readonly SimpleCurve RoomCellCountSpaceCurve = new SimpleCurve
		{
			{
				new CurvePoint(3f, 0f),
				true
			},
			{
				new CurvePoint(9f, 0.25f),
				true
			},
			{
				new CurvePoint(16f, 0.5f),
				true
			},
			{
				new CurvePoint(42f, 0.71f),
				true
			},
			{
				new CurvePoint(100f, 1f),
				true
			}
		};
	}
}
