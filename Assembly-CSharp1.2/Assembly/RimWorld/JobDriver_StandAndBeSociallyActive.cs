using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000B7E RID: 2942
	public class JobDriver_StandAndBeSociallyActive : JobDriver
	{
		// Token: 0x06004533 RID: 17715 RVA: 0x0000A2A7 File Offset: 0x000084A7
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return true;
		}

		// Token: 0x06004534 RID: 17716 RVA: 0x00032F21 File Offset: 0x00031121
		protected override IEnumerable<Toil> MakeNewToils()
		{
			yield return new Toil
			{
				tickAction = delegate()
				{
					Pawn pawn = this.FindClosePawn();
					if (pawn != null)
					{
						this.pawn.rotationTracker.FaceCell(pawn.Position);
					}
					this.pawn.GainComfortFromCellIfPossible(false);
				},
				socialMode = RandomSocialMode.SuperActive,
				defaultCompleteMode = ToilCompleteMode.Never,
				handlingFacing = true
			};
			yield break;
		}

		// Token: 0x06004535 RID: 17717 RVA: 0x00191D70 File Offset: 0x0018FF70
		private Pawn FindClosePawn()
		{
			IntVec3 position = this.pawn.Position;
			for (int i = 0; i < 24; i++)
			{
				IntVec3 intVec = position + GenRadial.RadialPattern[i];
				if (intVec.InBounds(base.Map))
				{
					Thing thing = intVec.GetThingList(base.Map).Find((Thing x) => x is Pawn);
					if (thing != null && thing != this.pawn && GenSight.LineOfSight(position, intVec, base.Map, false, null, 0, 0))
					{
						return (Pawn)thing;
					}
				}
			}
			return null;
		}
	}
}
