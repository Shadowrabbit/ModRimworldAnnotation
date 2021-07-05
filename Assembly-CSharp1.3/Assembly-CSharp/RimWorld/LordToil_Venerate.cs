using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020008C4 RID: 2244
	public class LordToil_Venerate : LordToil
	{
		// Token: 0x17000AA7 RID: 2727
		// (get) Token: 0x06003B28 RID: 15144 RVA: 0x0014A8C9 File Offset: 0x00148AC9
		private LordToilData_Venerate Data
		{
			get
			{
				return (LordToilData_Venerate)this.data;
			}
		}

		// Token: 0x06003B29 RID: 15145 RVA: 0x0014A8D8 File Offset: 0x00148AD8
		public LordToil_Venerate(Thing target)
		{
			this.target = target;
			this.data = new LordToilData_Venerate();
		}

		// Token: 0x06003B2A RID: 15146 RVA: 0x0014A928 File Offset: 0x00148B28
		public override void UpdateAllDuties()
		{
			LordToilData_Venerate data = this.Data;
			CellRect cellRect = CellRect.CenteredOn(this.target.Position, this.target.def.size.x, this.target.def.size.z);
			CellRect cellRect2 = cellRect.ExpandedBy(this.FarSpectateRectExpansion);
			SpectateRectSide preferedSpectateSide = this.GetPreferedSpectateSide();
			int num = -1;
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				Pawn pawn = this.lord.ownedPawns[i];
				if (pawn.mindState.duty != null && pawn.mindState.duty.def == DutyDefOf.Pilgrims_Spectate && pawn.mindState.duty.spectateRect == cellRect)
				{
					num = i;
					break;
				}
			}
			if (num >= 0 && this.lord.ownedPawns.Count > 1)
			{
				this.MakePawnSpectate(this.lord.ownedPawns[num], cellRect2, preferedSpectateSide);
				data.currentNearVeneratorTicks = this.VenerateHandoverTicksRange.RandomInRange;
				return;
			}
			num = (data.lastNearVeneratorIndex + 1) % this.lord.ownedPawns.Count;
			for (int j = 0; j < this.lord.ownedPawns.Count; j++)
			{
				Pawn pawn2 = this.lord.ownedPawns[j];
				CellRect cellRect3 = (num == j) ? cellRect : cellRect2;
				if (pawn2.mindState.duty == null || pawn2.mindState.duty.def != DutyDefOf.Pilgrims_Spectate || pawn2.mindState.duty.spectateRect != cellRect3)
				{
					this.MakePawnSpectate(pawn2, cellRect3, preferedSpectateSide);
				}
			}
			data.currentNearVeneratorTicks = this.VenerateCloseTicksRange.RandomInRange;
			data.lastNearVeneratorIndex = num;
		}

		// Token: 0x06003B2B RID: 15147 RVA: 0x0014AB0C File Offset: 0x00148D0C
		private void MakePawnSpectate(Pawn p, CellRect rect, SpectateRectSide side)
		{
			PawnDuty pawnDuty = new PawnDuty(DutyDefOf.Pilgrims_Spectate);
			pawnDuty.spectateRect = rect;
			pawnDuty.spectateRectAllowedSides = side;
			pawnDuty.locomotion = LocomotionUrgency.Amble;
			pawnDuty.focus = this.target;
			p.mindState.duty = pawnDuty;
			if (p.jobs.curJob != null)
			{
				p.jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
			}
		}

		// Token: 0x06003B2C RID: 15148 RVA: 0x0014AB71 File Offset: 0x00148D71
		public override void LordToilTick()
		{
			LordToilData_Venerate data = this.Data;
			if (data.currentNearVeneratorTicks < 0)
			{
				this.UpdateAllDuties();
			}
			data.currentNearVeneratorTicks--;
		}

		// Token: 0x06003B2D RID: 15149 RVA: 0x0014AB98 File Offset: 0x00148D98
		private SpectateRectSide GetPreferedSpectateSide()
		{
			IntVec3 interactionCell = this.target.InteractionCell;
			IntVec3 intVec = this.target.Position - interactionCell;
			if (intVec.x > 0)
			{
				return SpectateRectSide.Left;
			}
			if (intVec.x < 0)
			{
				return SpectateRectSide.Right;
			}
			if (intVec.z > 0)
			{
				return SpectateRectSide.Down;
			}
			if (intVec.z < 0)
			{
				return SpectateRectSide.Up;
			}
			return SpectateRectSide.All;
		}

		// Token: 0x04002037 RID: 8247
		private Thing target;

		// Token: 0x04002038 RID: 8248
		private int FarSpectateRectExpansion = 2;

		// Token: 0x04002039 RID: 8249
		private IntRange VenerateCloseTicksRange = new IntRange(750, 1500);

		// Token: 0x0400203A RID: 8250
		private IntRange VenerateHandoverTicksRange = new IntRange(60, 120);
	}
}
