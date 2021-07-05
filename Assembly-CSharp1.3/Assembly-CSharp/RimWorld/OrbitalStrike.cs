using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001095 RID: 4245
	public class OrbitalStrike : ThingWithComps
	{
		// Token: 0x17001155 RID: 4437
		// (get) Token: 0x0600653C RID: 25916 RVA: 0x00223504 File Offset: 0x00221704
		protected int TicksPassed
		{
			get
			{
				return Find.TickManager.TicksGame - this.startTick;
			}
		}

		// Token: 0x17001156 RID: 4438
		// (get) Token: 0x0600653D RID: 25917 RVA: 0x00223517 File Offset: 0x00221717
		protected int TicksLeft
		{
			get
			{
				return this.duration - this.TicksPassed;
			}
		}

		// Token: 0x0600653E RID: 25918 RVA: 0x00223528 File Offset: 0x00221728
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Thing>(ref this.instigator, "instigator", false);
			Scribe_Defs.Look<ThingDef>(ref this.weaponDef, "weaponDef");
			Scribe_Values.Look<int>(ref this.duration, "duration", 0, false);
			Scribe_Values.Look<float>(ref this.angle, "angle", 0f, false);
			Scribe_Values.Look<int>(ref this.startTick, "startTick", 0, false);
		}

		// Token: 0x0600653F RID: 25919 RVA: 0x00223596 File Offset: 0x00221796
		public override void Draw()
		{
			base.Comps_PostDraw();
		}

		// Token: 0x06006540 RID: 25920 RVA: 0x002235A0 File Offset: 0x002217A0
		public virtual void StartStrike()
		{
			if (!base.Spawned)
			{
				Log.Error("Called StartStrike() on unspawned thing.");
				return;
			}
			this.angle = OrbitalStrike.AngleRange.RandomInRange;
			this.startTick = Find.TickManager.TicksGame;
			CompAffectsSky comp = base.GetComp<CompAffectsSky>();
			if (comp != null)
			{
				comp.StartFadeInHoldFadeOut(30, this.duration - 30 - 15, 15, 1f);
			}
			base.GetComp<CompOrbitalBeam>().StartAnimation(this.duration, 10, this.angle);
		}

		// Token: 0x06006541 RID: 25921 RVA: 0x00223621 File Offset: 0x00221821
		public override void Tick()
		{
			base.Tick();
			if (this.TicksPassed >= this.duration)
			{
				this.Destroy(DestroyMode.Vanish);
			}
		}

		// Token: 0x040038F8 RID: 14584
		public int duration;

		// Token: 0x040038F9 RID: 14585
		public Thing instigator;

		// Token: 0x040038FA RID: 14586
		public ThingDef weaponDef;

		// Token: 0x040038FB RID: 14587
		private float angle;

		// Token: 0x040038FC RID: 14588
		private int startTick;

		// Token: 0x040038FD RID: 14589
		private static readonly FloatRange AngleRange = new FloatRange(-12f, 12f);

		// Token: 0x040038FE RID: 14590
		private const int SkyColorFadeInTicks = 30;

		// Token: 0x040038FF RID: 14591
		private const int SkyColorFadeOutTicks = 15;

		// Token: 0x04003900 RID: 14592
		private const int OrbitalBeamFadeOutTicks = 10;
	}
}
