using System;

namespace Verse
{
	// Token: 0x02000420 RID: 1056
	public struct TaggedString
	{
		// Token: 0x17000617 RID: 1559
		// (get) Token: 0x06001FBB RID: 8123 RVA: 0x000C5091 File Offset: 0x000C3291
		public string RawText
		{
			get
			{
				return this.rawText;
			}
		}

		// Token: 0x17000618 RID: 1560
		public char this[int i]
		{
			get
			{
				return this.RawText[i];
			}
		}

		// Token: 0x17000619 RID: 1561
		// (get) Token: 0x06001FBD RID: 8125 RVA: 0x000C50A7 File Offset: 0x000C32A7
		public int Length
		{
			get
			{
				return this.RawText.Length;
			}
		}

		// Token: 0x1700061A RID: 1562
		// (get) Token: 0x06001FBE RID: 8126 RVA: 0x000C50B4 File Offset: 0x000C32B4
		public int StrippedLength
		{
			get
			{
				return this.RawText.StripTags().Length;
			}
		}

		// Token: 0x1700061B RID: 1563
		// (get) Token: 0x06001FBF RID: 8127 RVA: 0x000C50C6 File Offset: 0x000C32C6
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

		// Token: 0x06001FC0 RID: 8128 RVA: 0x000C50E8 File Offset: 0x000C32E8
		public TaggedString(string dat)
		{
			this.rawText = dat;
		}

		// Token: 0x06001FC1 RID: 8129 RVA: 0x000C50F1 File Offset: 0x000C32F1
		public string Resolve()
		{
			return ColoredText.Resolve(this);
		}

		// Token: 0x06001FC2 RID: 8130 RVA: 0x000C5100 File Offset: 0x000C3300
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

		// Token: 0x06001FC3 RID: 8131 RVA: 0x000C51D0 File Offset: 0x000C33D0
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

		// Token: 0x06001FC4 RID: 8132 RVA: 0x000C524C File Offset: 0x000C344C
		public bool NullOrEmpty()
		{
			return this.RawText.NullOrEmpty();
		}

		// Token: 0x06001FC5 RID: 8133 RVA: 0x000C5259 File Offset: 0x000C3459
		public TaggedString AdjustedFor(Pawn p, string pawnSymbol = "PAWN", bool addRelationInfoSymbol = true)
		{
			return this.RawText.AdjustedFor(p, pawnSymbol, addRelationInfoSymbol);
		}

		// Token: 0x06001FC6 RID: 8134 RVA: 0x000C526E File Offset: 0x000C346E
		public float GetWidthCached()
		{
			return this.RawText.StripTags().GetWidthCached();
		}

		// Token: 0x06001FC7 RID: 8135 RVA: 0x000C5280 File Offset: 0x000C3480
		public TaggedString Trim()
		{
			return new TaggedString(this.RawText.Trim());
		}

		// Token: 0x06001FC8 RID: 8136 RVA: 0x000C5292 File Offset: 0x000C3492
		public TaggedString Shorten()
		{
			this.rawText = this.rawText.Shorten();
			return this;
		}

		// Token: 0x06001FC9 RID: 8137 RVA: 0x000C52AB File Offset: 0x000C34AB
		public TaggedString ToLower()
		{
			return new TaggedString(this.RawText.ToLower());
		}

		// Token: 0x06001FCA RID: 8138 RVA: 0x000C52BD File Offset: 0x000C34BD
		public TaggedString Replace(string oldValue, string newValue)
		{
			return new TaggedString(this.RawText.Replace(oldValue, newValue));
		}

		// Token: 0x06001FCB RID: 8139 RVA: 0x000C52D1 File Offset: 0x000C34D1
		public static implicit operator string(TaggedString taggedString)
		{
			return taggedString.RawText.StripTags();
		}

		// Token: 0x06001FCC RID: 8140 RVA: 0x000C52DF File Offset: 0x000C34DF
		public static implicit operator TaggedString(string str)
		{
			return new TaggedString(str);
		}

		// Token: 0x06001FCD RID: 8141 RVA: 0x000C52E7 File Offset: 0x000C34E7
		public static TaggedString operator +(TaggedString t1, TaggedString t2)
		{
			return new TaggedString(t1.RawText + t2.RawText);
		}

		// Token: 0x06001FCE RID: 8142 RVA: 0x000C5301 File Offset: 0x000C3501
		public static TaggedString operator +(string t1, TaggedString t2)
		{
			return new TaggedString(t1 + t2.RawText);
		}

		// Token: 0x06001FCF RID: 8143 RVA: 0x000C5315 File Offset: 0x000C3515
		public static TaggedString operator +(TaggedString t1, string t2)
		{
			return new TaggedString(t1.RawText + t2);
		}

		// Token: 0x06001FD0 RID: 8144 RVA: 0x000C5329 File Offset: 0x000C3529
		public override string ToString()
		{
			return this.RawText;
		}

		// Token: 0x04001340 RID: 4928
		private string rawText;

		// Token: 0x04001341 RID: 4929
		private static TaggedString empty;
	}
}
