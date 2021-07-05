using System;
using UnityEngine;

namespace Verse.Sound
{
	// Token: 0x02000936 RID: 2358
	public class SoundParamTarget_PropertyEcho : SoundParamTarget
	{
		// Token: 0x1700093C RID: 2364
		// (get) Token: 0x060039F5 RID: 14837 RVA: 0x0002CB26 File Offset: 0x0002AD26
		public override string Label
		{
			get
			{
				return "EchoFilter-" + this.filterProperty;
			}
		}

		// Token: 0x1700093D RID: 2365
		// (get) Token: 0x060039F6 RID: 14838 RVA: 0x0002CB3D File Offset: 0x0002AD3D
		public override Type NeededFilterType
		{
			get
			{
				return typeof(SoundFilterEcho);
			}
		}

		// Token: 0x060039F7 RID: 14839 RVA: 0x001684CC File Offset: 0x001666CC
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

		// Token: 0x04002827 RID: 10279
		private EchoFilterProperty filterProperty;
	}
}
