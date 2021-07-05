using System;
using RimWorld;

namespace Verse
{
	// Token: 0x020001A5 RID: 421
	public class CompGlower : ThingComp
	{
		// Token: 0x1700023A RID: 570
		// (get) Token: 0x06000BCA RID: 3018 RVA: 0x0003FF74 File Offset: 0x0003E174
		public CompProperties_Glower Props
		{
			get
			{
				return (CompProperties_Glower)this.props;
			}
		}

		// Token: 0x1700023B RID: 571
		// (get) Token: 0x06000BCB RID: 3019 RVA: 0x0003FF84 File Offset: 0x0003E184
		private bool ShouldBeLitNow
		{
			get
			{
				if (!this.parent.Spawned)
				{
					return false;
				}
				if (!FlickUtility.WantsToBeOn(this.parent))
				{
					return false;
				}
				CompPowerTrader compPowerTrader = this.parent.TryGetComp<CompPowerTrader>();
				if (compPowerTrader != null && !compPowerTrader.PowerOn)
				{
					return false;
				}
				CompRefuelable compRefuelable = this.parent.TryGetComp<CompRefuelable>();
				if (compRefuelable != null && !compRefuelable.HasFuel)
				{
					return false;
				}
				CompSendSignalOnCountdown compSendSignalOnCountdown = this.parent.TryGetComp<CompSendSignalOnCountdown>();
				if (compSendSignalOnCountdown != null && compSendSignalOnCountdown.ticksLeft <= 0)
				{
					return false;
				}
				CompSendSignalOnPawnProximity compSendSignalOnPawnProximity = this.parent.TryGetComp<CompSendSignalOnPawnProximity>();
				if (compSendSignalOnPawnProximity != null && compSendSignalOnPawnProximity.Sent)
				{
					return false;
				}
				CompLoudspeaker compLoudspeaker = this.parent.TryGetComp<CompLoudspeaker>();
				if (compLoudspeaker != null && !compLoudspeaker.Active)
				{
					return false;
				}
				CompHackable compHackable = this.parent.TryGetComp<CompHackable>();
				if (compHackable != null && compHackable.IsHacked && !compHackable.Props.glowIfHacked)
				{
					return false;
				}
				CompRitualSignalSender compRitualSignalSender = this.parent.TryGetComp<CompRitualSignalSender>();
				Building_Crate building_Crate;
				return (compRitualSignalSender == null || compRitualSignalSender.ritualTarget) && ((building_Crate = (this.parent as Building_Crate)) == null || building_Crate.HasAnyContents);
			}
		}

		// Token: 0x1700023C RID: 572
		// (get) Token: 0x06000BCC RID: 3020 RVA: 0x00040092 File Offset: 0x0003E292
		public bool Glows
		{
			get
			{
				return this.glowOnInt;
			}
		}

		// Token: 0x06000BCD RID: 3021 RVA: 0x0004009C File Offset: 0x0003E29C
		public void UpdateLit(Map map)
		{
			bool shouldBeLitNow = this.ShouldBeLitNow;
			if (this.glowOnInt == shouldBeLitNow)
			{
				return;
			}
			this.glowOnInt = shouldBeLitNow;
			if (!this.glowOnInt)
			{
				map.mapDrawer.MapMeshDirty(this.parent.Position, MapMeshFlag.Things);
				map.glowGrid.DeRegisterGlower(this);
				return;
			}
			map.mapDrawer.MapMeshDirty(this.parent.Position, MapMeshFlag.Things);
			map.glowGrid.RegisterGlower(this);
		}

		// Token: 0x06000BCE RID: 3022 RVA: 0x00040110 File Offset: 0x0003E310
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			if (this.ShouldBeLitNow)
			{
				this.UpdateLit(this.parent.Map);
				this.parent.Map.glowGrid.RegisterGlower(this);
				return;
			}
			this.UpdateLit(this.parent.Map);
		}

		// Token: 0x06000BCF RID: 3023 RVA: 0x00040160 File Offset: 0x0003E360
		public override void ReceiveCompSignal(string signal)
		{
			if (signal == "PowerTurnedOn" || signal == "PowerTurnedOff" || signal == "FlickedOn" || signal == "FlickedOff" || signal == "Refueled" || signal == "RanOutOfFuel" || signal == "ScheduledOn" || signal == "ScheduledOff" || signal == "MechClusterDefeated" || signal == "Hackend" || signal == "RitualTargetChanged" || signal == "CrateContentsChanged")
			{
				this.UpdateLit(this.parent.Map);
			}
		}

		// Token: 0x06000BD0 RID: 3024 RVA: 0x00040220 File Offset: 0x0003E420
		public override void PostExposeData()
		{
			Scribe_Values.Look<bool>(ref this.glowOnInt, "glowOn", false, false);
		}

		// Token: 0x06000BD1 RID: 3025 RVA: 0x00040234 File Offset: 0x0003E434
		public override void PostDeSpawn(Map map)
		{
			base.PostDeSpawn(map);
			this.UpdateLit(map);
		}

		// Token: 0x040009C5 RID: 2501
		private bool glowOnInt;
	}
}
