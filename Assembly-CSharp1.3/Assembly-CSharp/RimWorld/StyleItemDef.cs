using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000ACF RID: 2767
	public abstract class StyleItemDef : Def
	{
		// Token: 0x17000B75 RID: 2933
		// (get) Token: 0x06004150 RID: 16720 RVA: 0x0015F331 File Offset: 0x0015D531
		public virtual Texture2D Icon
		{
			get
			{
				return ContentFinder<Texture2D>.Get(this.iconPath ?? (this.texPath + "_south"), true);
			}
		}

		// Token: 0x17000B76 RID: 2934
		// (get) Token: 0x06004151 RID: 16721 RVA: 0x0015F353 File Offset: 0x0015D553
		public StyleItemCategoryDef StyleItemCategory
		{
			get
			{
				if (this.category == null)
				{
					return StyleItemCategoryDefOf.Misc;
				}
				return this.category;
			}
		}

		// Token: 0x17000B77 RID: 2935
		// (get) Token: 0x06004152 RID: 16722 RVA: 0x0015F369 File Offset: 0x0015D569
		public static IEnumerable<StyleItemDef> AllStyleItemDefs
		{
			get
			{
				foreach (HairDef hairDef in DefDatabase<HairDef>.AllDefs)
				{
					yield return hairDef;
				}
				IEnumerator<HairDef> enumerator = null;
				foreach (BeardDef beardDef in DefDatabase<BeardDef>.AllDefs)
				{
					yield return beardDef;
				}
				IEnumerator<BeardDef> enumerator2 = null;
				foreach (TattooDef tattooDef in DefDatabase<TattooDef>.AllDefs)
				{
					yield return tattooDef;
				}
				IEnumerator<TattooDef> enumerator3 = null;
				yield break;
				yield break;
			}
		}

		// Token: 0x04002723 RID: 10019
		[LoadAlias("hairTags")]
		[NoTranslate]
		public List<string> styleTags = new List<string>();

		// Token: 0x04002724 RID: 10020
		[LoadAlias("hairGender")]
		public StyleGender styleGender = StyleGender.Any;

		// Token: 0x04002725 RID: 10021
		private StyleItemCategoryDef category;

		// Token: 0x04002726 RID: 10022
		[NoTranslate]
		public string texPath;

		// Token: 0x04002727 RID: 10023
		[NoTranslate]
		public string iconPath;
	}
}
