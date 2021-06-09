using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml;

namespace Verse
{
	// Token: 0x02000497 RID: 1175
	public static class XmlInheritance
	{
		// Token: 0x06001D72 RID: 7538 RVA: 0x000F5B70 File Offset: 0x000F3D70
		static XmlInheritance()
		{
			foreach (Type type in GenTypes.AllTypes)
			{
				foreach (FieldInfo fieldInfo in type.GetFields())
				{
					if (fieldInfo.IsDefined(typeof(XmlInheritanceAllowDuplicateNodes), false))
					{
						XmlInheritance.allowDuplicateNodesFieldNames.Add(fieldInfo.Name);
					}
				}
			}
		}

		// Token: 0x06001D73 RID: 7539 RVA: 0x000F5C24 File Offset: 0x000F3E24
		public static void TryRegisterAllFrom(LoadableXmlAsset xmlAsset, ModContentPack mod)
		{
			if (xmlAsset.xmlDoc == null)
			{
				return;
			}
			DeepProfiler.Start("XmlInheritance.TryRegisterAllFrom");
			foreach (object obj in xmlAsset.xmlDoc.DocumentElement.ChildNodes)
			{
				XmlNode xmlNode = (XmlNode)obj;
				if (xmlNode.NodeType == XmlNodeType.Element)
				{
					XmlInheritance.TryRegister(xmlNode, mod);
				}
			}
			DeepProfiler.End();
		}

		// Token: 0x06001D74 RID: 7540 RVA: 0x000F5CA8 File Offset: 0x000F3EA8
		public static void TryRegister(XmlNode node, ModContentPack mod)
		{
			XmlAttribute xmlAttribute = node.Attributes["Name"];
			XmlAttribute xmlAttribute2 = node.Attributes["ParentName"];
			if (xmlAttribute == null && xmlAttribute2 == null)
			{
				return;
			}
			List<XmlInheritance.XmlInheritanceNode> list = null;
			if (xmlAttribute != null && XmlInheritance.nodesByName.TryGetValue(xmlAttribute.Value, out list))
			{
				int i = 0;
				while (i < list.Count)
				{
					if (list[i].mod == mod)
					{
						if (mod == null)
						{
							Log.Error("XML error: Could not register node named \"" + xmlAttribute.Value + "\" because this name is already used.", false);
							return;
						}
						Log.Error(string.Concat(new string[]
						{
							"XML error: Could not register node named \"",
							xmlAttribute.Value,
							"\" in mod ",
							mod.ToString(),
							" because this name is already used in this mod."
						}), false);
						return;
					}
					else
					{
						i++;
					}
				}
			}
			XmlInheritance.XmlInheritanceNode xmlInheritanceNode = new XmlInheritance.XmlInheritanceNode();
			xmlInheritanceNode.xmlNode = node;
			xmlInheritanceNode.mod = mod;
			XmlInheritance.unresolvedNodes.Add(xmlInheritanceNode);
			if (xmlAttribute != null)
			{
				if (list != null)
				{
					list.Add(xmlInheritanceNode);
					return;
				}
				list = new List<XmlInheritance.XmlInheritanceNode>();
				list.Add(xmlInheritanceNode);
				XmlInheritance.nodesByName.Add(xmlAttribute.Value, list);
			}
		}

		// Token: 0x06001D75 RID: 7541 RVA: 0x0001A673 File Offset: 0x00018873
		public static void Resolve()
		{
			XmlInheritance.ResolveParentsAndChildNodesLinks();
			XmlInheritance.ResolveXmlNodes();
		}

		// Token: 0x06001D76 RID: 7542 RVA: 0x000F5DC8 File Offset: 0x000F3FC8
		public static XmlNode GetResolvedNodeFor(XmlNode originalNode)
		{
			if (originalNode.Attributes["ParentName"] != null)
			{
				XmlInheritance.XmlInheritanceNode xmlInheritanceNode;
				if (XmlInheritance.resolvedNodes.TryGetValue(originalNode, out xmlInheritanceNode))
				{
					return xmlInheritanceNode.resolvedXmlNode;
				}
				if (XmlInheritance.unresolvedNodes.Any((XmlInheritance.XmlInheritanceNode x) => x.xmlNode == originalNode))
				{
					Log.Error("XML error: XML node \"" + originalNode.Name + "\" has not been resolved yet. There's probably a Resolve() call missing somewhere.", false);
				}
				else
				{
					Log.Error("XML error: Tried to get resolved node for node \"" + originalNode.Name + "\" which uses a ParentName attribute, but it is not in a resolved nodes collection, which means that it was never registered or there was an error while resolving it.", false);
				}
			}
			return originalNode;
		}

