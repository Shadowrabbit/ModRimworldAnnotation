using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020006F2 RID: 1778
	public class JobDriver_StandAndBeSociallyActive : JobDriver
	{
		// Token: 0x06003184 RID: 12676 RVA: 0x000126F5 File Offset: 0x000108F5
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return true;
		}

		// Token: 0x06003185 RID: 12677 RVA: 0x00120268 File Offset: 0x0011E468
		protected override IEnumerable<Toil> MakeNewToils()
		{
			yield return new Toil
			{
				tickAction = delegate()
				{
					if (this.job.lookDirection != Direction8Way.Invalid)
					{
						this.pawn.rotationTracker.Face(this.pawn.Position.ToVector3() + this.job.lookDirection.AsVector());
					}
					else
					{
						Pawn pawn = JobDriver_StandAndBeSociallyActive.FindClosePawn(this.pawn);
						if (pawn != null)
						{
							this.pawn.rotationTracker.FaceCell(pawn.Position);
						}
					}
					this.pawn.GainComfortFromCellIfPossible(false);
				},
				socialMode = RandomSocialMode.SuperActive,
				defaultCompleteMode = ToilCompleteMode.Never,
				handlingFacing = true
			};
			yield break;
		}

		// Token: 0x06003186 RID: 12678 RVA: 0x00120278 File Offset: 0x0011E478
		public static Pawn FindClosePawn(Pawn pawn)
		{
			IntVec3 position = pawn.Position;
			Map map = pawn.Map;
			for (int i = 0; i < 24; i++)
			{
				IntVec3 intVec = position + GenRadial.RadialPattern[i];
				if (intVec.InBounds(map))
				{
					Thing thing = intVec.GetThingList(map).Find((Thing x) => x is Pawn);
					if (thing != null && thing != pawn && GenSight.LineOfSight(position, intVec, map, false, null, 0, 0))
					{
						return (Pawn)thing;
					}
				}
			}
			return null;
		}
	}
}
