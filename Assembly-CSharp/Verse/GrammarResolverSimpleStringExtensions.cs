using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x020001F5 RID: 501
	public static class GrammarResolverSimpleStringExtensions
	{
		// Token: 0x06000CF8 RID: 3320 RVA: 0x000A9378 File Offset: 0x000A7578
		public static TaggedString Formatted(this string str, NamedArgument arg1)
		{
			GrammarResolverSimpleStringExtensions.argsLabels.Clear();
			GrammarResolverSimpleStringExtensions.argsObjects.Clear();
			GrammarResolverSimpleStringExtensions.argsLabels.Add(arg1.label);
			GrammarResolverSimpleStringExtensions.argsObjects.Add(arg1.arg);
			return GrammarResolverSimple.Formatted(str, GrammarResolverSimpleStringExtensions.argsLabels, GrammarResolverSimpleStringExtensions.argsObjects);
		}

		// Token: 0x06000CF9 RID: 3321 RVA: 0x0000FE1A File Offset: 0x0000E01A
		public static TaggedString Formatted(this TaggedString str, NamedArgument arg1)
		{
			return str.RawText.Formatted(arg1);
		}

		// Token: 0x06000CFA RID: 3322 RVA: 0x000A93D0 File Offset: 0x000A75D0
		public static TaggedString Formatted(this string str, NamedArgument arg1, NamedArgument arg2)
		{
			GrammarResolverSimpleStringExtensions.argsLabels.Clear();
			GrammarResolverSimpleStringExtensions.argsObjects.Clear();
			GrammarResolverSimpleStringExtensions.argsLabels.Add(arg1.label);
			GrammarResolverSimpleStringExtensions.argsObjects.Add(arg1.arg);
			GrammarResolverSimpleStringExtensions.argsLabels.Add(arg2.label);
			GrammarResolverSimpleStringExtensions.argsObjects.Add(arg2.arg);
			return GrammarResolverSimple.Formatted(str, GrammarResolverSimpleStringExtensions.argsLabels, GrammarResolverSimpleStringExtensions.argsObjects);
		}

		// Token: 0x06000CFB RID: 3323 RVA: 0x0000FE29 File Offset: 0x0000E029
		public static TaggedString Formatted(this TaggedString str, NamedArgument arg1, NamedArgument arg2)
		{
			return str.RawText.Formatted(arg1, arg2);
		}

		// Token: 0x06000CFC RID: 3324 RVA: 0x000A9448 File Offset: 0x000A7648
		public static TaggedString Formatted(this string str, NamedArgument arg1, NamedArgument arg2, NamedArgument arg3)
		{
			GrammarResolverSimpleStringExtensions.argsLabels.Clear();
			GrammarResolverSimpleStringExtensions.argsObjects.Clear();
			GrammarResolverSimpleStringExtensions.argsLabels.Add(arg1.label);
			GrammarResolverSimpleStringExtensions.argsObjects.Add(arg1.arg);
			GrammarResolverSimpleStringExtensions.argsLabels.Add(arg2.label);
			GrammarResolverSimpleStringExtensions.argsObjects.Add(arg2.arg);
			GrammarResolverSimpleStringExtensions.argsLabels.Add(arg3.label);
			GrammarResolverSimpleStringExtensions.argsObjects.Add(arg3.arg);
			return GrammarResolverSimple.Formatted(str, GrammarResolverSimpleStringExtensions.argsLabels, GrammarResolverSimpleStringExtensions.argsObjects);
		}

		// Token: 0x06000CFD RID: 3325 RVA: 0x0000FE39 File Offset: 0x0000E039
		public static TaggedString Formatted(this TaggedString str, NamedArgument arg1, NamedArgument arg2, NamedArgument arg3)
		{
			return str.RawText.Formatted(arg1, arg2, arg3);
		}

		// Token: 0x06000CFE RID: 3326 RVA: 0x000A94E0 File Offset: 0x000A76E0
		public static TaggedString Formatted(this string str, NamedArgument arg1, NamedArgument arg2, NamedArgument arg3, NamedArgument arg4)
		{
			GrammarResolverSimpleStringExtensions.argsLabels.Clear();
			GrammarResolverSimpleStringExtensions.argsObjects.Clear();
			GrammarResolverSimpleStringExtensions.argsLabels.Add(arg1.label);
			GrammarResolverSimpleStringExtensions.argsObjects.Add(arg1.arg);
			GrammarResolverSimpleStringExtensions.argsLabels.Add(arg2.label);
			GrammarResolverSimpleStringExtensions.argsObjects.Add(arg2.arg);
			GrammarResolverSimpleStringExtensions.argsLabels.Add(arg3.label);
			GrammarResolverSimpleStringExtensions.argsObjects.Add(arg3.arg);
			GrammarResolverSimpleStringExtensions.argsLabels.Add(arg4.label);
			GrammarResolverSimpleStringExtensions.argsObjects.Add(arg4.arg);
			return GrammarResolverSimple.Formatted(str, GrammarResolverSimpleStringExtensions.argsLabels, GrammarResolverSimpleStringExtensions.argsObjects);
		}

		// Token: 0x06000CFF RID: 3327 RVA: 0x0000FE4A File Offset: 0x0000E04A
		public static TaggedString Formatted(this TaggedString str, NamedArgument arg1, NamedArgument arg2, NamedArgument arg3, NamedArgument arg4)
		{
			return str.RawText.Formatted(arg1, arg2, arg3, arg4);
		}

		// Token: 0x06000D00 RID: 3328 RVA: 0x000A9598 File Offset: 0x000A7798
		public static TaggedString Formatted(this string str, NamedArgument arg1, NamedArgument arg2, NamedArgument arg3, NamedArgument arg4, NamedArgument arg5)
		{
			GrammarResolverSimpleStringExtensions.argsLabels.Clear();
			GrammarResolverSimpleStringExtensions.argsObjects.Clear();
			GrammarResolverSimpleStringExtensions.argsLabels.Add(arg1.label);
			GrammarResolverSimpleStringExtensions.argsObjects.Add(arg1.arg);
			GrammarResolverSimpleStringExtensions.argsLabels.Add(arg2.label);
			GrammarResolverSimpleStringExtensions.argsObjects.Add(arg2.arg);
			GrammarResolverSimpleStringExtensions.argsLabels.Add(arg3.label);
			GrammarResolverSimpleStringExtensions.argsObjects.Add(arg3.arg);
			GrammarResolverSimpleStringExtensions.argsLabels.Add(arg4.label);
			GrammarResolverSimpleStringExtensions.argsObjects.Add(arg4.arg);
			GrammarResolverSimpleStringExtensions.argsLabels.Add(arg5.label);
			GrammarResolverSimpleStringExtensions.argsObjects.Add(arg5.arg);
			return GrammarResolverSimple.Formatted(str, GrammarResolverSimpleStringExtensions.argsLabels, GrammarResolverSimpleStringExtensions.argsObjects);
		}

		// Token: 0x06000D01 RID: 3329 RVA: 0x0000FE5D File Offset: 0x0000E05D
		public static TaggedString Formatted(this TaggedString str, NamedArgument arg1, NamedArgument arg2, NamedArgument arg3, NamedArgument arg4, NamedArgument arg5)
		{
			return str.RawText.Formatted(arg1, arg2, arg3, arg4, arg5);
		}

		// Token: 0x06000D02 RID: 3330 RVA: 0x000A9674 File Offset: 0x000A7874
		public static TaggedString Formatted(this string str, NamedArgument arg1, NamedArgument arg2, NamedArgument arg3, NamedArgument arg4, NamedArgument arg5, NamedArgument arg6)
		{
			GrammarResolverSimpleStringExtensions.argsLabels.Clear();
			GrammarResolverSimpleStringExtensions.argsObjects.Clear();
			GrammarResolverSimpleStringExtensions.argsLabels.Add(arg1.label);
			GrammarResolverSimpleStringExtensions.argsObjects.Add(arg1.arg);
			GrammarResolverSimpleStringExtensions.argsLabels.Add(arg2.label);
			GrammarResolverSimpleStringExtensions.argsObjects.Add(arg2.arg);
			GrammarResolverSimpleStringExtensions.argsLabels.Add(arg3.label);
			GrammarResolverSimpleStringExtensions.argsObjects.Add(arg3.arg);
			GrammarResolverSimpleStringExtensions.argsLabels.Add(arg4.label);
			GrammarResolverSimpleStringExtensions.argsObjects.Add(arg4.arg);
			GrammarResolverSimpleStringExtensions.argsLabels.Add(arg5.label);
			GrammarResolverSimpleStringExtensions.argsObjects.Add(arg5.arg);
			GrammarResolverSimpleStringExtensions.argsLabels.Add(arg6.label);
			GrammarResolverSimpleStringExtensions.argsObjects.Add(arg6.arg);
			return GrammarResolverSimple.Formatted(str, GrammarResolverSimpleStringExtensions.argsLabels, GrammarResolverSimpleStringExtensions.argsObjects);
		}

		// Token: 0x06000D03 RID: 3331 RVA: 0x0000FE72 File Offset: 0x0000E072
		public static TaggedString Formatted(this TaggedString str, NamedArgument arg1, NamedArgument arg2, NamedArgument arg3, NamedArgument arg4, NamedArgument arg5, NamedArgument arg6)
		{
			return str.RawText.Formatted(arg1, arg2, arg3, arg4, arg5, arg6);
		}

		// Token: 0x06000D04 RID: 3332 RVA: 0x000A9770 File Offset: 0x000A7970
		public static TaggedString Formatted(this string str, NamedArgument arg1, NamedArgument arg2, NamedArgument arg3, NamedArgument arg4, NamedArgument arg5, NamedArgument arg6, NamedArgument arg7)
		{
			GrammarResolverSimpleStringExtensions.argsLabels.Clear();
			GrammarResolverSimpleStringExtensions.argsObjects.Clear();
			GrammarResolverSimpleStringExtensions.argsLabels.Add(arg1.label);
			GrammarResolverSimpleStringExtensions.argsObjects.Add(arg1.arg);
			GrammarResolverSimpleStringExtensions.argsLabels.Add(arg2.label);
			GrammarResolverSimpleStringExtensions.argsObjects.Add(arg2.arg);
			GrammarResolverSimpleStringExtensions.argsLabels.Add(arg3.label);
			GrammarResolverSimpleStringExtensions.argsObjects.Add(arg3.arg);
			GrammarResolverSimpleStringExtensions.argsLabels.Add(arg4.label);
			GrammarResolverSimpleStringExtensions.argsObjects.Add(arg4.arg);
			GrammarResolverSimpleStringExtensions.argsLabels.Add(arg5.label);
			GrammarResolverSimpleStringExtensions.argsObjects.Add(arg5.arg);
			GrammarResolverSimpleStringExtensions.argsLabels.Add(arg6.label);
			GrammarResolverSimpleStringExtensions.argsObjects.Add(arg6.arg);
			GrammarResolverSimpleStringExtensions.argsLabels.Add(arg7.label);
			GrammarResolverSimpleStringExtensions.argsObjects.Add(arg7.arg);
			return GrammarResolverSimple.Formatted(str, GrammarResolverSimpleStringExtensions.argsLabels, GrammarResolverSimpleStringExtensions.argsObjects);
		}

		// Token: 0x06000D05 RID: 3333 RVA: 0x0000FE89 File Offset: 0x0000E089
		public static TaggedString Formatted(this TaggedString str, NamedArgument arg1, NamedArgument arg2, NamedArgument arg3, NamedArgument arg4, NamedArgument arg5, NamedArgument arg6, NamedArgument arg7)
		{
			return str.RawText.Formatted(arg1, arg2, arg3, arg4, arg5, arg6, arg7);
		}

		// Token: 0x06000D06 RID: 3334 RVA: 0x000A9890 File Offset: 0x000A7A90
		public static TaggedString Formatted(this string str, NamedArgument arg1, NamedArgument arg2, NamedArgument arg3, NamedArgument arg4, NamedArgument arg5, NamedArgument arg6, NamedArgument arg7, NamedArgument arg8)
		{
			GrammarResolverSimpleStringExtensions.argsLabels.Clear();
			GrammarResolverSimpleStringExtensions.argsObjects.Clear();
			GrammarResolverSimpleStringExtensions.argsLabels.Add(arg1.label);
			GrammarResolverSimpleStringExtensions.argsObjects.Add(arg1.arg);
			GrammarResolverSimpleStringExtensions.argsLabels.Add(arg2.label);
			GrammarResolverSimpleStringExtensions.argsObjects.Add(arg2.arg);
			GrammarResolverSimpleStringExtensions.argsLabels.Add(arg3.label);
			GrammarResolverSimpleStringExtensions.argsObjects.Add(arg3.arg);
			GrammarResolverSimpleStringExtensions.argsLabels.Add(arg4.label);
			GrammarResolverSimpleStringExtensions.argsObjects.Add(arg4.arg);
			GrammarResolverSimpleStringExtensions.argsLabels.Add(arg5.label);
			GrammarResolverSimpleStringExtensions.argsObjects.Add(arg5.arg);
			GrammarResolverSimpleStringExtensions.argsLabels.Add(arg6.label);
			GrammarResolverSimpleStringExtensions.argsObjects.Add(arg6.arg);
			GrammarResolverSimpleStringExtensions.argsLabels.Add(arg7.label);
			GrammarResolverSimpleStringExtensions.argsObjects.Add(arg7.arg);
			GrammarResolverSimpleStringExtensions.argsLabels.Add(arg8.label);
			GrammarResolverSimpleStringExtensions.argsObjects.Add(arg8.arg);
			return GrammarResolverSimple.Formatted(str, GrammarResolverSimpleStringExtensions.argsLabels, GrammarResolverSimpleStringExtensions.argsObjects);
		}

		// Token: 0x06000D07 RID: 3335 RVA: 0x000A99D0 File Offset: 0x000A7BD0
		public static TaggedString Formatted(this TaggedString str, NamedArgument arg1, NamedArgument arg2, NamedArgument arg3, NamedArgument arg4, NamedArgument arg5, NamedArgument arg6, NamedArgument arg7, NamedArgument arg8)
		{
			return str.RawText.Formatted(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
		}

		// Token: 0x06000D08 RID: 3336 RVA: 0x000A99F8 File Offset: 0x000A7BF8
		public static TaggedString Formatted(this string str, params NamedArgument[] args)
		{
			GrammarResolverSimpleStringExtensions.argsLabels.Clear();
			GrammarResolverSimpleStringExtensions.argsObjects.Clear();
			for (int i = 0; i < args.Length; i++)
			{
				GrammarResolverSimpleStringExtensions.argsLabels.Add(args[i].label);
				GrammarResolverSimpleStringExtensions.argsObjects.Add(args[i].arg);
			}
			return GrammarResolverSimple.Formatted(str, GrammarResolverSimpleStringExtensions.argsLabels, GrammarResolverSimpleStringExtensions.argsObjects);
		}

		// Token: 0x06000D09 RID: 3337 RVA: 0x0000FEA2 File Offset: 0x0000E0A2
		public static TaggedString Formatted(this TaggedString str, params NamedArgument[] args)
		{
			return str.RawText.Formatted(args);
		}

		// Token: 0x04000B20 RID: 2848
		private static List<string> argsLabels = new List<string>();

		// Token: 0x04000B21 RID: 2849
		private static List<object> argsObjects = new List<object>();
	}
}
