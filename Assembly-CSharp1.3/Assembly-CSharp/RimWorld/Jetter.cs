using System;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x020010AF RID: 4271
	public class Jetter : Thing
	{
		// Token: 0x06006609 RID: 26121 RVA: 0x00227368 File Offset: 0x00225568
		public override void Tick()
		{
			if (this.JState == Jetter.JetterState.WickBurning)
			{
				base.Map.overlayDrawer.DrawOverlay(this, OverlayTypes.BurningWick);
				this.WickTicksLeft--;
				if (this.WickTicksLeft == 0)
				{
					this.StartJetting();
					return;
				}
			}
			else if (this.JState == Jetter.JetterState.Jetting)
			{
				this.TicksUntilMove--;
				if (this.TicksUntilMove <= 0)
				{
					this.MoveJetter();
					this.TicksUntilMove = 3;
				}
			}
		}

		// Token: 0x0600660A RID: 26122 RVA: 0x002273DA File Offset: 0x002255DA
		public override void PostApplyDamage(DamageInfo dinfo, float totalDamageDealt)
		{
			base.PostApplyDamage(dinfo, totalDamageDealt);
			if (!base.Destroyed && dinfo.Def.harmsHealth && this.JState == Jetter.JetterState.Resting)
			{
				this.StartWick();
			}
		}

		// Token: 0x0600660B RID: 26123 RVA: 0x00227408 File Offset: 0x00225608
		protected void StartWick()
		{
			this.JState = Jetter.JetterState.WickBurning;
			this.WickTicksLeft = 25;
			SoundDefOf.MetalHitImportant.PlayOneShot(this);
			this.wickSoundSustainer = SoundDefOf.HissSmall.TrySpawnSustainer(this);
		}

		// Token: 0x0600660C RID: 26124 RVA: 0x0022743F File Offset: 0x0022563F
		protected void StartJetting()
		{
			this.JState = Jetter.JetterState.Jetting;
			this.TicksUntilMove = 3;
			this.wickSoundSustainer.End();
			this.wickSoundSustainer = null;
			this.wickSoundSustainer = SoundDefOf.HissJet.TrySpawnSustainer(this);
		}

		// Token: 0x0600660D RID: 26125 RVA: 0x00227478 File Offset: 0x00225678
		protected void MoveJetter()
		{
			IntVec3 intVec = base.Position + base.Rotation.FacingCell;
			if (!intVec.Walkable(base.Map) || base.Map.thingGrid.CellContains(intVec, ThingCategory.Pawn) || intVec.GetEdifice(base.Map) != null)
			{
				this.Destroy(DestroyMode.Vanish);
				GenExplosion.DoExplosion(base.Position, base.Map, 2.9f, DamageDefOf.Bomb, null, -1, -1f, null, null, null, null, null, 0f, 1, false, null, 0f, 1, 0f, false, null, null);
				return;
			}
			base.Position = intVec;
		}

		// Token: 0x0600660E RID: 26126 RVA: 0x00227523 File Offset: 0x00225723
		public override void Destroy(DestroyMode mode = DestroyMode.Vanish)
		{
			base.Destroy(mode);
			if (this.wickSoundSustainer != null)
			{
				this.wickSoundSustainer.End();
				this.wickSoundSustainer = null;
			}
			if (this.jetSoundSustainer != null)
			{
				this.jetSoundSustainer.End();
				this.jetSoundSustainer = null;
			}
		}

		// Token: 0x04003990 RID: 14736
		private Jetter.JetterState JState;

		// Token: 0x04003991 RID: 14737
		private int WickTicksLeft;

		// Token: 0x04003992 RID: 14738
		private int TicksUntilMove;

		// Token: 0x04003993 RID: 14739
		protected Sustainer wickSoundSustainer;

		// Token: 0x04003994 RID: 14740
		protected Sustainer jetSoundSustainer;

		// Token: 0x04003995 RID: 14741
		private const int TicksBeforeBeginAccelerate = 25;

		// Token: 0x04003996 RID: 14742
		private const int TicksBetweenMoves = 3;

		// Token: 0x020024E8 RID: 9448
		private enum JetterState
		{
			// Token: 0x04008C8E RID: 35982
			Resting,
			// Token: 0x04008C8F RID: 35983
			WickBurning,
			// Token: 0x04008C90 RID: 35984
			Jetting
		}
	}
}
