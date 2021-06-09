using System;

namespace Verse
{
	// Token: 0x02000756 RID: 1878
	public struct TaggedString
	{
		// Token: 0x1700072F RID: 1839
		// (get) Token: 0x06002F53 RID: 12115 RVA: 0x00025143 File Offset: 0x00023343
		public string RawText
		{
			get
			{
				return this.rawText;
			}
		}

		// Token: 0x17000730 RID: 1840
		public char this[int i]
		{
			get
			{
				return this.RawText[i];
			}
		}

		// Token: 0x17000731 RID: 1841
		// (get) Token: 0x06002F55 RID: 12117 RVA: 0x00025159 File Offset: 0x00023359
		public int Length
		{
			get
			{
				return this.RawText.Length;
			}
		}

		// Token: 0x17000732 RID: 1842
		// (get) Token: 0x06002F56 RID: 12118 RVA: 0x00025166 File Offset: 0x00023366
		public int StrippedLength
		{
			get
			{
				return this.RawText.StripTags().Length;
			}
		}

		// Token: 0x17000733 RID: 1843
		// (get) Token: 0x06002F57 RID: 12119 RVA: 0x00025178 File Offset: 0x00023378
		public static TaggedString Empty
		{
			get
			{
				if (TaggedString.empty == null)
				{
					TaggedString.empty = new TaggedString("");
				}
				return TaggedString.empty;
			}
		}

		// Token: 0x06002F58 RID: 12120 RVA: 0x0002519A File Offset: 0x0002339A
		public TaggedString(string dat)
		{
			this.rawText = dat;
		}

		// Token: 0x06002F59 RID: 12121 RVA: 0x000251A3 File Offset: 0x000233A3
		public string Resolve()
		{
			return ColoredText.Resolve(this);
		}

		// Token: 0x06002F5A RID: 12122 RVA: 0x0013AE28 File Offset: 0x00139028
		public TaggedString CapitalizeFirst()
		{
			if (this.rawText.NullOrEmpty() || char.IsUpper(this.rawText[0]))
			{
				return this;
			}
			if (this.rawText.Length == 1)
			{
				return new TaggedString(this.rawText.ToUpper());
			}
			int num = this.FirstLetterBetweenTags();
			if (num == 0)
			{
				return new TaggedString(char.ToUpper(this.rawText[num]).ToString() + this.rawText.Substring(num + 1));
			}
			return new TaggedString(this.rawText.Substring(0, num) + char.ToUpper(this.rawText[num]).ToString() + this.rawText.Substring(num + 1));
		}

		// Token: 0x06002F5B RID: 12123 RVA: 0x0013AEF8 File Offset: 0x001390F8
		private int FirstLetterBetweenTags()
		{
			bool flag = false;
			for (int i = 0; i < this.rawText.Length - 1; i++)
			{
				if (this.rawText[i] == '(' && this.rawText[i + 1] == '*')
				{
					flag = true;
				}
				else
				{
					if (flag && this.rawText[i] == ')' && this.rawText[i + 1] != '(')
					{
						return i + 1;
					}
					if (!flag)
					{
						return i;
					}
				}
			}
			return 0;
		}

		// Token: 0x06002F5C RID: 12124 RVA: 0x000251B0 File Offset: 0x000233B0
		public bool NullOrEmpty()
		{
			return this.RawText.NullOrEmpty();
		}

		// Token: 0x06002F5D RID: 12125 RVA: 0x000251BD File Offset: 0x000233BD
		public TaggedString AdjustedFor(Pawn p, string pawnSymbol = "PAWN", bool addRelationInfoSymbol = true)
		{
			return this.RawText.AdjustedFor(p, pawnSymbol, addRelationInfoSymbol);
		}

		// Token: 0x06002F5E RID: 12126 RVA: 0x000251D2 File Offset: 0x000233D2
		public float GetWidthCached()
		{
			return this.RawText.StripTags().GetWidthCached();
		}

		// Token: 0x06002F5F RID: 12127 RVA: 0x000251E4 File Offset: 0x000233E4
		public TaggedString Trim()
		{
			return new TaggedString(this.RawText.Trim());
		}

		// Token: 0x06002F60 RID: 12128 RVA: 0x000251F6 File Offset: 0x000233F6
		public TaggedString Shorten()
		{
			this.rawText = this.rawText.Shorten();
			return this;
		}

		// Token: 0x06002F61 RID: 12129 RVA: 0x0002520F File Offset: 0x0002340F
		public TaggedString ToLower()
		{
			return new TaggedString(this.RawText.ToLower());
		}

		// Token: 0x06002F62 RID: 12130 RVA: 0x00025221 File Offset: 0x00023421
		public TaggedString Replace(string oldValue, string newValue)
		{
			return new TaggedString(this.RawText.Replace(oldValue, newValue));
		}

		// Token: 0x06002F63 RID: 12131 RVA: 0x00025235 File Offset: 0x00023435
		public static implicit operator string(TaggedString taggedString)
		{
			return taggedString.RawText.StripTags();
		}

		// Token: 0x06002F64 RID: 12132 RVA: 0x00025243 File Offset: 0x00023443
		public static implicit operator TaggedString(string str)
		{
			return new TaggedString(str);
		}

		// Token: 0x06002F65 RID: 12133 RVA: 0x0002524B File Offset: 0x0002344B
		public static TaggedString operator +(TaggedString t1, TaggedString t2)
		{
			return new TaggedString(t1.RawText + t2.RawText);
		}

		// Token: 0x06002F66 RID: 12134 RVA: 0x00025265 File Offset: 0x00023465
		public static TaggedString operator +(string t1, TaggedString t2)
		{
			return new TaggedString(t1 + t2.RawText);
		}

		// Token: 0x06002F67 RID: 12135 RVA: 0x00025279 File Offset: 0x00023479
		public static TaggedString operator +(TaggedString t1, string t2)
		{
			return new TaggedString(t1.RawText + t2);
		}

		// Token: 0x06002F68 RID: 12136 RVA: 0x0002528D File Offset: 0x0002348D
		public override string ToString()
		{
			return this.RawText;
		}

		// Token: 0x0400201A RID: 8218
		private string rawText;

		// Token: 0x0400201B RID: 8219
		private static TaggedString empty;
	}
}
