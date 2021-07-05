using System;

namespace Verse
{
	// Token: 0x020002E4 RID: 740
	public class NameSingle : Name
	{
		// Token: 0x170003FD RID: 1021
		// (get) Token: 0x060013FB RID: 5115 RVA: 0x00071978 File Offset: 0x0006FB78
		public string Name
		{
			get
			{
				return this.nameInt;
			}
		}

		// Token: 0x170003FE RID: 1022
		// (get) Token: 0x060013FC RID: 5116 RVA: 0x00071978 File Offset: 0x0006FB78
		public override string ToStringFull
		{
			get
			{
				return this.nameInt;
			}
		}

		// Token: 0x170003FF RID: 1023
		// (get) Token: 0x060013FD RID: 5117 RVA: 0x00071978 File Offset: 0x0006FB78
		public override string ToStringShort
		{
			get
			{
				return this.nameInt;
			}
		}

		// Token: 0x17000400 RID: 1024
		// (get) Token: 0x060013FE RID: 5118 RVA: 0x00071980 File Offset: 0x0006FB80
		public override bool IsValid
		{
			get
			{
				return !this.nameInt.NullOrEmpty();
			}
		}

		// Token: 0x17000401 RID: 1025
		// (get) Token: 0x060013FF RID: 5119 RVA: 0x00071990 File Offset: 0x0006FB90
		public override bool Numerical
		{
			get
			{
				return this.numerical;
			}
		}

		// Token: 0x17000402 RID: 1026
		// (get) Token: 0x06001400 RID: 5120 RVA: 0x00071998 File Offset: 0x0006FB98
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

		// Token: 0x17000403 RID: 1027
		// (get) Token: 0x06001401 RID: 5121 RVA: 0x00071A10 File Offset: 0x0006FC10
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

		// Token: 0x17000404 RID: 1028
		// (get) Token: 0x06001402 RID: 5122 RVA: 0x00071A74 File Offset: 0x0006FC74
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

		// Token: 0x06001403 RID: 5123 RVA: 0x00071AA9 File Offset: 0x0006FCA9
		public NameSingle()
		{
		}

		// Token: 0x06001404 RID: 5124 RVA: 0x00071AB1 File Offset: 0x0006FCB1
		public NameSingle(string name, bool numerical = false)
		{
			this.nameInt = name;
			this.numerical = numerical;
		}

		// Token: 0x06001405 RID: 5125 RVA: 0x00071AC7 File Offset: 0x0006FCC7
		public override void ExposeData()
		{
			Scribe_Values.Look<string>(ref this.nameInt, "name", null, false);
			Scribe_Values.Look<bool>(ref this.numerical, "numerical", false, false);
		}

		// Token: 0x06001406 RID: 5126 RVA: 0x00071AF0 File Offset: 0x0006FCF0
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

		// Token: 0x06001407 RID: 5127 RVA: 0x00071978 File Offset: 0x0006FB78
		public override string ToString()
		{
			return this.nameInt;
		}

		// Token: 0x06001408 RID: 5128 RVA: 0x00071B3C File Offset: 0x0006FD3C
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

		// Token: 0x06001409 RID: 5129 RVA: 0x00071B70 File Offset: 0x0006FD70
		public override int GetHashCode()
		{
			return Gen.HashCombineInt(this.nameInt.GetHashCode(), 1384661390);
		}

		// Token: 0x04000E8C RID: 3724
		private string nameInt;

		// Token: 0x04000E8D RID: 3725
		private bool numerical;
	}
}
