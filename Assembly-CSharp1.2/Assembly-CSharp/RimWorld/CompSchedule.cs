using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020012F2 RID: 4850
	public class CompSchedule : ThingComp
	{
		// Token: 0x1700103A RID: 4154
		// (get) Token: 0x06006938 RID: 26936 RVA: 0x00047C19 File Offset: 0x00045E19
		public CompProperties_Schedule Props
		{
			get
			{
				return (CompProperties_Schedule)this.props;
			}
		}

		// Token: 0x1700103B RID: 4155
		// (get) Token: 0x06006939 RID: 26937 RVA: 0x00047C26 File Offset: 0x00045E26
		// (set) Token: 0x0600693A RID: 26938 RVA: 0x00047C2E File Offset: 0x00045E2E
		public bool Allowed
		{
			get
			{
				return this.intAllowed;
			}
			set
			{
				if (this.intAllowed == value)
				{
					return;
				}
				this.intAllowed = value;
				this.parent.BroadcastCompSignal(this.intAllowed ? "ScheduledOn" : "ScheduledOff");
			}
		}

		// Token: 0x0600693B RID: 26939 RVA: 0x00047C60 File Offset: 0x00045E60
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostSpawnSetup(respawningAfterLoad);
			this.RecalculateAllowed();
		}

		// Token: 0x0600693C RID: 26940 RVA: 0x00047C6F File Offset: 0x00045E6F
		public override void CompTickRare()
		{
			base.CompTickRare();
			this.RecalculateAllowed();
		}

		// Token: 0x0600693D RID: 26941 RVA: 0x0020646C File Offset: 0x0020466C
		public void RecalculateAllowed()
		{
			float num = GenLocalDate.DayPercent(this.parent);
			if (this.Props.startTime <= this.Props.endTime)
			{
				this.Allowed = (num > this.Props.startTime && num < this.Props.endTime);
				return;
			}
			this.Allowed = (num < this.Props.endTime || num > this.Props.startTime);
		}

		// Token: 0x0600693E RID: 26942 RVA: 0x00047C7D File Offset: 0x00045E7D
		public override string CompInspectStringExtra()
		{
			if (!this.Allowed)
			{
				return this.Props.offMessage;
			}
			return null;
		}

		// Token: 0x0400460A RID: 17930
		public const string ScheduledOnSignal = "ScheduledOn";

		// Token: 0x0400460B RID: 17931
		public const string ScheduledOffSignal = "ScheduledOff";

		// Token: 0x0400460C RID: 17932
		private bool intAllowed;
	}
}
