using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020016F4 RID: 5876
	public class OrbitalStrike : ThingWithComps
	{
		// Token: 0x1700140F RID: 5135
		// (get) Token: 0x06008124 RID: 33060 RVA: 0x00056B70 File Offset: 0x00054D70
		protected int TicksPassed
		{
			get
			{
				return Find.TickManager.TicksGame - this.startTick;
			}
		}

		// Token: 0x17001410 RID: 5136
		// (get) Token: 0x06008125 RID: 33061 RVA: 0x00056B83 File Offset: 0x00054D83
		protected int TicksLeft
		{
			get
			{
				return this.duration - this.TicksPassed;
			}
		}

		// Token: 0x06008126 RID: 33062 RVA: 0x00264E30 File Offset: 0x00263030
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Thing>(ref this.instigator, "instigator", false);
			Scribe_Defs.Look<ThingDef>(ref this.weaponDef, "weaponDef");
			Scribe_Values.Look<int>(ref this.duration, "duration", 0, false);
			Scribe_Values.Look<float>(ref this.angle, "angle", 0f, false);
			Scribe_Values.Look<int>(ref this.startTick, "startTick", 0, false);
		}

		// Token: 0x06008127 RID: 33063 RVA: 0x00056B92 File Offset: 0x00054D92
		public override void Draw()
		{
			base.Comps_PostDraw();
		}

		// Token: 0x06008128 RID: 33064 RVA: 0x00264EA0 File Offset: 0x002630A0
		public virtual void StartStrike()
		{
			if (!base.Spawned)
			{
				Log.Error("Called StartStrike() on unspawned thing.", false);
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

		// Token: 0x06008129 RID: 33065 RVA: 0x00056B9A File Offset: 0x00054D9A
		public override void Tick()
		{
			base.Tick();
			if (this.TicksPassed >= this.duration)
			{
				this.Destroy(DestroyMode.Vanish);
			}
		}

		// Token: 0x040053A7 RID: 21415
		public int duration;

		// Token: 0x040053A8 RID: 21416
		public Thing instigator;

		// Token: 0x040053A9 RID: 21417
		public ThingDef weaponDef;

		// Token: 0x040053AA RID: 21418
		private float angle;

		// Token: 0x040053AB RID: 21419
		private int startTick;

		// Token: 0x040053AC RID: 21420
		private static readonly FloatRange AngleRange = new FloatRange(-12f, 12f);

		// Token: 0x040053AD RID: 21421
		private const int SkyColorFadeInTicks = 30;

		// Token: 0x040053AE RID: 21422
		private const int SkyColorFadeOutTicks = 15;

		// Token: 0x040053AF RID: 21423
		private const int OrbitalBeamFadeOutTicks = 10;
	}
}
