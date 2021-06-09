using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02000BD5 RID: 3029
	public class JobDriver_LinkPsylinkable : JobDriver
	{
		// Token: 0x17000B31 RID: 2865
		// (get) Token: 0x06004741 RID: 18241 RVA: 0x00197604 File Offset: 0x00195804
		private Thing PsylinkableThing
		{
			get
			{
				return base.TargetA.Thing;
			}
		}

		// Token: 0x17000B32 RID: 2866
		// (get) Token: 0x06004742 RID: 18242 RVA: 0x00033ECA File Offset: 0x000320CA
		private CompPsylinkable Psylinkable
		{
			get
			{
				return this.PsylinkableThing.TryGetComp<CompPsylinkable>();
			}
		}

		// Token: 0x17000B33 RID: 2867
		// (get) Token: 0x06004743 RID: 18243 RVA: 0x0002DE50 File Offset: 0x0002C050
		private LocalTargetInfo LinkSpot
		{
			get
			{
				return this.job.targetB;
			}
		}

		// Token: 0x06004744 RID: 18244 RVA: 0x00197620 File Offset: 0x00195820
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.PsylinkableThing, this.job, 1, -1, null, errorOnFailed) && this.pawn.Reserve(this.LinkSpot, this.job, 1, -1, null, errorOnFailed);
		}

		// Token: 0x06004745 RID: 18245 RVA: 0x00033ED7 File Offset: 0x000320D7
		protected override IEnumerable<Toil> MakeNewToils()
		{
			if (!ModLister.RoyaltyInstalled)
			{
				Log.ErrorOnce("Psylinkables are a Royalty-specific game system. If you want to use this code please check ModLister.RoyaltyInstalled before calling it. See rules on the Ludeon forum for more info.", 5464564, false);
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
					MoteMaker.MakeStaticMote(vector, this.pawn.Map, ThingDefOf.Mote_PsycastAreaEffect, 0.5f);
					this.Psylinkable.Props.linkSound.PlayOneShot(SoundInfo.InMap(new TargetInfo(this.PsylinkableThing), MaintenanceType.None));
				}
			};
			toil.handlingFacing = false;
			toil.socialMode = RandomSocialMode.Off;
			toil.WithProgressBarToilDelay(TargetIndex.A, false, -0.5f);
			yield return toil;
			yield return Toils_General.Do(delegate
			{
				this.Psylinkable.FinishLinkingRitual(this.pawn);
			});
			yield break;
		}

		// Token: 0x04002FBA RID: 12218
		public const int LinkTimeTicks = 15000;

		// Token: 0x04002FBB RID: 12219
		public const int EffectsTickInterval = 720;

		// Token: 0x04002FBC RID: 12220
		protected const TargetIndex PsylinkableInd = TargetIndex.A;

		// Token: 0x04002FBD RID: 12221
		protected const TargetIndex LinkSpotInd = TargetIndex.B;
	}
}
