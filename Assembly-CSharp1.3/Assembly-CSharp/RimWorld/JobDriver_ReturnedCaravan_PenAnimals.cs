using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020006CC RID: 1740
	public class JobDriver_ReturnedCaravan_PenAnimals : JobDriver_RopeToDestination
	{
		// Token: 0x0600308A RID: 12426 RVA: 0x0011DE90 File Offset: 0x0011C090
		protected override bool UpdateDestination()
		{
			this.tmpRopees.Clear();
			this.tmpRopees.AddRange(this.pawn.roping.Ropees);
			bool result;
			try
			{
				IntVec3 cell = this.job.GetTarget(TargetIndex.B).Cell;
				District district = cell.IsValid ? cell.GetDistrict(base.Map, RegionType.Set_Passable) : null;
				IntVec3 c = IntVec3.Invalid;
				foreach (Pawn pawn in this.tmpRopees)
				{
					string text;
					CompAnimalPenMarker penAnimalShouldBeTakenTo = AnimalPenUtility.GetPenAnimalShouldBeTakenTo(this.pawn, pawn, out text, false, true, this.job.ropeToUnenclosedPens, true, RopingPriority.Closest);
					if (penAnimalShouldBeTakenTo == null)
					{
						this.pawn.roping.DropRope(pawn);
					}
					else
					{
						if (penAnimalShouldBeTakenTo.parent.GetDistrict(RegionType.Set_Passable) == district)
						{
							return false;
						}
						if (!c.IsValid)
						{
							c = penAnimalShouldBeTakenTo.parent.Position;
						}
					}
				}
				if (c.IsValid)
				{
					this.job.SetTarget(TargetIndex.B, c);
					result = true;
				}
				else
				{
					result = false;
				}
			}
			finally
			{
				this.tmpRopees.Clear();
			}
			return result;
		}

		// Token: 0x0600308B RID: 12427 RVA: 0x0011DFE0 File Offset: 0x0011C1E0
		protected override bool HasRopeeArrived(Pawn ropee, bool roperWaitingAtDest)
		{
			if (this.job.ropeToUnenclosedPens)
			{
				return roperWaitingAtDest;
			}
			return AnimalPenUtility.GetCurrentPenOf(ropee, this.job.ropeToUnenclosedPens) != null;
		}

		// Token: 0x0600308C RID: 12428 RVA: 0x0011E008 File Offset: 0x0011C208
		protected override void ProcessArrivedRopee(Pawn ropee)
		{
			Lord lord = ropee.GetLord();
			if (lord == null)
			{
				return;
			}
			lord.Notify_PawnLost(ropee, PawnLostCondition.LeftVoluntarily, null);
		}

		// Token: 0x0600308D RID: 12429 RVA: 0x0001276E File Offset: 0x0001096E
		protected override bool ShouldOpportunisticallyRopeAnimal(Pawn animal)
		{
			return false;
		}

		// Token: 0x04001D50 RID: 7504
		private List<Pawn> tmpRopees = new List<Pawn>();
	}
}
