using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000CD3 RID: 3283
	public class CompSchedule : ThingComp
	{
		// Token: 0x17000D39 RID: 3385
		// (get) Token: 0x06004C97 RID: 19607 RVA: 0x0019855D File Offset: 0x0019675D
		public CompProperties_Schedule Props
		{
			get
			{
				return (CompProperties_Schedule)this.props;
			}
		}

		// Token: 0x17000D3A RID: 3386
		// (get) Token: 0x06004C98 RID: 19608 RVA: 0x0019856A File Offset: 0x0019676A
		// (set) Token: 0x06004C99 RID: 19609 RVA: 0x00198572 File Offset: 0x00196772
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

		// Token: 0x06004C9A RID: 19610 RVA: 0x001985A4 File Offset: 0x001967A4
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostSpawnSetup(respawningAfterLoad);
			this.RecalculateAllowed();
		}

		// Token: 0x06004C9B RID: 19611 RVA: 0x001985B3 File Offset: 0x001967B3
		public override void CompTickRare()
		{
			base.CompTickRare();
			this.RecalculateAllowed();
		}

		// Token: 0x06004C9C RID: 19612 RVA: 0x001985C4 File Offset: 0x001967C4
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

		// Token: 0x06004C9D RID: 19613 RVA: 0x00198640 File Offset: 0x00196840
		public override string CompInspectStringExtra()
		{
			if (!this.Allowed)
			{
				return this.Props.offMessage;
			}
			return null;
		}

		// Token: 0x04002E56 RID: 11862
		public const string ScheduledOnSignal = "ScheduledOn";

		// Token: 0x04002E57 RID: 11863
		public const string ScheduledOffSignal = "ScheduledOff";

		// Token: 0x04002E58 RID: 11864
		private bool intAllowed;
	}
}
