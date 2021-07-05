using System;
using UnityEngine;

namespace Verse.Sound
{
	// Token: 0x02000557 RID: 1367
	public class SoundParamSource_SourceAge : SoundParamSource
	{
		// Token: 0x170007F7 RID: 2039
		// (get) Token: 0x060028A5 RID: 10405 RVA: 0x000F7100 File Offset: 0x000F5300
		public override string Label
		{
			get
			{
				return "Sustainer age";
			}
		}

		// Token: 0x060028A6 RID: 10406 RVA: 0x000F7107 File Offset: 0x000F5307
		public override float ValueFor(Sample samp)
		{
			if (this.timeType == TimeType.RealtimeSeconds)
			{
				return Time.realtimeSinceStartup - samp.ParentStartRealTime;
			}
			if (this.timeType == TimeType.Ticks && Find.TickManager != null)
			{
				return (float)Find.TickManager.TicksGame - samp.ParentStartTick;
			}
			return 0f;
		}

		// Token: 0x04001918 RID: 6424
		public TimeType timeType;
	}
}
