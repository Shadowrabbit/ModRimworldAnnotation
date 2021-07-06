using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200014B RID: 331
	public class LogEntryDef : Def
	{
		// Token: 0x06000894 RID: 2196 RVA: 0x0000CCA7 File Offset: 0x0000AEA7
		public override void PostLoad()
		{
			base.PostLoad();
			LongEventHandler.ExecuteWhenFinished(delegate
			{
				if (!this.iconMiss.NullOrEmpty())
				{
					this.iconMissTex = ContentFinder<Texture2D>.Get(this.iconMiss, true);
				}
				if (!this.iconDamaged.NullOrEmpty())
				{
					this.iconDamagedTex = ContentFinder<Texture2D>.Get(this.iconDamaged, true);
				}
				if (!this.iconDamagedFromInstigator.NullOrEmpty())
				{
					this.iconDamagedFromInstigatorTex = ContentFinder<Texture2D>.Get(this.iconDamagedFromInstigator, true);
				}
			});
		}

		// Token: 0x040006C5 RID: 1733
		[NoTranslate]
		public string iconMiss;

		// Token: 0x040006C6 RID: 1734
		[NoTranslate]
		public string iconDamaged;

		// Token: 0x040006C7 RID: 1735
		[NoTranslate]
		public string iconDamagedFromInstigator;

		// Token: 0x040006C8 RID: 1736
		[Unsaved(false)]
		public Texture2D iconMissTex;

		// Token: 0x040006C9 RID: 1737
		[Unsaved(false)]
		public Texture2D iconDamagedTex;

		// Token: 0x040006CA RID: 1738
		[Unsaved(false)]
		public Texture2D iconDamagedFromInstigatorTex;
	}
}
