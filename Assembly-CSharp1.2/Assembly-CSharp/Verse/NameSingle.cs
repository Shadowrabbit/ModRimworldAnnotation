using System;

namespace Verse
{
	// Token: 0x02000433 RID: 1075
	public class NameSingle : Name
	{
		// Token: 0x170004D0 RID: 1232
		// (get) Token: 0x06001A02 RID: 6658 RVA: 0x00018279 File Offset: 0x00016479
		public string Name
		{
			get
			{
				return this.nameInt;
			}
		}

		// Token: 0x170004D1 RID: 1233
		// (get) Token: 0x06001A03 RID: 6659 RVA: 0x00018279 File Offset: 0x00016479
		public override string ToStringFull
		{
			get
			{
				return this.nameInt;
			}
		}

		// Token: 0x170004D2 RID: 1234
		// (get) Token: 0x06001A04 RID: 6660 RVA: 0x00018279 File Offset: 0x00016479
		public override string ToStringShort
		{
			get
			{
				return this.nameInt;
			}
		}

		// Token: 0x170004D3 RID: 1235
		// (get) Token: 0x06001A05 RID: 6661 RVA: 0x00018281 File Offset: 0x00016481
		public override bool IsValid
		{
			get
			{
				return !this.nameInt.NullOrEmpty();
			}
		}

		// Token: 0x170004D4 RID: 1236
		// (get) Token: 0x06001A06 RID: 6662 RVA: 0x00018291 File Offset: 0x00016491
		public override bool Numerical
		{
			get
			{
				return this.numerical;
			}
		}

		// Token: 0x170004D5 RID: 1237
		// (get) Token: 0x06001A07 RID: 6663 RVA: 0x000E4698 File Offset: 0x000E2898
		private int FirstDigitPosition
		{
			get
			{
				if (!this.numerical)
				{
					return -1;
				}
				if (this.nameInt.NullOrEmpty() || !char.IsDigit(this.nameInt[this.nameInt.Length - 1]))
				{
					return -1;
				}
				for (int i = this.nameInt.Length - 2; i >= 0; i--)
				{
					if (!char.IsDigit(this.nameInt[i]))
					{
						return i + 1;
					}
				}
				return 0;
			}
		}

		// Token: 0x170004D6 RID: 1238
		// (get) Token: 0x06001A08 RID: 6664 RVA: 0x000E4710 File Offset: 0x000E2910
		public string NameWithoutNumber
		{
			get
			{
				if (!this.numerical)
				{
					return this.nameInt;
				}
				int firstDigitPosition = this.FirstDigitPosition;
				if (firstDigitPosition < 0)
				{
					return this.nameInt;
				}
				int num = firstDigitPosition;
				if (num - 1 >= 0 && this.nameInt[num - 1] == ' ')
				{
					num--;
				}
				if (num <= 0)
				{
					return "";
				}
				return this.nameInt.Substring(0, num);
			}
		}

		// Token: 0x170004D7 RID: 1239
		// (get) Token: 0x06001A09 RID: 6665 RVA: 0x000E4774 File Offset: 0x000E2974
		public int Number
		{
			get
			{
				if (!this.numerical)
				{
					return 0;
				}
				int firstDigitPosition = this.FirstDigitPosition;
				if (firstDigitPosition < 0)
				{
					return 0;
				}
				return int.Parse(this.nameInt.Substring(firstDigitPosition));
			}
		}

		// Token: 0x06001A0A RID: 6666 RVA: 0x00018299 File Offset: 0x00016499
		public NameSingle()
		{
		}

		// Token: 0x06001A0B RID: 6667 RVA: 0x000182A1 File Offset: 0x000164A1
		public NameSingle(string name, bool numerical = false)
		{
			this.nameInt = name;
			this.numerical = numerical;
		}

		// Token: 0x06001A0C RID: 6668 RVA: 0x000182B7 File Offset: 0x000164B7
		public override void ExposeData()
		{
			Scribe_Values.Look<string>(ref this.nameInt, "name", null, false);
			Scribe_Values.Look<bool>(ref this.numerical, "numerical", false, false);
		}

		// Token: 0x06001A0D RID: 6669 RVA: 0x000E47AC File Offset: 0x000E29AC
		public override bool ConfusinglySimilarTo(Name other)
		{
			NameSingle nameSingle = other as NameSingle;
			if (nameSingle != null && nameSingle.nameInt == this.nameInt)
			{
				return true;
			}
			NameTriple nameTriple = other as NameTriple;
			return nameTriple != null && nameTriple.Nick == this.nameInt;
		}

		// Token: 0x06001A0E RID: 6670 RVA: 0x00018279 File Offset: 0x00016479
		public override string ToString()
		{
			return this.nameInt;
		}

		// Token: 0x06001A0F RID: 6671 RVA: 0x000E47F8 File Offset: 0x000E29F8
		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				return false;
			}
			if (!(obj is NameSingle))
			{
				return false;
			}
			NameSingle nameSingle = (NameSingle)obj;
			return this.nameInt == nameSingle.nameInt;
		}

		// Token: 0x06001A10 RID: 6672 RVA: 0x000182DD File Offset: 0x000164DD
		public override int GetHashCode()
		{
			return Gen.HashCombineInt(this.nameInt.GetHashCode(), 1384661390);
		}

		// Token: 0x04001351 RID: 4945
		private string nameInt;

		// Token: 0x04001352 RID: 4946
		private bool numerical;
	}
}
