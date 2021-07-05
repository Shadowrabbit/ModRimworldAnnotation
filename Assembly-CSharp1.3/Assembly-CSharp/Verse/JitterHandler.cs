using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000363 RID: 867
	public class JitterHandler
	{
		// Token: 0x170004F8 RID: 1272
		// (get) Token: 0x06001890 RID: 6288 RVA: 0x00091498 File Offset: 0x0008F698
		public Vector3 CurrentOffset
		{
			get
			{
				return this.curOffset;
			}
		}

		// Token: 0x06001891 RID: 6289 RVA: 0x000914A0 File Offset: 0x0008F6A0
		public void JitterHandlerTick()
		{
			if (this.curOffset.sqrMagnitude < this.JitterDropPerTick * this.JitterDropPerTick)
			{
				this.curOffset = new Vector3(0f, 0f, 0f);
				return;
			}
			this.curOffset -= this.curOffset.normalized * this.JitterDropPerTick;
		}

		// Token: 0x06001892 RID: 6290 RVA: 0x00091509 File Offset: 0x0008F709
		public void Notify_DamageApplied(DamageInfo dinfo)
		{
			if (dinfo.Def.hasForcefulImpact)
			{
				this.AddOffset(this.DamageJitterDistance, dinfo.Angle);
			}
		}

		// Token: 0x06001893 RID: 6291 RVA: 0x0009152C File Offset: 0x0008F72C
		public void Notify_DamageDeflected(DamageInfo dinfo)
		{
			if (dinfo.Def.hasForcefulImpact)
			{
				this.AddOffset(this.DeflectJitterDistance, dinfo.Angle);
			}
		}

		// Token: 0x06001894 RID: 6292 RVA: 0x00091550 File Offset: 0x0008F750
		public void AddOffset(float dist, float dir)
		{
			this.curOffset += Quaternion.AngleAxis(dir, Vector3.up) * Vector3.forward * dist;
			if (this.curOffset.sqrMagnitude > this.JitterMax * this.JitterMax)
			{
				this.curOffset *= this.JitterMax / this.curOffset.magnitude;
			}
		}

		// Token: 0x040010A8 RID: 4264
		private Vector3 curOffset = new Vector3(0f, 0f, 0f);

		// Token: 0x040010A9 RID: 4265
		private float DamageJitterDistance = 0.17f;

		// Token: 0x040010AA RID: 4266
		private float DeflectJitterDistance = 0.1f;

		// Token: 0x040010AB RID: 4267
		private float JitterDropPerTick = 0.018f;

		// Token: 0x040010AC RID: 4268
		private float JitterMax = 0.35f;
	}
}
