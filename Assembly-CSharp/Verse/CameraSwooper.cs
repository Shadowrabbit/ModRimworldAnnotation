using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020000C5 RID: 197
	public class CameraSwooper
	{
		// Token: 0x060005F8 RID: 1528 RVA: 0x0000B1A9 File Offset: 0x000093A9
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

		// Token: 0x060005F9 RID: 1529 RVA: 0x0000B1E1 File Offset: 0x000093E1
		public void StartSwoopToRoot(Vector3 FinalOffset, float FinalOrthoSizeOffset, float TotalSwoopTime, SwoopCallbackMethod SwoopFinishedCallback)
		{
			this.StartSwoopFromRoot(FinalOffset, FinalOrthoSizeOffset, TotalSwoopTime, SwoopFinishedCallback);
			this.SwoopingTo = true;
		}

		// Token: 0x060005FA RID: 1530 RVA: 0x0008D8E4 File Offset: 0x0008BAE4
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

		// Token: 0x060005FB RID: 1531 RVA: 0x0008D934 File Offset: 0x0008BB34
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

		// Token: 0x04000307 RID: 775
		public bool Swooping;

		// Token: 0x04000308 RID: 776
		private bool SwoopingTo;

		// Token: 0x04000309 RID: 777
		private float TimeSinceSwoopStart;

		// Token: 0x0400030A RID: 778
		private Vector3 FinalOffset;

		// Token: 0x0400030B RID: 779
		private float FinalOrthoSizeOffset;

		// Token: 0x0400030C RID: 780
		private float TotalSwoopTime;

		// Token: 0x0400030D RID: 781
		private SwoopCallbackMethod SwoopFinishedCallback;
	}
}
