using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020000D9 RID: 217
	public class LetterDef : Def
	{
		// Token: 0x1700010E RID: 270
		// (get) Token: 0x06000624 RID: 1572 RVA: 0x0001ED35 File Offset: 0x0001CF35
		public Texture2D Icon
		{
			get
			{
				if (this.iconTex == null && !this.icon.NullOrEmpty())
				{
					this.iconTex = ContentFinder<Texture2D>.Get(this.icon, true);
				}
				return this.iconTex;
			}
		}

		// Token: 0x06000625 RID: 1573 RVA: 0x0001ED6A File Offset: 0x0001CF6A
		public override void ResolveReferences()
		{
			base.ResolveReferences();
			if (this.arriveSound == null)
			{
				this.arriveSound = SoundDefOf.LetterArrive;
			}
		}

		// Token: 0x040004BF RID: 1215
		public Type letterClass = typeof(StandardLetter);

		// Token: 0x040004C0 RID: 1216
		public Color color = Color.white;

		// Token: 0x040004C1 RID: 1217
		public Color flashColor = Color.white;

		// Token: 0x040004C2 RID: 1218
		public float flashInterval = 90f;

		// Token: 0x040004C3 RID: 1219
		public bool bounce;

		// Token: 0x040004C4 RID: 1220
		public SoundDef arriveSound;

		// Token: 0x040004C5 RID: 1221
		[NoTranslate]
		public string icon = "UI/Letters/LetterUnopened";

		// Token: 0x040004C6 RID: 1222
		public AutomaticPauseMode pauseMode = AutomaticPauseMode.AnyLetter;

		// Token: 0x040004C7 RID: 1223
		public bool forcedSlowdown;

		// Token: 0x040004C8 RID: 1224
		[Unsaved(false)]
		private Texture2D iconTex;
	}
}
