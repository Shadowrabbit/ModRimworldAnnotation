using System;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02000CCB RID: 3275
	public class CompPowerPlant : CompPowerTrader
	{
		// Token: 0x17000D2F RID: 3375
		// (get) Token: 0x06004C55 RID: 19541 RVA: 0x00196E6B File Offset: 0x0019506B
		protected virtual float DesiredPowerOutput
		{
			get
			{
				return -base.Props.basePowerConsumption;
			}
		}

		// Token: 0x06004C56 RID: 19542 RVA: 0x00196E7C File Offset: 0x0019507C
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostSpawnSetup(respawningAfterLoad);
			this.refuelableComp = this.parent.GetComp<CompRefuelable>();
			this.breakdownableComp = this.parent.GetComp<CompBreakdownable>();
			this.autoPoweredComp = this.parent.GetComp<CompAutoPowered>();
			if (base.Props.basePowerConsumption < 0f && !this.parent.IsBrokenDown() && FlickUtility.WantsToBeOn(this.parent))
			{
				base.PowerOn = true;
			}
		}

		// Token: 0x06004C57 RID: 19543 RVA: 0x00196EF6 File Offset: 0x001950F6
		public override void PostDeSpawn(Map map)
		{
			base.PostDeSpawn(map);
			if (this.sustainerProducingPower != null && !this.sustainerProducingPower.Ended)
			{
				this.sustainerProducingPower.End();
			}
		}

		// Token: 0x06004C58 RID: 19544 RVA: 0x00196F20 File Offset: 0x00195120
		public override void CompTick()
		{
			base.CompTick();
			this.UpdateDesiredPowerOutput();
			if (base.Props.soundAmbientProducingPower != null)
			{
				if (base.PowerOutput > 0.01f)
				{
					if (this.sustainerProducingPower == null || this.sustainerProducingPower.Ended)
					{
						this.sustainerProducingPower = base.Props.soundAmbientProducingPower.TrySpawnSustainer(SoundInfo.InMap(this.parent, MaintenanceType.None));
					}
					this.sustainerProducingPower.Maintain();
					return;
				}
				if (this.sustainerProducingPower != null)
				{
					this.sustainerProducingPower.End();
					this.sustainerProducingPower = null;
				}
			}
		}

		// Token: 0x06004C59 RID: 19545 RVA: 0x00196FB8 File Offset: 0x001951B8
		public void UpdateDesiredPowerOutput()
		{
			if ((this.breakdownableComp != null && this.breakdownableComp.BrokenDown) || (this.refuelableComp != null && !this.refuelableComp.HasFuel) || (this.flickableComp != null && !this.flickableComp.SwitchIsOn) || (this.autoPoweredComp != null && !this.autoPoweredComp.WantsToBeOn) || !base.PowerOn)
			{
				base.PowerOutput = 0f;
				return;
			}
			base.PowerOutput = this.DesiredPowerOutput;
		}

		// Token: 0x04002E22 RID: 11810
		protected CompRefuelable refuelableComp;

		// Token: 0x04002E23 RID: 11811
		protected CompBreakdownable breakdownableComp;

		// Token: 0x04002E24 RID: 11812
		protected CompAutoPowered autoPoweredComp;

		// Token: 0x04002E25 RID: 11813
		private Sustainer sustainerProducingPower;
	}
}
