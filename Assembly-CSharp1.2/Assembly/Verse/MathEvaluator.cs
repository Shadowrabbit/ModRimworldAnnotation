using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace Verse
{
	// Token: 0x0200007B RID: 123
	public static class MathEvaluator
	{
		// Token: 0x170000D2 RID: 210
		// (get) Token: 0x0600049E RID: 1182 RVA: 0x0000A24A File Offset: 0x0000844A
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

		// Token: 0x0600049F RID: 1183 RVA: 0x00088654 File Offset: 0x00086854
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
					Log.ErrorOnce("Expression \"" + expr + "\" evaluated to NaN.", expr.GetHashCode() ^ 48337162, false);
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
				}), expr.GetHashCode() ^ 980986121, false);
				result = 0.0;
			}
			return result;
		}

		// Token: 0x04000220 RID: 544
		private static XPathDocument doc;

		// Token: 0x04000221 RID: 545
		private static XPathNavigator navigator;

		// Token: 0x04000222 RID: 546
		private static readonly Regex AddSpacesRegex = new Regex("([\\+\\-\\*])");

		// Token: 0x04000223 RID: 547
		private static readonly MathEvaluatorCustomContext Context = new MathEvaluatorCustomContext(new NameTable(), new XsltArgumentList());
	}
}
