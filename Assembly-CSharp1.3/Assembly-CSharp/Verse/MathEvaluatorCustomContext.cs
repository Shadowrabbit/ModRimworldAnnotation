using System;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace Verse
{
	// Token: 0x02000040 RID: 64
	public class MathEvaluatorCustomContext : XsltContext
	{
		// Token: 0x17000085 RID: 133
		// (get) Token: 0x06000365 RID: 869 RVA: 0x000126F5 File Offset: 0x000108F5
		public override bool Whitespace
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000086 RID: 134
		// (get) Token: 0x06000366 RID: 870 RVA: 0x000126F8 File Offset: 0x000108F8
		public XsltArgumentList ArgList
		{
			get
			{
				return this.argList;
			}
		}

		// Token: 0x06000367 RID: 871 RVA: 0x00012700 File Offset: 0x00010900
		public MathEvaluatorCustomContext()
		{
		}

		// Token: 0x06000368 RID: 872 RVA: 0x00012708 File Offset: 0x00010908
		public MathEvaluatorCustomContext(NameTable nt, XsltArgumentList args) : base(nt)
		{
			this.argList = args;
		}

		// Token: 0x06000369 RID: 873 RVA: 0x00012718 File Offset: 0x00010918
		public override IXsltContextFunction ResolveFunction(string prefix, string name, XPathResultType[] argTypes)
		{
			MathEvaluatorCustomFunctions.FunctionType[] functionTypes = MathEvaluatorCustomFunctions.FunctionTypes;
			for (int i = 0; i < functionTypes.Length; i++)
			{
				if (functionTypes[i].name == name)
				{
					return new MathEvaluatorCustomFunction(functionTypes[i], argTypes);
				}
			}
			return null;
		}

		// Token: 0x0600036A RID: 874 RVA: 0x00012754 File Offset: 0x00010954
		public override IXsltContextVariable ResolveVariable(string prefix, string name)
		{
			if (this.ArgList.GetParam(name, prefix) != null)
			{
				return new MathEvaluatorCustomVariable(prefix, name);
			}
			return null;
		}

		// Token: 0x0600036B RID: 875 RVA: 0x0001276E File Offset: 0x0001096E
		public override bool PreserveWhitespace(XPathNavigator node)
		{
			return false;
		}

		// Token: 0x0600036C RID: 876 RVA: 0x0001276E File Offset: 0x0001096E
		public override int CompareDocument(string baseUri, string nextbaseUri)
		{
			return 0;
		}

		// Token: 0x040000D1 RID: 209
		private XsltArgumentList argList;
	}
}
