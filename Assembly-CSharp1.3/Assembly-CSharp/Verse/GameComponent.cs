using System;

namespace Verse
{
	// Token: 0x02000130 RID: 304
	public abstract class GameComponent : IExposable
	{
		// Token: 0x0600085B RID: 2139 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void GameComponentUpdate()
		{
		}

		// Token: 0x0600085C RID: 2140 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void GameComponentTick()
		{
		}

		// Token: 0x0600085D RID: 2141 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void GameComponentOnGUI()
		{
		}

		// Token: 0x0600085E RID: 2142 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void ExposeData()
		{
		}

		// Token: 0x0600085F RID: 2143 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void FinalizeInit()
		{
		}

		// Token: 0x06000860 RID: 2144 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void StartedNewGame()
		{
		}

		// Token: 0x06000861 RID: 2145 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void LoadedGame()
		{
		}
	}
}
