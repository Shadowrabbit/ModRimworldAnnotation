using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020000C3 RID: 195
	public class CameraShaker
	{
		// Token: 0x17000107 RID: 263
		// (get) Token: 0x060005ED RID: 1517 RVA: 0x0000B12A File Offset: 0x0000932A
		// (set) Token: 0x060005EE RID: 1518 RVA: 0x0000B132 File Offset: 0x00009332
		public float CurShakeMag
		{
			get
			{
				return this.curShakeMag;
			}
			set
			{
				this.curShakeMag = Mathf.Clamp(value, 0f, 0.2f);
			}
		}

		// Token: 0x17000108 RID: 264
		// (get) Token: 0x060005EF RID: 1519 RVA: 0x0008D87C File Offset: 0x0008BA7C
		public Vector3 ShakeOffset
		{
			get
			{
				float x = Mathf.Sin(Time.realtimeSinceStartup * 24f) * this.curShakeMag;
				float y = Mathf.Sin(Time.realtimeSinceStartup * 24f * 1.05f) * this.curShakeMag;
				float z = Mathf.Sin(Time.realtimeSinceStartup * 24f * 1.1f) * this.curShakeMag;
				return new Vector3(x, y, z);
			}
		}

		// Token: 0x060005F0 RID: 1520 RVA: 0x0000B14A File Offset: 0x0000934A
		public void DoShake(float mag)
		{
			if (mag <= 0f)
			{
				return;
			}
			this.CurShakeMag += mag;
		}

		// Token: 0x060005F1 RID: 1521 RVA: 0x0000B163 File Offset: 0x00009363
		public void SetMinShake(float mag)
		{
			this.CurShakeMag = Mathf.Max(this.CurShakeMag, mag);
		}

		// Token: 0x060005F2 RID: 1522 RVA: 0x0000B177 File Offset: 0x00009377
		public void Update()
		{
			this.curShakeMag -= 0.5f * RealTime.realDeltaTime;
			if (this.curShakeMag < 0f)
			{
				this.curShakeMag = 0f;
			}
		}

		// Token: 0x04000303 RID: 771
		private float curShakeMag;

		// Token: 0x04000304 RID: 772
		private const float ShakeDecayRate = 0.5f;

		// Token: 0x04000305 RID: 773
		private const float ShakeFrequency = 24f;

		// Token: 0x04000306 RID: 774
		private const float MaxShakeMag = 0.2f;
	}
}
