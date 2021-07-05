using System;
using UnityEngine;

namespace Verse.Sound
{
	// Token: 0x02000952 RID: 2386
	public class AudioSourcePool
	{
		// Token: 0x06003A7E RID: 14974 RVA: 0x0002D0BC File Offset: 0x0002B2BC
		public AudioSourcePool()
		{
			this.sourcePoolCamera = new AudioSourcePoolCamera();
			this.sourcePoolWorld = new AudioSourcePoolWorld();
		}

		// Token: 0x06003A7F RID: 14975 RVA: 0x0002D0DA File Offset: 0x0002B2DA
		public AudioSource GetSource(bool onCamera)
		{
			if (onCamera)
			{
				return this.sourcePoolCamera.GetSourceCamera();
			}
			return this.sourcePoolWorld.GetSourceWorld();
		}

		// Token: 0x04002884 RID: 10372
		public AudioSourcePoolCamera sourcePoolCamera;

		// Token: 0x04002885 RID: 10373
		public AudioSourcePoolWorld sourcePoolWorld;
	}
}
