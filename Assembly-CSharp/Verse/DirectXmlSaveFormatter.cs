using System;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Verse
{
	// Token: 0x0200048D RID: 1165
	public static class DirectXmlSaveFormatter
	{
		// Token: 0x06001D4E RID: 7502 RVA: 0x000F40F8 File Offset: 0x000F22F8
		public static void AddWhitespaceFromRoot(XElement root)
		{
			if (!root.Elements().Any<XElement>())
			{
				return;
			}
			foreach (XNode xnode in root.Elements().ToList<XElement>())
			{
				XText content = new XText("\n");
				xnode.AddAfterSelf(content);
			}
			root.Elements().First<XElement>().AddBeforeSelf(new XText("\n"));
			root.Elements().Last<XElement>().AddAfterSelf(new XText("\n"));
			foreach (XElement element in root.Elements().ToList<XElement>())
			{
				DirectXmlSaveFormatter.IndentXml(element, 1);
			}
		}

		// Token: 0x06001D4F RID: 7503 RVA: 0x000F41E0 File Offset: 0x000F23E0
		private static void IndentXml(XElement element, int depth)
		{
			element.AddBeforeSelf(new XText(DirectXmlSaveFormatter.IndentString(depth, true)));
			bool startWithNewline = element.NextNode == null;
			element.AddAfterSelf(new XText(DirectXmlSaveFormatter.IndentString(depth - 1, startWithNewline)));
			foreach (XElement element2 in element.Elements().ToList<XElement>())
			{
				DirectXmlSaveFormatter.IndentXml(element2, depth + 1);
			}
		}

		// Token: 0x06001D50 RID: 7504 RVA: 0x000F4268 File Offset: 0x000F2468
		private static string IndentString(int depth, bool startWithNewline)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (startWithNewline)
			{
				stringBuilder.Append("\n");
			}
			for (int i = 0; i < depth; i++)
			{
				stringBuilder.Append("  ");
			}
			return stringBuilder.ToString();
		}
	}
}
