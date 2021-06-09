using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020012FB RID: 4859
	public class PowerNetManager
	{
		// Token: 0x0600696F RID: 26991 RVA: 0x00047E15 File Offset: 0x00046015
		public PowerNetManager(Map map)
		{
			this.map = map;
		}

		// Token: 0x17001040 RID: 4160
		// (get) Token: 0x06006970 RID: 26992 RVA: 0x00047E3A File Offset: 0x0004603A
		public List<PowerNet> AllNetsListForReading
		{
			get
			{
				return this.allNets;
			}
		}

		// Token: 0x06006971 RID: 26993 RVA: 0x00047E42 File Offset: 0x00046042
		public void Notify_TransmitterSpawned(CompPower newTransmitter)
		{
			this.delayedActions.Add(new PowerNetManager.DelayedAction(PowerNetManager.DelayedActionType.RegisterTransmitter, newTransmitter));
			this.NotifyDrawersForWireUpdate(newTransmitter.parent.Position);
		}

		// Token: 0x06006972 RID: 26994 RVA: 0x00047E67 File Offset: 0x00046067
		public void Notify_TransmitterDespawned(CompPower oldTransmitter)
		{
			this.delayedActions.Add(new PowerNetManager.DelayedAction(PowerNetManager.DelayedActionType.DeregisterTransmitter, oldTransmitter));
			this.NotifyDrawersForWireUpdate(oldTransmitter.parent.Position);
		}

		// Token: 0x06006973 RID: 26995 RVA: 0x00207A78 File Offset: 0x00205C78
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

		// Token: 0x06006974 RID: 26996 RVA: 0x00047E8C File Offset: 0x0004608C
		public void Notify_ConnectorWantsConnect(CompPower wantingCon)
		{
			if (Scribe.mode == LoadSaveMode.Inactive && !this.HasRegisterConnectorDuplicate(wantingCon))
			{
				this.delayedActions.Add(new PowerNetManager.DelayedAction(PowerNetManager.DelayedActionType.RegisterConnector, wantingCon));
			}
			this.NotifyDrawersForWireUpdate(wantingCon.parent.Position);
		}

		// Token: 0x06006975 RID: 26997 RVA: 0x00047EC1 File Offset: 0x000460C1
		public void Notify_ConnectorDespawned(CompPower oldCon)
		{
			this.delayedActions.Add(new PowerNetManager.DelayedAction(PowerNetManager.DelayedActionType.DeregisterConnector, oldCon));
			this.NotifyDrawersForWireUpdate(oldCon.parent.Position);
		}

		// Token: 0x06006976 RID: 26998 RVA: 0x00047EE6 File Offset: 0x000460E6
		public void NotifyDrawersForWireUpdate(IntVec3 root)
		{
			this.map.mapDrawer.MapMeshDirty(root, MapMeshFlag.Things, true, false);
			this.map.mapDrawer.MapMeshDirty(root, MapMeshFlag.PowerGrid, true, false);
		}

		// Token: 0x06006977 RID: 26999 RVA: 0x00047F14 File Offset: 0x00046114
		public void RegisterPowerNet(PowerNet newNet)
		{
			this.allNets.Add(newNet);
			newNet.powerNetManager = this;
			this.map.powerNetGrid.Notify_PowerNetCreated(newNet);
			PowerNetMaker.UpdateVisualLinkagesFor(newNet);
		}

		// Token: 0x06006978 RID: 27000 RVA: 0x00047F40 File Offset: 0x00046140
		public void DeletePowerNet(PowerNet oldNet)
		{
			this.allNets.Remove(oldNet);
			this.map.powerNetGrid.Notify_PowerNetDeleted(oldNet);
		}

		// Token: 0x06006979 RID: 27001 RVA: 0x00207AC8 File Offset: 0x00205CC8
		public void PowerNetsTick()
		{
			for (int i = 0; i < this.allNets.Count; i++)
			{
				this.allNets[i].PowerNetTick();
			}
		}

		// Token: 0x0600697A RID: 27002 RVA: 0x00207AFC File Offset: 0x00205CFC
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
						goto IL_107;
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
						}), false);
					}
					delayedAction.compPower.SetUpPowerVars();
					using (IEnumerator<IntVec3> enumerator = GenAdj.CellsAdjacentCardinal(parent).GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							IntVec3 cell = enumerator.Current;
							this.TryDestroyNetAt(cell);
						}
						goto IL_12F;
					}
					goto IL_107;
				}
				IL_12F:
				i++;
				continue;
				IL_107:
				this.TryDestroyNetAt(delayedAction.position);
				PowerConnectionMaker.DisconnectAllFromTransmitterAndSetWantConnect(delayedAction.compPower, this.map);
				delayedAction.compPower.ResetPowerVars();
				goto IL_12F;
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

		// Token: 0x0600697B RID: 27003 RVA: 0x00207DCC File Offset: 0x00205FCC
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

		// Token: 0x0600697C RID: 27004 RVA: 0x00207E34 File Offset: 0x00206034
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

		// Token: 0x0600697D RID: 27005 RVA: 0x00207EB0 File Offset: 0x002060B0
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

		// Token: 0x0600697E RID: 27006 RVA: 0x00207EE8 File Offset: 0x002060E8
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

		// Token: 0x0400463C RID: 17980
		public Map map;

		// Token: 0x0400463D RID: 17981
		private List<PowerNet> allNets = new List<PowerNet>();

		// Token: 0x0400463E RID: 17982
		private List<PowerNetManager.DelayedAction> delayedActions = new List<PowerNetManager.DelayedAction>();

		// Token: 0x020012FC RID: 4860
		private enum DelayedActionType
		{
			// Token: 0x04004640 RID: 17984
			RegisterTransmitter,
			// Token: 0x04004641 RID: 17985
			DeregisterTransmitter,
			// Token: 0x04004642 RID: 17986
			RegisterConnector,
			// Token: 0x04004643 RID: 17987
			DeregisterConnector
		}

		// Token: 0x020012FD RID: 4861
		private struct DelayedAction
		{
			// Token: 0x0600697F RID: 27007 RVA: 0x00047F60 File Offset: 0x00046160
			public DelayedAction(PowerNetManager.DelayedActionType type, CompPower compPower)
			{
				this.type = type;
				this.compPower = compPower;
				this.position = compPower.parent.Position;
				this.rotation = compPower.parent.Rotation;
			}

			// Token: 0x04004644 RID: 17988
			public PowerNetManager.DelayedActionType type;

			// Token: 0x04004645 RID: 17989
			public CompPower compPower;

			// Token: 0x04004646 RID: 17990
			public IntVec3 position;

			// Token: 0x04004647 RID: 17991
			public Rot4 rotation;
		}
	}
}
