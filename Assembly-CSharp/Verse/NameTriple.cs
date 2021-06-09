using System;
using System.Linq;

namespace Verse
{
	// Token: 0x02000434 RID: 1076
	public class NameTriple : Name
	{
		// Token: 0x170004D8 RID: 1240
		// (get) Token: 0x06001A11 RID: 6673 RVA: 0x000182F4 File Offset: 0x000164F4
		public string First
		{
			get
			{
				return this.firstInt;
			}
		}

		// Token: 0x170004D9 RID: 1241
		// (get) Token: 0x06001A12 RID: 6674 RVA: 0x000182FC File Offset: 0x000164FC
		public string Nick
		{
			get
			{
				return this.nickInt;
			}
		}

		// Token: 0x170004DA RID: 1242
		// (get) Token: 0x06001A13 RID: 6675 RVA: 0x00018304 File Offset: 0x00016504
		public string Last
		{
			get
			{
				return this.lastInt;
			}
		}

		// Token: 0x170004DB RID: 1243
		// (get) Token: 0x06001A14 RID: 6676 RVA: 0x000E482C File Offset: 0x000E2A2C
		public override string ToStringFull
		{
			get
			{
				if (this.First == this.Nick || this.Last == this.Nick)
				{
					return this.First + " " + this.Last;
				}
				return string.Concat(new string[]
				{
					this.First,
					" '",
					this.Nick,
					"' ",
					this.Last
				});
			}
		}

		// Token: 0x170004DC RID: 1244
		// (get) Token: 0x06001A15 RID: 6677 RVA: 0x000182FC File Offset: 0x000164FC
		public override string ToStringShort
		{
			get
			{
				return this.nickInt;
			}
		}

		// Token: 0x170004DD RID: 1245
		// (get) Token: 0x06001A16 RID: 6678 RVA: 0x0001830C File Offset: 0x0001650C
		public override bool IsValid
		{
			get
			{
				return !this.First.NullOrEmpty() && !this.Last.NullOrEmpty();
			}
		}

		// Token: 0x170004DE RID: 1246
		// (get) Token: 0x06001A17 RID: 6679 RVA: 0x0000A2E4 File Offset: 0x000084E4
		public override bool Numerical
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170004DF RID: 1247
		// (get) Token: 0x06001A18 RID: 6680 RVA: 0x0001832B File Offset: 0x0001652B
		public static NameTriple Invalid
		{
			get
			{
				return NameTriple.invalidInt;
			}
		}

		// Token: 0x06001A19 RID: 6681 RVA: 0x00018299 File Offset: 0x00016499
		public NameTriple()
		{
		}

		// Token: 0x06001A1A RID: 6682 RVA: 0x00018332 File Offset: 0x00016532
		public NameTriple(string first, string nick, string last)
		{
			this.firstInt = first.Trim();
			this.nickInt = nick.Trim();
			this.lastInt = last.Trim();
		}

		// Token: 0x06001A1B RID: 6683 RVA: 0x0001835E File Offset: 0x0001655E
		public override void ExposeData()
		{
			Scribe_Values.Look<string>(ref this.firstInt, "first", null, false);
			Scribe_Values.Look<string>(ref this.nickInt, "nick", null, false);
			Scribe_Values.Look<string>(ref this.lastInt, "last", null, false);
		}

		// Token: 0x06001A1C RID: 6684 RVA: 0x000E48AC File Offset: 0x000E2AAC
		public void PostLoad()
		{
			if (this.firstInt != null)
			{
				this.firstInt = this.firstInt.Trim();
			}
			if (this.nickInt != null)
			{
				this.nickInt = this.nickInt.Trim();
			}
			if (this.lastInt != null)
			{
				this.lastInt = this.lastInt.Trim();
			}
		}

		// Token: 0x06001A1D RID: 6685 RVA: 0x000E4904 File Offset: 0x000E2B04
		public void ResolveMissingPieces(string overrideLastName = null)
		{
			if (this.First.NullOrEmpty() && this.Nick.NullOrEmpty() && this.Last.NullOrEmpty())
			{
				Log.Error("Cannot resolve misssing pieces in PawnName: No name data.", false);
				this.firstInt = (this.nickInt = (this.lastInt = "Empty"));
				return;
			}
			if (this.First == null)
			{
				this.firstInt = "";
			}
			if (this.Last == null)
			{
				this.lastInt = "";
			}
			if (overrideLastName != null)
			{
				this.lastInt = overrideLastName;
			}
			if (this.Nick.NullOrEmpty())
			{
				if (this.Last == "")
				{
					this.nickInt = this.First;
					return;
				}
				if (Rand.ValueSeeded(Gen.HashCombine<string>(this.First.GetHashCode(), this.Last)) < 0.5f)
				{
					this.nickInt = this.First;
				}
				else
				{
					this.nickInt = this.Last;
				}
				this.CapitalizeNick();
			}
		}

