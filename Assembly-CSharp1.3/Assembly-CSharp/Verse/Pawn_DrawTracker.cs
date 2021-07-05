using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200025F RID: 607
	public class Pawn_DrawTracker
	{
		// Token: 0x1700035F RID: 863
		// (get) Token: 0x06001135 RID: 4405 RVA: 0x00061E24 File Offset: 0x00060024
		public Vector3 DrawPos
		{
			get
			{
				this.tweener.PreDrawPosCalculation();
				Vector3 vector = this.tweener.TweenedPos;
				vector += this.jitterer.CurrentOffset;
				vector += this.leaner.LeanOffset;
				vector += this.OffsetForcedByJob();
				vector.y = this.pawn.def.Altitude;
				return vector;
			}
		}

		// Token: 0x06001136 RID: 4406 RVA: 0x00061E94 File Offset: 0x00060094
		public Pawn_DrawTracker(Pawn pawn)
		{
			this.pawn = pawn;
			this.tweener = new PawnTweener(pawn);
			this.jitterer = new JitterHandler();
			this.leaner = new PawnLeaner(pawn);
			this.renderer = new PawnRenderer(pawn);
			this.ui = new PawnUIOverlay(pawn);
			this.footprintMaker = new PawnFootprintMaker(pawn);
			this.breathMoteMaker = new PawnBreathMoteMaker(pawn);
		}

		// Token: 0x06001137 RID: 4407 RVA: 0x00061F04 File Offset: 0x00060104
		public void DrawTrackerTick()
		{
			if (!this.pawn.Spawned)
			{
				return;
			}
			if (Current.ProgramState == ProgramState.Playing && !Find.CameraDriver.CurrentViewRect.ExpandedBy(3).Contains(this.pawn.Position))
			{
				return;
			}
			this.jitterer.JitterHandlerTick();
			this.footprintMaker.FootprintMakerTick();
			this.breathMoteMaker.BreathMoteMakerTick();
			this.leaner.LeanerTick();
			this.renderer.RendererTick();
		}

		// Token: 0x06001138 RID: 4408 RVA: 0x00061F88 File Offset: 0x00060188
		public void DrawAt(Vector3 loc)
		{
			this.renderer.RenderPawnAt(loc, null, false);
		}

		// Token: 0x06001139 RID: 4409 RVA: 0x00061FAB File Offset: 0x000601AB
		private Vector3 OffsetForcedByJob()
		{
			if (this.pawn.jobs != null && this.pawn.jobs.curDriver != null)
			{
				return this.pawn.jobs.curDriver.ForcedBodyOffset;
			}
			return Vector3.zero;
		}

		// Token: 0x0600113A RID: 4410 RVA: 0x00061FE7 File Offset: 0x000601E7
		public void Notify_Spawned()
		{
			this.tweener.ResetTweenedPosToRoot();
		}

		// Token: 0x0600113B RID: 4411 RVA: 0x00061FF4 File Offset: 0x000601F4
		public void Notify_WarmingCastAlongLine(ShootLine newShootLine, IntVec3 ShootPosition)
		{
			this.leaner.Notify_WarmingCastAlongLine(newShootLine, ShootPosition);
		}

		// Token: 0x0600113C RID: 4412 RVA: 0x00062003 File Offset: 0x00060203
		public void Notify_DamageApplied(DamageInfo dinfo)
		{
			if (this.pawn.Destroyed || !this.pawn.Spawned)
			{
				return;
			}
			this.jitterer.Notify_DamageApplied(dinfo);
			this.renderer.Notify_DamageApplied(dinfo);
		}

		// Token: 0x0600113D RID: 4413 RVA: 0x00062038 File Offset: 0x00060238
		public void Notify_DamageDeflected(DamageInfo dinfo)
		{
			if (this.pawn.Destroyed)
			{
				return;
			}
			this.jitterer.Notify_DamageDeflected(dinfo);
		}

		// Token: 0x0600113E RID: 4414 RVA: 0x00062054 File Offset: 0x00060254
		public void Notify_MeleeAttackOn(Thing Target)
		{
			if (Target.Position != this.pawn.Position)
			{
				this.jitterer.AddOffset(0.5f, (Target.Position - this.pawn.Position).AngleFlat);
				return;
			}
			if (Target.DrawPos != this.pawn.DrawPos)
			{
				this.jitterer.AddOffset(0.25f, (Target.DrawPos - this.pawn.DrawPos).AngleFlat());
			}
		}

		// Token: 0x0600113F RID: 4415 RVA: 0x000620EC File Offset: 0x000602EC
		public void Notify_DebugAffected()
		{
			for (int i = 0; i < 10; i++)
			{
				FleckMaker.ThrowAirPuffUp(this.pawn.DrawPos, this.pawn.Map);
			}
			this.jitterer.AddOffset(0.05f, (float)Rand.Range(0, 360));
		}

		// Token: 0x04000D1C RID: 3356
		private Pawn pawn;

		// Token: 0x04000D1D RID: 3357
		public PawnTweener tweener;

		// Token: 0x04000D1E RID: 3358
		private JitterHandler jitterer;

		// Token: 0x04000D1F RID: 3359
		public PawnLeaner leaner;

		// Token: 0x04000D20 RID: 3360
		public PawnRenderer renderer;

		// Token: 0x04000D21 RID: 3361
		public PawnUIOverlay ui;

		// Token: 0x04000D22 RID: 3362
		private PawnFootprintMaker footprintMaker;

		// Token: 0x04000D23 RID: 3363
		private PawnBreathMoteMaker breathMoteMaker;

		// Token: 0x04000D24 RID: 3364
		private const float MeleeJitterDistance = 0.5f;
	}
}
