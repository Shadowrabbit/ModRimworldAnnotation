using System;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001512 RID: 5394
	public class Verb_Jump : Verb
	{
		// Token: 0x170015DF RID: 5599
		// (get) Token: 0x0600806E RID: 32878 RVA: 0x002D7FE6 File Offset: 0x002D61E6
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

		// Token: 0x170015E0 RID: 5600
		// (get) Token: 0x0600806F RID: 32879 RVA: 0x000126F5 File Offset: 0x000108F5
		public override bool MultiSelect
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06008070 RID: 32880 RVA: 0x002D8014 File Offset: 0x002D6214
		protected override bool TryCastShot()
		{
			if (!ModLister.CheckRoyalty("Jumping"))
			{
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

		// Token: 0x06008071 RID: 32881 RVA: 0x002D8088 File Offset: 0x002D6288
		public override void OrderForceTarget(LocalTargetInfo target)
		{
			Verb_Jump.<>c__DisplayClass6_0 CS$<>8__locals1 = new Verb_Jump.<>c__DisplayClass6_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.map = this.CasterPawn.Map;
			IntVec3 intVec = RCellFinder.BestOrderedGotoDestNear(target.Cell, this.CasterPawn, new Predicate<IntVec3>(CS$<>8__locals1.<OrderForceTarget>g__AcceptableDestination|0));
			Job job = JobMaker.MakeJob(JobDefOf.CastJump, intVec);
			job.verbToUse = this;
			if (this.CasterPawn.jobs.TryTakeOrderedJob(job, new JobTag?(JobTag.Misc), false))
			{
				FleckMaker.Static(intVec, CS$<>8__locals1.map, FleckDefOf.FeedbackGoto, 1f);
			}
		}

		// Token: 0x06008072 RID: 32882 RVA: 0x002D811C File Offset: 0x002D631C
		public override bool ValidateTarget(LocalTargetInfo target, bool showMessages = true)
		{
			return this.caster != null && this.CanHitTarget(target) && Verb_Jump.ValidJumpTarget(this.caster.Map, target.Cell) && ReloadableUtility.CanUseConsideringQueuedJobs(this.CasterPawn, base.EquipmentSource, true);
		}

		// Token: 0x06008073 RID: 32883 RVA: 0x002D8170 File Offset: 0x002D6370
		public override bool CanHitTargetFrom(IntVec3 root, LocalTargetInfo targ)
		{
			float num = this.EffectiveRange * this.EffectiveRange;
			IntVec3 cell = targ.Cell;
			return (float)this.caster.Position.DistanceToSquared(cell) <= num && GenSight.LineOfSight(root, cell, this.caster.Map, false, null, 0, 0);
		}

		// Token: 0x06008074 RID: 32884 RVA: 0x002D81C0 File Offset: 0x002D63C0
		public override void OnGUI(LocalTargetInfo target)
		{
			if (this.CanHitTarget(target) && Verb_Jump.ValidJumpTarget(this.caster.Map, target.Cell))
			{
				base.OnGUI(target);
				return;
			}
			GenUI.DrawMouseAttachment(TexCommand.CannotShoot);
		}

		// Token: 0x06008075 RID: 32885 RVA: 0x002D81F8 File Offset: 0x002D63F8
		public override void DrawHighlight(LocalTargetInfo target)
		{
			if (target.IsValid && Verb_Jump.ValidJumpTarget(this.caster.Map, target.Cell))
			{
				GenDraw.DrawTargetHighlightWithLayer(target.CenterVector3, AltitudeLayer.MetaOverlays);
			}
			GenDraw.DrawRadiusRing(this.caster.Position, this.EffectiveRange, Color.white, (IntVec3 c) => GenSight.LineOfSight(this.caster.Position, c, this.caster.Map, false, null, 0, 0) && Verb_Jump.ValidJumpTarget(this.caster.Map, c));
		}

		// Token: 0x06008076 RID: 32886 RVA: 0x002D825C File Offset: 0x002D645C
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

		// Token: 0x04004FFC RID: 20476
		private float cachedEffectiveRange = -1f;
	}
}
