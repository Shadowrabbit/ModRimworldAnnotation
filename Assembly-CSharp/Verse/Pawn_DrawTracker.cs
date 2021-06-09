using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200037B RID: 891
	public class Pawn_DrawTracker
	{
		// Token: 0x17000419 RID: 1049
		// (get) Token: 0x06001662 RID: 5730 RVA: 0x000D6250 File Offset: 0x000D4450
		public Vector3 DrawPos
		{
			get
			{
				this.tweener.PreDrawPosCalculation();
				Vector3 vector = this.tweener.TweenedPos;
				vector += this.jitterer.CurrentOffset;
				vector += this.leaner.LeanOffset;
				vector.y = this.pawn.def.Altitude;
				return vector;
			}
		}

		// Token: 0x06001663 RID: 5731 RVA: 0x000D62B0 File Offset: 0x000D44B0
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

		// Token: 0x06001664 RID: 5732 RVA: 0x000D6320 File Offset: 0x000D4520
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

		// Token: 0x06001665 RID: 5733 RVA: 0x00015D29 File Offset: 0x00013F29
		public void DrawAt(Vector3 loc)
		{
			this.renderer.RenderPawnAt(loc);
		}

		// Token: 0x06001666 RID: 5734 RVA: 0x00015D37 File Offset: 0x00013F37
		public void Notify_Spawned()
		{
			this.tweener.ResetTweenedPosToRoot();
		}

		// Token: 0x06001667 RID: 5735 RVA: 0x00015D44 File Offset: 0x00013F44
		public void Notify_WarmingCastAlongLine(ShootLine newShootLine, IntVec3 ShootPosition)
		{
			this.leaner.Notify_WarmingCastAlongLine(newShootLine, ShootPosition);
		}

		// Token: 0x06001668 RID: 5736 RVA: 0x00015D53 File Offset: 0x00013F53
		public void Notify_DamageApplied(DamageInfo dinfo)
		{
			if (this.pawn.Destroyed || !this.pawn.Spawned)
			{
				return;
			}
			this.jitterer.Notify_DamageApplied(dinfo);
			this.renderer.Notify_DamageApplied(dinfo);
		}

		// Token: 0x06001669 RID: 5737 RVA: 0x00015D88 File Offset: 0x00013F88
		public void Notify_DamageDeflected(DamageInfo dinfo)
		{
			if (this.pawn.Destroyed)
			{
				return;
			}
			this.jitterer.Notify_DamageDeflected(dinfo);
		}

		// Token: 0x0600166A RID: 5738 RVA: 0x000D63A4 File Offset: 0x000D45A4
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

		// Token: 0x0600166B RID: 5739 RVA: 0x000D643C File Offset: 0x000D463C
		public void Notify_DebugAffected()
		{
			for (int i = 0; i < 10; i++)
			{
				MoteMaker.ThrowAirPuffUp(this.pawn.DrawPos, this.pawn.Map);
			}
			this.jitterer.AddOffset(0.05f, (float)Rand.Range(0, 360));
		}

		// Token: 0x0400112C RID: 4396
		private Pawn pawn;

		// Token: 0x0400112D RID: 4397
		public PawnTweener tweener;

		// Token: 0x0400112E RID: 4398
		private JitterHandler jitterer;

		// Token: 0x0400112F RID: 4399
		public PawnLeaner leaner;

		// Token: 0x04001130 RID: 4400
		public PawnRenderer renderer;

		// Token: 0x04001131 RID: 4401
		public PawnUIOverlay ui;

		// Token: 0x04001132 RID: 4402
		private PawnFootprintMaker footprintMaker;

		// Token: 0x04001133 RID: 4403
		private PawnBreathMoteMaker breathMoteMaker;

		// Token: 0x04001134 RID: 4404
		private const float MeleeJitterDistance = 0.5f;
	}
}