		// Token: 0x06001D77 RID: 7543 RVA: 0x0001A67F File Offset: 0x0001887F
		public static void Clear()
		{
			XmlInheritance.resolvedNodes.Clear();
			XmlInheritance.unresolvedNodes.Clear();
			XmlInheritance.nodesByName.Clear();
		}

		// Token: 0x06001D78 RID: 7544 RVA: 0x000F5E74 File Offset: 0x000F4074
		private static void ResolveParentsAndChildNodesLinks()
		{
			for (int i = 0; i < XmlInheritance.unresolvedNodes.Count; i++)
			{
				XmlAttribute xmlAttribute = XmlInheritance.unresolvedNodes[i].xmlNode.Attributes["ParentName"];
				if (xmlAttribute != null)
				{
					XmlInheritance.unresolvedNodes[i].parent = XmlInheritance.GetBestParentFor(XmlInheritance.unresolvedNodes[i], xmlAttribute.Value);
					if (XmlInheritance.unresolvedNodes[i].parent != null)
					{
						XmlInheritance.unresolvedNodes[i].parent.children.Add(XmlInheritance.unresolvedNodes[i]);
					}
				}
			}
		}

		// Token: 0x06001D79 RID: 7545 RVA: 0x000F5F1C File Offset: 0x000F411C
		private static void ResolveXmlNodes()
		{
			List<XmlInheritance.XmlInheritanceNode> list = (from x in XmlInheritance.unresolvedNodes
			where x.parent == null || x.parent.resolvedXmlNode != null
			select x).ToList<XmlInheritance.XmlInheritanceNode>();
			for (int i = 0; i < list.Count; i++)
			{
				XmlInheritance.ResolveXmlNodesRecursively(list[i]);
			}
			for (int j = 0; j < XmlInheritance.unresolvedNodes.Count; j++)
			{
				if (XmlInheritance.unresolvedNodes[j].resolvedXmlNode == null)
				{
					Log.Error("XML error: Cyclic inheritance hierarchy detected for node \"" + XmlInheritance.unresolvedNodes[j].xmlNode.Name + "\". Full node: " + XmlInheritance.unresolvedNodes[j].xmlNode.OuterXml, false);
				}
				else
				{
					XmlInheritance.resolvedNodes.Add(XmlInheritance.unresolvedNodes[j].xmlNode, XmlInheritance.unresolvedNodes[j]);
				}
			}
			XmlInheritance.unresolvedNodes.Clear();
		}

		// Token: 0x06001D7A RID: 7546 RVA: 0x000F6010 File Offset: 0x000F4210
		private static void ResolveXmlNodesRecursively(XmlInheritance.XmlInheritanceNode node)
		{
			if (node.resolvedXmlNode != null)
			{
				Log.Error("XML error: Cyclic inheritance hierarchy detected for node \"" + node.xmlNode.Name + "\". Full node: " + node.xmlNode.OuterXml, false);
				return;
			}
			XmlInheritance.ResolveXmlNodeFor(node);
			for (int i = 0; i < node.children.Count; i++)
			{
				XmlInheritance.ResolveXmlNodesRecursively(node.children[i]);
			}
		}

		// Token: 0x06001D7B RID: 7547 RVA: 0x000F6080 File Offset: 0x000F4280
		private static XmlInheritance.XmlInheritanceNode GetBestParentFor(XmlInheritance.XmlInheritanceNode node, string parentName)
		{
			XmlInheritance.XmlInheritanceNode xmlInheritanceNode = null;
			List<XmlInheritance.XmlInheritanceNode> list;
			if (XmlInheritance.nodesByName.TryGetValue(parentName, out list))
			{
				if (node.mod == null)
				{
					for (int i = 0; i < list.Count; i++)
					{
						if (list[i].mod == null)
						{
							xmlInheritanceNode = list[i];
							break;
						}
					}
					if (xmlInheritanceNode == null)
					{
						for (int j = 0; j < list.Count; j++)
						{
							if (xmlInheritanceNode == null || list[j].mod.loadOrder < xmlInheritanceNode.mod.loadOrder)
							{
								xmlInheritanceNode = list[j];
							}
						}
					}
				}
				else
				{
					for (int k = 0; k < list.Count; k++)
					{
						if (list[k].mod != null && list[k].mod.loadOrder <= node.mod.loadOrder && (xmlInheritanceNode == null || list[k].mod.loadOrder > xmlInheritanceNode.mod.loadOrder))
						{
							xmlInheritanceNode = list[k];
						}
					}
					if (xmlInheritanceNode == null)
					{
						for (int l = 0; l < list.Count; l++)
						{
							if (list[l].mod == null)
							{
								xmlInheritanceNode = list[l];
								break;
							}
						}
					}
				}
			}
			if (xmlInheritanceNode == null)
			{
				Log.Error(string.Concat(new string[]
				{
					"XML error: Could not find parent node named \"",
					parentName,
					"\" for node \"",
					node.xmlNode.Name,
					"\". Full node: ",
					node.xmlNode.OuterXml
				}), false);
				return null;
			}
			return xmlInheritanceNode;
		}

