using System;
using System.Text;
using RimWorld;
using RimWorld.Planet;

namespace Verse
{
	// Token: 0x020001F6 RID: 502
	public struct NamedArgument
	{
		// Token: 0x06000D0B RID: 3339 RVA: 0x0000FEC7 File Offset: 0x0000E0C7
		public NamedArgument(object arg, string label)
		{
			this.arg = arg;
			this.label = label;
		}

		// Token: 0x06000D0C RID: 3340 RVA: 0x0000FED7 File Offset: 0x0000E0D7
		public static implicit operator NamedArgument(int value)
		{
			return new NamedArgument(value, null);
		}

		// Token: 0x06000D0D RID: 3341 RVA: 0x0000FEE5 File Offset: 0x0000E0E5
		public static implicit operator NamedArgument(char value)
		{
			return new NamedArgument(value, null);
		}

		// Token: 0x06000D0E RID: 3342 RVA: 0x0000FEF3 File Offset: 0x0000E0F3
		public static implicit operator NamedArgument(float value)
		{
			return new NamedArgument(value, null);
		}

		// Token: 0x06000D0F RID: 3343 RVA: 0x0000FF01 File Offset: 0x0000E101
		public static implicit operator NamedArgument(double value)
		{
			return new NamedArgument(value, null);
		}

		// Token: 0x06000D10 RID: 3344 RVA: 0x0000FF0F File Offset: 0x0000E10F
		public static implicit operator NamedArgument(long value)
		{
			return new NamedArgument(value, null);
		}

		// Token: 0x06000D11 RID: 3345 RVA: 0x0000FF1D File Offset: 0x0000E11D
		public static implicit operator NamedArgument(string value)
		{
			return new NamedArgument(value, null);
		}

		// Token: 0x06000D12 RID: 3346 RVA: 0x0000FF26 File Offset: 0x0000E126
		public static implicit operator NamedArgument(uint value)
		{
			return new NamedArgument(value, null);
		}

		// Token: 0x06000D13 RID: 3347 RVA: 0x0000FF34 File Offset: 0x0000E134
		public static implicit operator NamedArgument(byte value)
		{
			return new NamedArgument(value, null);
		}

		// Token: 0x06000D14 RID: 3348 RVA: 0x0000FF42 File Offset: 0x0000E142
		public static implicit operator NamedArgument(ulong value)
		{
			return new NamedArgument(value, null);
		}

		// Token: 0x06000D15 RID: 3349 RVA: 0x0000FF1D File Offset: 0x0000E11D
		public static implicit operator NamedArgument(StringBuilder value)
		{
			return new NamedArgument(value, null);
		}

		// Token: 0x06000D16 RID: 3350 RVA: 0x0000FF1D File Offset: 0x0000E11D
		public static implicit operator NamedArgument(Thing value)
		{
			return new NamedArgument(value, null);
		}

		// Token: 0x06000D17 RID: 3351 RVA: 0x0000FF1D File Offset: 0x0000E11D
		public static implicit operator NamedArgument(Def value)
		{
			return new NamedArgument(value, null);
		}

		// Token: 0x06000D18 RID: 3352 RVA: 0x0000FF1D File Offset: 0x0000E11D
		public static implicit operator NamedArgument(WorldObject value)
		{
			return new NamedArgument(value, null);
		}

		// Token: 0x06000D19 RID: 3353 RVA: 0x0000FF1D File Offset: 0x0000E11D
		public static implicit operator NamedArgument(Faction value)
		{
			return new NamedArgument(value, null);
		}

		// Token: 0x06000D1A RID: 3354 RVA: 0x0000FF50 File Offset: 0x0000E150
		public static implicit operator NamedArgument(IntVec3 value)
		{
			return new NamedArgument(value, null);
		}

		// Token: 0x06000D1B RID: 3355 RVA: 0x0000FF5E File Offset: 0x0000E15E
		public static implicit operator NamedArgument(LocalTargetInfo value)
		{
			return new NamedArgument(value, null);
		}

		// Token: 0x06000D1C RID: 3356 RVA: 0x0000FF6C File Offset: 0x0000E16C
		public static implicit operator NamedArgument(TargetInfo value)
		{
			return new NamedArgument(value, null);
		}

		// Token: 0x06000D1D RID: 3357 RVA: 0x0000FF7A File Offset: 0x0000E17A
		public static implicit operator NamedArgument(GlobalTargetInfo value)
		{
			return new NamedArgument(value, null);
		}

		// Token: 0x06000D1E RID: 3358 RVA: 0x0000FF1D File Offset: 0x0000E11D
		public static implicit operator NamedArgument(Map value)
		{
			return new NamedArgument(value, null);
		}

		// Token: 0x06000D1F RID: 3359 RVA: 0x0000FF88 File Offset: 0x0000E188
		public static implicit operator NamedArgument(TaggedString value)
		{
			return new NamedArgument(value, null);
		}

		// Token: 0x06000D20 RID: 3360 RVA: 0x0000FF96 File Offset: 0x0000E196
		public override string ToString()
		{
			return (this.label.NullOrEmpty() ? "unnamed" : this.label) + "->" + this.arg.ToStringSafe<object>();
		}

		// Token: 0x04000B22 RID: 2850
		public object arg;

		// Token: 0x04000B23 RID: 2851
		public string label;
	}
}
