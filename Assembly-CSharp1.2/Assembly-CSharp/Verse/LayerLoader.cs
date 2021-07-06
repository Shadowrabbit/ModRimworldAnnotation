using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using System.Xml.XPath;
using UnityEngine;

namespace Verse
{
	// Token: 0x020007A3 RID: 1955
	public static class LayerLoader
	{
		// Token: 0x06003146 RID: 12614 RVA: 0x00145100 File Offset: 0x00143300
		public static void LoadFileIntoList(TextAsset ass, List<DiaNodeMold> NodeListToFill, List<DiaNodeList> ListListToFill, DiaNodeType NodesType)
		{
			XPathNavigator xpathNavigator = new XPathDocument(new StringReader(ass.text)).CreateNavigator();
			xpathNavigator.MoveToFirst();
			xpathNavigator.MoveToFirstChild();
			foreach (object obj in xpathNavigator.Select("Node"))
			{
				XPathNavigator xpathNavigator2 = (XPathNavigator)obj;
				try
				{
					TextReader textReader = new StringReader(xpathNavigator2.OuterXml);
					DiaNodeMold diaNodeMold = (DiaNodeMold)new XmlSerializer(typeof(DiaNodeMold)).Deserialize(textReader);
					diaNodeMold.nodeType = NodesType;
					NodeListToFill.Add(diaNodeMold);
					textReader.Dispose();
				}
				catch (Exception ex)
				{
					Log.Message(string.Concat(new object[]
					{
						"Exception deserializing ",
						xpathNavigator2.OuterXml,
						":\n",
						ex.InnerException
					}), false);
				}
			}
			foreach (object obj2 in xpathNavigator.Select("NodeList"))
			{
				XPathNavigator xpathNavigator3 = (XPathNavigator)obj2;
				try
				{
					TextReader textReader2 = new StringReader(xpathNavigator3.OuterXml);
					DiaNodeList item = (DiaNodeList)new XmlSerializer(typeof(DiaNodeList)).Deserialize(textReader2);
					ListListToFill.Add(item);
				}
				catch (Exception ex2)
				{
					Log.Message(string.Concat(new object[]
					{
						"Exception deserializing ",
						xpathNavigator3.OuterXml,
						":\n",
						ex2.InnerException
					}), false);
				}
			}
		}

		// Token: 0x06003147 RID: 12615 RVA: 0x001452D0 File Offset: 0x001434D0
		public static void MarkNonRootNodes(List<DiaNodeMold> NodeList)
		{
			foreach (DiaNodeMold d in NodeList)
			{
				LayerLoader.RecursiveSetIsRootFalse(d);
			}
			foreach (DiaNodeMold diaNodeMold in NodeList)
			{
				foreach (DiaNodeMold diaNodeMold2 in NodeList)
				{
					foreach (DiaOptionMold diaOptionMold in diaNodeMold2.optionList)
					{
						bool flag = false;
						using (List<string>.Enumerator enumerator4 = diaOptionMold.ChildNodeNames.GetEnumerator())
						{
							while (enumerator4.MoveNext())
							{
								if (enumerator4.Current == diaNodeMold.name)
								{
									flag = true;
								}
							}
						}
						if (flag)
						{
							diaNodeMold.isRoot = false;
						}
					}
				}
			}
		}

		// Token: 0x06003148 RID: 12616 RVA: 0x00145424 File Offset: 0x00143624
		private static void RecursiveSetIsRootFalse(DiaNodeMold d)
		{
			foreach (DiaOptionMold diaOptionMold in d.optionList)
			{
				foreach (DiaNodeMold diaNodeMold in diaOptionMold.ChildNodes)
				{
					diaNodeMold.isRoot = false;
					LayerLoader.RecursiveSetIsRootFalse(diaNodeMold);
				}
			}
		}
	}
}