		// Token: 0x06001D7C RID: 7548 RVA: 0x000F6208 File Offset: 0x000F4408
		private static void ResolveXmlNodeFor(XmlInheritance.XmlInheritanceNode node)
		{
			if (node.parent == null)
			{
				node.resolvedXmlNode = node.xmlNode;
				return;
			}
			if (node.parent.resolvedXmlNode == null)
			{
				Log.Error("XML error: Internal error. Tried to resolve node whose parent has not been resolved yet. This means that this method was called in incorrect order.", false);
				node.resolvedXmlNode = node.xmlNode;
				return;
			}
			XmlInheritance.CheckForDuplicateNodes(node.xmlNode, node.xmlNode);
			XmlNode xmlNode = node.parent.resolvedXmlNode.CloneNode(true);
			XmlInheritance.RecursiveNodeCopyOverwriteElements(node.xmlNode, xmlNode);
			node.resolvedXmlNode = xmlNode;
		}

		// Token: 0x06001D7D RID: 7549 RVA: 0x000F6288 File Offset: 0x000F4488
		private static void RecursiveNodeCopyOverwriteElements(XmlNode child, XmlNode current)
		{
			DeepProfiler.Start("RecursiveNodeCopyOverwriteElements");
			try
			{
				XmlAttribute xmlAttribute = child.Attributes["Inherit"];
				if (xmlAttribute != null && xmlAttribute.Value.ToLower() == "false")
				{
					while (current.HasChildNodes)
					{
						current.RemoveChild(current.FirstChild);
					}
					using (IEnumerator enumerator = child.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							object obj = enumerator.Current;
							XmlNode node = (XmlNode)obj;
							XmlNode newChild = current.OwnerDocument.ImportNode(node, true);
							current.AppendChild(newChild);
						}
						return;
					}
				}
				current.Attributes.RemoveAll();
				XmlAttributeCollection attributes = child.Attributes;
				for (int i = 0; i < attributes.Count; i++)
				{
					XmlAttribute node2 = (XmlAttribute)current.OwnerDocument.ImportNode(attributes[i], true);
					current.Attributes.Append(node2);
				}
				List<XmlElement> list = new List<XmlElement>();
				XmlNode xmlNode = null;
				foreach (object obj2 in child)
				{
					XmlNode xmlNode2 = (XmlNode)obj2;
					if (xmlNode2.NodeType == XmlNodeType.Text)
					{
						xmlNode = xmlNode2;
					}
					else if (xmlNode2.NodeType == XmlNodeType.Element)
					{
						list.Add((XmlElement)xmlNode2);
					}
				}
				if (xmlNode != null)
				{
					DeepProfiler.Start("RecursiveNodeCopyOverwriteElements - Remove all current nodes");
					for (int j = current.ChildNodes.Count - 1; j >= 0; j--)
					{
						XmlNode xmlNode3 = current.ChildNodes[j];
						if (xmlNode3.NodeType != XmlNodeType.Attribute)
						{
							current.RemoveChild(xmlNode3);
						}
					}
					DeepProfiler.End();
					XmlNode newChild2 = current.OwnerDocument.ImportNode(xmlNode, true);
					current.AppendChild(newChild2);
				}
				else
				{
					if (!list.Any<XmlElement>())
					{
						bool flag = false;
						using (IEnumerator enumerator = current.ChildNodes.GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								if (((XmlNode)enumerator.Current).NodeType == XmlNodeType.Element)
								{
									flag = true;
									break;
								}
							}
						}
						if (flag)
						{
							goto IL_2F0;
						}
						using (IEnumerator enumerator = current.ChildNodes.GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								object obj3 = enumerator.Current;
								XmlNode xmlNode4 = (XmlNode)obj3;
								if (xmlNode4.NodeType != XmlNodeType.Attribute)
								{
									current.RemoveChild(xmlNode4);
								}
							}
							return;
						}
					}
					for (int k = 0; k < list.Count; k++)
					{
						XmlElement xmlElement = list[k];
						if (XmlInheritance.IsListElement(xmlElement))
						{
							XmlNode newChild3 = current.OwnerDocument.ImportNode(xmlElement, true);
							current.AppendChild(newChild3);
						}
						else
						{
							XmlElement xmlElement2 = current[xmlElement.Name];
							if (xmlElement2 != null)
							{
								XmlInheritance.RecursiveNodeCopyOverwriteElements(xmlElement, xmlElement2);
							}
							else
							{
								XmlNode newChild4 = current.OwnerDocument.ImportNode(xmlElement, true);
								current.AppendChild(newChild4);
							}
						}
					}
					IL_2F0:;
				}
			}
			finally
			{
				DeepProfiler.End();
			}
		}

		// Token: 0x06001D7E RID: 7550 RVA: 0x000F660C File Offset: 0x000F480C
		private static void CheckForDuplicateNodes(XmlNode node, XmlNode root)
		{
			XmlInheritance.tempUsedNodeNames.Clear();
			foreach (object obj in node.ChildNodes)
			{
				XmlNode xmlNode = (XmlNode)obj;
				if (xmlNode.NodeType == XmlNodeType.Element && !XmlInheritance.IsListElement(xmlNode))
				{
					if (XmlInheritance.tempUsedNodeNames.Contains(xmlNode.Name))
					{
						Log.Error(string.Concat(new string[]
						{
							"XML error: Duplicate XML node name ",
							xmlNode.Name,
							" in this XML block: ",
							node.OuterXml,
							(node != root) ? ("\n\nRoot node: " + root.OuterXml) : ""
						}), false);
					}
					else
					{
						XmlInheritance.tempUsedNodeNames.Add(xmlNode.Name);
					}
				}
			}
			XmlInheritance.tempUsedNodeNames.Clear();
			foreach (object obj2 in node.ChildNodes)
			{
				XmlNode xmlNode2 = (XmlNode)obj2;
				if (xmlNode2.NodeType == XmlNodeType.Element)
				{
					XmlInheritance.CheckForDuplicateNodes(xmlNode2, root);
				}
			}
		}

		// Token: 0x06001D7F RID: 7551 RVA: 0x0001A69F File Offset: 0x0001889F
		private static bool IsListElement(XmlNode node)
		{
			return node.Name == "li" || (node.ParentNode != null && XmlInheritance.allowDuplicateNodesFieldNames.Contains(node.ParentNode.Name));
		}

		// Token: 0x0400150C RID: 5388
		private static Dictionary<XmlNode, XmlInheritance.XmlInheritanceNode> resolvedNodes = new Dictionary<XmlNode, XmlInheritance.XmlInheritanceNode>();

		// Token: 0x0400150D RID: 5389
		private static List<XmlInheritance.XmlInheritanceNode> unresolvedNodes = new List<XmlInheritance.XmlInheritanceNode>();

		// Token: 0x0400150E RID: 5390
		private static Dictionary<string, List<XmlInheritance.XmlInheritanceNode>> nodesByName = new Dictionary<string, List<XmlInheritance.XmlInheritanceNode>>();

		// Token: 0x0400150F RID: 5391
		public static HashSet<string> allowDuplicateNodesFieldNames = new HashSet<string>();

		// Token: 0x04001510 RID: 5392
		private const string NameAttributeName = "Name";

		// Token: 0x04001511 RID: 5393
		private const string ParentNameAttributeName = "ParentName";

		// Token: 0x04001512 RID: 5394
		private const string InheritAttributeName = "Inherit";

		// Token: 0x04001513 RID: 5395
		private static HashSet<string> tempUsedNodeNames = new HashSet<string>();

		// Token: 0x02000498 RID: 1176
		private class XmlInheritanceNode
		{
			// Token: 0x04001514 RID: 5396
			public XmlNode xmlNode;

			// Token: 0x04001515 RID: 5397
			public XmlNode resolvedXmlNode;

			// Token: 0x04001516 RID: 5398
			public ModContentPack mod;

			// Token: 0x04001517 RID: 5399
			public XmlInheritance.XmlInheritanceNode parent;

			// Token: 0x04001518 RID: 5400
			public List<XmlInheritance.XmlInheritanceNode> children = new List<XmlInheritance.XmlInheritanceNode>();
		}
	}
}
