using System;

namespace Verse.Sound
{
	// Token: 0x0200094C RID: 2380
	public class SoundRoot
	{
		// Token: 0x06003A70 RID: 14960 RVA: 0x0002D01B File Offset: 0x0002B21B
		public SoundRoot()
		{
			this.sourcePool = new AudioSourcePool();
			this.sustainerManager = new SustainerManager();
			this.oneShotManager = new SampleOneShotManager();
		}

		// Token: 0x06003A71 RID: 14961 RVA: 0x0002D044 File Offset: 0x0002B244
		public void Update()
		{
			this.sustainerManager.SustainerManagerUpdate();
			this.oneShotManager.SampleOneShotManagerUpdate();
		}

		// Token: 0x0400287D RID: 10365
		public AudioSourcePool sourcePool;

		// Token: 0x0400287E RID: 10366
		public SampleOneShotManager oneShotManager;

		// Token: 0x0400287F RID: 10367
		public SustainerManager sustainerManager;
	}
}
