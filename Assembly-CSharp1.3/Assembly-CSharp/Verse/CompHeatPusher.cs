using System;

namespace Verse
{
	// Token: 0x02000386 RID: 902
	public class CompHeatPusher : ThingComp
	{
		// Token: 0x17000576 RID: 1398
		// (get) Token: 0x06001A71 RID: 6769 RVA: 0x00099968 File Offset: 0x00097B68
		public CompProperties_HeatPusher Props
		{
			get
			{
				return (CompProperties_HeatPusher)this.props;
			}
		}

		// Token: 0x17000577 RID: 1399
		// (get) Token: 0x06001A72 RID: 6770 RVA: 0x00099978 File Offset: 0x00097B78
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

		// Token: 0x06001A73 RID: 6771 RVA: 0x000999BC File Offset: 0x00097BBC
		public override void CompTick()
		{
			base.CompTick();
			if (this.parent.IsHashIntervalTick(60) && this.ShouldPushHeatNow)
			{
				GenTemperature.PushHeat(this.parent.PositionHeld, this.parent.MapHeld, this.Props.heatPerSecond);
			}
		}

		// Token: 0x06001A74 RID: 6772 RVA: 0x00099A0D File Offset: 0x00097C0D
		public override void CompTickRare()
		{
			base.CompTickRare();
			if (this.ShouldPushHeatNow)
			{
				GenTemperature.PushHeat(this.parent.PositionHeld, this.parent.MapHeld, this.Props.heatPerSecond * 4.1666665f);
			}
		}

		// Token: 0x04001133 RID: 4403
		private const int HeatPushInterval = 60;
	}
}
