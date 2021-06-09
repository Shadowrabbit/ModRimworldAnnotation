using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001000 RID: 4096
	public class TrainableDef : Def
	{
		// Token: 0x17000DD4 RID: 3540
		// (get) Token: 0x0600595A RID: 22874 RVA: 0x0003E0EA File Offset: 0x0003C2EA
		public Texture2D Icon
		{
			get
			{
				if (this.iconTex == null)
				{
					this.iconTex = ContentFinder<Texture2D>.Get(this.icon, true);
				}
				return this.iconTex;
			}
		}

		// Token: 0x0600595B RID: 22875 RVA: 0x001D2240 File Offset: 0x001D0440
		public bool MatchesTag(string tag)
		{
			if (tag == this.defName)
			{
				return true;
			}
			for (int i = 0; i < this.tags.Count; i++)
			{
				if (this.tags[i] == tag)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600595C RID: 22876 RVA: 0x0003E112 File Offset: 0x0003C312
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string text in base.ConfigErrors())
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (this.difficulty < 0f)
			{
				yield return "difficulty not set";
			}
			yield break;
			yield break;
		}

		// Token: 0x04003BE8 RID: 15336
		public float difficulty = -1f;

		// Token: 0x04003BE9 RID: 15337
		public float minBodySize;

		// Token: 0x04003BEA RID: 15338
		public List<TrainableDef> prerequisites;

		// Token: 0x04003BEB RID: 15339
		[NoTranslate]
		public List<string> tags = new List<string>();

		// Token: 0x04003BEC RID: 15340
		public bool defaultTrainable;

		// Token: 0x04003BED RID: 15341
		public TrainabilityDef requiredTrainability;

		// Token: 0x04003BEE RID: 15342
		public int steps = 1;

		// Token: 0x04003BEF RID: 15343
		public float listPriority;

		// Token: 0x04003BF0 RID: 15344
		[NoTranslate]
		public string icon;

		// Token: 0x04003BF1 RID: 15345
		[Unsaved(false)]
		public int indent;

		// Token: 0x04003BF2 RID: 15346
		[Unsaved(false)]
		private Texture2D iconTex;
	}
}