		// Token: 0x06001A1E RID: 6686 RVA: 0x000E4A00 File Offset: 0x000E2C00
		public override bool ConfusinglySimilarTo(Name other)
		{
			NameTriple nameTriple = other as NameTriple;
			if (nameTriple != null)
			{
				if (this.Nick != null && this.Nick == nameTriple.Nick)
				{
					return true;
				}
				if (this.First == nameTriple.First && this.Last == nameTriple.Last)
				{
					return true;
				}
			}
			NameSingle nameSingle = other as NameSingle;
			return nameSingle != null && nameSingle.Name == this.Nick;
		}

		// Token: 0x06001A1F RID: 6687 RVA: 0x000E4A7C File Offset: 0x000E2C7C
		public static NameTriple FromString(string rawName)
		{
			if (rawName.Trim().Length == 0)
			{
				Log.Error("Tried to parse PawnName from empty or whitespace string.", false);
				return NameTriple.Invalid;
			}
			NameTriple nameTriple = new NameTriple();
			int num = -1;
			int num2 = -1;
			for (int i = 0; i < rawName.Length - 1; i++)
			{
				if (rawName[i] == ' ' && rawName[i + 1] == '\'' && num == -1)
				{
					num = i;
				}
				if (rawName[i] == '\'' && rawName[i + 1] == ' ')
				{
					num2 = i;
				}
			}
			if (num == -1 || num2 == -1)
			{
				if (!rawName.Contains(' '))
				{
					nameTriple.nickInt = rawName.Trim();
				}
				else
				{
					string[] array = rawName.Split(new char[]
					{
						' '
					});
					if (array.Length == 1)
					{
						nameTriple.nickInt = array[0].Trim();
					}
					else if (array.Length == 2)
					{
						nameTriple.firstInt = array[0].Trim();
						nameTriple.lastInt = array[1].Trim();
					}
					else
					{
						nameTriple.firstInt = array[0].Trim();
						nameTriple.lastInt = "";
						for (int j = 1; j < array.Length; j++)
						{
							NameTriple nameTriple2 = nameTriple;
							nameTriple2.lastInt += array[j];
							if (j < array.Length - 1)
							{
								NameTriple nameTriple3 = nameTriple;
								nameTriple3.lastInt += " ";
							}
						}
					}
				}
			}
			else
			{
				nameTriple.firstInt = rawName.Substring(0, num).Trim();
				nameTriple.nickInt = rawName.Substring(num + 2, num2 - num - 2).Trim();
				nameTriple.lastInt = ((num2 < rawName.Length - 2) ? rawName.Substring(num2 + 2).Trim() : "");
			}
			return nameTriple;
		}

		// Token: 0x06001A20 RID: 6688 RVA: 0x000E4C34 File Offset: 0x000E2E34
		public void CapitalizeNick()
		{
			if (!this.nickInt.NullOrEmpty())
			{
				this.nickInt = char.ToUpper(this.Nick[0]).ToString() + this.Nick.Substring(1);
			}
		}

		// Token: 0x06001A21 RID: 6689 RVA: 0x00018396 File Offset: 0x00016596
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				this.First,
				" '",
				this.Nick,
				"' ",
				this.Last
			});
		}

		// Token: 0x06001A22 RID: 6690 RVA: 0x000E4C80 File Offset: 0x000E2E80
		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				return false;
			}
			if (!(obj is NameTriple))
			{
				return false;
			}
			NameTriple nameTriple = (NameTriple)obj;
			return this.First == nameTriple.First && this.Last == nameTriple.Last && this.Nick == nameTriple.Nick;
		}

		// Token: 0x06001A23 RID: 6691 RVA: 0x000183CE File Offset: 0x000165CE
		public override int GetHashCode()
		{
			return Gen.HashCombine<string>(Gen.HashCombine<string>(Gen.HashCombine<string>(0, this.First), this.Last), this.Nick);
		}

		// Token: 0x04001353 RID: 4947
		[LoadAlias("first")]
		private string firstInt;

		// Token: 0x04001354 RID: 4948
		[LoadAlias("nick")]
		private string nickInt;

		// Token: 0x04001355 RID: 4949
		[LoadAlias("last")]
		private string lastInt;

		// Token: 0x04001356 RID: 4950
		private static NameTriple invalidInt = new NameTriple("Invalid", "Invalid", "Invalid");
	}
}
