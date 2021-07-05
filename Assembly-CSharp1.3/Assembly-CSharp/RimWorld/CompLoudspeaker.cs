using System;
using System.Collections.Generic;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02001203 RID: 4611
	public class CompLoudspeaker : CompAutoPowered
	{
		// Token: 0x1700133F RID: 4927
		// (get) Token: 0x06006ED3 RID: 28371 RVA: 0x002511F0 File Offset: 0x0024F3F0
		private CompProperties_Loudspeaker Props
		{
			get
			{
				return (CompProperties_Loudspeaker)this.props;
			}
		}

		// Token: 0x17001340 RID: 4928
		// (get) Token: 0x06006ED4 RID: 28372 RVA: 0x002511FD File Offset: 0x0024F3FD
		public bool Active
		{
			get
			{
				return this.inRitual;
			}
		}

		// Token: 0x17001341 RID: 4929
		// (get) Token: 0x06006ED5 RID: 28373 RVA: 0x002511FD File Offset: 0x0024F3FD
		public override bool WantsToBeOn
		{
			get
			{
				return this.inRitual;
			}
		}

		// Token: 0x06006ED6 RID: 28374 RVA: 0x00251208 File Offset: 0x0024F408
		public override void CompTick()
		{
			base.CompTick();
			if (!ModLister.CheckIdeology("Speaker"))
			{
				return;
			}
			if (this.compPowerCached == null)
			{
				this.compPowerCached = this.parent.GetComp<CompPowerTrader>();
			}
			if (this.parent.IsHashIntervalTick(30))
			{
				this.UpdateInRitual();
			}
			if (this.compPowerCached.PowerOn && this.inRitual)
			{
				if (this.parent.Rotation == Rot4.North && (this.lightsMote == null || this.lightsMote.Destroyed))
				{
					this.lightsMote = MoteMaker.MakeStaticMote(this.parent.TrueCenter(), this.parent.Map, ThingDefOf.Mote_LoudspeakerLights, 1f);
				}
			}
			else
			{
				Mote mote = this.lightsMote;
				if (mote != null)
				{
					mote.Destroy(DestroyMode.Vanish);
				}
				this.lightsMote = null;
			}
			if (this.compPowerCached.PowerOn && !this.inRitual)
			{
				this.parent.BroadcastCompSignal("AutoPoweredWantsOff");
			}
			if (this.inRitual)
			{
				Mote mote2 = this.lightsMote;
				if (mote2 == null)
				{
					return;
				}
				mote2.Maintain();
			}
		}

		// Token: 0x06006ED7 RID: 28375 RVA: 0x0025131A File Offset: 0x0024F51A
		public override void Notify_SignalReceived(Signal signal)
		{
			if (signal.tag == "RitualStarted")
			{
				this.UpdateInRitual();
			}
		}

		// Token: 0x06006ED8 RID: 28376 RVA: 0x00251334 File Offset: 0x0024F534
		private void UpdateInRitual()
		{
			this.inRitual = false;
			using (List<Lord>.Enumerator enumerator = this.parent.Map.lordManager.lords.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					LordJob_Ritual lordJob_Ritual;
					if ((lordJob_Ritual = (enumerator.Current.LordJob as LordJob_Ritual)) != null && lordJob_Ritual.selectedTarget.Thing != null && lordJob_Ritual.selectedTarget.Thing.def == ThingDefOf.LightBall && this.parent.GetRoom(RegionType.Set_All) == lordJob_Ritual.selectedTarget.Thing.GetRoom(RegionType.Set_All))
					{
						this.inRitual = true;
						break;
					}
				}
			}
		}

		// Token: 0x06006ED9 RID: 28377 RVA: 0x002513F4 File Offset: 0x0024F5F4
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			if (respawningAfterLoad)
			{
				this.UpdateInRitual();
			}
		}

		// Token: 0x04003D59 RID: 15705
		private CompPowerTrader compPowerCached;

		// Token: 0x04003D5A RID: 15706
		private Mote lightsMote;

		// Token: 0x04003D5B RID: 15707
		private bool inRitual;
	}
}
