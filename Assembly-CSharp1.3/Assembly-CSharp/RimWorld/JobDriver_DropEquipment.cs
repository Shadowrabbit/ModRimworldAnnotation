using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200075C RID: 1884
	public class JobDriver_DropEquipment : JobDriver
	{
		// Token: 0x170009AE RID: 2478
		// (get) Token: 0x0600342D RID: 13357 RVA: 0x001281B4 File Offset: 0x001263B4
		private ThingWithComps TargetEquipment
		{
			get
			{
				return (ThingWithComps)base.TargetA.Thing;
			}
		}

		// Token: 0x0600342E RID: 13358 RVA: 0x000126F5 File Offset: 0x000108F5
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return true;
		}

		// Token: 0x0600342F RID: 13359 RVA: 0x001281D4 File Offset: 0x001263D4
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDestroyedOrNull(TargetIndex.A);
			yield return new Toil
			{
				initAction = delegate()
				{
					this.pawn.pather.StopDead();
				},
				defaultCompleteMode = ToilCompleteMode.Delay,
				defaultDuration = 30
			};
			yield return new Toil
			{
				initAction = delegate()
				{
					ThingWithComps thingWithComps;
					if (!this.pawn.equipment.TryDropEquipment(this.TargetEquipment, out thingWithComps, this.pawn.Position, true))
					{
						base.EndJobWith(JobCondition.Incompletable);
					}
				}
			};
			yield break;
		}

		// Token: 0x04001E39 RID: 7737
		private const int DurationTicks = 30;
	}
}
