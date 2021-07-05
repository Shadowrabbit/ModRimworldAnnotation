using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000CD4 RID: 3284
	public static class PowerConnectionMaker
	{
		// Token: 0x06004C9F RID: 19615 RVA: 0x00198658 File Offset: 0x00196858
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

		// Token: 0x06004CA0 RID: 19616 RVA: 0x001986B0 File Offset: 0x001968B0
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

		// Token: 0x06004CA1 RID: 19617 RVA: 0x00198710 File Offset: 0x00196910
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

		// Token: 0x06004CA2 RID: 19618 RVA: 0x00198764 File Offset: 0x00196964
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

		// Token: 0x06004CA3 RID: 19619 RVA: 0x001987D2 File Offset: 0x001969D2
		private static IEnumerable<CompPower> PotentialConnectorsForTransmitter(CompPower b)
		{
			if (!b.parent.Spawned)
			{
				Log.Warning("Can't check potential connectors for " + b + " because it's unspawned.");
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

		// Token: 0x06004CA4 RID: 19620 RVA: 0x001987E4 File Offset: 0x001969E4
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

		// Token: 0x04002E59 RID: 11865
		private const int ConnectMaxDist = 6;
	}
}
