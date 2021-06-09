using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000730 RID: 1840
	public abstract class LetterWithTimeout : Letter
	{
		// Token: 0x170006FB RID: 1787
		// (get) Token: 0x06002E56 RID: 11862 RVA: 0x000245F0 File Offset: 0x000227F0
		public bool TimeoutActive
		{
			get
			{
				return this.disappearAtTick >= 0;
			}
		}

		// Token: 0x170006FC RID: 1788
		// (get) Token: 0x06002E57 RID: 11863 RVA: 0x000245FE File Offset: 0x000227FE
		public bool TimeoutPassed
		{
			get
			{
				return this.TimeoutActive && Find.TickManager.TicksGame >= this.disappearAtTick;
			}
		}

		// Token: 0x170006FD RID: 1789
		// (get) Token: 0x06002E58 RID: 11864 RVA: 0x0002461F File Offset: 0x0002281F
		public override bool CanShowInLetterStack
		{
			get
			{
				return base.CanShowInLetterStack && !this.TimeoutPassed;
			}
		}

		// Token: 0x06002E59 RID: 11865 RVA: 0x00024636 File Offset: 0x00022836
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.disappearAtTick, "disappearAtTick", -1, false);
		}

		// Token: 0x06002E5A RID: 11866 RVA: 0x00024650 File Offset: 0x00022850
		public void StartTimeout(int duration)
		{
			this.disappearAtTick = Find.TickManager.TicksGame + duration;
		}

		// Token: 0x06002E5B RID: 11867 RVA: 0x00137430 File Offset: 0x00135630
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

		// Token: 0x04001F9D RID: 8093
		public int disappearAtTick = -1;
	}
}
