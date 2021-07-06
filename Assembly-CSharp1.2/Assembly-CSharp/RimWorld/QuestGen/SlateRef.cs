using System;
using System.Text.RegularExpressions;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02002004 RID: 8196
	public struct SlateRef<T> : ISlateRef, IEquatable<SlateRef<T>>
	{
		// Token: 0x17001988 RID: 6536
		// (get) Token: 0x0600AD94 RID: 44436 RVA: 0x00070F78 File Offset: 0x0006F178
		// (set) Token: 0x0600AD95 RID: 44437 RVA: 0x00070F80 File Offset: 0x0006F180
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

		// Token: 0x0600AD96 RID: 44438 RVA: 0x00070F80 File Offset: 0x0006F180
		public SlateRef(string slateRef)
		{
			this.slateRef = slateRef;
		}

		// Token: 0x0600AD97 RID: 44439 RVA: 0x003287D4 File Offset: 0x003269D4
		public T GetValue(Slate slate)
		{
			T result;
			this.TryGetValue(slate, out result);
			return result;
		}

		// Token: 0x0600AD98 RID: 44440 RVA: 0x00070F89 File Offset: 0x0006F189
		public bool TryGetValue(Slate slate, out T value)
		{
			return this.TryGetConvertedValue<T>(slate, out value);
		}

		// Token: 0x0600AD99 RID: 44441 RVA: 0x003287EC File Offset: 0x003269EC
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
			}), false);
			value = default(TAnything);
			return false;
		}

		// Token: 0x0600AD9A RID: 44442 RVA: 0x00328908 File Offset: 0x00326B08
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

		// Token: 0x0600AD9B RID: 44443 RVA: 0x0032894C File Offset: 0x00326B4C
		private static string RegexMatchEvaluatorConcatenateZeroIfEmpty(Match match)
		{
			string value = match.Groups[1].Value;
			object obj;
			if (!SlateRef<T>.tmpCurSlate.TryGet<object>(value, out obj, false))
			{
				Log.ErrorOnce("Tried to use variable \"" + value + "\" in a math expression but it doesn't exist.", value.GetHashCode() ^ 194857119, false);
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

		// Token: 0x0600AD9C RID: 44444 RVA: 0x003289C8 File Offset: 0x00326BC8
		private static string RegexMatchEvaluatorResolveMathExpression(Match match)
		{
			string text = match.Groups[1].Value;
			text = SlateRef<T>.VarsRegex.Replace(text, SlateRef<T>.RegexMatchEvaluatorConcatenateZeroIfEmptyCached);
			return MathEvaluator.Evaluate(text).ToString();
		}

		// Token: 0x0600AD9D RID: 44445 RVA: 0x00070F93 File Offset: 0x0006F193
		public override bool Equals(object obj)
		{
			return obj is SlateRef<T> && this.Equals((SlateRef<T>)obj);
		}

		// Token: 0x0600AD9E RID: 44446 RVA: 0x00070FAB File Offset: 0x0006F1AB
		public bool Equals(SlateRef<T> other)
		{
			return this == other;
		}

		// Token: 0x0600AD9F RID: 44447 RVA: 0x00070FB9 File Offset: 0x0006F1B9
		public static bool operator ==(SlateRef<T> a, SlateRef<T> b)
		{
			return a.slateRef == b.slateRef;
		}

		// Token: 0x0600ADA0 RID: 44448 RVA: 0x00070FCC File Offset: 0x0006F1CC
		public static bool operator !=(SlateRef<T> a, SlateRef<T> b)
		{
			return !(a == b);
		}

		// Token: 0x0600ADA1 RID: 44449 RVA: 0x00070FD8 File Offset: 0x0006F1D8
		public static implicit operator SlateRef<T>(T t)
		{
			return new SlateRef<T>((t != null) ? t.ToString() : null);
		}

		// Token: 0x0600ADA2 RID: 44450 RVA: 0x00070FF7 File Offset: 0x0006F1F7
		public override int GetHashCode()
		{
			if (this.slateRef == null)
			{
				return 0;
			}
			return this.slateRef.GetHashCode();
		}

		// Token: 0x0600ADA3 RID: 44451 RVA: 0x00328A08 File Offset: 0x00326C08
		public string ToString(Slate slate)
		{
			string result;
			this.TryGetConvertedValue<string>(slate, out result);
			return result;
		}

		// Token: 0x0600ADA4 RID: 44452 RVA: 0x0007100E File Offset: 0x0006F20E
		public override string ToString()
		{
			if (!QuestGen.Working)
			{
				return this.slateRef;
			}
			return this.ToString(QuestGen.slate);
		}

		// Token: 0x04007743 RID: 30531
		public const string SlateRefFieldName = "slateRef";

		// Token: 0x04007744 RID: 30532
		[MustTranslate_SlateRef]
		private string slateRef;

		// Token: 0x04007745 RID: 30533
		private static Slate tmpCurSlate;

		// Token: 0x04007746 RID: 30534
		private static readonly Regex VarsRegex = new Regex("\\$([a-zA-Z0-1_/]*)");

		// Token: 0x04007747 RID: 30535
		private static readonly Regex HighPriorityVarsRegex = new Regex("\\(\\(\\$([a-zA-Z0-1_/]*)\\)\\)");

		// Token: 0x04007748 RID: 30536
		private static readonly Regex MathExprRegex = new Regex("\\$\\((.*)\\)");

		// Token: 0x04007749 RID: 30537
		private static MatchEvaluator RegexMatchEvaluatorConcatenateCached = new MatchEvaluator(SlateRef<T>.RegexMatchEvaluatorConcatenate);

		// Token: 0x0400774A RID: 30538
		private static MatchEvaluator RegexMatchEvaluatorConcatenateZeroIfEmptyCached = new MatchEvaluator(SlateRef<T>.RegexMatchEvaluatorConcatenateZeroIfEmpty);

		// Token: 0x0400774B RID: 30539
		private static MatchEvaluator RegexMatchEvaluatorEvaluateMathExpressionCached = new MatchEvaluator(SlateRef<T>.RegexMatchEvaluatorResolveMathExpression);
	}
}
