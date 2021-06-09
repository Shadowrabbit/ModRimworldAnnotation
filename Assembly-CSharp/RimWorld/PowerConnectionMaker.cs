using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020012F3 RID: 4851
	public static class PowerConnectionMaker
	{
		// Token: 0x06006940 RID: 26944 RVA: 0x002064E8 File Offset: 0x002046E8
		public static void ConnectAllConnectorsToTransmitter(CompPower newTransmitter)
		{
			foreach (CompPower compPower in PowerConnectionMaker.PotentialConnectorsForTransmitter(newTransmitter))
			{
				if (compPower.connectParent == null)
				{
					compPower.ConnectToTransmitter(newTransmitter, false);
				}
			}
		}

		// Token: 0x06006941 RID: 26945 RVA: 0x00206540 File Offset: 0x00204740
		public static void DisconnectAllFromTransmitterAndSetWantConnect(CompPower deadPc, Map map)
		{
			if (deadPc.connectChildren == null)
			{
				return;
			}
			for (int i = 0; i < deadPc.connectChildren.Count; i++)
			{
				CompPower compPower = deadPc.connectChildren[i];
				compPower.connectParent = null;
				CompPowerTrader compPowerTrader = compPower as CompPowerTrader;
				if (compPowerTrader != null)
				{
					compPowerTrader.PowerOn = false;
				}
				map.powerNetManager.Notify_ConnectorWantsConnect(compPower);
			}
		}

		// Token: 0x06006942 RID: 26946 RVA: 0x002065A0 File Offset: 0x002047A0
		public static void TryConnectToAnyPowerNet(CompPower pc, List<PowerNet> disallowedNets = null)
		{
			if (pc.connectParent != null)
			{
				return;
			}
			if (!pc.parent.Spawned)
			{
				return;
			}
			CompPower compPower = PowerConnectionMaker.BestTransmitterForConnector(pc.parent.Position, pc.parent.Map, disallowedNets);
			if (compPower != null)
			{
				pc.ConnectToTransmitter(compPower, false);
				return;
			}
			pc.connectParent = null;
		}

		// Token: 0x06006943 RID: 26947 RVA: 0x002065F4 File Offset: 0x002047F4
		public static void DisconnectFromPowerNet(CompPower pc)
		{
			if (pc.connectParent == null)
			{
				return;
			}
			if (pc.PowerNet != null)
			{
				pc.PowerNet.DeregisterConnector(pc);
			}
			if (pc.connectParent.connectChildren != null)
			{
				pc.connectParent.connectChildren.Remove(pc);
				if (pc.connectParent.connectChildren.Count == 0)
				{
					pc.connectParent.connectChildren = null;
				}
			}
			pc.connectParent = null;
		}

		// Token: 0x06006944 RID: 26948 RVA: 0x00047C94 File Offset: 0x00045E94
		private static IEnumerable<CompPower> PotentialConnectorsForTransmitter(CompPower b)
		{
			if (!b.parent.Spawned)
			{
				Log.Warning("Can't check potential connectors for " + b + " because it's unspawned.", false);
				yield break;
			}
			CellRect rect = b.parent.OccupiedRect().ExpandedBy(6).ClipInsideMap(b.parent.Map);
			int num;
			for (int z = rect.minZ; z <= rect.maxZ; z = num + 1)
			{
				for (int x = rect.minX; x <= rect.maxX; x = num + 1)
				{
					IntVec3 c = new IntVec3(x, 0, z);
					List<Thing> thingList = b.parent.Map.thingGrid.ThingsListAt(c);
					for (int i = 0; i < thingList.Count; i = num + 1)
					{
						if (thingList[i].def.ConnectToPower)
						{
							yield return ((Building)thingList[i]).PowerComp;
						}
						num = i;
					}
					thingList = null;
					num = x;
				}
				num = z;
			}
			yield break;
		}

		// Token: 0x06006945 RID: 26949 RVA: 0x00206664 File Offset: 0x00204864
		public static CompPower BestTransmitterForConnector(IntVec3 connectorPos, Map map, List<PowerNet> disallowedNets = null)
		{
			CellRect cellRect = CellRect.SingleCell(connectorPos).ExpandedBy(6).ClipInsideMap(map);
			cellRect.ClipInsideMap(map);
			float num = 999999f;
			CompPower result = null;
			for (int i = cellRect.minZ; i <= cellRect.maxZ; i++)
			{
				for (int j = cellRect.minX; j <= cellRect.maxX; j++)
				{
					Building transmitter = new IntVec3(j, 0, i).GetTransmitter(map);
					if (transmitter != null && !transmitter.Destroyed)
					{
						CompPower powerComp = transmitter.PowerComp;
						if (powerComp != null && powerComp.TransmitsPowerNow && (transmitter.def.building == null || transmitter.def.building.allowWireConnection) && (disallowedNets == null || !disallowedNets.Contains(powerComp.transNet)))
						{
							float num2 = (float)(transmitter.Position - connectorPos).LengthHorizontalSquared;
							if (num2 < num)
							{
								num = num2;
								result = powerComp;
							}
						}
					}
				}
			}
			return result;
		}

		// Token: 0x0400460D RID: 17933
		private const int ConnectMaxDist = 6;
	}
}
