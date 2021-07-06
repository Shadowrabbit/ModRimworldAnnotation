using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020014F3 RID: 5363
	public class Need_RoomSize : Need_Seeker
	{
		// Token: 0x170011D0 RID: 4560
		// (get) Token: 0x06007387 RID: 29575 RVA: 0x0004DCCD File Offset: 0x0004BECD
		public override float CurInstantLevel
		{
			get
			{
				return this.SpacePerceptibleNow();
			}
		}

		// Token: 0x170011D1 RID: 4561
		// (get) Token: 0x06007388 RID: 29576 RVA: 0x0004DCD5 File Offset: 0x0004BED5
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

		// Token: 0x06007389 RID: 29577 RVA: 0x0004DD05 File Offset: 0x0004BF05
		public Need_RoomSize(Pawn pawn) : base(pawn)
		{
			this.threshPercents = new List<float>();
			this.threshPercents.Add(0.3f);
			this.threshPercents.Add(0.7f);
		}

		// Token: 0x0600738A RID: 29578 RVA: 0x002343E0 File Offset: 0x002325E0
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
				Room room = (position + GenRadial.RadialPattern[i]).GetRoom(this.pawn.Map, RegionType.Set_Passable);
				if (room != null)
				{
					if (i == 0 && room.PsychologicallyOutdoors)
					{
						return 1f;
					}
					if ((i == 0 || room.RegionType != RegionType.Portal) && !Need_RoomSize.tempScanRooms.Contains(room))
					{
						Need_RoomSize.tempScanRooms.Add(room);
					}
				}
			}
			float num = 0f;
			for (int j = 0; j < Need_RoomSize.SampleNumCells; j++)
			{
				IntVec3 loc = position + GenRadial.RadialPattern[j];
				if (Need_RoomSize.tempScanRooms.Contains(loc.GetRoom(this.pawn.Map, RegionType.Set_Passable)))
				{
					num += 1f;
				}
			}
			Need_RoomSize.tempScanRooms.Clear();
			return Need_RoomSize.RoomCellCountSpaceCurve.Evaluate(num);
		}

		// Token: 0x04004C51 RID: 19537
		private static List<Room> tempScanRooms = new List<Room>();

		// Token: 0x04004C52 RID: 19538
		private const float MinCramped = 0.01f;

		// Token: 0x04004C53 RID: 19539
		private const float MinNormal = 0.3f;

		// Token: 0x04004C54 RID: 19540
		private const float MinSpacious = 0.7f;

		// Token: 0x04004C55 RID: 19541
		public static readonly int SampleNumCells = GenRadial.NumCellsInRadius(7.9f);

		// Token: 0x04004C56 RID: 19542
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
