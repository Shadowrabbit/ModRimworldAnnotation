using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace Verse
{
	// Token: 0x0200003F RID: 63
	public static class MathEvaluator
	{
		// Token: 0x17000084 RID: 132
		// (get) Token: 0x06000362 RID: 866 RVA: 0x00012587 File Offset: 0x00010787
		private static XPathNavigator Navigator
		{
			get
			{
				if (MathEvaluator.doc == null)
				{
					MathEvaluator.doc = new XPathDocument(new StringReader("<root />"));
				}
				if (MathEvaluator.navigator == null)
				{
					MathEvaluator.navigator = MathEvaluator.doc.CreateNavigator();
				}
				return MathEvaluator.navigator;
			}
		}

		// Token: 0x06000363 RID: 867 RVA: 0x000125C0 File Offset: 0x000107C0
		public static double Evaluate(string expr)
		{
			if (expr.NullOrEmpty())
			{
				return 0.0;
			}
			expr = MathEvaluator.AddSpacesRegex.Replace(expr, " ${1} ");
			expr = expr.Replace("/", " div ");
			expr = expr.Replace("%", " mod ");
			double result;
			try
			{
				XPathExpression xpathExpression = XPathExpression.Compile("number(" + expr + ")");
				xpathExpression.SetContext(MathEvaluator.Context);
				double num = (double)MathEvaluator.Navigator.Evaluate(xpathExpression);
				if (double.IsNaN(num))
				{
					Log.ErrorOnce("Expression \"" + expr + "\" evaluated to NaN.", expr.GetHashCode() ^ 48337162);
					num = 0.0;
				}
				result = num;
			}
			catch (XPathException ex)
			{
				Log.ErrorOnce(string.Concat(new object[]
				{
					"Could not evaluate expression \"",
					expr,
					"\". Error: ",
					ex
				}), expr.GetHashCode() ^ 980986121);
				result = 0.0;
			}
			return result;
		}

		// Token: 0x040000CD RID: 205
		private static XPathDocument doc;

		// Token: 0x040000CE RID: 206
		private static XPathNavigator navigator;

		// Token: 0x040000CF RID: 207
		private static readonly Regex AddSpacesRegex = new Regex("([\\+\\-\\*])");

		// Token: 0x040000D0 RID: 208
		private static readonly MathEvaluatorCustomContext Context = new MathEvaluatorCustomContext(new NameTable(), new XsltArgumentList());
	}
}
