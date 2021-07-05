using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020008A0 RID: 2208
	public class LordToil_ReturnedCaravan_PenAnimals : LordToil_PrepareCaravan_RopeAnimals
	{
		// Token: 0x06003A79 RID: 14969 RVA: 0x001474F2 File Offset: 0x001456F2
		public LordToil_ReturnedCaravan_PenAnimals(IntVec3 entryPoint) : base(entryPoint, new int?(int.MaxValue))
		{
		}

		// Token: 0x06003A7A RID: 14970 RVA: 0x00147510 File Offset: 0x00145710
		public override void LordToilTick()
		{
			if (Find.TickManager.TicksGame % 100 == 0)
			{
				this.tmpOwnedPawns.Clear();
				this.tmpOwnedPawns.AddRange(this.lord.ownedPawns);
				bool flag = false;
				bool flag2 = false;
				foreach (Pawn pawn in this.tmpOwnedPawns)
				{
					if (AnimalPenUtility.NeedsToBeManagedByRope(pawn))
					{
						if (this.tmpOwnedPawns.Contains(pawn.roping.RopedByPawn))
						{
							flag2 = true;
						}
						else
						{
							this.lord.Notify_PawnLost(pawn, PawnLostCondition.LeftVoluntarily, null);
						}
					}
					else if (pawn.Spawned && pawn.IsColonist && !pawn.Downed && !pawn.Dead && pawn.roping.HasAnyRope)
					{
						flag = true;
					}
					else
					{
						this.lord.Notify_PawnLost(pawn, PawnLostCondition.LeftVoluntarily, null);
					}
				}
				if (!flag || !flag2)
				{
					this.lord.ReceiveMemo("RepenningFinished");
				}
				this.tmpOwnedPawns.Clear();
			}
		}

		// Token: 0x06003A7B RID: 14971 RVA: 0x00147640 File Offset: 0x00145840
		protected override PawnDuty MakeRopeDuty()
		{
			return new PawnDuty(DutyDefOf.ReturnedCaravan_PenAnimals);
		}

		// Token: 0x04001FFB RID: 8187
		private List<Pawn> tmpOwnedPawns = new List<Pawn>();
	}
}
