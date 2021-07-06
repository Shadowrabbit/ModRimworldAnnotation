using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000259 RID: 601
	public class CompGlower : ThingComp
	{
		// Token: 0x170002C3 RID: 707
		// (get) Token: 0x06000F3E RID: 3902 RVA: 0x000116DE File Offset: 0x0000F8DE
		public CompProperties_Glower Props
		{
			get
			{
				return (CompProperties_Glower)this.props;
			}
		}

		// Token: 0x170002C4 RID: 708
		// (get) Token: 0x06000F3F RID: 3903 RVA: 0x000B5E10 File Offset: 0x000B4010
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
				return compSendSignalOnPawnProximity == null || !compSendSignalOnPawnProximity.Sent;
			}
		}

		// Token: 0x170002C5 RID: 709
		// (get) Token: 0x06000F40 RID: 3904 RVA: 0x000116EB File Offset: 0x0000F8EB
		public bool Glows
		{
			get
			{
				return this.glowOnInt;
			}
		}

		// Token: 0x06000F41 RID: 3905 RVA: 0x000B5EA4 File Offset: 0x000B40A4
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

		// Token: 0x06000F42 RID: 3906 RVA: 0x000B5F18 File Offset: 0x000B4118
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

		// Token: 0x06000F43 RID: 3907 RVA: 0x000B5F68 File Offset: 0x000B4168
		public override void ReceiveCompSignal(string signal)
		{
			if (signal == "PowerTurnedOn" || signal == "PowerTurnedOff" || signal == "FlickedOn" || signal == "FlickedOff" || signal == "Refueled" || signal == "RanOutOfFuel" || signal == "ScheduledOn" || signal == "ScheduledOff" || signal == "MechClusterDefeated")
			{
				this.UpdateLit(this.parent.Map);
			}
		}

		// Token: 0x06000F44 RID: 3908 RVA: 0x000116F3 File Offset: 0x0000F8F3
		public override void PostExposeData()
		{
			Scribe_Values.Look<bool>(ref this.glowOnInt, "glowOn", false, false);
		}

		// Token: 0x06000F45 RID: 3909 RVA: 0x00011707 File Offset: 0x0000F907
		public override void PostDeSpawn(Map map)
		{
			base.PostDeSpawn(map);
			this.UpdateLit(map);
		}

		// Token: 0x04000C7E RID: 3198
		private bool glowOnInt;
	}
}
