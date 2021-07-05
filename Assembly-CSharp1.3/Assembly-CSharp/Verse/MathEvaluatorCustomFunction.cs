using System;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace Verse
{
	// Token: 0x02000042 RID: 66
	public class MathEvaluatorCustomFunction : IXsltContextFunction
	{
		// Token: 0x17000087 RID: 135
		// (get) Token: 0x06000379 RID: 889 RVA: 0x00012C1C File Offset: 0x00010E1C
		public XPathResultType[] ArgTypes
		{
			get
			{
				return this.argTypes;
			}
		}

		// Token: 0x17000088 RID: 136
		// (get) Token: 0x0600037A RID: 890 RVA: 0x00012C24 File Offset: 0x00010E24
		public int Maxargs
		{
			get
			{
				return this.functionType.maxArgs;
			}
		}

		// Token: 0x17000089 RID: 137
		// (get) Token: 0x0600037B RID: 891 RVA: 0x00012C31 File Offset: 0x00010E31
		public int Minargs
		{
			get
			{
				return this.functionType.minArgs;
			}
		}

		// Token: 0x1700008A RID: 138
		// (get) Token: 0x0600037C RID: 892 RVA: 0x0001276E File Offset: 0x0001096E
		public XPathResultType ReturnType
		{
			get
			{
				return XPathResultType.Number;
			}
		}

		// Token: 0x0600037D RID: 893 RVA: 0x00012C3E File Offset: 0x00010E3E
		public MathEvaluatorCustomFunction(MathEvaluatorCustomFunctions.FunctionType functionType, XPathResultType[] argTypes)
		{
			this.functionType = functionType;
			this.argTypes = argTypes;
		}

		// Token: 0x0600037E RID: 894 RVA: 0x00012C54 File Offset: 0x00010E54
		public object Invoke(XsltContext xsltContext, object[] args, XPathNavigator docContext)
		{
			return this.functionType.func(args);
		}

		// Token: 0x040000D3 RID: 211
		private XPathResultType[] argTypes;

		// Token: 0x040000D4 RID: 212
		private MathEvaluatorCustomFunctions.FunctionType functionType;
	}
}
