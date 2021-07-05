using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200089F RID: 2207
	public class LordToil_PrepareCaravan_CollectAnimals : LordToil_PrepareCaravan_RopeAnimals
	{
		// Token: 0x06003A76 RID: 14966 RVA: 0x00147451 File Offset: 0x00145651
		public LordToil_PrepareCaravan_CollectAnimals(IntVec3 destinationPoint) : base(destinationPoint, new int?(int.MaxValue))
		{
		}

		// Token: 0x06003A77 RID: 14967 RVA: 0x00147464 File Offset: 0x00145664
		protected override PawnDuty MakeRopeDuty()
		{
			return new PawnDuty(DutyDefOf.PrepareCaravan_CollectAnimals, this.destinationPoint, -1f);
		}

		// Token: 0x06003A78 RID: 14968 RVA: 0x00147480 File Offset: 0x00145680
		public override void LordToilTick()
		{
			if (Find.TickManager.TicksGame % 100 == 0)
			{
				bool flag = true;
				for (int i = 0; i < this.lord.ownedPawns.Count; i++)
				{
					Pawn pawn = this.lord.ownedPawns[i];
					if (AnimalPenUtility.NeedsToBeManagedByRope(pawn) && !GatherAnimalsAndSlavesForCaravanUtility.IsRopedByCaravanPawn(pawn))
					{
						flag = false;
						break;
					}
				}
				if (flag)
				{
					this.lord.ReceiveMemo("AllAnimalsCollected");
				}
			}
		}
	}
}
