using System;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x020012E6 RID: 4838
	public class CompPowerPlant : CompPowerTrader
	{
		// Token: 0x17001028 RID: 4136
		// (get) Token: 0x060068D5 RID: 26837 RVA: 0x000476E8 File Offset: 0x000458E8
		protected virtual float DesiredPowerOutput
		{
			get
			{
				return -base.Props.basePowerConsumption;
			}
		}

		// Token: 0x060068D6 RID: 26838 RVA: 0x00204998 File Offset: 0x00202B98
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostSpawnSetup(respawningAfterLoad);
			this.refuelableComp = this.parent.GetComp<CompRefuelable>();
			this.breakdownableComp = this.parent.GetComp<CompBreakdownable>();
			if (base.Props.basePowerConsumption < 0f && !this.parent.IsBrokenDown() && FlickUtility.WantsToBeOn(this.parent))
			{
				base.PowerOn = true;
			}
		}

		// Token: 0x060068D7 RID: 26839 RVA: 0x000476F6 File Offset: 0x000458F6
		public override void PostDeSpawn(Map map)
		{
			base.PostDeSpawn(map);
			if (this.sustainerProducingPower != null && !this.sustainerProducingPower.Ended)
			{
				this.sustainerProducingPower.End();
			}
		}

		// Token: 0x060068D8 RID: 26840 RVA: 0x00204A04 File Offset: 0x00202C04
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

		// Token: 0x060068D9 RID: 26841 RVA: 0x00204A9C File Offset: 0x00202C9C
		public void UpdateDesiredPowerOutput()
		{
			if ((this.breakdownableComp != null && this.breakdownableComp.BrokenDown) || (this.refuelableComp != null && !this.refuelableComp.HasFuel) || (this.flickableComp != null && !this.flickableComp.SwitchIsOn) || !base.PowerOn)
			{
				base.PowerOutput = 0f;
				return;
			}
			base.PowerOutput = this.DesiredPowerOutput;
		}

		// Token: 0x040045B6 RID: 17846
		protected CompRefuelable refuelableComp;

		// Token: 0x040045B7 RID: 17847
		protected CompBreakdownable breakdownableComp;

		// Token: 0x040045B8 RID: 17848
		private Sustainer sustainerProducingPower;
	}
}
