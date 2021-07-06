using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001936 RID: 6454
	public abstract class Alert_Critical : Alert
	{
		// Token: 0x1700169A RID: 5786
		// (get) Token: 0x06008F18 RID: 36632 RVA: 0x0005FC13 File Offset: 0x0005DE13
		protected override Color BGColor
		{
			get
			{
				float num = Pulser.PulseBrightness(0.5f, Pulser.PulseBrightness(0.5f, 0.6f));
				return new Color(num, num, num) * Color.red;
			}
		}

		// Token: 0x06008F19 RID: 36633 RVA: 0x00293688 File Offset: 0x00291888
		public override void AlertActiveUpdate()
		{
			if (this.lastActiveFrame < Time.frameCount - 1)
			{
				Messages.Message("MessageCriticalAlert".Translate(base.Label.CapitalizeFirst()), new LookTargets(this.GetReport().AllCulprits), MessageTypeDefOf.ThreatBig, true);
			}
			this.lastActiveFrame = Time.frameCount;
		}

		// Token: 0x06008F1A RID: 36634 RVA: 0x0005FC3F File Offset: 0x0005DE3F
		public Alert_Critical()
		{
			this.defaultPriority = AlertPriority.Critical;
		}

		// Token: 0x04005B4C RID: 23372
		private int lastActiveFrame = -1;

		// Token: 0x04005B4D RID: 23373
		private const float PulseFreq = 0.5f;

		// Token: 0x04005B4E RID: 23374
		private const float PulseAmpCritical = 0.6f;

		// Token: 0x04005B4F RID: 23375
		private const float PulseAmpTutorial = 0.2f;
	}
}
