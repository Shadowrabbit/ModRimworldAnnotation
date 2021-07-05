using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000076 RID: 118
	public class CameraShaker
	{
		// Token: 0x170000B3 RID: 179
		// (get) Token: 0x06000481 RID: 1153 RVA: 0x0001804A File Offset: 0x0001624A
		// (set) Token: 0x06000482 RID: 1154 RVA: 0x00018052 File Offset: 0x00016252
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

		// Token: 0x170000B4 RID: 180
		// (get) Token: 0x06000483 RID: 1155 RVA: 0x0001806C File Offset: 0x0001626C
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

		// Token: 0x06000484 RID: 1156 RVA: 0x000180D3 File Offset: 0x000162D3
		public void DoShake(float mag)
		{
			if (mag <= 0f)
			{
				return;
			}
			this.CurShakeMag += mag;
		}

		// Token: 0x06000485 RID: 1157 RVA: 0x000180EC File Offset: 0x000162EC
		public void SetMinShake(float mag)
		{
			this.CurShakeMag = Mathf.Max(this.CurShakeMag, mag);
		}

		// Token: 0x06000486 RID: 1158 RVA: 0x00018100 File Offset: 0x00016300
		public void Update()
		{
			this.curShakeMag -= 0.5f * RealTime.realDeltaTime;
			if (this.curShakeMag < 0f)
			{
				this.curShakeMag = 0f;
			}
		}

		// Token: 0x04000189 RID: 393
		private float curShakeMag;

		// Token: 0x0400018A RID: 394
		private const float ShakeDecayRate = 0.5f;

		// Token: 0x0400018B RID: 395
		private const float ShakeFrequency = 24f;

		// Token: 0x0400018C RID: 396
		private const float MaxShakeMag = 0.2f;
	}
}
