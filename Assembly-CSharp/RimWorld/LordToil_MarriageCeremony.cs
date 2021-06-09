using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000E17 RID: 3607
	public class LordToil_MarriageCeremony : LordToil
	{
		// Token: 0x17000CA7 RID: 3239
		// (get) Token: 0x060051EA RID: 20970 RVA: 0x00039430 File Offset: 0x00037630
		public LordToilData_MarriageCeremony Data
		{
			get
			{
				return (LordToilData_MarriageCeremony)this.data;
			}
		}

		// Token: 0x060051EB RID: 20971 RVA: 0x0003943D File Offset: 0x0003763D
		public LordToil_MarriageCeremony(Pawn firstPawn, Pawn secondPawn, IntVec3 spot)
		{
			this.firstPawn = firstPawn;
			this.secondPawn = secondPawn;
			this.spot = spot;
			this.data = new LordToilData_MarriageCeremony();
		}

		// Token: 0x060051EC RID: 20972 RVA: 0x001BD010 File Offset: 0x001BB210
		public override void Init()
		{
			base.Init();
			this.Data.spectateRect = this.CalculateSpectateRect();
			SpectateRectSide allowedSides = SpectateRectSide.All;
			if (this.Data.spectateRect.Width > this.Data.spectateRect.Height)
			{
				allowedSides = SpectateRectSide.Vertical;
			}
			else if (this.Data.spectateRect.Height > this.Data.spectateRect.Width)
			{
				allowedSides = SpectateRectSide.Horizontal;
			}
			this.Data.spectateRectAllowedSides = SpectatorCellFinder.FindSingleBestSide(this.Data.spectateRect, base.Map, allowedSides, 1);
		}

		// Token: 0x060051ED RID: 20973 RVA: 0x00039465 File Offset: 0x00037665
		public override ThinkTreeDutyHook VoluntaryJoinDutyHookFor(Pawn p)
		{
			if (this.IsFiance(p))
			{
				return DutyDefOf.MarryPawn.hook;
			}
			return DutyDefOf.Spectate.hook;
		}

		// Token: 0x060051EE RID: 20974 RVA: 0x001BD0A8 File Offset: 0x001BB2A8
		public override void UpdateAllDuties()
		{
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				Pawn pawn = this.lord.ownedPawns[i];
				if (this.IsFiance(pawn))
				{
					pawn.mindState.duty = new PawnDuty(DutyDefOf.MarryPawn, this.FianceStandingSpotFor(pawn), -1f);
				}
				else
				{
					PawnDuty pawnDuty = new PawnDuty(DutyDefOf.Spectate);
					pawnDuty.spectateRect = this.Data.spectateRect;
					pawnDuty.spectateRectAllowedSides = this.Data.spectateRectAllowedSides;
					pawn.mindState.duty = pawnDuty;
				}
			}
		}

		// Token: 0x060051EF RID: 20975 RVA: 0x00039485 File Offset: 0x00037685
		private bool IsFiance(Pawn p)
		{
			return p == this.firstPawn || p == this.secondPawn;
		}

		// Token: 0x060051F0 RID: 20976 RVA: 0x001BD154 File Offset: 0x001BB354
		public IntVec3 FianceStandingSpotFor(Pawn pawn)
		{
			Pawn pawn2;
			if (this.firstPawn == pawn)
			{
				pawn2 = this.secondPawn;
			}
			else
			{
				if (this.secondPawn != pawn)
				{
					Log.Warning("Called ExactStandingSpotFor but it's not this pawn's ceremony.", false);
					return IntVec3.Invalid;
				}
				pawn2 = this.firstPawn;
			}
			if (pawn.thingIDNumber < pawn2.thingIDNumber)
			{
				return this.spot;
			}
			if (this.GetMarriageSpotAt(this.spot) != null)
			{
				return this.FindCellForOtherPawnAtMarriageSpot(this.spot);
			}
			return this.spot + LordToil_MarriageCeremony.OtherFianceNoMarriageSpotCellOffset;
		}

		// Token: 0x060051F1 RID: 20977 RVA: 0x0003949B File Offset: 0x0003769B
		private Thing GetMarriageSpotAt(IntVec3 cell)
		{
			return cell.GetThingList(base.Map).Find((Thing x) => x.def == ThingDefOf.MarriageSpot);
		}

		// Token: 0x060051F2 RID: 20978 RVA: 0x001BD1D8 File Offset: 0x001BB3D8
		private IntVec3 FindCellForOtherPawnAtMarriageSpot(IntVec3 cell)
		{
			CellRect cellRect = this.GetMarriageSpotAt(cell).OccupiedRect();
			for (int i = cellRect.minX; i <= cellRect.maxX; i++)
			{
				for (int j = cellRect.minZ; j <= cellRect.maxZ; j++)
				{
					if (cell.x != i || cell.z != j)
					{
						return new IntVec3(i, 0, j);
					}
				}
			}
			Log.Warning("Marriage spot is 1x1. There's no place for 2 pawns.", false);
			return IntVec3.Invalid;
		}

		// Token: 0x060051F3 RID: 20979 RVA: 0x001BD24C File Offset: 0x001BB44C
		private CellRect CalculateSpectateRect()
		{
			IntVec3 first = this.FianceStandingSpotFor(this.firstPawn);
			IntVec3 second = this.FianceStandingSpotFor(this.secondPawn);
			return CellRect.FromLimits(first, second);
		}

		// Token: 0x04003473 RID: 13427
		private Pawn firstPawn;

		// Token: 0x04003474 RID: 13428
		private Pawn secondPawn;

		// Token: 0x04003475 RID: 13429
		private IntVec3 spot;

		// Token: 0x04003476 RID: 13430
		public static readonly IntVec3 OtherFianceNoMarriageSpotCellOffset = new IntVec3(-1, 0, 0);
	}
}
