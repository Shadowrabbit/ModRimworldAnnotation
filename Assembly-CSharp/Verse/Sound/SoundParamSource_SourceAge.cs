using System;
using UnityEngine;

namespace Verse.Sound
{
	// Token: 0x0200092C RID: 2348
	public class SoundParamSource_SourceAge : SoundParamSource
	{
		// Token: 0x17000933 RID: 2355
		// (get) Token: 0x060039E0 RID: 14816 RVA: 0x0002CA7A File Offset: 0x0002AC7A
		public override string Label
		{
			get
			{
				return "Sustainer age";
			}
		}

		// Token: 0x060039E1 RID: 14817 RVA: 0x0002CA81 File Offset: 0x0002AC81
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

		// Token: 0x04002815 RID: 10261
		public TimeType timeType;
	}
}
