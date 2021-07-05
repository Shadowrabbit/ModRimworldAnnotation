using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200123E RID: 4670
	public abstract class Alert_Critical : Alert
	{
		// Token: 0x17001392 RID: 5010
		// (get) Token: 0x06007022 RID: 28706 RVA: 0x00255AB2 File Offset: 0x00253CB2
		protected override Color BGColor
		{
			get
			{
				float num = Pulser.PulseBrightness(0.5f, Pulser.PulseBrightness(0.5f, 0.6f));
				return new Color(num, num, num) * Color.red;
			}
		}

		// Token: 0x06007023 RID: 28707 RVA: 0x00255AE0 File Offset: 0x00253CE0
		public override void AlertActiveUpdate()
		{
			if (this.lastActiveFrame < Time.frameCount - 1)
			{
				Messages.Message("MessageCriticalAlert".Translate(base.Label.CapitalizeFirst()), new LookTargets(this.GetReport().AllCulprits), MessageTypeDefOf.ThreatBig, true);
			}
			this.lastActiveFrame = Time.frameCount;
		}

		// Token: 0x06007024 RID: 28708 RVA: 0x00255B44 File Offset: 0x00253D44
		public Alert_Critical()
		{
			this.defaultPriority = AlertPriority.Critical;
		}

		// Token: 0x04003DEF RID: 15855
		private int lastActiveFrame = -1;

		// Token: 0x04003DF0 RID: 15856
		private const float PulseFreq = 0.5f;

		// Token: 0x04003DF1 RID: 15857
		private const float PulseAmpCritical = 0.6f;

		// Token: 0x04003DF2 RID: 15858
		private const float PulseAmpTutorial = 0.2f;
	}
}
