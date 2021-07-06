using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x020007F3 RID: 2035
	public struct Pair<T1, T2> : IEquatable<Pair<T1, T2>>
	{
		// Token: 0x170007D6 RID: 2006
		// (get) Token: 0x0600338F RID: 13199 RVA: 0x000287C3 File Offset: 0x000269C3
		public T1 First
		{
			get
			{
				return this.first;
			}
		}

		// Token: 0x170007D7 RID: 2007
		// (get) Token: 0x06003390 RID: 13200 RVA: 0x000287CB File Offset: 0x000269CB
		public T2 Second
		{
			get
			{
				return this.second;
			}
		}

		// Token: 0x06003391 RID: 13201 RVA: 0x000287D3 File Offset: 0x000269D3
		public Pair(T1 first, T2 second)
		{
			this.first = first;
			this.second = second;
		}

		// Token: 0x06003392 RID: 13202 RVA: 0x0014FBB0 File Offset: 0x0014DDB0
		public override string ToString()
		{
			string[] array = new string[5];
			array[0] = "(";
			int num = 1;
			T1 t = this.First;
			array[num] = t.ToString();
			array[2] = ", ";
			int num2 = 3;
			T2 t2 = this.Second;
			array[num2] = t2.ToString();
			array[4] = ")";
			return string.Concat(array);
		}

		// Token: 0x06003393 RID: 13203 RVA: 0x000287E3 File Offset: 0x000269E3
		public override int GetHashCode()
		{
			return Gen.HashCombine<T2>(Gen.HashCombine<T1>(0, this.first), this.second);
		}

		// Token: 0x06003394 RID: 13204 RVA: 0x000287FC File Offset: 0x000269FC
		public override bool Equals(object other)
		{
			return other is Pair<T1, T2> && this.Equals((Pair<T1, T2>)other);
		}

		// Token: 0x06003395 RID: 13205 RVA: 0x00028814 File Offset: 0x00026A14
		public bool Equals(Pair<T1, T2> other)
		{
			return EqualityComparer<T1>.Default.Equals(this.first, other.first) && EqualityComparer<T2>.Default.Equals(this.second, other.second);
		}

		// Token: 0x06003396 RID: 13206 RVA: 0x00028846 File Offset: 0x00026A46
		public static bool operator ==(Pair<T1, T2> lhs, Pair<T1, T2> rhs)
		{
			return lhs.Equals(rhs);
		}

		// Token: 0x06003397 RID: 13207 RVA: 0x00028850 File Offset: 0x00026A50
		public static bool operator !=(Pair<T1, T2> lhs, Pair<T1, T2> rhs)
		{
			return !(lhs == rhs);
		}

		// Token: 0x0400234B RID: 9035
		private T1 first;

		// Token: 0x0400234C RID: 9036
		private T2 second;
	}
}
