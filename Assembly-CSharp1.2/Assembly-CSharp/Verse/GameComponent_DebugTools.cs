using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x020001D1 RID: 465
	public class GameComponent_DebugTools : GameComponent
	{
		// Token: 0x06000C0F RID: 3087 RVA: 0x0000F465 File Offset: 0x0000D665
		public GameComponent_DebugTools(Game game)
		{
		}

		// Token: 0x06000C10 RID: 3088 RVA: 0x0000F478 File Offset: 0x0000D678
		public override void GameComponentUpdate()
		{
			if (this.callbacks.Count > 0 && this.callbacks[0]())
			{
				this.callbacks.RemoveAt(0);
			}
		}

		// Token: 0x06000C11 RID: 3089 RVA: 0x0000F4A7 File Offset: 0x0000D6A7
		public void AddPerFrameCallback(Func<bool> callback)
		{
			this.callbacks.Add(callback);
		}

		// Token: 0x04000A89 RID: 2697
		private List<Func<bool>> callbacks = new List<Func<bool>>();
	}
}
