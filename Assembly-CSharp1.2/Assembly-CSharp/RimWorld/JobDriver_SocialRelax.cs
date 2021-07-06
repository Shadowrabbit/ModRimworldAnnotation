using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000B9F RID: 2975
	public class JobDriver_SocialRelax : JobDriver
	{
		// Token: 0x17000AEA RID: 2794
		// (get) Token: 0x060045E0 RID: 17888 RVA: 0x0018EA98 File Offset: 0x0018CC98
		private Thing GatherSpotParent
		{
			get
			{
				return this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x17000AEB RID: 2795
		// (get) Token: 0x060045E1 RID: 17889 RVA: 0x00193C44 File Offset: 0x00191E44
		private bool HasChair
		{
			get
			{
				return this.job.GetTarget(TargetIndex.B).HasThing;
			}
		}

		// Token: 0x17000AEC RID: 2796
		// (get) Token: 0x060045E2 RID: 17890 RVA: 0x00193C68 File Offset: 0x00191E68
		private bool HasDrink
		{
			get
			{
				return this.job.GetTarget(TargetIndex.C).HasThing;
			}
		}

		// Token: 0x17000AED RID: 2797
		// (get) Token: 0x060045E3 RID: 17891 RVA: 0x00193C8C File Offset: 0x00191E8C
		private IntVec3 ClosestGatherSpotParentCell
		{
			get
			{
				return this.GatherSpotParent.OccupiedRect().ClosestCellTo(this.pawn.Position);
			}
		}

		// Token: 0x060045E4 RID: 17892 RVA: 0x00193CB8 File Offset: 0x00191EB8
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.job.GetTarget(TargetIndex.B), this.job, 1, -1, null, errorOnFailed) && (!this.HasDrink || this.pawn.Reserve(this.job.GetTarget(TargetIndex.C), this.job, 1, -1, null, errorOnFailed));
		}

		// Token: 0x060045E5 RID: 17893 RVA: 0x000333CB File Offset: 0x000315CB
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

		// Token: 0x060045E6 RID: 17894 RVA: 0x00193D18 File Offset: 0x00191F18
		public override bool ModifyCarriedThingDrawPos(ref Vector3 drawPos, ref bool behind, ref bool flip)
		{
			IntVec3 closestGatherSpotParentCell = this.ClosestGatherSpotParentCell;
			return JobDriver_Ingest.ModifyCarriedThingDrawPosWorker(ref drawPos, ref behind, ref flip, closestGatherSpotParentCell, this.pawn);
		}

		// Token: 0x04002F1D RID: 12061
		private const TargetIndex GatherSpotParentInd = TargetIndex.A;

		// Token: 0x04002F1E RID: 12062
		private const TargetIndex ChairOrSpotInd = TargetIndex.B;

		// Token: 0x04002F1F RID: 12063
		private const TargetIndex OptionalIngestibleInd = TargetIndex.C;
	}
}
