using System;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace Verse
{
	// Token: 0x02000080 RID: 128
	public class MathEvaluatorCustomVariable : IXsltContextVariable
	{
		// Token: 0x170000D9 RID: 217
		// (get) Token: 0x060004BC RID: 1212 RVA: 0x0000A2E4 File Offset: 0x000084E4
		public bool IsLocal
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170000DA RID: 218
		// (get) Token: 0x060004BD RID: 1213 RVA: 0x0000A2E4 File Offset: 0x000084E4
		public bool IsParam
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170000DB RID: 219
		// (get) Token: 0x060004BE RID: 1214 RVA: 0x0000A332 File Offset: 0x00008532
		public XPathResultType VariableType
		{
			get
			{
				return XPathResultType.Any;
			}
		}

		// Token: 0x060004BF RID: 1215 RVA: 0x0000A335 File Offset: 0x00008535
		public MathEvaluatorCustomVariable(string prefix, string name)
		{
			this.prefix = prefix;
			this.name = name;
		}

		// Token: 0x060004C0 RID: 1216 RVA: 0x0000A34B File Offset: 0x0000854B
		public object Evaluate(XsltContext xsltContext)
		{
			return ((MathEvaluatorCustomContext)xsltContext).ArgList.GetParam(this.name, this.prefix);
		}

		// Token: 0x0400022C RID: 556
		private string prefix;

		// Token: 0x0400022D RID: 557
		private string name;
	}
}
