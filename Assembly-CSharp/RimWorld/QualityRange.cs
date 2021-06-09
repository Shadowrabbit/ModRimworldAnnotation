using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200189A RID: 6298
	public struct QualityRange : IEquatable<QualityRange>
	{
		// Token: 0x170015FC RID: 5628
		// (get) Token: 0x06008BC5 RID: 35781 RVA: 0x0005DB8A File Offset: 0x0005BD8A
		public static QualityRange All
		{
			get
			{
				return new QualityRange(QualityCategory.Awful, QualityCategory.Legendary);
			}
		}

		// Token: 0x06008BC6 RID: 35782 RVA: 0x0005DB93 File Offset: 0x0005BD93
		public QualityRange(QualityCategory min, QualityCategory max)
		{
			this.min = min;
			this.max = max;
		}

		// Token: 0x06008BC7 RID: 35783 RVA: 0x0005DBA3 File Offset: 0x0005BDA3
		public bool Includes(QualityCategory p)
		{
			return p >= this.min && p <= this.max;
		}

		// Token: 0x06008BC8 RID: 35784 RVA: 0x0005DBBC File Offset: 0x0005BDBC
		public static bool operator ==(QualityRange a, QualityRange b)
		{
			return a.min == b.min && a.max == b.max;
		}

		// Token: 0x06008BC9 RID: 35785 RVA: 0x0005DBDC File Offset: 0x0005BDDC
		public static bool operator !=(QualityRange a, QualityRange b)
		{
			return !(a == b);
		}

		// Token: 0x06008BCA RID: 35786 RVA: 0x0028AF84 File Offset: 0x00289184
		public static QualityRange FromString(string s)
		{
			string[] array = s.Split(new char[]
			{
				'~'
			});
			return new QualityRange(ParseHelper.FromString<QualityCategory>(array[0]), ParseHelper.FromString<QualityCategory>(array[1]));
		}

		// Token: 0x06008BCB RID: 35787 RVA: 0x0005DBE8 File Offset: 0x0005BDE8
		public override string ToString()
		{
			return this.min.ToString() + "~" + this.max.ToString();
		}

		// Token: 0x06008BCC RID: 35788 RVA: 0x0005DC16 File Offset: 0x0005BE16
		public override int GetHashCode()
		{
			return Gen.HashCombineStruct<QualityCategory>(this.min.GetHashCode(), this.max);
		}

		// Token: 0x06008BCD RID: 35789 RVA: 0x0028AFB8 File Offset: 0x002891B8
		public override bool Equals(object obj)
		{
			if (!(obj is QualityRange))
			{
				return false;
			}
			QualityRange qualityRange = (QualityRange)obj;
			return qualityRange.min == this.min && qualityRange.max == this.max;
		}

		// Token: 0x06008BCE RID: 35790 RVA: 0x0005DC34 File Offset: 0x0005BE34
		public bool Equals(QualityRange other)
		{
			return other.min == this.min && other.max == this.max;
		}

		// Token: 0x0400598F RID: 22927
		public QualityCategory min;

		// Token: 0x04005990 RID: 22928
		public QualityCategory max;
	}
}
