using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000490 RID: 1168
	public struct Pair<T1, T2> : IEquatable<Pair<T1, T2>>
	{
		// Token: 0x170006BE RID: 1726
		// (get) Token: 0x060023B6 RID: 9142 RVA: 0x000DE494 File Offset: 0x000DC694
		public T1 First
		{
			get
			{
				return this.first;
			}
		}

		// Token: 0x170006BF RID: 1727
		// (get) Token: 0x060023B7 RID: 9143 RVA: 0x000DE49C File Offset: 0x000DC69C
		public T2 Second
		{
			get
			{
				return this.second;
			}
		}

		// Token: 0x060023B8 RID: 9144 RVA: 0x000DE4A4 File Offset: 0x000DC6A4
		public Pair(T1 first, T2 second)
		{
			this.first = first;
			this.second = second;
		}

		// Token: 0x060023B9 RID: 9145 RVA: 0x000DE4B4 File Offset: 0x000DC6B4
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

		// Token: 0x060023BA RID: 9146 RVA: 0x000DE512 File Offset: 0x000DC712
		public override int GetHashCode()
		{
			return Gen.HashCombine<T2>(Gen.HashCombine<T1>(0, this.first), this.second);
		}

		// Token: 0x060023BB RID: 9147 RVA: 0x000DE52B File Offset: 0x000DC72B
		public override bool Equals(object other)
		{
			return other is Pair<T1, T2> && this.Equals((Pair<T1, T2>)other);
		}

		// Token: 0x060023BC RID: 9148 RVA: 0x000DE543 File Offset: 0x000DC743
		public bool Equals(Pair<T1, T2> other)
		{
			return EqualityComparer<T1>.Default.Equals(this.first, other.first) && EqualityComparer<T2>.Default.Equals(this.second, other.second);
		}

		// Token: 0x060023BD RID: 9149 RVA: 0x000DE575 File Offset: 0x000DC775
		public static bool operator ==(Pair<T1, T2> lhs, Pair<T1, T2> rhs)
		{
			return lhs.Equals(rhs);
		}

		// Token: 0x060023BE RID: 9150 RVA: 0x000DE57F File Offset: 0x000DC77F
		public static bool operator !=(Pair<T1, T2> lhs, Pair<T1, T2> rhs)
		{
			return !(lhs == rhs);
		}

		// Token: 0x04001617 RID: 5655
		private T1 first;

		// Token: 0x04001618 RID: 5656
		private T2 second;
	}
}
