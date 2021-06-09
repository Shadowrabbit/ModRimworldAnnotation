using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000186 RID: 390
	public class SongDef : Def
	{
		// Token: 0x060009BC RID: 2492 RVA: 0x00099F80 File Offset: 0x00098180
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

		// Token: 0x060009BD RID: 2493 RVA: 0x0000D97F File Offset: 0x0000BB7F
		public override void ResolveReferences()
		{
			base.ResolveReferences();
			LongEventHandler.ExecuteWhenFinished(delegate
			{
				this.clip = ContentFinder<AudioClip>.Get(this.clipPath, true);
			});
		}

		// Token: 0x0400085B RID: 2139
		[NoTranslate]
		public string clipPath;

		// Token: 0x0400085C RID: 2140
		public float volume = 1f;

		// Token: 0x0400085D RID: 2141
		public bool playOnMap = true;

		// Token: 0x0400085E RID: 2142
		public float commonality = 1f;

		// Token: 0x0400085F RID: 2143
		public bool tense;

		// Token: 0x04000860 RID: 2144
		public TimeOfDay allowedTimeOfDay = TimeOfDay.Any;

		// Token: 0x04000861 RID: 2145
		public List<Season> allowedSeasons;

		// Token: 0x04000862 RID: 2146
		public RoyalTitleDef minRoyalTitle;

		// Token: 0x04000863 RID: 2147
		[Unsaved(false)]
		public AudioClip clip;
	}
}
