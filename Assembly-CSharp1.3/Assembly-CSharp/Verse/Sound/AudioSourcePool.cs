using System;
using UnityEngine;

namespace Verse.Sound
{
	// Token: 0x02000577 RID: 1399
	public class AudioSourcePool
	{
		// Token: 0x06002928 RID: 10536 RVA: 0x000F918E File Offset: 0x000F738E
		public AudioSourcePool()
		{
			this.sourcePoolCamera = new AudioSourcePoolCamera();
			this.sourcePoolWorld = new AudioSourcePoolWorld();
		}

		// Token: 0x06002929 RID: 10537 RVA: 0x000F91AC File Offset: 0x000F73AC
		public AudioSource GetSource(bool onCamera)
		{
			if (onCamera)
			{
				return this.sourcePoolCamera.GetSourceCamera();
			}
			return this.sourcePoolWorld.GetSourceWorld();
		}

		// Token: 0x0400197C RID: 6524
		public AudioSourcePoolCamera sourcePoolCamera;

		// Token: 0x0400197D RID: 6525
		public AudioSourcePoolWorld sourcePoolWorld;
	}
}
