using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000A78 RID: 2680
	public class IdeoIconDef : IdeoSymbolPartDef
	{
		// Token: 0x17000B3B RID: 2875
		// (get) Token: 0x0600402B RID: 16427 RVA: 0x0015B5FE File Offset: 0x001597FE
		public Texture2D Icon
		{
			get
			{
				if (this.cachedIcon == null)
				{
					this.cachedIcon = ContentFinder<Texture2D>.Get(this.iconPath, true);
				}
				return this.cachedIcon;
			}
		}

		// Token: 0x04002473 RID: 9331
		[NoTranslate]
		public string iconPath;

		// Token: 0x04002474 RID: 9332
		[Unsaved(false)]
		private Texture2D cachedIcon;
	}
}
