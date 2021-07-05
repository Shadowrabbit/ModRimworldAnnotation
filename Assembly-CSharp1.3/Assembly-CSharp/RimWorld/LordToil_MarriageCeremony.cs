using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020008D1 RID: 2257
	public class LordToil_MarriageCeremony : LordToil
	{
		// Token: 0x17000AAE RID: 2734
		// (get) Token: 0x06003B56 RID: 15190 RVA: 0x0014B8DD File Offset: 0x00149ADD
		public LordToilData_MarriageCeremony Data
		{
			get
			{
				return (LordToilData_MarriageCeremony)this.data;
			}
		}

		// Token: 0x06003B57 RID: 15191 RVA: 0x0014B8EA File Offset: 0x00149AEA
		public LordToil_MarriageCeremony(Pawn firstPawn, Pawn secondPawn, IntVec3 spot)
		{
			this.firstPawn = firstPawn;
			this.secondPawn = secondPawn;
			this.spot = spot;
			this.data = new LordToilData_MarriageCeremony();
		}

		// Token: 0x06003B58 RID: 15192 RVA: 0x0014B914 File Offset: 0x00149B14
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
			this.Data.spectateRectAllowedSides = SpectatorCellFinder.FindSingleBestSide(this.Data.spectateRect, base.Map, allowedSides, 1, null);
		}

		// Token: 0x06003B59 RID: 15193 RVA: 0x0014B9AA File Offset: 0x00149BAA
		public override ThinkTreeDutyHook VoluntaryJoinDutyHookFor(Pawn p)
		{
			if (this.IsFiance(p))
			{
				return DutyDefOf.MarryPawn.hook;
			}
			return DutyDefOf.Spectate.hook;
		}

		// Token: 0x06003B5A RID: 15194 RVA: 0x0014B9CC File Offset: 0x00149BCC
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

		// Token: 0x06003B5B RID: 15195 RVA: 0x0014BA76 File Offset: 0x00149C76
		private bool IsFiance(Pawn p)
		{
			return p == this.firstPawn || p == this.secondPawn;
		}

		// Token: 0x06003B5C RID: 15196 RVA: 0x0014BA8C File Offset: 0x00149C8C
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
					Log.Warning("Called ExactStandingSpotFor but it's not this pawn's ceremony.");
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

		// Token: 0x06003B5D RID: 15197 RVA: 0x0014BB0F File Offset: 0x00149D0F
		private Thing GetMarriageSpotAt(IntVec3 cell)
		{
			return cell.GetThingList(base.Map).Find((Thing x) => x.def == ThingDefOf.MarriageSpot);
		}

		// Token: 0x06003B5E RID: 15198 RVA: 0x0014BB44 File Offset: 0x00149D44
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
			Log.Warning("Marriage spot is 1x1. There's no place for 2 pawns.");
			return IntVec3.Invalid;
		}

		// Token: 0x06003B5F RID: 15199 RVA: 0x0014BBB4 File Offset: 0x00149DB4
		private CellRect CalculateSpectateRect()
		{
			IntVec3 first = this.FianceStandingSpotFor(this.firstPawn);
			IntVec3 second = this.FianceStandingSpotFor(this.secondPawn);
			return CellRect.FromLimits(first, second);
		}

		// Token: 0x04002058 RID: 8280
		private Pawn firstPawn;

		// Token: 0x04002059 RID: 8281
		private Pawn secondPawn;

		// Token: 0x0400205A RID: 8282
		private IntVec3 spot;

		// Token: 0x0400205B RID: 8283
		public static readonly IntVec3 OtherFianceNoMarriageSpotCellOffset = new IntVec3(-1, 0, 0);
	}
}
