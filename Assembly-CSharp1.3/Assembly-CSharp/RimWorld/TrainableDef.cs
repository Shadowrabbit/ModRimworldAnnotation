using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000ADE RID: 2782
	public class TrainableDef : Def
	{
		// Token: 0x17000B87 RID: 2951
		// (get) Token: 0x0600418D RID: 16781 RVA: 0x0015FCA1 File Offset: 0x0015DEA1
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

		// Token: 0x0600418E RID: 16782 RVA: 0x0015FCCC File Offset: 0x0015DECC
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

		// Token: 0x0600418F RID: 16783 RVA: 0x0015FD16 File Offset: 0x0015DF16
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

		// Token: 0x04002796 RID: 10134
		public float difficulty = -1f;

		// Token: 0x04002797 RID: 10135
		public float minBodySize;

		// Token: 0x04002798 RID: 10136
		public List<TrainableDef> prerequisites;

		// Token: 0x04002799 RID: 10137
		[NoTranslate]
		public List<string> tags = new List<string>();

		// Token: 0x0400279A RID: 10138
		public bool defaultTrainable;

		// Token: 0x0400279B RID: 10139
		public TrainabilityDef requiredTrainability;

		// Token: 0x0400279C RID: 10140
		public int steps = 1;

		// Token: 0x0400279D RID: 10141
		public float listPriority;

		// Token: 0x0400279E RID: 10142
		[NoTranslate]
		public string icon;

		// Token: 0x0400279F RID: 10143
		[Unsaved(false)]
		public int indent;

		// Token: 0x040027A0 RID: 10144
		[Unsaved(false)]
		private Texture2D iconTex;
	}
}
