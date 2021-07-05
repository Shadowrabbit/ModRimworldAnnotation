using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000CDA RID: 3290
	public class PowerNetManager
	{
		// Token: 0x06004CC3 RID: 19651 RVA: 0x00199B29 File Offset: 0x00197D29
		public PowerNetManager(Map map)
		{
			this.map = map;
		}

		// Token: 0x17000D3D RID: 3389
		// (get) Token: 0x06004CC4 RID: 19652 RVA: 0x00199B4E File Offset: 0x00197D4E
		public List<PowerNet> AllNetsListForReading
		{
			get
			{
				return this.allNets;
			}
		}

		// Token: 0x06004CC5 RID: 19653 RVA: 0x00199B56 File Offset: 0x00197D56
		public void Notify_TransmitterSpawned(CompPower newTransmitter)
		{
			this.delayedActions.Add(new PowerNetManager.DelayedAction(PowerNetManager.DelayedActionType.RegisterTransmitter, newTransmitter));
			this.NotifyDrawersForWireUpdate(newTransmitter.parent.Position);
		}

		// Token: 0x06004CC6 RID: 19654 RVA: 0x00199B7B File Offset: 0x00197D7B
		public void Notify_TransmitterDespawned(CompPower oldTransmitter)
		{
			this.delayedActions.Add(new PowerNetManager.DelayedAction(PowerNetManager.DelayedActionType.DeregisterTransmitter, oldTransmitter));
			this.NotifyDrawersForWireUpdate(oldTransmitter.parent.Position);
		}

		// Token: 0x06004CC7 RID: 19655 RVA: 0x00199BA0 File Offset: 0x00197DA0
		public void Notfiy_TransmitterTransmitsPowerNowChanged(CompPower transmitter)
		{
			if (!transmitter.parent.Spawned)
			{
				return;
			}
			this.delayedActions.Add(new PowerNetManager.DelayedAction(PowerNetManager.DelayedActionType.DeregisterTransmitter, transmitter));
			this.delayedActions.Add(new PowerNetManager.DelayedAction(PowerNetManager.DelayedActionType.RegisterTransmitter, transmitter));
			this.NotifyDrawersForWireUpdate(transmitter.parent.Position);
		}

		// Token: 0x06004CC8 RID: 19656 RVA: 0x00199BF0 File Offset: 0x00197DF0
		public void Notify_ConnectorWantsConnect(CompPower wantingCon)
		{
			if (Scribe.mode == LoadSaveMode.Inactive && !this.HasRegisterConnectorDuplicate(wantingCon))
			{
				this.delayedActions.Add(new PowerNetManager.DelayedAction(PowerNetManager.DelayedActionType.RegisterConnector, wantingCon));
			}
			this.NotifyDrawersForWireUpdate(wantingCon.parent.Position);
		}

		// Token: 0x06004CC9 RID: 19657 RVA: 0x00199C25 File Offset: 0x00197E25
		public void Notify_ConnectorDespawned(CompPower oldCon)
		{
			this.delayedActions.Add(new PowerNetManager.DelayedAction(PowerNetManager.DelayedActionType.DeregisterConnector, oldCon));
			this.NotifyDrawersForWireUpdate(oldCon.parent.Position);
		}

		// Token: 0x06004CCA RID: 19658 RVA: 0x00199C4A File Offset: 0x00197E4A
		public void NotifyDrawersForWireUpdate(IntVec3 root)
		{
			this.map.mapDrawer.MapMeshDirty(root, MapMeshFlag.Things, true, false);
			this.map.mapDrawer.MapMeshDirty(root, MapMeshFlag.PowerGrid, true, false);
		}

		// Token: 0x06004CCB RID: 19659 RVA: 0x00199C78 File Offset: 0x00197E78
		public void RegisterPowerNet(PowerNet newNet)
		{
			this.allNets.Add(newNet);
			newNet.powerNetManager = this;
			this.map.powerNetGrid.Notify_PowerNetCreated(newNet);
			PowerNetMaker.UpdateVisualLinkagesFor(newNet);
		}

		// Token: 0x06004CCC RID: 19660 RVA: 0x00199CA4 File Offset: 0x00197EA4
		public void DeletePowerNet(PowerNet oldNet)
		{
			this.allNets.Remove(oldNet);
			this.map.powerNetGrid.Notify_PowerNetDeleted(oldNet);
		}

		// Token: 0x06004CCD RID: 19661 RVA: 0x00199CC4 File Offset: 0x00197EC4
		public void PowerNetsTick()
		{
			for (int i = 0; i < this.allNets.Count; i++)
			{
				this.allNets[i].PowerNetTick();
			}
		}

		// Token: 0x06004CCE RID: 19662 RVA: 0x00199CF8 File Offset: 0x00197EF8
		public void UpdatePowerNetsAndConnections_First()
		{
			int count = this.delayedActions.Count;
			int i = 0;
			while (i < count)
			{
				PowerNetManager.DelayedAction delayedAction = this.delayedActions[i];
				PowerNetManager.DelayedActionType type = this.delayedActions[i].type;
				if (type != PowerNetManager.DelayedActionType.RegisterTransmitter)
				{
					if (type == PowerNetManager.DelayedActionType.DeregisterTransmitter)
					{
						goto IL_106;
					}
				}
				else if (delayedAction.position == delayedAction.compPower.parent.Position)
				{
					ThingWithComps parent = delayedAction.compPower.parent;
					if (this.map.powerNetGrid.TransmittedPowerNetAt(parent.Position) != null)
					{
						Log.Warning(string.Concat(new object[]
						{
							"Tried to register trasmitter ",
							parent,
							" at ",
							parent.Position,
							", but there is already a power net here. There can't be two transmitters on the same cell."
						}));
					}
					delayedAction.compPower.SetUpPowerVars();
					using (IEnumerator<IntVec3> enumerator = GenAdj.CellsAdjacentCardinal(parent).GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							IntVec3 cell = enumerator.Current;
							this.TryDestroyNetAt(cell);
						}
						goto IL_12E;
					}
					goto IL_106;
				}
				IL_12E:
				i++;
				continue;
				IL_106:
				this.TryDestroyNetAt(delayedAction.position);
				PowerConnectionMaker.DisconnectAllFromTransmitterAndSetWantConnect(delayedAction.compPower, this.map);
				delayedAction.compPower.ResetPowerVars();
				goto IL_12E;
			}
			for (int j = 0; j < count; j++)
			{
				PowerNetManager.DelayedAction delayedAction2 = this.delayedActions[j];
				if ((delayedAction2.type == PowerNetManager.DelayedActionType.RegisterTransmitter && delayedAction2.position == delayedAction2.compPower.parent.Position) || delayedAction2.type == PowerNetManager.DelayedActionType.DeregisterTransmitter)
				{
					this.TryCreateNetAt(delayedAction2.position);
					foreach (IntVec3 cell2 in GenAdj.CellsAdjacentCardinal(delayedAction2.position, delayedAction2.rotation, delayedAction2.compPower.parent.def.size))
					{
						this.TryCreateNetAt(cell2);
					}
				}
			}
			for (int k = 0; k < count; k++)
			{
				PowerNetManager.DelayedAction delayedAction3 = this.delayedActions[k];
				PowerNetManager.DelayedActionType type = this.delayedActions[k].type;
				if (type != PowerNetManager.DelayedActionType.RegisterConnector)
				{
					if (type == PowerNetManager.DelayedActionType.DeregisterConnector)
					{
						PowerConnectionMaker.DisconnectFromPowerNet(delayedAction3.compPower);
						delayedAction3.compPower.ResetPowerVars();
					}
				}
				else if (delayedAction3.position == delayedAction3.compPower.parent.Position)
				{
					delayedAction3.compPower.SetUpPowerVars();
					PowerConnectionMaker.TryConnectToAnyPowerNet(delayedAction3.compPower, null);
				}
			}
			this.delayedActions.RemoveRange(0, count);
			if (DebugViewSettings.drawPower)
			{
				this.DrawDebugPowerNets();
			}
		}

		// Token: 0x06004CCF RID: 19663 RVA: 0x00199FC8 File Offset: 0x001981C8
		private bool HasRegisterConnectorDuplicate(CompPower compPower)
		{
			for (int i = this.delayedActions.Count - 1; i >= 0; i--)
			{
				if (this.delayedActions[i].compPower == compPower)
				{
					if (this.delayedActions[i].type == PowerNetManager.DelayedActionType.DeregisterConnector)
					{
						return false;
					}
					if (this.delayedActions[i].type == PowerNetManager.DelayedActionType.RegisterConnector)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06004CD0 RID: 19664 RVA: 0x0019A030 File Offset: 0x00198230
		private void TryCreateNetAt(IntVec3 cell)
		{
			if (!cell.InBounds(this.map))
			{
				return;
			}
			if (this.map.powerNetGrid.TransmittedPowerNetAt(cell) == null)
			{
				Building transmitter = cell.GetTransmitter(this.map);
				if (transmitter != null && transmitter.TransmitsPowerNow)
				{
					PowerNet powerNet = PowerNetMaker.NewPowerNetStartingFrom(transmitter);
					this.RegisterPowerNet(powerNet);
					for (int i = 0; i < powerNet.transmitters.Count; i++)
					{
						PowerConnectionMaker.ConnectAllConnectorsToTransmitter(powerNet.transmitters[i]);
					}
				}
			}
		}

		// Token: 0x06004CD1 RID: 19665 RVA: 0x0019A0AC File Offset: 0x001982AC
		private void TryDestroyNetAt(IntVec3 cell)
		{
			if (!cell.InBounds(this.map))
			{
				return;
			}
			PowerNet powerNet = this.map.powerNetGrid.TransmittedPowerNetAt(cell);
			if (powerNet != null)
			{
				this.DeletePowerNet(powerNet);
			}
		}

		// Token: 0x06004CD2 RID: 19666 RVA: 0x0019A0E4 File Offset: 0x001982E4
		private void DrawDebugPowerNets()
		{
			if (Current.ProgramState != ProgramState.Playing)
			{
				return;
			}
			if (Find.CurrentMap != this.map)
			{
				return;
			}
			int num = 0;
			foreach (PowerNet powerNet in this.allNets)
			{
				foreach (CompPower compPower in powerNet.transmitters.Concat(powerNet.connectors))
				{
					foreach (IntVec3 c in GenAdj.CellsOccupiedBy(compPower.parent))
					{
						CellRenderer.RenderCell(c, (float)num * 0.44f);
					}
				}
				num++;
			}
		}

		// Token: 0x04002E7C RID: 11900
		public Map map;

		// Token: 0x04002E7D RID: 11901
		private List<PowerNet> allNets = new List<PowerNet>();

		// Token: 0x04002E7E RID: 11902
		private List<PowerNetManager.DelayedAction> delayedActions = new List<PowerNetManager.DelayedAction>();

		// Token: 0x020021D2 RID: 8658
		private enum DelayedActionType
		{
			// Token: 0x04008127 RID: 33063
			RegisterTransmitter,
			// Token: 0x04008128 RID: 33064
			DeregisterTransmitter,
			// Token: 0x04008129 RID: 33065
			RegisterConnector,
			// Token: 0x0400812A RID: 33066
			DeregisterConnector
		}

		// Token: 0x020021D3 RID: 8659
		private struct DelayedAction
		{
			// Token: 0x0600C05A RID: 49242 RVA: 0x003D06CB File Offset: 0x003CE8CB
			public DelayedAction(PowerNetManager.DelayedActionType type, CompPower compPower)
			{
				this.type = type;
				this.compPower = compPower;
				this.position = compPower.parent.Position;
				this.rotation = compPower.parent.Rotation;
			}

			// Token: 0x0400812B RID: 33067
			public PowerNetManager.DelayedActionType type;

			// Token: 0x0400812C RID: 33068
			public CompPower compPower;

			// Token: 0x0400812D RID: 33069
			public IntVec3 position;

			// Token: 0x0400812E RID: 33070
			public Rot4 rotation;
		}
	}
}
