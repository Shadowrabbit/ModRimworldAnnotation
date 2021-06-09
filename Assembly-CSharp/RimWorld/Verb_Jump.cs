using System;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001D87 RID: 7559
	public class Verb_Jump : Verb
	{
		// Token: 0x1700192E RID: 6446
		// (get) Token: 0x0600A43D RID: 42045 RVA: 0x0006CE52 File Offset: 0x0006B052
		protected override float EffectiveRange
		{
			get
			{
				if (this.cachedEffectiveRange < 0f)
				{
					this.cachedEffectiveRange = base.EquipmentSource.GetStatValue(StatDefOf.JumpRange, true);
				}
				return this.cachedEffectiveRange;
			}
		}

		// Token: 0x1700192F RID: 6447
		// (get) Token: 0x0600A43E RID: 42046 RVA: 0x0000A2A7 File Offset: 0x000084A7
		public override bool MultiSelect
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600A43F RID: 42047 RVA: 0x002FD018 File Offset: 0x002FB218
		protected override bool TryCastShot()
		{
			if (!ModLister.RoyaltyInstalled)
			{
				Log.ErrorOnce("Items with jump capability are a Royalty-specific game system. If you want to use this code please check ModLister.RoyaltyInstalled before calling it. See rules on the Ludeon forum for more info.", 550187797, false);
				return false;
			}
			CompReloadable reloadableCompSource = base.ReloadableCompSource;
			Pawn casterPawn = this.CasterPawn;
			if (casterPawn == null || reloadableCompSource == null || !reloadableCompSource.CanBeUsed)
			{
				return false;
			}
			IntVec3 cell = this.currentTarget.Cell;
			Map map = casterPawn.Map;
			reloadableCompSource.UsedOnce();
			PawnFlyer pawnFlyer = PawnFlyer.MakeFlyer(ThingDefOf.PawnJumper, casterPawn, cell);
			if (pawnFlyer != null)
			{
				GenSpawn.Spawn(pawnFlyer, cell, map, WipeMode.Vanish);
				return true;
			}
			return false;
		}

		// Token: 0x0600A440 RID: 42048 RVA: 0x002FD098 File Offset: 0x002FB298
		public override void OrderForceTarget(LocalTargetInfo target)
		{
			Verb_Jump.<>c__DisplayClass6_0 CS$<>8__locals1 = new Verb_Jump.<>c__DisplayClass6_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.map = this.CasterPawn.Map;
			IntVec3 intVec = RCellFinder.BestOrderedGotoDestNear_NewTemp(target.Cell, this.CasterPawn, new Predicate<IntVec3>(CS$<>8__locals1.<OrderForceTarget>g__AcceptableDestination|0));
			Job job = JobMaker.MakeJob(JobDefOf.CastJump, intVec);
			job.verbToUse = this;
			if (this.CasterPawn.jobs.TryTakeOrderedJob(job, JobTag.Misc))
			{
				MoteMaker.MakeStaticMote(intVec, CS$<>8__locals1.map, ThingDefOf.Mote_FeedbackGoto, 1f);
			}
		}

		// Token: 0x0600A441 RID: 42049 RVA: 0x002FD124 File Offset: 0x002FB324
		public override bool ValidateTarget(LocalTargetInfo target)
		{
			return this.caster != null && this.CanHitTarget(target) && Verb_Jump.ValidJumpTarget(this.caster.Map, target.Cell) && ReloadableUtility.CanUseConsideringQueuedJobs(this.CasterPawn, base.EquipmentSource, true);
		}

		// Token: 0x0600A442 RID: 42050 RVA: 0x002FD178 File Offset: 0x002FB378
		public override bool CanHitTargetFrom(IntVec3 root, LocalTargetInfo targ)
		{
			float num = this.EffectiveRange * this.EffectiveRange;
			IntVec3 cell = targ.Cell;
			return (float)this.caster.Position.DistanceToSquared(cell) <= num && GenSight.LineOfSight(root, cell, this.caster.Map, false, null, 0, 0);
		}

		// Token: 0x0600A443 RID: 42051 RVA: 0x0006CE7E File Offset: 0x0006B07E
		public override void OnGUI(LocalTargetInfo target)
		{
			if (this.CanHitTarget(target) && Verb_Jump.ValidJumpTarget(this.caster.Map, target.Cell))
			{
				base.OnGUI(target);
				return;
			}
			GenUI.DrawMouseAttachment(TexCommand.CannotShoot);
		}

		// Token: 0x0600A444 RID: 42052 RVA: 0x002FD1C8 File Offset: 0x002FB3C8
		public override void DrawHighlight(LocalTargetInfo target)
		{
			if (target.IsValid && Verb_Jump.ValidJumpTarget(this.caster.Map, target.Cell))
			{
				GenDraw.DrawTargetHighlightWithLayer(target.CenterVector3, AltitudeLayer.MetaOverlays);
			}
			GenDraw.DrawRadiusRing(this.caster.Position, this.EffectiveRange, Color.white, (IntVec3 c) => GenSight.LineOfSight(this.caster.Position, c, this.caster.Map, false, null, 0, 0) && Verb_Jump.ValidJumpTarget(this.caster.Map, c));
		}

		// Token: 0x0600A445 RID: 42053 RVA: 0x002FD22C File Offset: 0x002FB42C
		public static bool ValidJumpTarget(Map map, IntVec3 cell)
		{
			if (!cell.IsValid || !cell.InBounds(map))
			{
				return false;
			}
			if (cell.Impassable(map) || !cell.Walkable(map) || cell.Fogged(map))
			{
				return false;
			}
			Building edifice = cell.GetEdifice(map);
			Building_Door building_Door;
			return edifice == null || (building_Door = (edifice as Building_Door)) == null || building_Door.Open;
		}

		// Token: 0x04006F59 RID: 28505
		private float cachedEffectiveRange = -1f;
	}
}
