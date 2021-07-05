using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02000CCA RID: 3274
	public abstract class CompPower : ThingComp
	{
		// Token: 0x17000D2C RID: 3372
		// (get) Token: 0x06004C42 RID: 19522 RVA: 0x001968FB File Offset: 0x00194AFB
		public bool TransmitsPowerNow
		{
			get
			{
				return ((Building)this.parent).TransmitsPowerNow;
			}
		}

		// Token: 0x17000D2D RID: 3373
		// (get) Token: 0x06004C43 RID: 19523 RVA: 0x0019690D File Offset: 0x00194B0D
		public PowerNet PowerNet
		{
			get
			{
				if (this.transNet != null)
				{
					return this.transNet;
				}
				if (this.connectParent != null)
				{
					return this.connectParent.transNet;
				}
				return null;
			}
		}

		// Token: 0x17000D2E RID: 3374
		// (get) Token: 0x06004C44 RID: 19524 RVA: 0x00196933 File Offset: 0x00194B33
		public CompProperties_Power Props
		{
			get
			{
				return (CompProperties_Power)this.props;
			}
		}

		// Token: 0x06004C45 RID: 19525 RVA: 0x00196940 File Offset: 0x00194B40
		public virtual void ResetPowerVars()
		{
			this.transNet = null;
			this.connectParent = null;
			this.connectChildren = null;
			CompPower.recentlyConnectedNets.Clear();
			CompPower.lastManualReconnector = null;
		}

		// Token: 0x06004C46 RID: 19526 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void SetUpPowerVars()
		{
		}

		// Token: 0x06004C47 RID: 19527 RVA: 0x00196968 File Offset: 0x00194B68
		public override void PostExposeData()
		{
			Thing thing = null;
			if (Scribe.mode == LoadSaveMode.Saving && this.connectParent != null)
			{
				thing = this.connectParent.parent;
			}
			Scribe_References.Look<Thing>(ref thing, "parentThing", false);
			if (thing != null)
			{
				this.connectParent = ((ThingWithComps)thing).GetComp<CompPower>();
			}
			if (Scribe.mode == LoadSaveMode.PostLoadInit && this.connectParent != null)
			{
				this.ConnectToTransmitter(this.connectParent, true);
			}
		}

		// Token: 0x06004C48 RID: 19528 RVA: 0x001969D4 File Offset: 0x00194BD4
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostSpawnSetup(respawningAfterLoad);
			if (this.Props.transmitsPower || this.parent.def.ConnectToPower)
			{
				this.parent.Map.mapDrawer.MapMeshDirty(this.parent.Position, MapMeshFlag.PowerGrid, true, false);
				if (this.Props.transmitsPower)
				{
					this.parent.Map.powerNetManager.Notify_TransmitterSpawned(this);
				}
				if (this.parent.def.ConnectToPower)
				{
					this.parent.Map.powerNetManager.Notify_ConnectorWantsConnect(this);
				}
				this.SetUpPowerVars();
			}
		}

		// Token: 0x06004C49 RID: 19529 RVA: 0x00196A80 File Offset: 0x00194C80
		public override void PostDeSpawn(Map map)
		{
			base.PostDeSpawn(map);
			if (this.Props.transmitsPower || this.parent.def.ConnectToPower)
			{
				if (this.Props.transmitsPower)
				{
					if (this.connectChildren != null)
					{
						for (int i = 0; i < this.connectChildren.Count; i++)
						{
							this.connectChildren[i].LostConnectParent();
						}
					}
					map.powerNetManager.Notify_TransmitterDespawned(this);
				}
				if (this.parent.def.ConnectToPower)
				{
					map.powerNetManager.Notify_ConnectorDespawned(this);
				}
				map.mapDrawer.MapMeshDirty(this.parent.Position, MapMeshFlag.PowerGrid, true, false);
			}
		}

		// Token: 0x06004C4A RID: 19530 RVA: 0x00196B39 File Offset: 0x00194D39
		public virtual void LostConnectParent()
		{
			this.connectParent = null;
			if (this.parent.Spawned)
			{
				this.parent.Map.powerNetManager.Notify_ConnectorWantsConnect(this);
			}
		}

		// Token: 0x06004C4B RID: 19531 RVA: 0x00196B65 File Offset: 0x00194D65
		public override void PostPrintOnto(SectionLayer layer)
		{
			base.PostPrintOnto(layer);
			if (this.connectParent != null)
			{
				PowerNetGraphics.PrintWirePieceConnecting(layer, this.parent, this.connectParent.parent, false);
			}
		}

		// Token: 0x06004C4C RID: 19532 RVA: 0x00196B90 File Offset: 0x00194D90
		public override void CompPrintForPowerGrid(SectionLayer layer)
		{
			if (this.TransmitsPowerNow)
			{
				PowerOverlayMats.LinkedOverlayGraphic.Print(layer, this.parent, 0f);
			}
			if (this.parent.def.ConnectToPower)
			{
				PowerNetGraphics.PrintOverlayConnectorBaseFor(layer, this.parent);
			}
			if (this.connectParent != null)
			{
				PowerNetGraphics.PrintWirePieceConnecting(layer, this.parent, this.connectParent.parent, true);
			}
		}

		// Token: 0x06004C4D RID: 19533 RVA: 0x00196BF9 File Offset: 0x00194DF9
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			foreach (Gizmo gizmo in base.CompGetGizmosExtra())
			{
				yield return gizmo;
			}
			IEnumerator<Gizmo> enumerator = null;
			if (this.connectParent != null && this.parent.Faction == Faction.OfPlayer)
			{
				yield return new Command_Action
				{
					action = delegate()
					{
						SoundDefOf.Tick_Tiny.PlayOneShotOnCamera(null);
						this.TryManualReconnect();
					},
					hotKey = KeyBindingDefOf.Misc2,
					defaultDesc = "CommandTryReconnectDesc".Translate(),
					icon = ContentFinder<Texture2D>.Get("UI/Commands/TryReconnect", true),
					defaultLabel = "CommandTryReconnectLabel".Translate()
				};
			}
			yield break;
			yield break;
		}

		// Token: 0x06004C4E RID: 19534 RVA: 0x00196C0C File Offset: 0x00194E0C
		private void TryManualReconnect()
		{
			if (CompPower.lastManualReconnector != this)
			{
				CompPower.recentlyConnectedNets.Clear();
				CompPower.lastManualReconnector = this;
			}
			if (this.PowerNet != null)
			{
				CompPower.recentlyConnectedNets.Add(this.PowerNet);
			}
			CompPower compPower = PowerConnectionMaker.BestTransmitterForConnector(this.parent.Position, this.parent.Map, CompPower.recentlyConnectedNets);
			if (compPower == null)
			{
				CompPower.recentlyConnectedNets.Clear();
				compPower = PowerConnectionMaker.BestTransmitterForConnector(this.parent.Position, this.parent.Map, null);
			}
			if (compPower != null)
			{
				PowerConnectionMaker.DisconnectFromPowerNet(this);
				this.ConnectToTransmitter(compPower, false);
				for (int i = 0; i < 5; i++)
				{
					FleckMaker.ThrowMetaPuff(compPower.parent.Position.ToVector3Shifted(), compPower.parent.Map);
				}
				this.parent.Map.mapDrawer.MapMeshDirty(this.parent.Position, MapMeshFlag.PowerGrid);
				this.parent.Map.mapDrawer.MapMeshDirty(this.parent.Position, MapMeshFlag.Things);
			}
		}

		// Token: 0x06004C4F RID: 19535 RVA: 0x00196D20 File Offset: 0x00194F20
		public void ConnectToTransmitter(CompPower transmitter, bool reconnectingAfterLoading = false)
		{
			if (this.connectParent != null && (!reconnectingAfterLoading || this.connectParent != transmitter))
			{
				Log.Error(string.Concat(new object[]
				{
					"Tried to connect ",
					this,
					" to transmitter ",
					transmitter,
					" but it's already connected to ",
					this.connectParent,
					"."
				}));
				return;
			}
			this.connectParent = transmitter;
			if (this.connectParent.connectChildren == null)
			{
				this.connectParent.connectChildren = new List<CompPower>();
			}
			transmitter.connectChildren.Add(this);
			PowerNet powerNet = this.PowerNet;
			if (powerNet != null)
			{
				powerNet.RegisterConnector(this);
			}
		}

		// Token: 0x06004C50 RID: 19536 RVA: 0x00196DC4 File Offset: 0x00194FC4
		public override string CompInspectStringExtra()
		{
			if (this.PowerNet == null)
			{
				return "PowerNotConnected".Translate();
			}
			string value = (this.PowerNet.CurrentEnergyGainRate() / CompPower.WattsToWattDaysPerTick).ToString("F0");
			string value2 = this.PowerNet.CurrentStoredEnergy().ToString("F0");
			return "PowerConnectedRateStored".Translate(value, value2);
		}

		// Token: 0x04002E1C RID: 11804
		public PowerNet transNet;

		// Token: 0x04002E1D RID: 11805
		public CompPower connectParent;

		// Token: 0x04002E1E RID: 11806
		public List<CompPower> connectChildren;

		// Token: 0x04002E1F RID: 11807
		private static List<PowerNet> recentlyConnectedNets = new List<PowerNet>();

		// Token: 0x04002E20 RID: 11808
		private static CompPower lastManualReconnector = null;

		// Token: 0x04002E21 RID: 11809
		public static readonly float WattsToWattDaysPerTick = 1.6666667E-05f;
	}
}
