using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000132 RID: 306
	public class GameComponent_DebugTools : GameComponent
	{
		// Token: 0x06000869 RID: 2153 RVA: 0x000276F0 File Offset: 0x000258F0
		public GameComponent_DebugTools(Game game)
		{
		}

		// Token: 0x0600086A RID: 2154 RVA: 0x00027703 File Offset: 0x00025903
		public override void GameComponentUpdate()
		{
			if (this.callbacks.Count > 0 && this.callbacks[0]())
			{
				this.callbacks.RemoveAt(0);
			}
		}

		// Token: 0x0600086B RID: 2155 RVA: 0x00027732 File Offset: 0x00025932
		public void AddPerFrameCallback(Func<bool> callback)
		{
			this.callbacks.Add(callback);
		}

		// Token: 0x040007E1 RID: 2017
		private List<Func<bool>> callbacks = new List<Func<bool>>();
	}
}
