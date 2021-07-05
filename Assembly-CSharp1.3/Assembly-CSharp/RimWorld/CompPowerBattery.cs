using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000CC7 RID: 3271
	public class CompPowerBattery : CompPower
	{
		// Token: 0x17000D21 RID: 3361
		// (get) Token: 0x06004C19 RID: 19481 RVA: 0x001960C8 File Offset: 0x001942C8
		public float AmountCanAccept
		{
			get
			{
				if (this.parent.IsBrokenDown())
				{
					return 0f;
				}
				CompProperties_Battery props = this.Props;
				return (props.storedEnergyMax - this.storedEnergy) / props.efficiency;
			}
		}

		// Token: 0x17000D22 RID: 3362
		// (get) Token: 0x06004C1A RID: 19482 RVA: 0x00196103 File Offset: 0x00194303
		public float StoredEnergy
		{
			get
			{
				return this.storedEnergy;
			}
		}

		// Token: 0x17000D23 RID: 3363
		// (get) Token: 0x06004C1B RID: 19483 RVA: 0x0019610B File Offset: 0x0019430B
		public float StoredEnergyPct
		{
			get
			{
				return this.storedEnergy / this.Props.storedEnergyMax;
			}
		}

		// Token: 0x17000D24 RID: 3364
		// (get) Token: 0x06004C1C RID: 19484 RVA: 0x0019611F File Offset: 0x0019431F
		public new CompProperties_Battery Props
		{
			get
			{
				return (CompProperties_Battery)this.props;
			}
		}

		// Token: 0x06004C1D RID: 19485 RVA: 0x0019612C File Offset: 0x0019432C
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<float>(ref this.storedEnergy, "storedPower", 0f, false);
			CompProperties_Battery props = this.Props;
			if (this.storedEnergy > props.storedEnergyMax)
			{
				this.storedEnergy = props.storedEnergyMax;
			}
		}

		// Token: 0x06004C1E RID: 19486 RVA: 0x00196176 File Offset: 0x00194376
		public override void CompTick()
		{
			base.CompTick();
			this.DrawPower(Mathf.Min(5f * CompPower.WattsToWattDaysPerTick, this.storedEnergy));
		}

		// Token: 0x06004C1F RID: 19487 RVA: 0x0019619C File Offset: 0x0019439C
		public void AddEnergy(float amount)
		{
			if (amount < 0f)
			{
				Log.Error("Cannot add negative energy " + amount);
				return;
			}
			if (amount > this.AmountCanAccept)
			{
				amount = this.AmountCanAccept;
			}
			amount *= this.Props.efficiency;
			this.storedEnergy += amount;
		}

		// Token: 0x06004C20 RID: 19488 RVA: 0x001961F5 File Offset: 0x001943F5
		public void DrawPower(float amount)
		{
			this.storedEnergy -= amount;
			if (this.storedEnergy < 0f)
			{
				Log.Error("Drawing power we don't have from " + this.parent);
				this.storedEnergy = 0f;
			}
		}

		// Token: 0x06004C21 RID: 19489 RVA: 0x00196232 File Offset: 0x00194432
		public void SetStoredEnergyPct(float pct)
		{
			pct = Mathf.Clamp01(pct);
			this.storedEnergy = this.Props.storedEnergyMax * pct;
		}

		// Token: 0x06004C22 RID: 19490 RVA: 0x0019624F File Offset: 0x0019444F
		public override void ReceiveCompSignal(string signal)
		{
			if (signal == "Breakdown")
			{
				this.DrawPower(this.StoredEnergy);
			}
		}

		// Token: 0x06004C23 RID: 19491 RVA: 0x0019626C File Offset: 0x0019446C
		public override string CompInspectStringExtra()
		{
			CompProperties_Battery props = this.Props;
			string text = "PowerBatteryStored".Translate() + ": " + this.storedEnergy.ToString("F0") + " / " + props.storedEnergyMax.ToString("F0") + " Wd";
			text += "\n" + "PowerBatteryEfficiency".Translate() + ": " + (props.efficiency * 100f).ToString("F0") + "%";
			if (this.storedEnergy > 0f)
			{
				text += "\n" + "SelfDischarging".Translate() + ": " + 5f.ToString("F0") + " W";
			}
			return text + "\n" + base.CompInspectStringExtra();
		}

		// Token: 0x06004C24 RID: 19492 RVA: 0x00196395 File Offset: 0x00194595
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			foreach (Gizmo gizmo in base.CompGetGizmosExtra())
			{
				yield return gizmo;
			}
			IEnumerator<Gizmo> enumerator = null;
			if (Prefs.DevMode)
			{
				yield return new Command_Action
				{
					defaultLabel = "DEBUG: Fill",
					action = delegate()
					{
						this.SetStoredEnergyPct(1f);
					}
				};
				yield return new Command_Action
				{
					defaultLabel = "DEBUG: Empty",
					action = delegate()
					{
						this.SetStoredEnergyPct(0f);
					}
				};
			}
			yield break;
			yield break;
		}

		// Token: 0x04002E10 RID: 11792
		private float storedEnergy;

		// Token: 0x04002E11 RID: 11793
		private const float SelfDischargingWatts = 5f;
	}
}
