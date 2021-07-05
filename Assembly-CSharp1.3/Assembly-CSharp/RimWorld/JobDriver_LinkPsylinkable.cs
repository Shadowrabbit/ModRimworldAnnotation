using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02000723 RID: 1827
	public class JobDriver_LinkPsylinkable : JobDriver
	{
		// Token: 0x17000976 RID: 2422
		// (get) Token: 0x060032BB RID: 12987 RVA: 0x00123494 File Offset: 0x00121694
		private Thing PsylinkableThing
		{
			get
			{
				return base.TargetA.Thing;
			}
		}

		// Token: 0x17000977 RID: 2423
		// (get) Token: 0x060032BC RID: 12988 RVA: 0x001234AF File Offset: 0x001216AF
		private CompPsylinkable Psylinkable
		{
			get
			{
				return this.PsylinkableThing.TryGetComp<CompPsylinkable>();
			}
		}

		// Token: 0x17000978 RID: 2424
		// (get) Token: 0x060032BD RID: 12989 RVA: 0x000FE3EF File Offset: 0x000FC5EF
		private LocalTargetInfo LinkSpot
		{
			get
			{
				return this.job.targetB;
			}
		}

		// Token: 0x060032BE RID: 12990 RVA: 0x001234BC File Offset: 0x001216BC
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.PsylinkableThing, this.job, 1, -1, null, errorOnFailed) && this.pawn.Reserve(this.LinkSpot, this.job, 1, -1, null, errorOnFailed);
		}

		// Token: 0x060032BF RID: 12991 RVA: 0x00123508 File Offset: 0x00121708
		protected override IEnumerable<Toil> MakeNewToils()
		{
			if (!ModLister.CheckRoyalty("Psylinkable"))
			{
				yield break;
			}
			base.AddFailCondition(() => !this.Psylinkable.CanPsylink(this.pawn, new LocalTargetInfo?(this.LinkSpot)).Accepted);
			yield return Toils_Goto.GotoCell(TargetIndex.B, PathEndMode.OnCell);
			Toil toil = Toils_General.Wait(15000, TargetIndex.None);
			toil.tickAction = delegate()
			{
				this.pawn.rotationTracker.FaceTarget(this.PsylinkableThing);
				if (Find.TickManager.TicksGame % 720 == 0)
				{
					Vector3 vector = this.pawn.TrueCenter();
					vector += (this.PsylinkableThing.TrueCenter() - vector) * Rand.Value;
					FleckMaker.Static(vector, this.pawn.Map, FleckDefOf.PsycastAreaEffect, 0.5f);
					this.Psylinkable.Props.linkSound.PlayOneShot(SoundInfo.InMap(new TargetInfo(this.PsylinkableThing), MaintenanceType.None));
				}
			};
			toil.handlingFacing = false;
			toil.socialMode = RandomSocialMode.Off;
			yield return toil;
			yield break;
		}

		// Token: 0x04001DCF RID: 7631
		public const int LinkTimeTicks = 15000;

		// Token: 0x04001DD0 RID: 7632
		public const int EffectsTickInterval = 720;

		// Token: 0x04001DD1 RID: 7633
		protected const TargetIndex PsylinkableInd = TargetIndex.A;

		// Token: 0x04001DD2 RID: 7634
		protected const TargetIndex LinkSpotInd = TargetIndex.B;
	}
}
