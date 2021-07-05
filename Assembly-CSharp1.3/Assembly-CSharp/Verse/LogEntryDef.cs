using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020000DA RID: 218
	public class LogEntryDef : Def
	{
		// Token: 0x06000627 RID: 1575 RVA: 0x0001EDDE File Offset: 0x0001CFDE
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

		// Token: 0x040004C9 RID: 1225
		[NoTranslate]
		public string iconMiss;

		// Token: 0x040004CA RID: 1226
		[NoTranslate]
		public string iconDamaged;

		// Token: 0x040004CB RID: 1227
		[NoTranslate]
		public string iconDamagedFromInstigator;

		// Token: 0x040004CC RID: 1228
		[Unsaved(false)]
		public Texture2D iconMissTex;

		// Token: 0x040004CD RID: 1229
		[Unsaved(false)]
		public Texture2D iconDamagedTex;

		// Token: 0x040004CE RID: 1230
		[Unsaved(false)]
		public Texture2D iconDamagedFromInstigatorTex;
	}
}
