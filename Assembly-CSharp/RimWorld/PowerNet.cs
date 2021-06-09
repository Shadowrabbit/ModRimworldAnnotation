using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020012F5 RID: 4853
	public class PowerNet
	{
		// Token: 0x1700103E RID: 4158
		// (get) Token: 0x0600694E RID: 26958 RVA: 0x00047CCE File Offset: 0x00045ECE
		public Map Map
		{
			get
			{
				return this.powerNetManager.map;
			}
		}

		// Token: 0x1700103F RID: 4159
		// (get) Token: 0x0600694F RID: 26959 RVA: 0x00206968 File Offset: 0x00204B68
		public bool HasActivePowerSource
		{
			get
			{
				if (!this.hasPowerSource)
				{
					return false;
				}
				for (int i = 0; i < this.transmitters.Count; i++)
				{
					if (this.IsActivePowerSource(this.transmitters[i]))
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x06006950 RID: 26960 RVA: 0x002069AC File Offset: 0x00204BAC
		public PowerNet(IEnumerable<CompPower> newTransmitters)
		{
			foreach (CompPower compPower in newTransmitters)
			{
				this.transmitters.Add(compPower);
				compPower.transNet = this;
				this.RegisterAllComponentsOf(compPower.parent);
				if (compPower.connectChildren != null)
				{
					List<CompPower> connectChildren = compPower.connectChildren;
					for (int i = 0; i < connectChildren.Count; i++)
					{
						this.RegisterConnector(connectChildren[i]);
					}
				}
			}
			this.hasPowerSource = false;
			for (int j = 0; j < this.transmitters.Count; j++)
			{
				if (this.IsPowerSource(this.transmitters[j]))
				{
					this.hasPowerSource = true;
					return;
				}
			}
		}

		// Token: 0x06006951 RID: 26961 RVA: 0x00047CDB File Offset: 0x00045EDB
		private bool IsPowerSource(CompPower cp)
		{
			return cp is CompPowerBattery || (cp is CompPowerTrader && cp.Props.basePowerConsumption < 0f);
		}

		// Token: 0x06006952 RID: 26962 RVA: 0x00206AB4 File Offset: 0x00204CB4
		private bool IsActivePowerSource(CompPower cp)
		{
			CompPowerBattery compPowerBattery = cp as CompPowerBattery;
			if (compPowerBattery != null && compPowerBattery.StoredEnergy > 0f)
			{
				return true;
			}
			CompPowerTrader compPowerTrader = cp as CompPowerTrader;
			return compPowerTrader != null && compPowerTrader.PowerOutput > 0f;
		}

		// Token: 0x06006953 RID: 26963 RVA: 0x00047D04 File Offset: 0x00045F04
		public void RegisterConnector(CompPower b)
		{
			if (this.connectors.Contains(b))
			{
				Log.Error("PowerNet registered connector it already had: " + b, false);
				return;
			}
			this.connectors.Add(b);
			this.RegisterAllComponentsOf(b.parent);
		}

		// Token: 0x06006954 RID: 26964 RVA: 0x00047D3E File Offset: 0x00045F3E
		public void DeregisterConnector(CompPower b)
		{
			this.connectors.Remove(b);
			this.DeregisterAllComponentsOf(b.parent);
		}

		// Token: 0x06006955 RID: 26965 RVA: 0x00206AF4 File Offset: 0x00204CF4
		private void RegisterAllComponentsOf(ThingWithComps parentThing)
		{
			CompPowerTrader comp = parentThing.GetComp<CompPowerTrader>();
			if (comp != null)
			{
				if (this.powerComps.Contains(comp))
				{
					Log.Error("PowerNet adding powerComp " + comp + " which it already has.", false);
				}
				else
				{
					this.powerComps.Add(comp);
				}
			}
			CompPowerBattery comp2 = parentThing.GetComp<CompPowerBattery>();
			if (comp2 != null)
			{
				if (this.batteryComps.Contains(comp2))
				{
					Log.Error("PowerNet adding batteryComp " + comp2 + " which it already has.", false);
					return;
				}
				this.batteryComps.Add(comp2);
			}
		}

		// Token: 0x06006956 RID: 26966 RVA: 0x00206B78 File Offset: 0x00204D78
		private void DeregisterAllComponentsOf(ThingWithComps parentThing)
		{
			CompPowerTrader comp = parentThing.GetComp<CompPowerTrader>();
			if (comp != null)
			{
				this.powerComps.Remove(comp);
			}
			CompPowerBattery comp2 = parentThing.GetComp<CompPowerBattery>();
			if (comp2 != null)
			{
				this.batteryComps.Remove(comp2);
			}
		}

		// Token: 0x06006957 RID: 26967 RVA: 0x00206BB4 File Offset: 0x00204DB4
		public float CurrentEnergyGainRate()
		{
			if (DebugSettings.unlimitedPower)
			{
				return 100000f;
			}
			float num = 0f;
			for (int i = 0; i < this.powerComps.Count; i++)
			{
				if (this.powerComps[i].PowerOn)
				{
					num += this.powerComps[i].EnergyOutputPerTick;
				}
			}
			return num;
		}

		// Token: 0x06006958 RID: 26968 RVA: 0x00206C14 File Offset: 0x00204E14
		public float CurrentStoredEnergy()
		{
			float num = 0f;
			for (int i = 0; i < this.batteryComps.Count; i++)
			{
				num += this.batteryComps[i].StoredEnergy;
			}
			return num;
		}

		// Token: 0x06006959 RID: 26969 RVA: 0x00206C54 File Offset: 0x00204E54
		public void PowerNetTick()
		{
			float num = this.CurrentEnergyGainRate();
			float num2 = this.CurrentStoredEnergy();
			if (num2 + num >= -1E-07f && !this.Map.gameConditionManager.ElectricityDisabled)
			{
				float num3;
				if (this.batteryComps.Count > 0 && num2 >= 0.1f)
				{
					num3 = num2 - 5f;
				}
				else
				{
					num3 = num2;
				}
				if (num3 + num >= 0f)
				{
					PowerNet.partsWantingPowerOn.Clear();
					for (int i = 0; i < this.powerComps.Count; i++)
					{
						if (!this.powerComps[i].PowerOn && FlickUtility.WantsToBeOn(this.powerComps[i].parent) && !this.powerComps[i].parent.IsBrokenDown())
						{
							PowerNet.partsWantingPowerOn.Add(this.powerComps[i]);
						}
					}
					if (PowerNet.partsWantingPowerOn.Count > 0)
					{
						int num4 = 200 / PowerNet.partsWantingPowerOn.Count;
						if (num4 < 30)
						{
							num4 = 30;
						}
						if (Find.TickManager.TicksGame % num4 == 0)
						{
							int num5 = Mathf.Max(1, Mathf.RoundToInt((float)PowerNet.partsWantingPowerOn.Count * 0.05f));
							for (int j = 0; j < num5; j++)
							{
								CompPowerTrader compPowerTrader = PowerNet.partsWantingPowerOn.RandomElement<CompPowerTrader>();
								if (!compPowerTrader.PowerOn && num + num2 >= -(compPowerTrader.EnergyOutputPerTick + 1E-07f))
								{
									compPowerTrader.PowerOn = true;
									num += compPowerTrader.EnergyOutputPerTick;
								}
							}
						}
					}
				}
				this.ChangeStoredEnergy(num);
				return;
			}
			if (Find.TickManager.TicksGame % 20 == 0)
			{
				PowerNet.potentialShutdownParts.Clear();
				for (int k = 0; k < this.powerComps.Count; k++)
				{
					if (this.powerComps[k].PowerOn && this.powerComps[k].EnergyOutputPerTick < 0f)
					{
						PowerNet.potentialShutdownParts.Add(this.powerComps[k]);
					}
				}
				if (PowerNet.potentialShutdownParts.Count > 0)
				{
					int num6 = Mathf.Max(1, Mathf.RoundToInt((float)PowerNet.potentialShutdownParts.Count * 0.05f));
					for (int l = 0; l < num6; l++)
					{
						PowerNet.potentialShutdownParts.RandomElement<CompPowerTrader>().PowerOn = false;
					}
				}
			}
		}

		// Token: 0x0600695A RID: 26970 RVA: 0x00206EAC File Offset: 0x002050AC
		private void ChangeStoredEnergy(float extra)
		{
			if (extra > 0f)
			{
				this.DistributeEnergyAmongBatteries(extra);
				return;
			}
			float num = -extra;
			this.givingBats.Clear();
			for (int i = 0; i < this.batteryComps.Count; i++)
			{
				if (this.batteryComps[i].StoredEnergy > 1E-07f)
				{
					this.givingBats.Add(this.batteryComps[i]);
				}
			}
			float a = num / (float)this.givingBats.Count;
			int num2 = 0;
			while (num > 1E-07f)
			{
				for (int j = 0; j < this.givingBats.Count; j++)
				{
					float num3 = Mathf.Min(a, this.givingBats[j].StoredEnergy);
					this.givingBats[j].DrawPower(num3);
					num -= num3;
					if (num < 1E-07f)
					{
						return;
					}
				}
				num2++;
				if (num2 > 10)
				{
					break;
				}
			}
			if (num > 1E-07f)
			{
				Log.Warning("Drew energy from a PowerNet that didn't have it.", false);
			}
		}

		// Token: 0x0600695B RID: 26971 RVA: 0x00206FAC File Offset: 0x002051AC
		private void DistributeEnergyAmongBatteries(float energy)
		{
			if (energy <= 0f || !this.batteryComps.Any<CompPowerBattery>())
			{
				return;
			}
			PowerNet.batteriesShuffled.Clear();
			PowerNet.batteriesShuffled.AddRange(this.batteryComps);
			PowerNet.batteriesShuffled.Shuffle<CompPowerBattery>();
			int num = 0;
			for (;;)
			{
				num++;
				if (num > 10000)
				{
					break;
				}
				float num2 = float.MaxValue;
				for (int i = 0; i < PowerNet.batteriesShuffled.Count; i++)
				{
					num2 = Mathf.Min(num2, PowerNet.batteriesShuffled[i].AmountCanAccept);
				}
				if (energy < num2 * (float)PowerNet.batteriesShuffled.Count)
				{
					goto IL_101;
				}
				for (int j = PowerNet.batteriesShuffled.Count - 1; j >= 0; j--)
				{
					float amountCanAccept = PowerNet.batteriesShuffled[j].AmountCanAccept;
					bool flag = amountCanAccept <= 0f || amountCanAccept == num2;
					if (num2 > 0f)
					{
						PowerNet.batteriesShuffled[j].AddEnergy(num2);
						energy -= num2;
					}
					if (flag)
					{
						PowerNet.batteriesShuffled.RemoveAt(j);
					}
				}
				if (energy < 0.0005f || !PowerNet.batteriesShuffled.Any<CompPowerBattery>())
				{
					goto IL_15C;
				}
			}
			Log.Error("Too many iterations.", false);
			goto IL_15C;
			IL_101:
			float amount = energy / (float)PowerNet.batteriesShuffled.Count;
			for (int k = 0; k < PowerNet.batteriesShuffled.Count; k++)
			{
				PowerNet.batteriesShuffled[k].AddEnergy(amount);
			}
			energy = 0f;
			IL_15C:
			PowerNet.batteriesShuffled.Clear();
		}

		// Token: 0x0600695C RID: 26972 RVA: 0x00207120 File Offset: 0x00205320
		public string DebugString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("POWERNET:");
			stringBuilder.AppendLine("  Created energy: " + this.debugLastCreatedEnergy);
			stringBuilder.AppendLine("  Raw stored energy: " + this.debugLastRawStoredEnergy);
			stringBuilder.AppendLine("  Apparent stored energy: " + this.debugLastApparentStoredEnergy);
			stringBuilder.AppendLine("  hasPowerSource: " + this.hasPowerSource.ToString());
			stringBuilder.AppendLine("  Connectors: ");
			foreach (CompPower compPower in this.connectors)
			{
				stringBuilder.AppendLine("      " + compPower.parent);
			}
			stringBuilder.AppendLine("  Transmitters: ");
			foreach (CompPower compPower2 in this.transmitters)
			{
				stringBuilder.AppendLine("      " + compPower2.parent);
			}
			stringBuilder.AppendLine("  powerComps: ");
			foreach (CompPowerTrader compPowerTrader in this.powerComps)
			{
				stringBuilder.AppendLine("      " + compPowerTrader.parent);
			}
			stringBuilder.AppendLine("  batteryComps: ");
			foreach (CompPowerBattery compPowerBattery in this.batteryComps)
			{
				stringBuilder.AppendLine("      " + compPowerBattery.parent);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x04004618 RID: 17944
		public PowerNetManager powerNetManager;

		// Token: 0x04004619 RID: 17945
		public bool hasPowerSource;

		// Token: 0x0400461A RID: 17946
		public List<CompPower> connectors = new List<CompPower>();

		// Token: 0x0400461B RID: 17947
		public List<CompPower> transmitters = new List<CompPower>();

		// Token: 0x0400461C RID: 17948
		public List<CompPowerTrader> powerComps = new List<CompPowerTrader>();

		// Token: 0x0400461D RID: 17949
		public List<CompPowerBattery> batteryComps = new List<CompPowerBattery>();

		// Token: 0x0400461E RID: 17950
		private float debugLastCreatedEnergy;

		// Token: 0x0400461F RID: 17951
		private float debugLastRawStoredEnergy;

		// Token: 0x04004620 RID: 17952
		private float debugLastApparentStoredEnergy;

		// Token: 0x04004621 RID: 17953
		private const int MaxRestartTryInterval = 200;

		// Token: 0x04004622 RID: 17954
		private const int MinRestartTryInterval = 30;

		// Token: 0x04004623 RID: 17955
		private const float RestartMinFraction = 0.05f;

		// Token: 0x04004624 RID: 17956
		private const int ShutdownInterval = 20;

		// Token: 0x04004625 RID: 17957
		private const float ShutdownMinFraction = 0.05f;

		// Token: 0x04004626 RID: 17958
		private const float MinStoredEnergyToTurnOn = 5f;

		// Token: 0x04004627 RID: 17959
		private static List<CompPowerTrader> partsWantingPowerOn = new List<CompPowerTrader>();

		// Token: 0x04004628 RID: 17960
		private static List<CompPowerTrader> potentialShutdownParts = new List<CompPowerTrader>();

		// Token: 0x04004629 RID: 17961
		private List<CompPowerBattery> givingBats = new List<CompPowerBattery>();

		// Token: 0x0400462A RID: 17962
		private static List<CompPowerBattery> batteriesShuffled = new List<CompPowerBattery>();
	}
}
