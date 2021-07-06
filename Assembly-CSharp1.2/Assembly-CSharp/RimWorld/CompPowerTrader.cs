using System;
using System.Text;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x020012F0 RID: 4848
	public class CompPowerTrader : CompPower
	{
		// Token: 0x17001036 RID: 4150
		// (get) Token: 0x06006924 RID: 26916 RVA: 0x00047AF5 File Offset: 0x00045CF5
		// (set) Token: 0x06006925 RID: 26917 RVA: 0x00047AFD File Offset: 0x00045CFD
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

		// Token: 0x17001037 RID: 4151
		// (get) Token: 0x06006926 RID: 26918 RVA: 0x00047B2E File Offset: 0x00045D2E
		public float EnergyOutputPerTick
		{
			get
			{
				return this.PowerOutput * CompPower.WattsToWattDaysPerTick;
			}
		}

		// Token: 0x17001038 RID: 4152
		// (get) Token: 0x06006927 RID: 26919 RVA: 0x00047B3C File Offset: 0x00045D3C
		// (set) Token: 0x06006928 RID: 26920 RVA: 0x0020606C File Offset: 0x0020426C
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
				if (!this.powerOnInt)
				{
					if (this.powerStoppedAction != null)
					{
						this.powerStoppedAction();
					}
					this.parent.BroadcastCompSignal("PowerTurnedOff");
					SoundDef soundDef = ((CompProperties_Power)this.parent.def.CompDefForAssignableFrom<CompPowerTrader>()).soundPowerOff;
					if (soundDef.NullOrUndefined())
					{
						soundDef = SoundDefOf.Power_OffSmall;
					}
					if (this.parent.Spawned)
					{
						soundDef.PlayOneShot(new TargetInfo(this.parent.Position, this.parent.Map, false));
					}
					this.EndSustainerPoweredIfActive();
					return;
				}
				if (!FlickUtility.WantsToBeOn(this.parent))
				{
					Log.Warning("Tried to power on " + this.parent + " which did not desire it.", false);
					return;
				}
				if (this.parent.IsBrokenDown())
				{
					Log.Warning("Tried to power on " + this.parent + " which is broken down.", false);
					return;
				}
				if (this.powerStartedAction != null)
				{
					this.powerStartedAction();
				}
				this.parent.BroadcastCompSignal("PowerTurnedOn");
				SoundDef soundDef2 = ((CompProperties_Power)this.parent.def.CompDefForAssignableFrom<CompPowerTrader>()).soundPowerOn;
				if (soundDef2.NullOrUndefined())
				{
					soundDef2 = SoundDefOf.Power_OnSmall;
				}
				soundDef2.PlayOneShot(new TargetInfo(this.parent.Position, this.parent.Map, false));
				this.StartSustainerPoweredIfInactive();
			}
		}

		// Token: 0x17001039 RID: 4153
		// (get) Token: 0x06006929 RID: 26921 RVA: 0x002061E8 File Offset: 0x002043E8
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

		// Token: 0x0600692A RID: 26922 RVA: 0x00206258 File Offset: 0x00204458
		public override void ReceiveCompSignal(string signal)
		{
			if (signal == "FlickedOff" || signal == "ScheduledOff" || signal == "Breakdown")
			{
				this.PowerOn = false;
			}
			if (signal == "RanOutOfFuel" && this.powerLastOutputted)
			{
				this.PowerOn = false;
			}
		}

		// Token: 0x0600692B RID: 26923 RVA: 0x00047B44 File Offset: 0x00045D44
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
		}

		// Token: 0x0600692C RID: 26924 RVA: 0x00047B77 File Offset: 0x00045D77
		public override void PostDeSpawn(Map map)
		{
			base.PostDeSpawn(map);
			this.EndSustainerPoweredIfActive();
			this.powerOutputInt = 0f;
		}

		// Token: 0x0600692D RID: 26925 RVA: 0x00047B91 File Offset: 0x00045D91
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<bool>(ref this.powerOnInt, "powerOn", true, false);
		}

		// Token: 0x0600692E RID: 26926 RVA: 0x002062B0 File Offset: 0x002044B0
		public override void PostDraw()
		{
			base.PostDraw();
			if (!this.parent.IsBrokenDown())
			{
				if (this.flickableComp != null && !this.flickableComp.SwitchIsOn)
				{
					this.parent.Map.overlayDrawer.DrawOverlay(this.parent, OverlayTypes.PowerOff);
					return;
				}
				if (FlickUtility.WantsToBeOn(this.parent) && !this.PowerOn)
				{
					this.parent.Map.overlayDrawer.DrawOverlay(this.parent, OverlayTypes.NeedsPower);
				}
			}
		}

		// Token: 0x0600692F RID: 26927 RVA: 0x00206334 File Offset: 0x00204534
		public override void SetUpPowerVars()
		{
			base.SetUpPowerVars();
			CompProperties_Power props = base.Props;
			this.PowerOutput = -1f * props.basePowerConsumption;
			this.powerLastOutputted = (props.basePowerConsumption <= 0f);
		}

		// Token: 0x06006930 RID: 26928 RVA: 0x00047BAB File Offset: 0x00045DAB
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

		// Token: 0x06006931 RID: 26929 RVA: 0x00047BE6 File Offset: 0x00045DE6
		public override void LostConnectParent()
		{
			base.LostConnectParent();
			this.PowerOn = false;
		}

		// Token: 0x06006932 RID: 26930 RVA: 0x00206378 File Offset: 0x00204578
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

		// Token: 0x06006933 RID: 26931 RVA: 0x0020641C File Offset: 0x0020461C
		private void StartSustainerPoweredIfInactive()
		{
			CompProperties_Power props = base.Props;
			if (!props.soundAmbientPowered.NullOrUndefined() && this.sustainerPowered == null)
			{
				SoundInfo info = SoundInfo.InMap(this.parent, MaintenanceType.None);
				this.sustainerPowered = props.soundAmbientPowered.TrySpawnSustainer(info);
			}
		}

		// Token: 0x06006934 RID: 26932 RVA: 0x00047BF5 File Offset: 0x00045DF5
		private void EndSustainerPoweredIfActive()
		{
			if (this.sustainerPowered != null)
			{
				this.sustainerPowered.End();
				this.sustainerPowered = null;
			}
		}

		// Token: 0x04004601 RID: 17921
		public Action powerStartedAction;

		// Token: 0x04004602 RID: 17922
		public Action powerStoppedAction;

		// Token: 0x04004603 RID: 17923
		private bool powerOnInt;

		// Token: 0x04004604 RID: 17924
		public float powerOutputInt;

		// Token: 0x04004605 RID: 17925
		private bool powerLastOutputted;

		// Token: 0x04004606 RID: 17926
		private Sustainer sustainerPowered;

		// Token: 0x04004607 RID: 17927
		protected CompFlickable flickableComp;

		// Token: 0x04004608 RID: 17928
		public const string PowerTurnedOnSignal = "PowerTurnedOn";

		// Token: 0x04004609 RID: 17929
		public const string PowerTurnedOffSignal = "PowerTurnedOff";
	}
}
