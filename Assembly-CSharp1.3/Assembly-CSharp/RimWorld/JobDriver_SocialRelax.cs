using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000700 RID: 1792
	public class JobDriver_SocialRelax : JobDriver
	{
		// Token: 0x17000943 RID: 2371
		// (get) Token: 0x060031CD RID: 12749 RVA: 0x0012132C File Offset: 0x0011F52C
		private Thing GatherSpotParent
		{
			get
			{
				return this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x17000944 RID: 2372
		// (get) Token: 0x060031CE RID: 12750 RVA: 0x00121350 File Offset: 0x0011F550
		private bool HasChair
		{
			get
			{
				return this.job.GetTarget(TargetIndex.B).HasThing;
			}
		}

		// Token: 0x17000945 RID: 2373
		// (get) Token: 0x060031CF RID: 12751 RVA: 0x00121374 File Offset: 0x0011F574
		private bool HasDrink
		{
			get
			{
				return this.job.GetTarget(TargetIndex.C).HasThing;
			}
		}

		// Token: 0x17000946 RID: 2374
		// (get) Token: 0x060031D0 RID: 12752 RVA: 0x00121398 File Offset: 0x0011F598
		private IntVec3 ClosestGatherSpotParentCell
		{
			get
			{
				return this.GatherSpotParent.OccupiedRect().ClosestCellTo(this.pawn.Position);
			}
		}

		// Token: 0x060031D1 RID: 12753 RVA: 0x001213C4 File Offset: 0x0011F5C4
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.job.GetTarget(TargetIndex.B), this.job, 1, -1, null, errorOnFailed) && (!this.HasDrink || this.pawn.Reserve(this.job.GetTarget(TargetIndex.C), this.job, 1, -1, null, errorOnFailed));
		}

		// Token: 0x060031D2 RID: 12754 RVA: 0x00121424 File Offset: 0x0011F624
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.EndOnDespawnedOrNull(TargetIndex.A, JobCondition.Incompletable);
			if (this.HasChair)
			{
				this.EndOnDespawnedOrNull(TargetIndex.B, JobCondition.Incompletable);
			}
			if (this.HasDrink)
			{
				this.FailOnDestroyedNullOrForbidden(TargetIndex.C);
				yield return Toils_Goto.GotoThing(TargetIndex.C, PathEndMode.OnCell).FailOnSomeonePhysicallyInteracting(TargetIndex.C);
				yield return Toils_Haul.StartCarryThing(TargetIndex.C, false, false, false);
			}
			yield return Toils_Goto.GotoCell(TargetIndex.B, PathEndMode.OnCell);
			Toil toil = new Toil();
			toil.tickAction = delegate()
			{
				this.pawn.rotationTracker.FaceCell(this.ClosestGatherSpotParentCell);
				this.pawn.GainComfortFromCellIfPossible(false);
				JoyUtility.JoyTickCheckEnd(this.pawn, JoyTickFullJoyAction.GoToNextToil, 1f, null);
			};
			toil.handlingFacing = true;
			toil.defaultCompleteMode = ToilCompleteMode.Delay;
			toil.defaultDuration = this.job.def.joyDuration;
			toil.AddFinishAction(delegate
			{
				JoyUtility.TryGainRecRoomThought(this.pawn);
			});
			toil.socialMode = RandomSocialMode.SuperActive;
			Toils_Ingest.AddIngestionEffects(toil, this.pawn, TargetIndex.C, TargetIndex.None);
			yield return toil;
			if (this.HasDrink)
			{
				yield return Toils_Ingest.FinalizeIngest(this.pawn, TargetIndex.C);
			}
			yield break;
		}

		// Token: 0x060031D3 RID: 12755 RVA: 0x00121434 File Offset: 0x0011F634
		public override bool ModifyCarriedThingDrawPos(ref Vector3 drawPos, ref bool behind, ref bool flip)
		{
			IntVec3 closestGatherSpotParentCell = this.ClosestGatherSpotParentCell;
			return JobDriver_Ingest.ModifyCarriedThingDrawPosWorker(ref drawPos, ref behind, ref flip, closestGatherSpotParentCell, this.pawn);
		}

		// Token: 0x04001D9A RID: 7578
		private const TargetIndex GatherSpotParentInd = TargetIndex.A;

		// Token: 0x04001D9B RID: 7579
		private const TargetIndex ChairOrSpotInd = TargetIndex.B;

		// Token: 0x04001D9C RID: 7580
		private const TargetIndex OptionalIngestibleInd = TargetIndex.C;
	}
}
