using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000105 RID: 261
	public class SongDef : Def
	{
		// Token: 0x060006EE RID: 1774 RVA: 0x0002145C File Offset: 0x0001F65C
		public override void PostLoad()
		{
			base.PostLoad();
			if (this.defName == "UnnamedDef")
			{
				string[] array = this.clipPath.Split(new char[]
				{
					'/',
					'\\'
				});
				this.defName = array[array.Length - 1];
			}
		}

		// Token: 0x060006EF RID: 1775 RVA: 0x000214AA File Offset: 0x0001F6AA
		public override void ResolveReferences()
		{
			base.ResolveReferences();
			LongEventHandler.ExecuteWhenFinished(delegate
			{
				this.clip = ContentFinder<AudioClip>.Get(this.clipPath, true);
			});
		}

		// Token: 0x0400062F RID: 1583
		[NoTranslate]
		public string clipPath;

		// Token: 0x04000630 RID: 1584
		public float volume = 1f;

		// Token: 0x04000631 RID: 1585
		public bool playOnMap = true;

		// Token: 0x04000632 RID: 1586
		public float commonality = 1f;

		// Token: 0x04000633 RID: 1587
		public bool tense;

		// Token: 0x04000634 RID: 1588
		public TimeOfDay allowedTimeOfDay = TimeOfDay.Any;

		// Token: 0x04000635 RID: 1589
		public List<Season> allowedSeasons;

		// Token: 0x04000636 RID: 1590
		public RoyalTitleDef minRoyalTitle;

		// Token: 0x04000637 RID: 1591
		[Unsaved(false)]
		public AudioClip clip;
	}
}
