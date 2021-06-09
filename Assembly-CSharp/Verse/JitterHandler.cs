using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020004F3 RID: 1267
	public class JitterHandler
	{
		// Token: 0x170005D8 RID: 1496
		// (get) Token: 0x06001F89 RID: 8073 RVA: 0x0001BC2A File Offset: 0x00019E2A
		public Vector3 CurrentOffset
		{
			get
			{
				return this.curOffset;
			}
		}

		// Token: 0x06001F8A RID: 8074 RVA: 0x00100560 File Offset: 0x000FE760
		public void JitterHandlerTick()
		{
			if (this.curOffset.sqrMagnitude < this.JitterDropPerTick * this.JitterDropPerTick)
			{
				this.curOffset = new Vector3(0f, 0f, 0f);
				return;
			}
			this.curOffset -= this.curOffset.normalized * this.JitterDropPerTick;
		}

		// Token: 0x06001F8B RID: 8075 RVA: 0x0001BC32 File Offset: 0x00019E32
		public void Notify_DamageApplied(DamageInfo dinfo)
		{
			if (dinfo.Def.hasForcefulImpact)
			{
				this.AddOffset(this.DamageJitterDistance, dinfo.Angle);
			}
		}

		// Token: 0x06001F8C RID: 8076 RVA: 0x0001BC55 File Offset: 0x00019E55
		public void Notify_DamageDeflected(DamageInfo dinfo)
		{
			if (dinfo.Def.hasForcefulImpact)
			{
				this.AddOffset(this.DeflectJitterDistance, dinfo.Angle);
			}
		}

		// Token: 0x06001F8D RID: 8077 RVA: 0x001005CC File Offset: 0x000FE7CC
		public void AddOffset(float dist, float dir)
		{
			this.curOffset += Quaternion.AngleAxis(dir, Vector3.up) * Vector3.forward * dist;
			if (this.curOffset.sqrMagnitude > this.JitterMax * this.JitterMax)
			{
				this.curOffset *= this.JitterMax / this.curOffset.magnitude;
			}
		}

		// Token: 0x04001626 RID: 5670
		private Vector3 curOffset = new Vector3(0f, 0f, 0f);

		// Token: 0x04001627 RID: 5671
		private float DamageJitterDistance = 0.17f;

		// Token: 0x04001628 RID: 5672
		private float DeflectJitterDistance = 0.1f;

		// Token: 0x04001629 RID: 5673
		private float JitterDropPerTick = 0.018f;

		// Token: 0x0400162A RID: 5674
		private float JitterMax = 0.35f;
	}
}
