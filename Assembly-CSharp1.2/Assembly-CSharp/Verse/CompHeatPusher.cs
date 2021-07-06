using System;

namespace Verse
{
	// Token: 0x02000527 RID: 1319
	public class CompHeatPusher : ThingComp
	{
		// Token: 0x1700066A RID: 1642
		// (get) Token: 0x060021D7 RID: 8663 RVA: 0x0001D479 File Offset: 0x0001B679
		public CompProperties_HeatPusher Props
		{
			get
			{
				return (CompProperties_HeatPusher)this.props;
			}
		}

		// Token: 0x1700066B RID: 1643
		// (get) Token: 0x060021D8 RID: 8664 RVA: 0x00107B18 File Offset: 0x00105D18
		protected virtual bool ShouldPushHeatNow
		{
			get
			{
				if (!this.parent.SpawnedOrAnyParentSpawned)
				{
					return false;
				}
				CompProperties_HeatPusher props = this.Props;
				float ambientTemperature = this.parent.AmbientTemperature;
				return ambientTemperature < props.heatPushMaxTemperature && ambientTemperature > props.heatPushMinTemperature;
			}
		}

		// Token: 0x060021D9 RID: 8665 RVA: 0x00107B5C File Offset: 0x00105D5C
		public override void CompTick()
		{
			base.CompTick();
			if (this.parent.IsHashIntervalTick(60) && this.ShouldPushHeatNow)
			{
				GenTemperature.PushHeat(this.parent.PositionHeld, this.parent.MapHeld, this.Props.heatPerSecond);
			}
		}

		// Token: 0x060021DA RID: 8666 RVA: 0x0001D486 File Offset: 0x0001B686
		public override void CompTickRare()
		{
			base.CompTickRare();
			if (this.ShouldPushHeatNow)
			{
				GenTemperature.PushHeat(this.parent.PositionHeld, this.parent.MapHeld, this.Props.heatPerSecond * 4.1666665f);
			}
		}

		// Token: 0x040016FB RID: 5883
		private const int HeatPushInterval = 60;
	}
}
