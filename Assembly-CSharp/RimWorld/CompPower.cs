using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x020012E4 RID: 4836
	public abstract class CompPower : ThingComp
	{
		// Token: 0x17001023 RID: 4131
		// (get) Token: 0x060068B9 RID: 26809 RVA: 0x000475A2 File Offset: 0x000457A2
		public bool TransmitsPowerNow
		{
			get
			{
				return ((Building)this.parent).TransmitsPowerNow;
			}
		}

		// Token: 0x17001024 RID: 4132
		// (get) Token: 0x060068BA RID: 26810 RVA: 0x000475B4 File Offset: 0x000457B4
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

		// Token: 0x17001025 RID: 4133
		// (get) Token: 0x060068BB RID: 26811 RVA: 0x000475DA File Offset: 0x000457DA
		public CompProperties_Power Props
		{
			get
			{
				return (CompProperties_Power)this.props;
			}
		}

		// Token: 0x060068BC RID: 26812 RVA: 0x000475E7 File Offset: 0x000457E7
		public virtual void ResetPowerVars()
		{
			this.transNet = null;
			this.connectParent = null;
			this.connectChildren = null;
			CompPower.recentlyConnectedNets.Clear();
			CompPower.lastManualReconnector = null;
		}

		// Token: 0x060068BD RID: 26813 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void SetUpPowerVars()
		{
		}

		// Token: 0x060068BE RID: 26814 RVA: 0x00204354 File Offset: 0x00202554
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

		// Token: 0x060068BF RID: 26815 RVA: 0x002043C0 File Offset: 0x002025C0
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

		// Token: 0x060068C0 RID: 26816 RVA: 0x0020446C File Offset: 0x0020266C
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

		// Token: 0x060068C1 RID: 26817 RVA: 0x0004760E File Offset: 0x0004580E
		public virtual void LostConnectParent()
		{
			this.connectParent = null;
			if (this.parent.Spawned)
			{
				this.parent.Map.powerNetManager.Notify_ConnectorWantsConnect(this);
			}
		}

		// Token: 0x060068C2 RID: 26818 RVA: 0x0004763A File Offset: 0x0004583A
		public override void PostPrintOnto(SectionLayer layer)
		{
			base.PostPrintOnto(layer);
			if (this.connectParent != null)
			{
				PowerNetGraphics.PrintWirePieceConnecting(layer, this.parent, this.connectParent.parent, false);
			}
		}

		// Token: 0x060068C3 RID: 26819 RVA: 0x00204528 File Offset: 0x00202728
		public override void CompPrintForPowerGrid(SectionLayer layer)
		{
			if (this.TransmitsPowerNow)
			{
				PowerOverlayMats.LinkedOverlayGraphic.Print(layer, this.parent);
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

		// Token: 0x060068C4 RID: 26820 RVA: 0x00047663 File Offset: 0x00045863
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

		// Token: 0x060068C5 RID: 26821 RVA: 0x0020458C File Offset: 0x0020278C
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
					MoteMaker.ThrowMetaPuff(compPower.parent.Position.ToVector3Shifted(), compPower.parent.Map);
				}
				this.parent.Map.mapDrawer.MapMeshDirty(this.parent.Position, MapMeshFlag.PowerGrid);
				this.parent.Map.mapDrawer.MapMeshDirty(this.parent.Position, MapMeshFlag.Things);
			}
		}

		// Token: 0x060068C6 RID: 26822 RVA: 0x002046A0 File Offset: 0x002028A0
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
				}), false);
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

		// Token: 0x060068C7 RID: 26823 RVA: 0x00204748 File Offset: 0x00202948
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

		// Token: 0x040045AB RID: 17835
		public PowerNet transNet;

		// Token: 0x040045AC RID: 17836
		public CompPower connectParent;

		// Token: 0x040045AD RID: 17837
		public List<CompPower> connectChildren;

		// Token: 0x040045AE RID: 17838
		private static List<PowerNet> recentlyConnectedNets = new List<PowerNet>();

		// Token: 0x040045AF RID: 17839
		private static CompPower lastManualReconnector = null;

		// Token: 0x040045B0 RID: 17840
		public static readonly float WattsToWattDaysPerTick = 1.6666667E-05f;
	}
}
