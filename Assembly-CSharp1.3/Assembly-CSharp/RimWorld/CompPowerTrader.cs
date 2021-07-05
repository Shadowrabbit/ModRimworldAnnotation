using System;
using System.Text;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02000CD1 RID: 3281
	public class CompPowerTrader : CompPower
	{
		// Token: 0x17000D35 RID: 3381
		// (get) Token: 0x06004C83 RID: 19587 RVA: 0x00197F91 File Offset: 0x00196191
		// (set) Token: 0x06004C84 RID: 19588 RVA: 0x00197F99 File Offset: 0x00196199
		public float PowerOutput
		{
			get
			{
				return this.powerOutputInt;
			}
			set
			{
				this.powerOutputInt = value;
				if (this.powerOutputInt > 0f)
				{
					this.powerLastOutputted = true;
				}
				if (this.powerOutputInt < 0f)
				{
					this.powerLastOutputted = false;
				}
			}
		}

		// Token: 0x17000D36 RID: 3382
		// (get) Token: 0x06004C85 RID: 19589 RVA: 0x00197FCA File Offset: 0x001961CA
		public float EnergyOutputPerTick
		{
			get
			{
				return this.PowerOutput * CompPower.WattsToWattDaysPerTick;
			}
		}

		// Token: 0x17000D37 RID: 3383
		// (get) Token: 0x06004C86 RID: 19590 RVA: 0x00197FD8 File Offset: 0x001961D8
		// (set) Token: 0x06004C87 RID: 19591 RVA: 0x00197FE0 File Offset: 0x001961E0
		public bool PowerOn
		{
			get
			{
				return this.powerOnInt;
			}
			set
			{
				if (this.powerOnInt == value)
				{
					return;
				}
				this.powerOnInt = value;
				if (this.powerOnInt)
				{
					if (!FlickUtility.WantsToBeOn(this.parent))
					{
						Log.Warning("Tried to power on " + this.parent + " which did not desire it.");
						return;
					}
					if (this.parent.IsBrokenDown())
					{
						Log.Warning("Tried to power on " + this.parent + " which is broken down.");
						return;
					}
					if (this.powerStartedAction != null)
					{
						this.powerStartedAction();
					}
					this.parent.BroadcastCompSignal("PowerTurnedOn");
					SoundDef soundDef = ((CompProperties_Power)this.parent.def.CompDefForAssignableFrom<CompPowerTrader>()).soundPowerOn;
					if (soundDef.NullOrUndefined())
					{
						soundDef = SoundDefOf.Power_OnSmall;
					}
					soundDef.PlayOneShot(new TargetInfo(this.parent.Position, this.parent.Map, false));
					this.StartSustainerPoweredIfInactive();
				}
				else
				{
					if (this.powerStoppedAction != null)
					{
						this.powerStoppedAction();
					}
					this.parent.BroadcastCompSignal("PowerTurnedOff");
					SoundDef soundDef2 = ((CompProperties_Power)this.parent.def.CompDefForAssignableFrom<CompPowerTrader>()).soundPowerOff;
					if (soundDef2.NullOrUndefined())
					{
						soundDef2 = SoundDefOf.Power_OffSmall;
					}
					if (this.parent.Spawned)
					{
						soundDef2.PlayOneShot(new TargetInfo(this.parent.Position, this.parent.Map, false));
					}
					this.EndSustainerPoweredIfActive();
				}
				this.UpdateOverlays();
			}
		}

		// Token: 0x17000D38 RID: 3384
		// (get) Token: 0x06004C88 RID: 19592 RVA: 0x00198164 File Offset: 0x00196364
		public string DebugString
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendLine(this.parent.LabelCap + " CompPower:");
				stringBuilder.AppendLine("   PowerOn: " + this.PowerOn.ToString());
				stringBuilder.AppendLine("   energyProduction: " + this.PowerOutput);
				return stringBuilder.ToString();
			}
		}

		// Token: 0x06004C89 RID: 19593 RVA: 0x001981D4 File Offset: 0x001963D4
		public override void ReceiveCompSignal(string signal)
		{
			if (signal == "FlickedOff" || signal == "ScheduledOff" || signal == "Breakdown" || signal == "AutoPoweredWantsOff")
			{
				this.PowerOn = false;
			}
			if (signal == "RanOutOfFuel" && this.powerLastOutputted)
			{
				this.PowerOn = false;
			}
			this.UpdateOverlays();
		}

		// Token: 0x06004C8A RID: 19594 RVA: 0x0019823E File Offset: 0x0019643E
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostSpawnSetup(respawningAfterLoad);
			this.flickableComp = this.parent.GetComp<CompFlickable>();
			if (this.PowerOn)
			{
				LongEventHandler.ExecuteWhenFinished(delegate
				{
					this.StartSustainerPoweredIfInactive();
				});
			}
			this.UpdateOverlays();
		}

		// Token: 0x06004C8B RID: 19595 RVA: 0x00198277 File Offset: 0x00196477
		public override void PostDeSpawn(Map map)
		{
			base.PostDeSpawn(map);
			this.EndSustainerPoweredIfActive();
			this.powerOutputInt = 0f;
		}

		// Token: 0x06004C8C RID: 19596 RVA: 0x00198291 File Offset: 0x00196491
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<bool>(ref this.powerOnInt, "powerOn", true, false);
		}

		// Token: 0x06004C8D RID: 19597 RVA: 0x001982AC File Offset: 0x001964AC
		private void UpdateOverlays()
		{
			if (!this.parent.Spawned)
			{
				return;
			}
			this.parent.Map.overlayDrawer.Disable(this.parent, ref this.overlayPowerOff);
			this.parent.Map.overlayDrawer.Disable(this.parent, ref this.overlayNeedsPower);
			if (!this.parent.IsBrokenDown())
			{
				if (this.flickableComp != null && !this.flickableComp.SwitchIsOn && this.overlayPowerOff == null)
				{
					this.overlayPowerOff = new OverlayHandle?(this.parent.Map.overlayDrawer.Enable(this.parent, OverlayTypes.PowerOff));
					return;
				}
				if (FlickUtility.WantsToBeOn(this.parent) && !this.PowerOn && this.overlayNeedsPower == null && base.Props.showPowerNeededIfOff)
				{
					this.overlayNeedsPower = new OverlayHandle?(this.parent.Map.overlayDrawer.Enable(this.parent, OverlayTypes.NeedsPower));
				}
			}
		}

		// Token: 0x06004C8E RID: 19598 RVA: 0x001983BC File Offset: 0x001965BC
		public override void SetUpPowerVars()
		{
			base.SetUpPowerVars();
			CompProperties_Power props = base.Props;
			this.PowerOutput = -1f * props.basePowerConsumption;
			this.powerLastOutputted = (props.basePowerConsumption <= 0f);
		}

		// Token: 0x06004C8F RID: 19599 RVA: 0x001983FE File Offset: 0x001965FE
		public override void ResetPowerVars()
		{
			base.ResetPowerVars();
			this.powerOnInt = false;
			this.powerOutputInt = 0f;
			this.powerLastOutputted = false;
			this.sustainerPowered = null;
			if (this.flickableComp != null)
			{
				this.flickableComp.ResetToOn();
			}
		}

		// Token: 0x06004C90 RID: 19600 RVA: 0x00198439 File Offset: 0x00196639
		public override void LostConnectParent()
		{
			base.LostConnectParent();
			this.PowerOn = false;
		}

		// Token: 0x06004C91 RID: 19601 RVA: 0x00198448 File Offset: 0x00196648
		public override string CompInspectStringExtra()
		{
			string str;
			if (this.powerLastOutputted)
			{
				str = "PowerOutput".Translate() + ": " + this.PowerOutput.ToString("#####0") + " W";
			}
			else
			{
				str = "PowerNeeded".Translate() + ": " + (-this.PowerOutput).ToString("#####0") + " W";
			}
			return str + "\n" + base.CompInspectStringExtra();
		}

		// Token: 0x06004C92 RID: 19602 RVA: 0x001984EC File Offset: 0x001966EC
		private void StartSustainerPoweredIfInactive()
		{
			CompProperties_Power props = base.Props;
			if (!props.soundAmbientPowered.NullOrUndefined() && this.sustainerPowered == null)
			{
				SoundInfo info = SoundInfo.InMap(this.parent, MaintenanceType.None);
				this.sustainerPowered = props.soundAmbientPowered.TrySpawnSustainer(info);
			}
		}

		// Token: 0x06004C93 RID: 19603 RVA: 0x00198539 File Offset: 0x00196739
		private void EndSustainerPoweredIfActive()
		{
			if (this.sustainerPowered != null)
			{
				this.sustainerPowered.End();
				this.sustainerPowered = null;
			}
		}

		// Token: 0x04002E4B RID: 11851
		public Action powerStartedAction;

		// Token: 0x04002E4C RID: 11852
		public Action powerStoppedAction;

		// Token: 0x04002E4D RID: 11853
		private bool powerOnInt;

		// Token: 0x04002E4E RID: 11854
		public float powerOutputInt;

		// Token: 0x04002E4F RID: 11855
		private bool powerLastOutputted;

		// Token: 0x04002E50 RID: 11856
		private Sustainer sustainerPowered;

		// Token: 0x04002E51 RID: 11857
		protected CompFlickable flickableComp;

		// Token: 0x04002E52 RID: 11858
		public const string PowerTurnedOnSignal = "PowerTurnedOn";

		// Token: 0x04002E53 RID: 11859
		public const string PowerTurnedOffSignal = "PowerTurnedOff";

		// Token: 0x04002E54 RID: 11860
		private OverlayHandle? overlayPowerOff;

		// Token: 0x04002E55 RID: 11861
		private OverlayHandle? overlayNeedsPower;
	}
}
