using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020012DF RID: 4831
	public class CompPowerBattery : CompPower
	{
		// Token: 0x17001014 RID: 4116
		// (get) Token: 0x0600687E RID: 26750 RVA: 0x00203A6C File Offset: 0x00201C6C
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

		// Token: 0x17001015 RID: 4117
		// (get) Token: 0x0600687F RID: 26751 RVA: 0x0004720D File Offset: 0x0004540D
		public float StoredEnergy
		{
			get
			{
				return this.storedEnergy;
			}
		}

		// Token: 0x17001016 RID: 4118
		// (get) Token: 0x06006880 RID: 26752 RVA: 0x00047215 File Offset: 0x00045415
		public float StoredEnergyPct
		{
			get
			{
				return this.storedEnergy / this.Props.storedEnergyMax;
			}
		}

		// Token: 0x17001017 RID: 4119
		// (get) Token: 0x06006881 RID: 26753 RVA: 0x00047229 File Offset: 0x00045429
		public new CompProperties_Battery Props
		{
			get
			{
				return (CompProperties_Battery)this.props;
			}
		}

		// Token: 0x06006882 RID: 26754 RVA: 0x00203AA8 File Offset: 0x00201CA8
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

		// Token: 0x06006883 RID: 26755 RVA: 0x00047236 File Offset: 0x00045436
		public override void CompTick()
		{
			base.CompTick();
			this.DrawPower(Mathf.Min(5f * CompPower.WattsToWattDaysPerTick, this.storedEnergy));
		}

		// Token: 0x06006884 RID: 26756 RVA: 0x00203AF4 File Offset: 0x00201CF4
		public void AddEnergy(float amount)
		{
			if (amount < 0f)
			{
				Log.Error("Cannot add negative energy " + amount, false);
				return;
			}
			if (amount > this.AmountCanAccept)
			{
				amount = this.AmountCanAccept;
			}
			amount *= this.Props.efficiency;
			this.storedEnergy += amount;
		}

		// Token: 0x06006885 RID: 26757 RVA: 0x0004725A File Offset: 0x0004545A
		public void DrawPower(float amount)
		{
			this.storedEnergy -= amount;
			if (this.storedEnergy < 0f)
			{
				Log.Error("Drawing power we don't have from " + this.parent, false);
				this.storedEnergy = 0f;
			}
		}

		// Token: 0x06006886 RID: 26758 RVA: 0x00047298 File Offset: 0x00045498
		public void SetStoredEnergyPct(float pct)
		{
			pct = Mathf.Clamp01(pct);
			this.storedEnergy = this.Props.storedEnergyMax * pct;
		}

		// Token: 0x06006887 RID: 26759 RVA: 0x000472B5 File Offset: 0x000454B5
		public override void ReceiveCompSignal(string signal)
		{
			if (signal == "Breakdown")
			{
				this.DrawPower(this.StoredEnergy);
			}
		}

		// Token: 0x06006888 RID: 26760 RVA: 0x00203B50 File Offset: 0x00201D50
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

		// Token: 0x06006889 RID: 26761 RVA: 0x000472D0 File Offset: 0x000454D0
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

		// Token: 0x04004595 RID: 17813
		private float storedEnergy;

		// Token: 0x04004596 RID: 17814
		private const float SelfDischargingWatts = 5f;
	}
}
