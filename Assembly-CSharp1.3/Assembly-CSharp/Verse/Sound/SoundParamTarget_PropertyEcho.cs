using System;
using UnityEngine;

namespace Verse.Sound
{
	// Token: 0x02000561 RID: 1377
	public class SoundParamTarget_PropertyEcho : SoundParamTarget
	{
		// Token: 0x17000800 RID: 2048
		// (get) Token: 0x060028BA RID: 10426 RVA: 0x000F729E File Offset: 0x000F549E
		public override string Label
		{
			get
			{
				return "EchoFilter-" + this.filterProperty;
			}
		}

		// Token: 0x17000801 RID: 2049
		// (get) Token: 0x060028BB RID: 10427 RVA: 0x000F72B5 File Offset: 0x000F54B5
		public override Type NeededFilterType
		{
			get
			{
				return typeof(SoundFilterEcho);
			}
		}

		// Token: 0x060028BC RID: 10428 RVA: 0x000F72C4 File Offset: 0x000F54C4
		public override void SetOn(Sample sample, float value)
		{
			AudioEchoFilter audioEchoFilter = sample.source.GetComponent<AudioEchoFilter>();
			if (audioEchoFilter == null)
			{
				audioEchoFilter = sample.source.gameObject.AddComponent<AudioEchoFilter>();
			}
			if (this.filterProperty == EchoFilterProperty.Delay)
			{
				audioEchoFilter.delay = value;
			}
			if (this.filterProperty == EchoFilterProperty.DecayRatio)
			{
				audioEchoFilter.decayRatio = value;
			}
			if (this.filterProperty == EchoFilterProperty.WetMix)
			{
				audioEchoFilter.wetMix = value;
			}
			if (this.filterProperty == EchoFilterProperty.DryMix)
			{
				audioEchoFilter.dryMix = value;
			}
		}

		// Token: 0x0400192A RID: 6442
		private EchoFilterProperty filterProperty;
	}
}
