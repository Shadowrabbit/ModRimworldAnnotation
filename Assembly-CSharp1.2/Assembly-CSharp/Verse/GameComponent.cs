using System;

namespace Verse
{
	// Token: 0x020001D0 RID: 464
	public abstract class GameComponent : IExposable
	{
		// Token: 0x06000C07 RID: 3079 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void GameComponentUpdate()
		{
		}

		// Token: 0x06000C08 RID: 3080 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void GameComponentTick()
		{
		}

		// Token: 0x06000C09 RID: 3081 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void GameComponentOnGUI()
		{
		}

		// Token: 0x06000C0A RID: 3082 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void ExposeData()
		{
		}

		// Token: 0x06000C0B RID: 3083 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void FinalizeInit()
		{
		}

		// Token: 0x06000C0C RID: 3084 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void StartedNewGame()
		{
		}

		// Token: 0x06000C0D RID: 3085 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void LoadedGame()
		{
		}
	}
}
