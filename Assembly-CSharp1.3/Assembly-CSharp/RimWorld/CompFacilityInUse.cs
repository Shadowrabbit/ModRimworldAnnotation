using System;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x0200112D RID: 4397
	public class CompFacilityInUse : ThingComp
	{
		// Token: 0x17001218 RID: 4632
		// (get) Token: 0x060069B4 RID: 27060 RVA: 0x00239FF1 File Offset: 0x002381F1
		public CompProperties_FacilityInUse Props
		{
			get
			{
				return this.props as CompProperties_FacilityInUse;
			}
		}

		// Token: 0x060069B5 RID: 27061 RVA: 0x00239FFE File Offset: 0x002381FE
		public override void PostDeSpawn(Map map)
		{
			base.PostDeSpawn(map);
			Effecter effecter = this.effecterInUse;
			if (effecter != null)
			{
				effecter.Cleanup();
			}
			this.effecterInUse = null;
		}

		// Token: 0x060069B6 RID: 27062 RVA: 0x0023A01F File Offset: 0x0023821F
		public override void CompTick()
		{
			base.CompTick();
			this.DoTick();
		}

		// Token: 0x060069B7 RID: 27063 RVA: 0x0023A02D File Offset: 0x0023822D
		public override void CompTickRare()
		{
			base.CompTickRare();
			this.DoTick();
		}

		// Token: 0x060069B8 RID: 27064 RVA: 0x0023A03B File Offset: 0x0023823B
		public override void CompTickLong()
		{
			base.CompTickLong();
			this.DoTick();
		}

		// Token: 0x060069B9 RID: 27065 RVA: 0x0023A04C File Offset: 0x0023824C
		private void DoTick()
		{
			CompFacility compFacility = this.parent.TryGetComp<CompFacility>();
			List<Thing> list = (compFacility != null) ? compFacility.LinkedBuildings : null;
			if (list == null)
			{
				return;
			}
			CompPowerTrader compPowerTrader = this.parent.TryGetComp<CompPowerTrader>();
			Thing thing = null;
			foreach (Thing thing2 in list)
			{
				if (compPowerTrader.PowerOn && this.BuildingInUse(thing2))
				{
					thing = thing2;
					break;
				}
			}
			bool flag = thing != null;
			this.operatingAtHighPower = false;
			if (this.Props.inUsePowerConsumption != null)
			{
				float num = compPowerTrader.Props.basePowerConsumption;
				if (flag)
				{
					num = this.Props.inUsePowerConsumption.Value;
					this.operatingAtHighPower = true;
				}
				compPowerTrader.PowerOutput = -num;
			}
			if (this.Props.effectInUse != null)
			{
				if (flag)
				{
					if (this.effecterInUse == null)
					{
						this.effecterInUse = this.Props.effectInUse.Spawn();
						this.effecterInUse.Trigger(this.parent, thing);
					}
					this.effecterInUse.EffectTick(this.parent, thing);
				}
				if (!flag && this.effecterInUse != null)
				{
					this.effecterInUse.Cleanup();
					this.effecterInUse = null;
				}
			}
		}

		// Token: 0x060069BA RID: 27066 RVA: 0x0023A1AC File Offset: 0x002383AC
		private bool BuildingInUse(Thing building)
		{
			Building_Bed building_Bed;
			return (building_Bed = (building as Building_Bed)) != null && building_Bed.AnyOccupants;
		}

		// Token: 0x060069BB RID: 27067 RVA: 0x0023A1D0 File Offset: 0x002383D0
		public override string CompInspectStringExtra()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("PowerConsumptionMode".Translate() + ": ");
			if (this.operatingAtHighPower)
			{
				stringBuilder.Append("PowerConsumptionHigh".Translate().CapitalizeFirst());
			}
			else
			{
				stringBuilder.Append("PowerConsumptionLow".Translate().CapitalizeFirst());
			}
			return stringBuilder.ToString();
		}

		// Token: 0x04003B08 RID: 15112
		[Unsaved(false)]
		private bool operatingAtHighPower;

		// Token: 0x04003B09 RID: 15113
		[Unsaved(false)]
		private Effecter effecterInUse;
	}
}
