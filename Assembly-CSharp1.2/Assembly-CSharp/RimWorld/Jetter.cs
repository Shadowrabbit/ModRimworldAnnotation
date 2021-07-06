using System;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x0200170C RID: 5900
	public class Jetter : Thing
	{
		// Token: 0x060081E8 RID: 33256 RVA: 0x00268130 File Offset: 0x00266330
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

		// Token: 0x060081E9 RID: 33257 RVA: 0x00057394 File Offset: 0x00055594
		public override void PostApplyDamage(DamageInfo dinfo, float totalDamageDealt)
		{
			base.PostApplyDamage(dinfo, totalDamageDealt);
			if (!base.Destroyed && dinfo.Def.harmsHealth && this.JState == Jetter.JetterState.Resting)
			{
				this.StartWick();
			}
		}

		// Token: 0x060081EA RID: 33258 RVA: 0x000573C2 File Offset: 0x000555C2
		protected void StartWick()
		{
			this.JState = Jetter.JetterState.WickBurning;
			this.WickTicksLeft = 25;
			SoundDefOf.MetalHitImportant.PlayOneShot(this);
			this.wickSoundSustainer = SoundDefOf.HissSmall.TrySpawnSustainer(this);
		}

		// Token: 0x060081EB RID: 33259 RVA: 0x000573F9 File Offset: 0x000555F9
		protected void StartJetting()
		{
			this.JState = Jetter.JetterState.Jetting;
			this.TicksUntilMove = 3;
			this.wickSoundSustainer.End();
			this.wickSoundSustainer = null;
			this.wickSoundSustainer = SoundDefOf.HissJet.TrySpawnSustainer(this);
		}

		// Token: 0x060081EC RID: 33260 RVA: 0x002681A4 File Offset: 0x002663A4
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

		// Token: 0x060081ED RID: 33261 RVA: 0x00057431 File Offset: 0x00055631
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

		// Token: 0x0400543F RID: 21567
		private Jetter.JetterState JState;

		// Token: 0x04005440 RID: 21568
		private int WickTicksLeft;

		// Token: 0x04005441 RID: 21569
		private int TicksUntilMove;

		// Token: 0x04005442 RID: 21570
		protected Sustainer wickSoundSustainer;

		// Token: 0x04005443 RID: 21571
		protected Sustainer jetSoundSustainer;

		// Token: 0x04005444 RID: 21572
		private const int TicksBeforeBeginAccelerate = 25;

		// Token: 0x04005445 RID: 21573
		private const int TicksBetweenMoves = 3;

		// Token: 0x0200170D RID: 5901
		private enum JetterState
		{
			// Token: 0x04005447 RID: 21575
			Resting,
			// Token: 0x04005448 RID: 21576
			WickBurning,
			// Token: 0x04005449 RID: 21577
			Jetting
		}
	}
}
