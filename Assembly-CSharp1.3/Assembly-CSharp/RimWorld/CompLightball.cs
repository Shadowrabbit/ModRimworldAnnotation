using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020011FB RID: 4603
	public class CompLightball : ThingComp
	{
		// Token: 0x17001338 RID: 4920
		// (get) Token: 0x06006EB7 RID: 28343 RVA: 0x00250CAD File Offset: 0x0024EEAD
		private CompProperties_Lightball Props
		{
			get
			{
				return (CompProperties_Lightball)this.props;
			}
		}

		// Token: 0x17001339 RID: 4921
		// (get) Token: 0x06006EB8 RID: 28344 RVA: 0x00250CBA File Offset: 0x0024EEBA
		public bool Playing
		{
			get
			{
				return this.soundToPlay != null;
			}
		}

		// Token: 0x1700133A RID: 4922
		// (get) Token: 0x06006EB9 RID: 28345 RVA: 0x00250CC5 File Offset: 0x0024EEC5
		public SoundDef SoundToPlay
		{
			get
			{
				return this.soundToPlay;
			}
		}

		// Token: 0x06006EBA RID: 28346 RVA: 0x00250CD0 File Offset: 0x0024EED0
		public override void CompTick()
		{
			base.CompTick();
			if (!ModLister.CheckIdeology("Lightball"))
			{
				return;
			}
			if (this.parent.IsHashIntervalTick(20) || this.numActiveSpeakers == -1)
			{
				this.numActiveSpeakers = 0;
				foreach (Thing thing in this.parent.Map.listerBuldingOfDefInProximity.GetForCell(this.parent.Position, (float)this.Props.maxSpeakerDistance, ThingDefOf.Loudspeaker, null))
				{
					CompPowerTrader compPowerTrader = thing.TryGetComp<CompPowerTrader>();
					if (thing.GetRoom(RegionType.Set_All) == this.parent.GetRoom(RegionType.Set_All) && compPowerTrader.PowerOn)
					{
						this.numActiveSpeakers++;
					}
				}
			}
			if (this.compPowerCached == null)
			{
				this.compPowerCached = this.parent.GetComp<CompPowerTrader>();
			}
			if (this.parent.IsHashIntervalTick(30))
			{
				this.inRitual = this.parent.IsRitualTarget();
			}
			if (this.compPowerCached.PowerOn && !this.inRitual)
			{
				this.parent.BroadcastCompSignal("AutoPoweredWantsOff");
			}
			if (this.compPowerCached.PowerOn && this.inRitual)
			{
				if (this.numActiveSpeakers > 0)
				{
					this.soundToPlay = ((!this.Props.soundDefsPerSpeakerCount.NullOrEmpty<SoundDef>()) ? this.Props.soundDefsPerSpeakerCount[Math.Min(this.numActiveSpeakers, this.Props.soundDefsPerSpeakerCount.Count) - 1] : null);
				}
				if (this.rotationMote == null || this.rotationMote.Destroyed)
				{
					this.rotationMote = MoteMaker.MakeStaticMote(this.parent.TrueCenter(), this.parent.Map, ThingDefOf.Mote_LightBall, 1f);
				}
				if (this.lightsMote == null || this.lightsMote.Destroyed)
				{
					this.lightsMote = MoteMaker.MakeStaticMote(this.parent.TrueCenter(), this.parent.Map, ThingDefOf.Mote_LightBallLights, 1f);
					if (this.lightsMote != null)
					{
						this.lightsMote.rotationRate = -3f;
					}
				}
			}
			else
			{
				Mote mote = this.rotationMote;
				if (mote != null)
				{
					mote.Destroy(DestroyMode.Vanish);
				}
				this.rotationMote = null;
				Mote mote2 = this.lightsMote;
				if (mote2 != null)
				{
					mote2.Destroy(DestroyMode.Vanish);
				}
				this.lightsMote = null;
			}
			if (this.inRitual)
			{
				Mote mote3 = this.rotationMote;
				if (mote3 != null)
				{
					mote3.Maintain();
				}
				Mote mote4 = this.lightsMote;
				if (mote4 == null)
				{
					return;
				}
				mote4.Maintain();
			}
		}

		// Token: 0x06006EBB RID: 28347 RVA: 0x00250F70 File Offset: 0x0024F170
		public override void Notify_SignalReceived(Signal signal)
		{
			if (signal.tag == "RitualStarted")
			{
				this.inRitual = this.parent.IsRitualTarget();
			}
		}

		// Token: 0x06006EBC RID: 28348 RVA: 0x00250F95 File Offset: 0x0024F195
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			if (respawningAfterLoad)
			{
				this.inRitual = this.parent.IsRitualTarget();
			}
		}

		// Token: 0x04003D4C RID: 15692
		private CompPowerTrader compPowerCached;

		// Token: 0x04003D4D RID: 15693
		private Mote rotationMote;

		// Token: 0x04003D4E RID: 15694
		private Mote lightsMote;

		// Token: 0x04003D4F RID: 15695
		private bool inRitual;

		// Token: 0x04003D50 RID: 15696
		private int numActiveSpeakers = -1;

		// Token: 0x04003D51 RID: 15697
		private SoundDef soundToPlay;
	}
}
