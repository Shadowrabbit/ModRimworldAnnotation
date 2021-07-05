using System;

namespace Verse
{
	// Token: 0x02000232 RID: 562
	public static class TranslatorFormattedStringExtensions
	{
		// Token: 0x06000E65 RID: 3685 RVA: 0x00010CF7 File Offset: 0x0000EEF7
		public static TaggedString Translate(this string key, NamedArgument arg1)
		{
			return key.Translate().Formatted(arg1);
		}

		// Token: 0x06000E66 RID: 3686 RVA: 0x00010D05 File Offset: 0x0000EF05
		public static TaggedString Translate(this string key, NamedArgument arg1, NamedArgument arg2)
		{
			return key.Translate().Formatted(arg1, arg2);
		}

		// Token: 0x06000E67 RID: 3687 RVA: 0x00010D14 File Offset: 0x0000EF14
		public static TaggedString Translate(this string key, NamedArgument arg1, NamedArgument arg2, NamedArgument arg3)
		{
			return key.Translate().Formatted(arg1, arg2, arg3);
		}

		// Token: 0x06000E68 RID: 3688 RVA: 0x00010D24 File Offset: 0x0000EF24
		public static TaggedString Translate(this string key, NamedArgument arg1, NamedArgument arg2, NamedArgument arg3, NamedArgument arg4)
		{
			return key.Translate().Formatted(arg1, arg2, arg3, arg4);
		}

		// Token: 0x06000E69 RID: 3689 RVA: 0x00010D36 File Offset: 0x0000EF36
		public static TaggedString Translate(this string key, NamedArgument arg1, NamedArgument arg2, NamedArgument arg3, NamedArgument arg4, NamedArgument arg5)
		{
			return key.Translate().Formatted(arg1, arg2, arg3, arg4, arg5);
		}

		// Token: 0x06000E6A RID: 3690 RVA: 0x00010D4A File Offset: 0x0000EF4A
		public static TaggedString Translate(this string key, NamedArgument arg1, NamedArgument arg2, NamedArgument arg3, NamedArgument arg4, NamedArgument arg5, NamedArgument arg6)
		{
			return key.Translate().Formatted(arg1, arg2, arg3, arg4, arg5, arg6);
		}

		// Token: 0x06000E6B RID: 3691 RVA: 0x00010D60 File Offset: 0x0000EF60
		public static TaggedString Translate(this string key, NamedArgument arg1, NamedArgument arg2, NamedArgument arg3, NamedArgument arg4, NamedArgument arg5, NamedArgument arg6, NamedArgument arg7)
		{
			return key.Translate().Formatted(arg1, arg2, arg3, arg4, arg5, arg6, arg7);
		}

		// Token: 0x06000E6C RID: 3692 RVA: 0x000B3430 File Offset: 0x000B1630
		public static TaggedString Translate(this string key, NamedArgument arg1, NamedArgument arg2, NamedArgument arg3, NamedArgument arg4, NamedArgument arg5, NamedArgument arg6, NamedArgument arg7, NamedArgument arg8)
		{
			return key.Translate().Formatted(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
		}

		// Token: 0x06000E6D RID: 3693 RVA: 0x00010D78 File Offset: 0x0000EF78
		public static TaggedString Translate(this string key, params NamedArgument[] args)
		{
			return key.Translate().Formatted(args);
		}
	}
}
