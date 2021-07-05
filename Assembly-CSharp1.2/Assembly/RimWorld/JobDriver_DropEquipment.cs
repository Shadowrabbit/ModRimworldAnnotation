using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000C6A RID: 3178
	public class JobDriver_DropEquipment : JobDriver
	{
		// Token: 0x17000BAC RID: 2988
		// (get) Token: 0x06004A6F RID: 19055 RVA: 0x001A1D34 File Offset: 0x0019FF34
		private ThingWithComps TargetEquipment
		{
			get
			{
				return (ThingWithComps)base.TargetA.Thing;
			}
		}

		// Token: 0x06004A70 RID: 19056 RVA: 0x0000A2A7 File Offset: 0x000084A7
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return true;
		}

		// Token: 0x06004A71 RID: 19057 RVA: 0x00035584 File Offset: 0x00033784
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

		// Token: 0x0400316D RID: 12653
		private const int DurationTicks = 30;
	}
}
