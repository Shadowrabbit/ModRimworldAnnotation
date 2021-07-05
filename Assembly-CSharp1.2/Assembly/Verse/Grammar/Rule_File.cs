using System;
using System.Collections.Generic;
using System.Linq;

namespace Verse.Grammar
{
	// Token: 0x02000909 RID: 2313
	public class Rule_File : Rule
	{
		// Token: 0x17000921 RID: 2337
		// (get) Token: 0x06003972 RID: 14706 RVA: 0x0002C589 File Offset: 0x0002A789
		public override float BaseSelectionWeight
		{
			get
			{
				return (float)this.cachedStrings.Count;
			}
		}

		// Token: 0x06003973 RID: 14707 RVA: 0x00167800 File Offset: 0x00165A00
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

		// Token: 0x06003974 RID: 14708 RVA: 0x0002C597 File Offset: 0x0002A797
		public override string Generate()
		{
			if (this.cachedStrings.NullOrEmpty<string>())
			{
				return "Filestring";
			}
			return this.cachedStrings.RandomElement<string>();
		}

		// Token: 0x06003975 RID: 14709 RVA: 0x00167858 File Offset: 0x00165A58
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

		// Token: 0x06003976 RID: 14710 RVA: 0x001678C4 File Offset: 0x00165AC4
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

		// Token: 0x06003977 RID: 14711 RVA: 0x00167924 File Offset: 0x00165B24
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

		// Token: 0x040027CE RID: 10190
		[MayTranslate]
		public string path;

		// Token: 0x040027CF RID: 10191
		[MayTranslate]
		[TranslationCanChangeCount]
		public List<string> pathList = new List<string>();

		// Token: 0x040027D0 RID: 10192
		[Unsaved(false)]
		private List<string> cachedStrings = new List<string>();
	}
}
