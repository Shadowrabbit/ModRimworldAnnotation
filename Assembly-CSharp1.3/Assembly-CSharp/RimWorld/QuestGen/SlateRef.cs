using System;
using System.Text.RegularExpressions;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001728 RID: 5928
	public struct SlateRef<T> : ISlateRef, IEquatable<SlateRef<T>>
	{
		// Token: 0x1700162A RID: 5674
		// (get) Token: 0x060088AA RID: 34986 RVA: 0x00311F75 File Offset: 0x00310175
		// (set) Token: 0x060088AB RID: 34987 RVA: 0x00311F7D File Offset: 0x0031017D
		string ISlateRef.SlateRef
		{
			get
			{
				return this.slateRef;
			}
			set
			{
				this.slateRef = value;
			}
		}

		// Token: 0x060088AC RID: 34988 RVA: 0x00311F7D File Offset: 0x0031017D
		public SlateRef(string slateRef)
		{
			this.slateRef = slateRef;
		}

		// Token: 0x060088AD RID: 34989 RVA: 0x00311F88 File Offset: 0x00310188
		public T GetValue(Slate slate)
		{
			T result;
			this.TryGetValue(slate, out result);
			return result;
		}

		// Token: 0x060088AE RID: 34990 RVA: 0x00311FA0 File Offset: 0x003101A0
		public bool TryGetValue(Slate slate, out T value)
		{
			return this.TryGetConvertedValue<T>(slate, out value);
		}

		// Token: 0x060088AF RID: 34991 RVA: 0x00311FAC File Offset: 0x003101AC
		public bool TryGetConvertedValue<TAnything>(Slate slate, out TAnything value)
		{
			if (this.slateRef == null)
			{
				value = default(TAnything);
				return true;
			}
			SlateRef<T>.tmpCurSlate = slate;
			string text = SlateRef<T>.HighPriorityVarsRegex.Replace(this.slateRef, new MatchEvaluator(SlateRef<T>.RegexMatchEvaluatorConcatenate));
			object obj;
			bool flag;
			if (!SlateRefUtility.CheckSingleVariableSyntax(text, slate, out obj, out flag))
			{
				obj = SlateRef<T>.MathExprRegex.Replace(text, SlateRef<T>.RegexMatchEvaluatorEvaluateMathExpressionCached);
				obj = SlateRef<T>.VarsRegex.Replace((string)obj, SlateRef<T>.RegexMatchEvaluatorConcatenateCached);
				flag = true;
			}
			SlateRef<T>.tmpCurSlate = null;
			if (!flag)
			{
				value = default(TAnything);
				return false;
			}
			if (obj == null)
			{
				value = default(TAnything);
				return true;
			}
			if (obj is TAnything)
			{
				value = (TAnything)((object)obj);
				return true;
			}
			if (ConvertHelper.CanConvert<TAnything>(obj))
			{
				value = ConvertHelper.Convert<TAnything>(obj);
				return true;
			}
			Log.Error(string.Concat(new string[]
			{
				"Could not convert SlateRef \"",
				this.slateRef,
				"\" (",
				obj.GetType().Name,
				") to ",
				typeof(TAnything).Name
			}));
			value = default(TAnything);
			return false;
		}

		// Token: 0x060088B0 RID: 34992 RVA: 0x003120C8 File Offset: 0x003102C8
		private static string RegexMatchEvaluatorConcatenate(Match match)
		{
			string value = match.Groups[1].Value;
			object obj;
			if (!SlateRef<T>.tmpCurSlate.TryGet<object>(value, out obj, false))
			{
				return "";
			}
			if (obj == null)
			{
				return "";
			}
			return obj.ToString();
		}

		// Token: 0x060088B1 RID: 34993 RVA: 0x0031210C File Offset: 0x0031030C
		private static string RegexMatchEvaluatorConcatenateZeroIfEmpty(Match match)
		{
			string value = match.Groups[1].Value;
			object obj;
			if (!SlateRef<T>.tmpCurSlate.TryGet<object>(value, out obj, false))
			{
				Log.ErrorOnce("Tried to use variable \"" + value + "\" in a math expression but it doesn't exist.", value.GetHashCode() ^ 194857119);
				return "0";
			}
			if (obj == null)
			{
				return "0";
			}
			string text = obj.ToString();
			if (text == "")
			{
				return "0";
			}
			return text;
		}

		// Token: 0x060088B2 RID: 34994 RVA: 0x00312188 File Offset: 0x00310388
		private static string RegexMatchEvaluatorResolveMathExpression(Match match)
		{
			string text = match.Groups[1].Value;
			text = SlateRef<T>.VarsRegex.Replace(text, SlateRef<T>.RegexMatchEvaluatorConcatenateZeroIfEmptyCached);
			return MathEvaluator.Evaluate(text).ToString();
		}

		// Token: 0x060088B3 RID: 34995 RVA: 0x003121C6 File Offset: 0x003103C6
		public override bool Equals(object obj)
		{
			return obj is SlateRef<T> && this.Equals((SlateRef<T>)obj);
		}

		// Token: 0x060088B4 RID: 34996 RVA: 0x003121DE File Offset: 0x003103DE
		public bool Equals(SlateRef<T> other)
		{
			return this == other;
		}

		// Token: 0x060088B5 RID: 34997 RVA: 0x003121EC File Offset: 0x003103EC
		public static bool operator ==(SlateRef<T> a, SlateRef<T> b)
		{
			return a.slateRef == b.slateRef;
		}

		// Token: 0x060088B6 RID: 34998 RVA: 0x003121FF File Offset: 0x003103FF
		public static bool operator !=(SlateRef<T> a, SlateRef<T> b)
		{
			return !(a == b);
		}

		// Token: 0x060088B7 RID: 34999 RVA: 0x0031220B File Offset: 0x0031040B
		public static implicit operator SlateRef<T>(T t)
		{
			return new SlateRef<T>((t != null) ? t.ToString() : null);
		}

		// Token: 0x060088B8 RID: 35000 RVA: 0x0031222A File Offset: 0x0031042A
		public override int GetHashCode()
		{
			if (this.slateRef == null)
			{
				return 0;
			}
			return this.slateRef.GetHashCode();
		}

		// Token: 0x060088B9 RID: 35001 RVA: 0x00312244 File Offset: 0x00310444
		public string ToString(Slate slate)
		{
			string result;
			this.TryGetConvertedValue<string>(slate, out result);
			return result;
		}

		// Token: 0x060088BA RID: 35002 RVA: 0x0031225C File Offset: 0x0031045C
		public override string ToString()
		{
			if (!QuestGen.Working)
			{
				return this.slateRef;
			}
			return this.ToString(QuestGen.slate);
		}

		// Token: 0x040056D9 RID: 22233
		public const string SlateRefFieldName = "slateRef";

		// Token: 0x040056DA RID: 22234
		[MustTranslate_SlateRef]
		private string slateRef;

		// Token: 0x040056DB RID: 22235
		private static Slate tmpCurSlate;

		// Token: 0x040056DC RID: 22236
		private static readonly Regex VarsRegex = new Regex("\\$([a-zA-Z0-1_/]*)");

		// Token: 0x040056DD RID: 22237
		private static readonly Regex HighPriorityVarsRegex = new Regex("\\(\\(\\$([a-zA-Z0-1_/]*)\\)\\)");

		// Token: 0x040056DE RID: 22238
		private static readonly Regex MathExprRegex = new Regex("\\$\\((.*)\\)");

		// Token: 0x040056DF RID: 22239
		private static MatchEvaluator RegexMatchEvaluatorConcatenateCached = new MatchEvaluator(SlateRef<T>.RegexMatchEvaluatorConcatenate);

		// Token: 0x040056E0 RID: 22240
		private static MatchEvaluator RegexMatchEvaluatorConcatenateZeroIfEmptyCached = new MatchEvaluator(SlateRef<T>.RegexMatchEvaluatorConcatenateZeroIfEmpty);

		// Token: 0x040056E1 RID: 22241
		private static MatchEvaluator RegexMatchEvaluatorEvaluateMathExpressionCached = new MatchEvaluator(SlateRef<T>.RegexMatchEvaluatorResolveMathExpression);
	}
}
