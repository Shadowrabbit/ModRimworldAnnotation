using System;
using System.Collections.Generic;
using System.Linq;

namespace Verse.Grammar
{
	// Token: 0x0200053A RID: 1338
	public class Rule_File : Rule
	{
		// Token: 0x170007EB RID: 2027
		// (get) Token: 0x06002855 RID: 10325 RVA: 0x000F6452 File Offset: 0x000F4652
		public override float BaseSelectionWeight
		{
			get
			{
				return (float)this.cachedStrings.Count;
			}
		}

		// Token: 0x06002856 RID: 10326 RVA: 0x000F6460 File Offset: 0x000F4660
		public override Rule DeepCopy()
		{
			Rule_File rule_File = (Rule_File)base.DeepCopy();
			rule_File.path = this.path;
			if (this.pathList != null)
			{
				rule_File.pathList = this.pathList.ToList<string>();
			}
			if (this.cachedStrings != null)
			{
				rule_File.cachedStrings = this.cachedStrings.ToList<string>();
			}
			return rule_File;
		}

		// Token: 0x06002857 RID: 10327 RVA: 0x000F64B8 File Offset: 0x000F46B8
		public override string Generate()
		{
			if (this.cachedStrings.NullOrEmpty<string>())
			{
				return "Filestring";
			}
			return this.cachedStrings.RandomElement<string>();
		}

		// Token: 0x06002858 RID: 10328 RVA: 0x000F64D8 File Offset: 0x000F46D8
		public override void Init()
		{
			if (!this.path.NullOrEmpty())
			{
				this.LoadStringsFromFile(this.path);
			}
			foreach (string filePath in this.pathList)
			{
				this.LoadStringsFromFile(filePath);
			}
		}

		// Token: 0x06002859 RID: 10329 RVA: 0x000F6544 File Offset: 0x000F4744
		private void LoadStringsFromFile(string filePath)
		{
			List<string> list;
			if (Translator.TryGetTranslatedStringsForFile(filePath, out list))
			{
				foreach (string item in list)
				{
					this.cachedStrings.Add(item);
				}
			}
		}

		// Token: 0x0600285A RID: 10330 RVA: 0x000F65A4 File Offset: 0x000F47A4
		public override string ToString()
		{
			if (!this.path.NullOrEmpty())
			{
				return string.Concat(new object[]
				{
					this.keyword,
					"->(",
					this.cachedStrings.Count,
					" strings from file: ",
					this.path,
					")"
				});
			}
			if (this.pathList.Count > 0)
			{
				return string.Concat(new object[]
				{
					this.keyword,
					"->(",
					this.cachedStrings.Count,
					" strings from ",
					this.pathList.Count,
					" files)"
				});
			}
			return this.keyword + "->(Rule_File with no configuration)";
		}

		// Token: 0x040018E7 RID: 6375
		[MayTranslate]
		public string path;

		// Token: 0x040018E8 RID: 6376
		[MayTranslate]
		[TranslationCanChangeCount]
		public List<string> pathList = new List<string>();

		// Token: 0x040018E9 RID: 6377
		[Unsaved(false)]
		private List<string> cachedStrings = new List<string>();
	}
}
