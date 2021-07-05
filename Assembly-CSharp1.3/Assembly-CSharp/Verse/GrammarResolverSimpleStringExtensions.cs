using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x0200014A RID: 330
	public static class GrammarResolverSimpleStringExtensions
	{
		// Token: 0x06000924 RID: 2340 RVA: 0x0002DFC8 File Offset: 0x0002C1C8
		public static TaggedString Formatted(this string str, NamedArgument arg1)
		{
			GrammarResolverSimpleStringExtensions.argsLabels.Clear();
			GrammarResolverSimpleStringExtensions.argsObjects.Clear();
			GrammarResolverSimpleStringExtensions.argsLabels.Add(arg1.label);
			GrammarResolverSimpleStringExtensions.argsObjects.Add(arg1.arg);
			return GrammarResolverSimple.Formatted(str, GrammarResolverSimpleStringExtensions.argsLabels, GrammarResolverSimpleStringExtensions.argsObjects);
		}

		// Token: 0x06000925 RID: 2341 RVA: 0x0002E01E File Offset: 0x0002C21E
		public static TaggedString Formatted(this TaggedString str, NamedArgument arg1)
		{
			return str.RawText.Formatted(arg1);
		}

		// Token: 0x06000926 RID: 2342 RVA: 0x0002E030 File Offset: 0x0002C230
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

		// Token: 0x06000927 RID: 2343 RVA: 0x0002E0A6 File Offset: 0x0002C2A6
		public static TaggedString Formatted(this TaggedString str, NamedArgument arg1, NamedArgument arg2)
		{
			return str.RawText.Formatted(arg1, arg2);
		}

		// Token: 0x06000928 RID: 2344 RVA: 0x0002E0B8 File Offset: 0x0002C2B8
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

		// Token: 0x06000929 RID: 2345 RVA: 0x0002E14E File Offset: 0x0002C34E
		public static TaggedString Formatted(this TaggedString str, NamedArgument arg1, NamedArgument arg2, NamedArgument arg3)
		{
			return str.RawText.Formatted(arg1, arg2, arg3);
		}

		// Token: 0x0600092A RID: 2346 RVA: 0x0002E160 File Offset: 0x0002C360
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

		// Token: 0x0600092B RID: 2347 RVA: 0x0002E218 File Offset: 0x0002C418
		public static TaggedString Formatted(this TaggedString str, NamedArgument arg1, NamedArgument arg2, NamedArgument arg3, NamedArgument arg4)
		{
			return str.RawText.Formatted(arg1, arg2, arg3, arg4);
		}

		// Token: 0x0600092C RID: 2348 RVA: 0x0002E22C File Offset: 0x0002C42C
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

		// Token: 0x0600092D RID: 2349 RVA: 0x0002E306 File Offset: 0x0002C506
		public static TaggedString Formatted(this TaggedString str, NamedArgument arg1, NamedArgument arg2, NamedArgument arg3, NamedArgument arg4, NamedArgument arg5)
		{
			return str.RawText.Formatted(arg1, arg2, arg3, arg4, arg5);
		}

		// Token: 0x0600092E RID: 2350 RVA: 0x0002E31C File Offset: 0x0002C51C
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

		// Token: 0x0600092F RID: 2351 RVA: 0x0002E418 File Offset: 0x0002C618
		public static TaggedString Formatted(this TaggedString str, NamedArgument arg1, NamedArgument arg2, NamedArgument arg3, NamedArgument arg4, NamedArgument arg5, NamedArgument arg6)
		{
			return str.RawText.Formatted(arg1, arg2, arg3, arg4, arg5, arg6);
		}

		// Token: 0x06000930 RID: 2352 RVA: 0x0002E430 File Offset: 0x0002C630
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

		// Token: 0x06000931 RID: 2353 RVA: 0x0002E54E File Offset: 0x0002C74E
		public static TaggedString Formatted(this TaggedString str, NamedArgument arg1, NamedArgument arg2, NamedArgument arg3, NamedArgument arg4, NamedArgument arg5, NamedArgument arg6, NamedArgument arg7)
		{
			return str.RawText.Formatted(arg1, arg2, arg3, arg4, arg5, arg6, arg7);
		}

		// Token: 0x06000932 RID: 2354 RVA: 0x0002E568 File Offset: 0x0002C768
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

		// Token: 0x06000933 RID: 2355 RVA: 0x0002E6A8 File Offset: 0x0002C8A8
		public static TaggedString Formatted(this TaggedString str, NamedArgument arg1, NamedArgument arg2, NamedArgument arg3, NamedArgument arg4, NamedArgument arg5, NamedArgument arg6, NamedArgument arg7, NamedArgument arg8)
		{
			return str.RawText.Formatted(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
		}

		// Token: 0x06000934 RID: 2356 RVA: 0x0002E6D0 File Offset: 0x0002C8D0
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

		// Token: 0x06000935 RID: 2357 RVA: 0x0002E740 File Offset: 0x0002C940
		public static TaggedString Formatted(this string str, IEnumerable<NamedArgument> args)
		{
			GrammarResolverSimpleStringExtensions.argsLabels.Clear();
			GrammarResolverSimpleStringExtensions.argsObjects.Clear();
			foreach (NamedArgument namedArgument in args)
			{
				GrammarResolverSimpleStringExtensions.argsLabels.Add(namedArgument.label);
				GrammarResolverSimpleStringExtensions.argsObjects.Add(namedArgument.arg);
			}
			return GrammarResolverSimple.Formatted(str, GrammarResolverSimpleStringExtensions.argsLabels, GrammarResolverSimpleStringExtensions.argsObjects);
		}

		// Token: 0x06000936 RID: 2358 RVA: 0x0002E7CC File Offset: 0x0002C9CC
		public static TaggedString Formatted(this TaggedString str, params NamedArgument[] args)
		{
			return str.RawText.Formatted(args);
		}

		// Token: 0x04000850 RID: 2128
		private static List<string> argsLabels = new List<string>();

		// Token: 0x04000851 RID: 2129
		private static List<object> argsObjects = new List<object>();
	}
}
