using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000408 RID: 1032
	public abstract class LetterWithTimeout : Letter
	{
		// Token: 0x170005ED RID: 1517
		// (get) Token: 0x06001EF5 RID: 7925 RVA: 0x000C12CA File Offset: 0x000BF4CA
		public bool TimeoutActive
		{
			get
			{
				return this.disappearAtTick >= 0;
			}
		}

		// Token: 0x170005EE RID: 1518
		// (get) Token: 0x06001EF6 RID: 7926 RVA: 0x000C12D8 File Offset: 0x000BF4D8
		public bool TimeoutPassed
		{
			get
			{
				return this.TimeoutActive && Find.TickManager.TicksGame >= this.disappearAtTick;
			}
		}

		// Token: 0x170005EF RID: 1519
		// (get) Token: 0x06001EF7 RID: 7927 RVA: 0x000C12F9 File Offset: 0x000BF4F9
		public override bool CanShowInLetterStack
		{
			get
			{
				return base.CanShowInLetterStack && !this.TimeoutPassed;
			}
		}

		// Token: 0x06001EF8 RID: 7928 RVA: 0x000C1310 File Offset: 0x000BF510
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.disappearAtTick, "disappearAtTick", -1, false);
		}

		// Token: 0x06001EF9 RID: 7929 RVA: 0x000C132A File Offset: 0x000BF52A
		public void StartTimeout(int duration)
		{
			this.disappearAtTick = Find.TickManager.TicksGame + duration;
		}

		// Token: 0x06001EFA RID: 7930 RVA: 0x000C1340 File Offset: 0x000BF540
		protected override string PostProcessedLabel()
		{
			string text = base.PostProcessedLabel();
			if (this.TimeoutActive)
			{
				int num = Mathf.RoundToInt((float)(this.disappearAtTick - Find.TickManager.TicksGame) / 2500f);
				text += " (" + num + "LetterHour".Translate() + ")";
			}
			return text;
		}

		// Token: 0x040012E8 RID: 4840
		public int disappearAtTick = -1;
	}
}
