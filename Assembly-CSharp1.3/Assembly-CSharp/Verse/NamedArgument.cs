using System;
using System.Text;
using RimWorld;
using RimWorld.Planet;

namespace Verse
{
	// Token: 0x0200014B RID: 331
	public struct NamedArgument
	{
		// Token: 0x06000938 RID: 2360 RVA: 0x0002E7F1 File Offset: 0x0002C9F1
		public NamedArgument(object arg, string label)
		{
			this.arg = arg;
			this.label = label;
		}

		// Token: 0x06000939 RID: 2361 RVA: 0x0002E801 File Offset: 0x0002CA01
		public static implicit operator NamedArgument(int value)
		{
			return new NamedArgument(value, null);
		}

		// Token: 0x0600093A RID: 2362 RVA: 0x0002E80F File Offset: 0x0002CA0F
		public static implicit operator NamedArgument(char value)
		{
			return new NamedArgument(value, null);
		}

		// Token: 0x0600093B RID: 2363 RVA: 0x0002E81D File Offset: 0x0002CA1D
		public static implicit operator NamedArgument(float value)
		{
			return new NamedArgument(value, null);
		}

		// Token: 0x0600093C RID: 2364 RVA: 0x0002E82B File Offset: 0x0002CA2B
		public static implicit operator NamedArgument(double value)
		{
			return new NamedArgument(value, null);
		}

		// Token: 0x0600093D RID: 2365 RVA: 0x0002E839 File Offset: 0x0002CA39
		public static implicit operator NamedArgument(long value)
		{
			return new NamedArgument(value, null);
		}

		// Token: 0x0600093E RID: 2366 RVA: 0x0002E847 File Offset: 0x0002CA47
		public static implicit operator NamedArgument(string value)
		{
			return new NamedArgument(value, null);
		}

		// Token: 0x0600093F RID: 2367 RVA: 0x0002E850 File Offset: 0x0002CA50
		public static implicit operator NamedArgument(uint value)
		{
			return new NamedArgument(value, null);
		}

		// Token: 0x06000940 RID: 2368 RVA: 0x0002E85E File Offset: 0x0002CA5E
		public static implicit operator NamedArgument(byte value)
		{
			return new NamedArgument(value, null);
		}

		// Token: 0x06000941 RID: 2369 RVA: 0x0002E86C File Offset: 0x0002CA6C
		public static implicit operator NamedArgument(ulong value)
		{
			return new NamedArgument(value, null);
		}

		// Token: 0x06000942 RID: 2370 RVA: 0x0002E847 File Offset: 0x0002CA47
		public static implicit operator NamedArgument(StringBuilder value)
		{
			return new NamedArgument(value, null);
		}

		// Token: 0x06000943 RID: 2371 RVA: 0x0002E847 File Offset: 0x0002CA47
		public static implicit operator NamedArgument(Thing value)
		{
			return new NamedArgument(value, null);
		}

		// Token: 0x06000944 RID: 2372 RVA: 0x0002E847 File Offset: 0x0002CA47
		public static implicit operator NamedArgument(Def value)
		{
			return new NamedArgument(value, null);
		}

		// Token: 0x06000945 RID: 2373 RVA: 0x0002E847 File Offset: 0x0002CA47
		public static implicit operator NamedArgument(WorldObject value)
		{
			return new NamedArgument(value, null);
		}

		// Token: 0x06000946 RID: 2374 RVA: 0x0002E847 File Offset: 0x0002CA47
		public static implicit operator NamedArgument(Faction value)
		{
			return new NamedArgument(value, null);
		}

		// Token: 0x06000947 RID: 2375 RVA: 0x0002E87A File Offset: 0x0002CA7A
		public static implicit operator NamedArgument(IntVec3 value)
		{
			return new NamedArgument(value, null);
		}

		// Token: 0x06000948 RID: 2376 RVA: 0x0002E888 File Offset: 0x0002CA88
		public static implicit operator NamedArgument(LocalTargetInfo value)
		{
			return new NamedArgument(value, null);
		}

		// Token: 0x06000949 RID: 2377 RVA: 0x0002E896 File Offset: 0x0002CA96
		public static implicit operator NamedArgument(TargetInfo value)
		{
			return new NamedArgument(value, null);
		}

		// Token: 0x0600094A RID: 2378 RVA: 0x0002E8A4 File Offset: 0x0002CAA4
		public static implicit operator NamedArgument(GlobalTargetInfo value)
		{
			return new NamedArgument(value, null);
		}

		// Token: 0x0600094B RID: 2379 RVA: 0x0002E847 File Offset: 0x0002CA47
		public static implicit operator NamedArgument(Map value)
		{
			return new NamedArgument(value, null);
		}

		// Token: 0x0600094C RID: 2380 RVA: 0x0002E8B2 File Offset: 0x0002CAB2
		public static implicit operator NamedArgument(TaggedString value)
		{
			return new NamedArgument(value, null);
		}

		// Token: 0x0600094D RID: 2381 RVA: 0x0002E847 File Offset: 0x0002CA47
		public static implicit operator NamedArgument(Ideo value)
		{
			return new NamedArgument(value, null);
		}

		// Token: 0x0600094E RID: 2382 RVA: 0x0002E8C0 File Offset: 0x0002CAC0
		public override string ToString()
		{
			return (this.label.NullOrEmpty() ? "unnamed" : this.label) + "->" + this.arg.ToStringSafe<object>();
		}

		// Token: 0x04000852 RID: 2130
		public object arg;

		// Token: 0x04000853 RID: 2131
		public string label;
	}
}
