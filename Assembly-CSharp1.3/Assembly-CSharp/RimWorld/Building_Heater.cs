using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001064 RID: 4196
	public class Building_Heater : Building_TempControl
	{
		// Token: 0x0600637A RID: 25466 RVA: 0x00219D30 File Offset: 0x00217F30
		public override void TickRare()
		{
			if (this.compPowerTrader.PowerOn)
			{
				float ambientTemperature = base.AmbientTemperature;
				float num;
				if (ambientTemperature < 20f)
				{
					num = 1f;
				}
				else if (ambientTemperature > 120f)
				{
					num = 0f;
				}
				else
				{
					num = Mathf.InverseLerp(120f, 20f, ambientTemperature);
				}
				float energyLimit = this.compTempControl.Props.energyPerSecond * num * 4.1666665f;
				float num2 = GenTemperature.ControlTemperatureTempChange(base.Position, base.Map, energyLimit, this.compTempControl.targetTemperature);
				bool flag = !Mathf.Approximately(num2, 0f);
				CompProperties_Power props = this.compPowerTrader.Props;
				if (flag)
				{
					this.GetRoom(RegionType.Set_All).Temperature += num2;
					this.compPowerTrader.PowerOutput = -props.basePowerConsumption;
				}
				else
				{
					this.compPowerTrader.PowerOutput = -props.basePowerConsumption * this.compTempControl.Props.lowPowerConsumptionFactor;
				}
				this.compTempControl.operatingAtHighPower = flag;
			}
		}

		// Token: 0x04003858 RID: 14424
		private const float EfficiencyFalloffSpan = 100f;
	}
}
