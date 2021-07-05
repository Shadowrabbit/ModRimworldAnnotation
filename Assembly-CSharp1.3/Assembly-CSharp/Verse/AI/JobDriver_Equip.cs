using System;
using System.Collections.Generic;
using Verse.Sound;

namespace Verse.AI
{
	// Token: 0x0200058E RID: 1422
	public class JobDriver_Equip : JobDriver
	{
		// Token: 0x060029A2 RID: 10658 RVA: 0x000FC0C8 File Offset: 0x000FA2C8
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			int maxPawns = 1;
			int stackCount = -1;
			if (this.job.targetA.HasThing && this.job.targetA.Thing.Spawned && this.job.targetA.Thing.def.IsIngestible)
			{
				maxPawns = 10;
				stackCount = 1;
			}
			return this.pawn.Reserve(this.job.targetA, this.job, maxPawns, stackCount, null, errorOnFailed);
		}

		// Token: 0x060029A3 RID: 10659 RVA: 0x000FC143 File Offset: 0x000FA343
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDestroyedOrNull(TargetIndex.A);
			this.FailOnBurningImmobile(TargetIndex.A);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.ClosestTouch).FailOnDespawnedNullOrForbidden(TargetIndex.A);
			yield return new Toil
			{
				initAction = delegate()
				{
					ThingWithComps thingWithComps = (ThingWithComps)this.job.targetA.Thing;
					ThingWithComps thingWithComps2;
					if (thingWithComps.def.stackLimit > 1 && thingWithComps.stackCount > 1)
					{
						thingWithComps2 = (ThingWithComps)thingWithComps.SplitOff(1);
					}
					else
					{
						thingWithComps2 = thingWithComps;
						thingWithComps2.DeSpawn(DestroyMode.Vanish);
					}
					this.pawn.equipment.MakeRoomFor(thingWithComps2);
					this.pawn.equipment.AddEquipment(thingWithComps2);
					if (thingWithComps.def.soundInteract != null)
					{
						thingWithComps.def.soundInteract.PlayOneShot(new TargetInfo(this.pawn.Position, this.pawn.Map, false));
					}
				},
				defaultCompleteMode = ToilCompleteMode.Instant
			};
			yield break;
		}
	}
}
