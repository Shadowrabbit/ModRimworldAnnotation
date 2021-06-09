using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200014A RID: 330
	public class LetterDef : Def
	{
		// Token: 0x1700018F RID: 399
		// (get) Token: 0x06000891 RID: 2193 RVA: 0x0000CC57 File Offset: 0x0000AE57
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

		// Token: 0x06000892 RID: 2194 RVA: 0x0000CC8C File Offset: 0x0000AE8C
		public override void ResolveReferences()
		{
			base.ResolveReferences();
			if (this.arriveSound == null)
			{
				this.arriveSound = SoundDefOf.LetterArrive;
			}
		}

		// Token: 0x040006BB RID: 1723
		public Type letterClass = typeof(StandardLetter);

		// Token: 0x040006BC RID: 1724
		public Color color = Color.white;

		// Token: 0x040006BD RID: 1725
		public Color flashColor = Color.white;

		// Token: 0x040006BE RID: 1726
		public float flashInterval = 90f;

		// Token: 0x040006BF RID: 1727
		public bool bounce;

		// Token: 0x040006C0 RID: 1728
		public SoundDef arriveSound;

		// Token: 0x040006C1 RID: 1729
		[NoTranslate]
		public string icon = "UI/Letters/LetterUnopened";

		// Token: 0x040006C2 RID: 1730
		public AutomaticPauseMode pauseMode = AutomaticPauseMode.AnyLetter;

		// Token: 0x040006C3 RID: 1731
		public bool forcedSlowdown;

		// Token: 0x040006C4 RID: 1732
		[Unsaved(false)]
		private Texture2D iconTex;
	}
}
