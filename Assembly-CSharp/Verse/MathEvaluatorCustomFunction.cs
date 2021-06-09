using System;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace Verse
{
	// Token: 0x0200007F RID: 127
	public class MathEvaluatorCustomFunction : IXsltContextFunction
	{
		// Token: 0x170000D5 RID: 213
		// (get) Token: 0x060004B6 RID: 1206 RVA: 0x0000A2E7 File Offset: 0x000084E7
		public XPathResultType[] ArgTypes
		{
			get
			{
				return this.argTypes;
			}
		}

		// Token: 0x170000D6 RID: 214
		// (get) Token: 0x060004B7 RID: 1207 RVA: 0x0000A2EF File Offset: 0x000084EF
		public int Maxargs
		{
			get
			{
				return this.functionType.maxArgs;
			}
		}

		// Token: 0x170000D7 RID: 215
		// (get) Token: 0x060004B8 RID: 1208 RVA: 0x0000A2FC File Offset: 0x000084FC
		public int Minargs
		{
			get
			{
				return this.functionType.minArgs;
			}
		}

		// Token: 0x170000D8 RID: 216
		// (get) Token: 0x060004B9 RID: 1209 RVA: 0x0000A2E4 File Offset: 0x000084E4
		public XPathResultType ReturnType
		{
			get
			{
				return XPathResultType.Number;
			}
		}

		// Token: 0x060004BA RID: 1210 RVA: 0x0000A309 File Offset: 0x00008509
		public MathEvaluatorCustomFunction(MathEvaluatorCustomFunctions.FunctionType functionType, XPathResultType[] argTypes)
		{
			this.functionType = functionType;
			this.argTypes = argTypes;
		}

		// Token: 0x060004BB RID: 1211 RVA: 0x0000A31F File Offset: 0x0000851F
		public object Invoke(XsltContext xsltContext, object[] args, XPathNavigator docContext)
		{
			return this.functionType.func(args);
		}

		// Token: 0x0400022A RID: 554
		private XPathResultType[] argTypes;

		// Token: 0x0400022B RID: 555
		private MathEvaluatorCustomFunctions.FunctionType functionType;
	}
}
