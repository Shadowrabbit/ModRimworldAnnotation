using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000078 RID: 120
	public class CameraSwooper
	{
		// Token: 0x0600048C RID: 1164 RVA: 0x00018132 File Offset: 0x00016332
		public void StartSwoopFromRoot(Vector3 FinalOffset, float FinalOrthoSizeOffset, float TotalSwoopTime, SwoopCallbackMethod SwoopFinishedCallback)
		{
			this.Swooping = true;
			this.TimeSinceSwoopStart = 0f;
			this.FinalOffset = FinalOffset;
			this.FinalOrthoSizeOffset = FinalOrthoSizeOffset;
			this.TotalSwoopTime = TotalSwoopTime;
			this.SwoopFinishedCallback = SwoopFinishedCallback;
			this.SwoopingTo = false;
		}

		// Token: 0x0600048D RID: 1165 RVA: 0x0001816A File Offset: 0x0001636A
		public void StartSwoopToRoot(Vector3 FinalOffset, float FinalOrthoSizeOffset, float TotalSwoopTime, SwoopCallbackMethod SwoopFinishedCallback)
		{
			this.StartSwoopFromRoot(FinalOffset, FinalOrthoSizeOffset, TotalSwoopTime, SwoopFinishedCallback);
			this.SwoopingTo = true;
		}

		// Token: 0x0600048E RID: 1166 RVA: 0x00018180 File Offset: 0x00016380
		public void Update()
		{
			if (this.Swooping)
			{
				this.TimeSinceSwoopStart += Time.deltaTime;
				if (this.TimeSinceSwoopStart >= this.TotalSwoopTime)
				{
					this.Swooping = false;
					if (this.SwoopFinishedCallback != null)
					{
						this.SwoopFinishedCallback();
					}
				}
			}
		}

		// Token: 0x0600048F RID: 1167 RVA: 0x000181D0 File Offset: 0x000163D0
		public void OffsetCameraFrom(GameObject camObj, Vector3 basePos, float baseSize)
		{
			float num = this.TimeSinceSwoopStart / this.TotalSwoopTime;
			if (!this.Swooping)
			{
				num = 0f;
			}
			else
			{
				num = this.TimeSinceSwoopStart / this.TotalSwoopTime;
				if (this.SwoopingTo)
				{
					num = 1f - num;
				}
				num = (float)Math.Pow((double)num, 1.7000000476837158);
			}
			camObj.transform.position = basePos + this.FinalOffset * num;
			Find.Camera.orthographicSize = baseSize + this.FinalOrthoSizeOffset * num;
		}

		// Token: 0x0400018D RID: 397
		public bool Swooping;

		// Token: 0x0400018E RID: 398
		private bool SwoopingTo;

		// Token: 0x0400018F RID: 399
		private float TimeSinceSwoopStart;

		// Token: 0x04000190 RID: 400
		private Vector3 FinalOffset;

		// Token: 0x04000191 RID: 401
		private float FinalOrthoSizeOffset;

		// Token: 0x04000192 RID: 402
		private float TotalSwoopTime;

		// Token: 0x04000193 RID: 403
		private SwoopCallbackMethod SwoopFinishedCallback;
	}
}
