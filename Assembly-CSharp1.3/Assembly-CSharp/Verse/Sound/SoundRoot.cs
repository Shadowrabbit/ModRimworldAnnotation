using System;

namespace Verse.Sound
{
	// Token: 0x02000571 RID: 1393
	public class SoundRoot
	{
		// Token: 0x0600291A RID: 10522 RVA: 0x000F8D48 File Offset: 0x000F6F48
		public SoundRoot()
		{
			this.sourcePool = new AudioSourcePool();
			this.sustainerManager = new SustainerManager();
			this.oneShotManager = new SampleOneShotManager();
		}

		// Token: 0x0600291B RID: 10523 RVA: 0x000F8D71 File Offset: 0x000F6F71
		public void Update()
		{
			this.sustainerManager.SustainerManagerUpdate();
			this.oneShotManager.SampleOneShotManagerUpdate();
		}

		// Token: 0x04001975 RID: 6517
		public AudioSourcePool sourcePool;

		// Token: 0x04001976 RID: 6518
		public SampleOneShotManager oneShotManager;

		// Token: 0x04001977 RID: 6519
		public SustainerManager sustainerManager;
	}
}
