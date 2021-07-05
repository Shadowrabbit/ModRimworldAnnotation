using System;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace Verse
{
	// Token: 0x02000043 RID: 67
	public class MathEvaluatorCustomVariable : IXsltContextVariable
	{
		// Token: 0x1700008B RID: 139
		// (get) Token: 0x0600037F RID: 895 RVA: 0x0001276E File Offset: 0x0001096E
		public bool IsLocal
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700008C RID: 140
		// (get) Token: 0x06000380 RID: 896 RVA: 0x0001276E File Offset: 0x0001096E
		public bool IsParam
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700008D RID: 141
		// (get) Token: 0x06000381 RID: 897 RVA: 0x00012C67 File Offset: 0x00010E67
		public XPathResultType VariableType
		{
			get
			{
				return XPathResultType.Any;
			}
		}

		// Token: 0x06000382 RID: 898 RVA: 0x00012C6A File Offset: 0x00010E6A
		public MathEvaluatorCustomVariable(string prefix, string name)
		{
			this.prefix = prefix;
			this.name = name;
		}

		// Token: 0x06000383 RID: 899 RVA: 0x00012C80 File Offset: 0x00010E80
		public object Evaluate(XsltContext xsltContext)
		{
			return ((MathEvaluatorCustomContext)xsltContext).ArgList.GetParam(this.name, this.prefix);
		}

		// Token: 0x040000D5 RID: 213
		private string prefix;

		// Token: 0x040000D6 RID: 214
		private string name;
	}
}
