using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020011D1 RID: 4561
	public struct QualityRange : IEquatable<QualityRange>
	{
		// Token: 0x1700131F RID: 4895
		// (get) Token: 0x06006E0F RID: 28175 RVA: 0x0024E3D1 File Offset: 0x0024C5D1
		public static QualityRange All
		{
			get
			{
				return new QualityRange(QualityCategory.Awful, QualityCategory.Legendary);
			}
		}

		// Token: 0x06006E10 RID: 28176 RVA: 0x0024E3DA File Offset: 0x0024C5DA
		public QualityRange(QualityCategory min, QualityCategory max)
		{
			this.min = min;
			this.max = max;
		}

		// Token: 0x06006E11 RID: 28177 RVA: 0x0024E3EA File Offset: 0x0024C5EA
		public bool Includes(QualityCategory p)
		{
			return p >= this.min && p <= this.max;
		}

		// Token: 0x06006E12 RID: 28178 RVA: 0x0024E403 File Offset: 0x0024C603
		public static bool operator ==(QualityRange a, QualityRange b)
		{
			return a.min == b.min && a.max == b.max;
		}

		// Token: 0x06006E13 RID: 28179 RVA: 0x0024E423 File Offset: 0x0024C623
		public static bool operator !=(QualityRange a, QualityRange b)
		{
			return !(a == b);
		}

		// Token: 0x06006E14 RID: 28180 RVA: 0x0024E430 File Offset: 0x0024C630
		public static QualityRange FromString(string s)
		{
			string[] array = s.Split(new char[]
			{
				'~'
			});
			return new QualityRange(ParseHelper.FromString<QualityCategory>(array[0]), ParseHelper.FromString<QualityCategory>(array[1]));
		}

		// Token: 0x06006E15 RID: 28181 RVA: 0x0024E464 File Offset: 0x0024C664
		public override string ToString()
		{
			return this.min.ToString() + "~" + this.max.ToString();
		}

		// Token: 0x06006E16 RID: 28182 RVA: 0x0024E492 File Offset: 0x0024C692
		public override int GetHashCode()
		{
			return Gen.HashCombineStruct<QualityCategory>(this.min.GetHashCode(), this.max);
		}

		// Token: 0x06006E17 RID: 28183 RVA: 0x0024E4B0 File Offset: 0x0024C6B0
		public override bool Equals(object obj)
		{
			if (!(obj is QualityRange))
			{
				return false;
			}
			QualityRange qualityRange = (QualityRange)obj;
			return qualityRange.min == this.min && qualityRange.max == this.max;
		}

		// Token: 0x06006E18 RID: 28184 RVA: 0x0024E4EC File Offset: 0x0024C6EC
		public bool Equals(QualityRange other)
		{
			return other.min == this.min && other.max == this.max;
		}

		// Token: 0x04003D1A RID: 15642
		public QualityCategory min;

		// Token: 0x04003D1B RID: 15643
		public QualityCategory max;
	}
}
