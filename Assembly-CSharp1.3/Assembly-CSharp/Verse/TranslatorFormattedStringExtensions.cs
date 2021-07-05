using System;

namespace Verse
{
	// Token: 0x0200016C RID: 364
	public static class TranslatorFormattedStringExtensions
	{
		// Token: 0x06000A30 RID: 2608 RVA: 0x00038531 File Offset: 0x00036731
		public static TaggedString Translate(this string key, NamedArgument arg1)
		{
			return key.Translate().Formatted(arg1);
		}

		// Token: 0x06000A31 RID: 2609 RVA: 0x0003853F File Offset: 0x0003673F
		public static TaggedString Translate(this string key, NamedArgument arg1, NamedArgument arg2)
		{
			return key.Translate().Formatted(arg1, arg2);
		}

		// Token: 0x06000A32 RID: 2610 RVA: 0x0003854E File Offset: 0x0003674E
		public static TaggedString Translate(this string key, NamedArgument arg1, NamedArgument arg2, NamedArgument arg3)
		{
			return key.Translate().Formatted(arg1, arg2, arg3);
		}

		// Token: 0x06000A33 RID: 2611 RVA: 0x0003855E File Offset: 0x0003675E
		public static TaggedString Translate(this string key, NamedArgument arg1, NamedArgument arg2, NamedArgument arg3, NamedArgument arg4)
		{
			return key.Translate().Formatted(arg1, arg2, arg3, arg4);
		}

		// Token: 0x06000A34 RID: 2612 RVA: 0x00038570 File Offset: 0x00036770
		public static TaggedString Translate(this string key, NamedArgument arg1, NamedArgument arg2, NamedArgument arg3, NamedArgument arg4, NamedArgument arg5)
		{
			return key.Translate().Formatted(arg1, arg2, arg3, arg4, arg5);
		}

		// Token: 0x06000A35 RID: 2613 RVA: 0x00038584 File Offset: 0x00036784
		public static TaggedString Translate(this string key, NamedArgument arg1, NamedArgument arg2, NamedArgument arg3, NamedArgument arg4, NamedArgument arg5, NamedArgument arg6)
		{
			return key.Translate().Formatted(arg1, arg2, arg3, arg4, arg5, arg6);
		}

		// Token: 0x06000A36 RID: 2614 RVA: 0x0003859A File Offset: 0x0003679A
		public static TaggedString Translate(this string key, NamedArgument arg1, NamedArgument arg2, NamedArgument arg3, NamedArgument arg4, NamedArgument arg5, NamedArgument arg6, NamedArgument arg7)
		{
			return key.Translate().Formatted(arg1, arg2, arg3, arg4, arg5, arg6, arg7);
		}

		// Token: 0x06000A37 RID: 2615 RVA: 0x000385B4 File Offset: 0x000367B4
		public static TaggedString Translate(this string key, NamedArgument arg1, NamedArgument arg2, NamedArgument arg3, NamedArgument arg4, NamedArgument arg5, NamedArgument arg6, NamedArgument arg7, NamedArgument arg8)
		{
			return key.Translate().Formatted(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
		}

		// Token: 0x06000A38 RID: 2616 RVA: 0x000385D9 File Offset: 0x000367D9
		public static TaggedString Translate(this string key, params NamedArgument[] args)
		{
			return key.Translate().Formatted(args);
		}
	}
}
