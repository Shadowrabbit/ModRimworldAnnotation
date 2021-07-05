using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000CD5 RID: 3285
	public class PowerNet
	{
		// Token: 0x17000D3B RID: 3387
		// (get) Token: 0x06004CA5 RID: 19621 RVA: 0x001988E8 File Offset: 0x00196AE8
		public Map Map
		{
			get
			{
				return this.powerNetManager.map;
			}
		}

		// Token: 0x17000D3C RID: 3388
		// (get) Token: 0x06004CA6 RID: 19622 RVA: 0x001988F8 File Offset: 0x00196AF8
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

		// Token: 0x06004CA7 RID: 19623 RVA: 0x0019893C File Offset: 0x00196B3C
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

		// Token: 0x06004CA8 RID: 19624 RVA: 0x00198A44 File Offset: 0x00196C44
		private bool IsPowerSource(CompPower cp)
		{
			return cp is CompPowerBattery || (cp is CompPowerTrader && cp.Props.basePowerConsumption < 0f);
		}

		// Token: 0x06004CA9 RID: 19625 RVA: 0x00198A70 File Offset: 0x00196C70
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

		// Token: 0x06004CAA RID: 19626 RVA: 0x00198AB0 File Offset: 0x00196CB0
		public void RegisterConnector(CompPower b)
		{
			if (this.connectors.Contains(b))
			{
				Log.Error("PowerNet registered connector it already had: " + b);
				return;
			}
			this.connectors.Add(b);
			this.RegisterAllComponentsOf(b.parent);
		}

		// Token: 0x06004CAB RID: 19627 RVA: 0x00198AE9 File Offset: 0x00196CE9
		public void DeregisterConnector(CompPower b)
		{
			this.connectors.Remove(b);
			this.DeregisterAllComponentsOf(b.parent);
		}

		// Token: 0x06004CAC RID: 19628 RVA: 0x00198B04 File Offset: 0x00196D04
		private void RegisterAllComponentsOf(ThingWithComps parentThing)
		{
			CompPowerTrader comp = parentThing.GetComp<CompPowerTrader>();
			if (comp != null)
			{
				if (this.powerComps.Contains(comp))
				{
					Log.Error("PowerNet adding powerComp " + comp + " which it already has.");
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
					Log.Error("PowerNet adding batteryComp " + comp2 + " which it already has.");
					return;
				}
				this.batteryComps.Add(comp2);
			}
		}

		// Token: 0x06004CAD RID: 19629 RVA: 0x00198B88 File Offset: 0x00196D88
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

		// Token: 0x06004CAE RID: 19630 RVA: 0x00198BC4 File Offset: 0x00196DC4
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

		// Token: 0x06004CAF RID: 19631 RVA: 0x00198C24 File Offset: 0x00196E24
		public float CurrentStoredEnergy()
		{
			float num = 0f;
			for (int i = 0; i < this.batteryComps.Count; i++)
			{
				num += this.batteryComps[i].StoredEnergy;
			}
			return num;
		}

		// Token: 0x06004CB0 RID: 19632 RVA: 0x00198C64 File Offset: 0x00196E64
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

		// Token: 0x06004CB1 RID: 19633 RVA: 0x00198EBC File Offset: 0x001970BC
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
				Log.Warning("Drew energy from a PowerNet that didn't have it.");
			}
		}

		// Token: 0x06004CB2 RID: 19634 RVA: 0x00198FB8 File Offset: 0x001971B8
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
					goto IL_100;
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
					goto IL_15B;
				}
			}
			Log.Error("Too many iterations.");
			goto IL_15B;
			IL_100:
			float amount = energy / (float)PowerNet.batteriesShuffled.Count;
			for (int k = 0; k < PowerNet.batteriesShuffled.Count; k++)
			{
				PowerNet.batteriesShuffled[k].AddEnergy(amount);
			}
			energy = 0f;
			IL_15B:
			PowerNet.batteriesShuffled.Clear();
		}

		// Token: 0x06004CB3 RID: 19635 RVA: 0x0019912C File Offset: 0x0019732C
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

		// Token: 0x04002E5A RID: 11866
		public PowerNetManager powerNetManager;

		// Token: 0x04002E5B RID: 11867
		public bool hasPowerSource;

		// Token: 0x04002E5C RID: 11868
		public List<CompPower> connectors = new List<CompPower>();

		// Token: 0x04002E5D RID: 11869
		public List<CompPower> transmitters = new List<CompPower>();

		// Token: 0x04002E5E RID: 11870
		public List<CompPowerTrader> powerComps = new List<CompPowerTrader>();

		// Token: 0x04002E5F RID: 11871
		public List<CompPowerBattery> batteryComps = new List<CompPowerBattery>();

		// Token: 0x04002E60 RID: 11872
		private float debugLastCreatedEnergy;

		// Token: 0x04002E61 RID: 11873
		private float debugLastRawStoredEnergy;

		// Token: 0x04002E62 RID: 11874
		private float debugLastApparentStoredEnergy;

		// Token: 0x04002E63 RID: 11875
		private const int MaxRestartTryInterval = 200;

		// Token: 0x04002E64 RID: 11876
		private const int MinRestartTryInterval = 30;

		// Token: 0x04002E65 RID: 11877
		private const float RestartMinFraction = 0.05f;

		// Token: 0x04002E66 RID: 11878
		private const int ShutdownInterval = 20;

		// Token: 0x04002E67 RID: 11879
		private const float ShutdownMinFraction = 0.05f;

		// Token: 0x04002E68 RID: 11880
		private const float MinStoredEnergyToTurnOn = 5f;

		// Token: 0x04002E69 RID: 11881
		private static List<CompPowerTrader> partsWantingPowerOn = new List<CompPowerTrader>();

		// Token: 0x04002E6A RID: 11882
		private static List<CompPowerTrader> potentialShutdownParts = new List<CompPowerTrader>();

		// Token: 0x04002E6B RID: 11883
		private List<CompPowerBattery> givingBats = new List<CompPowerBattery>();

		// Token: 0x04002E6C RID: 11884
		private static List<CompPowerBattery> batteriesShuffled = new List<CompPowerBattery>();
	}
}
