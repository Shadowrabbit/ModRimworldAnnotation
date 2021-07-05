using System;
using System.Linq;

namespace Verse
{
	// Token: 0x020002E5 RID: 741
	public class NameTriple : Name
	{
		// Token: 0x17000405 RID: 1029
		// (get) Token: 0x0600140A RID: 5130 RVA: 0x00071B87 File Offset: 0x0006FD87
		public string First
		{
			get
			{
				return this.firstInt;
			}
		}

		// Token: 0x17000406 RID: 1030
		// (get) Token: 0x0600140B RID: 5131 RVA: 0x00071B8F File Offset: 0x0006FD8F
		public string Nick
		{
			get
			{
				return this.nickInt;
			}
		}

		// Token: 0x17000407 RID: 1031
		// (get) Token: 0x0600140C RID: 5132 RVA: 0x00071B97 File Offset: 0x0006FD97
		public string Last
		{
			get
			{
				return this.lastInt;
			}
		}

		// Token: 0x17000408 RID: 1032
		// (get) Token: 0x0600140D RID: 5133 RVA: 0x00071BA0 File Offset: 0x0006FDA0
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

		// Token: 0x17000409 RID: 1033
		// (get) Token: 0x0600140E RID: 5134 RVA: 0x00071B8F File Offset: 0x0006FD8F
		public override string ToStringShort
		{
			get
			{
				return this.nickInt;
			}
		}

		// Token: 0x1700040A RID: 1034
		// (get) Token: 0x0600140F RID: 5135 RVA: 0x00071C20 File Offset: 0x0006FE20
		public override bool IsValid
		{
			get
			{
				return !this.First.NullOrEmpty() && !this.Last.NullOrEmpty();
			}
		}

		// Token: 0x1700040B RID: 1035
		// (get) Token: 0x06001410 RID: 5136 RVA: 0x0001276E File Offset: 0x0001096E
		public override bool Numerical
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700040C RID: 1036
		// (get) Token: 0x06001411 RID: 5137 RVA: 0x00071C3F File Offset: 0x0006FE3F
		public static NameTriple Invalid
		{
			get
			{
				return NameTriple.invalidInt;
			}
		}

		// Token: 0x06001412 RID: 5138 RVA: 0x00071AA9 File Offset: 0x0006FCA9
		public NameTriple()
		{
		}

		// Token: 0x06001413 RID: 5139 RVA: 0x00071C46 File Offset: 0x0006FE46
		public NameTriple(string first, string nick, string last)
		{
			this.firstInt = first.Trim();
			this.nickInt = nick.Trim();
			this.lastInt = last.Trim();
		}

		// Token: 0x06001414 RID: 5140 RVA: 0x00071C72 File Offset: 0x0006FE72
		public override void ExposeData()
		{
			Scribe_Values.Look<string>(ref this.firstInt, "first", null, false);
			Scribe_Values.Look<string>(ref this.nickInt, "nick", null, false);
			Scribe_Values.Look<string>(ref this.lastInt, "last", null, false);
		}

		// Token: 0x06001415 RID: 5141 RVA: 0x00071CAC File Offset: 0x0006FEAC
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

		// Token: 0x06001416 RID: 5142 RVA: 0x00071D04 File Offset: 0x0006FF04
		public void ResolveMissingPieces(string overrideLastName = null)
		{
			if (this.First.NullOrEmpty() && this.Nick.NullOrEmpty() && this.Last.NullOrEmpty())
			{
				Log.Error("Cannot resolve misssing pieces in PawnName: No name data.");
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

		// Token: 0x06001417 RID: 5143 RVA: 0x00071E00 File Offset: 0x00070000
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

		// Token: 0x06001418 RID: 5144 RVA: 0x00071E7C File Offset: 0x0007007C
		public static NameTriple FromString(string rawName)
		{
			if (rawName.Trim().Length == 0)
			{
				Log.Error("Tried to parse PawnName from empty or whitespace string.");
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

		// Token: 0x06001419 RID: 5145 RVA: 0x00072034 File Offset: 0x00070234
		public void CapitalizeNick()
		{
			if (!this.nickInt.NullOrEmpty())
			{
				this.nickInt = char.ToUpper(this.Nick[0]).ToString() + this.Nick.Substring(1);
			}
		}

		// Token: 0x0600141A RID: 5146 RVA: 0x0007207E File Offset: 0x0007027E
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

		// Token: 0x0600141B RID: 5147 RVA: 0x000720B8 File Offset: 0x000702B8
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

		// Token: 0x0600141C RID: 5148 RVA: 0x00072114 File Offset: 0x00070314
		public override int GetHashCode()
		{
			return Gen.HashCombine<string>(Gen.HashCombine<string>(Gen.HashCombine<string>(0, this.First), this.Last), this.Nick);
		}

		// Token: 0x04000E8E RID: 3726
		[LoadAlias("first")]
		private string firstInt;

		// Token: 0x04000E8F RID: 3727
		[LoadAlias("nick")]
		private string nickInt;

		// Token: 0x04000E90 RID: 3728
		[LoadAlias("last")]
		private string lastInt;

		// Token: 0x04000E91 RID: 3729
		private static NameTriple invalidInt = new NameTriple("Invalid", "Invalid", "Invalid");
	}
}
