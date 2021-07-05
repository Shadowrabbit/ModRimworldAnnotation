using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02000718 RID: 1816
	public class JobDriver_Hack : JobDriver
	{
		// Token: 0x17000961 RID: 2401
		// (get) Token: 0x06003266 RID: 12902 RVA: 0x00122865 File Offset: 0x00120A65
		private Thing HackTarget
		{
			get
			{
				return base.TargetThingA;
			}
		}

		// Token: 0x17000962 RID: 2402
		// (get) Token: 0x06003267 RID: 12903 RVA: 0x0012286D File Offset: 0x00120A6D
		private CompHackable CompHacking
		{
			get
			{
				return this.HackTarget.TryGetComp<CompHackable>();
			}
		}

		// Token: 0x06003268 RID: 12904 RVA: 0x0012287A File Offset: 0x00120A7A
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.HackTarget, this.job, 1, -1, null, errorOnFailed);
		}

		// Token: 0x06003269 RID: 12905 RVA: 0x0012289C File Offset: 0x00120A9C
		protected override IEnumerable<Toil> MakeNewToils()
		{
			if (!ModLister.CheckIdeology("Hack"))
			{
				yield break;
			}
			this.FailOnDespawnedNullOrForbidden(TargetIndex.A);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.InteractionCell);
			Toil toil = new Toil();
			toil.handlingFacing = true;
			toil.tickAction = delegate()
			{
				float statValue = this.pawn.GetStatValue(StatDefOf.HackingSpeed, true);
				this.CompHacking.Hack(statValue, this.pawn);
				this.pawn.skills.Learn(SkillDefOf.Intellectual, 0.1f, false);
				this.pawn.rotationTracker.FaceTarget(this.HackTarget);
			};
			toil.WithEffect(EffecterDefOf.Hacking, TargetIndex.A, null);
			if (this.CompHacking.Props.effectHacking != null)
			{
				toil.WithEffect(() => this.CompHacking.Props.effectHacking, () => this.HackTarget.OccupiedRect().ClosestCellTo(this.pawn.Position), null);
			}
			toil.WithProgressBar(TargetIndex.A, () => this.CompHacking.ProgressPercent, false, -0.5f, true);
			toil.PlaySoundAtStart(SoundDefOf.Hacking_Started);
			toil.PlaySustainerOrSound(SoundDefOf.Hacking_InProgress, 1f);
			toil.AddFinishAction(delegate
			{
				if (this.CompHacking.IsHacked)
				{
					SoundDefOf.Hacking_Completed.PlayOneShot(this.HackTarget);
					if (this.CompHacking.Props.hackingCompletedSound != null)
					{
						this.CompHacking.Props.hackingCompletedSound.PlayOneShot(this.HackTarget);
						return;
					}
				}
				else
				{
					SoundDefOf.Hacking_Suspended.PlayOneShot(this.HackTarget);
				}
			});
			toil.FailOnCannotTouch(TargetIndex.A, PathEndMode.InteractionCell);
			toil.FailOn(() => this.CompHacking.IsHacked);
			toil.defaultCompleteMode = ToilCompleteMode.Never;
			toil.activeSkill = (() => SkillDefOf.Intellectual);
			yield return toil;
			yield break;
		}
	}
}
