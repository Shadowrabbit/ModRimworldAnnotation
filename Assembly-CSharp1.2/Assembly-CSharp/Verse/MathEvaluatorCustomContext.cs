using System;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace Verse
{
	// Token: 0x0200007C RID: 124
	public class MathEvaluatorCustomContext : XsltContext
	{
		// Token: 0x170000D3 RID: 211
		// (get) Token: 0x060004A1 RID: 1185 RVA: 0x0000A2A7 File Offset: 0x000084A7
		public override bool Whitespace
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170000D4 RID: 212
		// (get) Token: 0x060004A2 RID: 1186 RVA: 0x0000A2AA File Offset: 0x000084AA
		public XsltArgumentList ArgList
		{
			get
			{
				return this.argList;
			}
		}

		// Token: 0x060004A3 RID: 1187 RVA: 0x0000A2B2 File Offset: 0x000084B2
		public MathEvaluatorCustomContext()
		{
		}

		// Token: 0x060004A4 RID: 1188 RVA: 0x0000A2BA File Offset: 0x000084BA
		public MathEvaluatorCustomContext(NameTable nt, XsltArgumentList args) : base(nt)
		{
			this.argList = args;
		}

		// Token: 0x060004A5 RID: 1189 RVA: 0x00088768 File Offset: 0x00086968
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

		// Token: 0x060004A6 RID: 1190 RVA: 0x0000A2CA File Offset: 0x000084CA
		public override IXsltContextVariable ResolveVariable(string prefix, string name)
		{
			if (this.ArgList.GetParam(name, prefix) != null)
			{
				return new MathEvaluatorCustomVariable(prefix, name);
			}
			return null;
		}

		// Token: 0x060004A7 RID: 1191 RVA: 0x0000A2E4 File Offset: 0x000084E4
		public override bool PreserveWhitespace(XPathNavigator node)
		{
			return false;
		}

		// Token: 0x060004A8 RID: 1192 RVA: 0x0000A2E4 File Offset: 0x000084E4
		public override int CompareDocument(string baseUri, string nextbaseUri)
		{
			return 0;
		}

		// Token: 0x04000224 RID: 548
		private XsltArgumentList argList;
	}
}
