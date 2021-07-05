using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml;

namespace Verse
{
	// Token: 0x0200031B RID: 795
	public static class XmlInheritance
	{
		// Token: 0x060016BA RID: 5818 RVA: 0x00085764 File Offset: 0x00083964
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

		// Token: 0x060016BB RID: 5819 RVA: 0x00085818 File Offset: 0x00083A18
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

		// Token: 0x060016BC RID: 5820 RVA: 0x0008589C File Offset: 0x00083A9C
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
							Log.Error("XML error: Could not register node named \"" + xmlAttribute.Value + "\" because this name is already used.");
							return;
						}
						Log.Error(string.Concat(new string[]
						{
							"XML error: Could not register node named \"",
							xmlAttribute.Value,
							"\" in mod ",
							mod.ToString(),
							" because this name is already used in this mod."
						}));
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

		// Token: 0x060016BD RID: 5821 RVA: 0x000859B7 File Offset: 0x00083BB7
		public static void Resolve()
		{
			XmlInheritance.ResolveParentsAndChildNodesLinks();
			XmlInheritance.ResolveXmlNodes();
		}

		// Token: 0x060016BE RID: 5822 RVA: 0x000859C4 File Offset: 0x00083BC4
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
					Log.Error("XML error: XML node \"" + originalNode.Name + "\" has not been resolved yet. There's probably a Resolve() call missing somewhere.");
				}
				else
				{
					Log.Error("XML error: Tried to get resolved node for node \"" + originalNode.Name + "\" which uses a ParentName attribute, but it is not in a resolved nodes collection, which means that it was never registered or there was an error while resolving it.");
				}
			}
			return originalNode;
		}

		// Token: 0x060016BF RID: 5823 RVA: 0x00085A6E File Offset: 0x00083C6E
		public static void Clear()
		{
			XmlInheritance.resolvedNodes.Clear();
			XmlInheritance.unresolvedNodes.Clear();
			XmlInheritance.nodesByName.Clear();
		}

		// Token: 0x060016C0 RID: 5824 RVA: 0x00085A90 File Offset: 0x00083C90
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

		// Token: 0x060016C1 RID: 5825 RVA: 0x00085B38 File Offset: 0x00083D38
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
					Log.Error("XML error: Cyclic inheritance hierarchy detected for node \"" + XmlInheritance.unresolvedNodes[j].xmlNode.Name + "\". Full node: " + XmlInheritance.unresolvedNodes[j].xmlNode.OuterXml);
				}
				else
				{
					XmlInheritance.resolvedNodes.Add(XmlInheritance.unresolvedNodes[j].xmlNode, XmlInheritance.unresolvedNodes[j]);
				}
			}
			XmlInheritance.unresolvedNodes.Clear();
		}

		// Token: 0x060016C2 RID: 5826 RVA: 0x00085C2C File Offset: 0x00083E2C
		private static void ResolveXmlNodesRecursively(XmlInheritance.XmlInheritanceNode node)
		{
			if (node.resolvedXmlNode != null)
			{
				Log.Error("XML error: Cyclic inheritance hierarchy detected for node \"" + node.xmlNode.Name + "\". Full node: " + node.xmlNode.OuterXml);
				return;
			}
			XmlInheritance.ResolveXmlNodeFor(node);
			for (int i = 0; i < node.children.Count; i++)
			{
				XmlInheritance.ResolveXmlNodesRecursively(node.children[i]);
			}
		}

		// Token: 0x060016C3 RID: 5827 RVA: 0x00085C9C File Offset: 0x00083E9C
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
				}));
				return null;
			}
			return xmlInheritanceNode;
		}

		// Token: 0x060016C4 RID: 5828 RVA: 0x00085E24 File Offset: 0x00084024
		private static void ResolveXmlNodeFor(XmlInheritance.XmlInheritanceNode node)
		{
			if (node.parent == null)
			{
				node.resolvedXmlNode = node.xmlNode;
				return;
			}
			if (node.parent.resolvedXmlNode == null)
			{
				Log.Error("XML error: Internal error. Tried to resolve node whose parent has not been resolved yet. This means that this method was called in incorrect order.");
				node.resolvedXmlNode = node.xmlNode;
				return;
			}
			XmlInheritance.CheckForDuplicateNodes(node.xmlNode, node.xmlNode);
			XmlNode xmlNode = node.parent.resolvedXmlNode.CloneNode(true);
			XmlInheritance.RecursiveNodeCopyOverwriteElements(node.xmlNode, xmlNode);
			node.resolvedXmlNode = xmlNode;
		}

		// Token: 0x060016C5 RID: 5829 RVA: 0x00085EA0 File Offset: 0x000840A0
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
					foreach (object obj in child)
					{
						XmlNode node = (XmlNode)obj;
						XmlNode newChild = current.OwnerDocument.ImportNode(node, true);
						current.AppendChild(newChild);
					}
					using (IEnumerator enumerator = child.Attributes.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							object obj2 = enumerator.Current;
							XmlAttribute xmlAttribute2 = (XmlAttribute)obj2;
							if (!(xmlAttribute2.Name == "Inherit"))
							{
								XmlAttribute xmlAttribute3 = current.OwnerDocument.CreateAttribute(xmlAttribute2.Name);
								xmlAttribute3.Value = xmlAttribute2.Value;
								current.Attributes.Append(xmlAttribute3);
							}
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
				foreach (object obj3 in child)
				{
					XmlNode xmlNode2 = (XmlNode)obj3;
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
							goto IL_373;
						}
						using (IEnumerator enumerator = current.ChildNodes.GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								object obj4 = enumerator.Current;
								XmlNode xmlNode4 = (XmlNode)obj4;
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
					IL_373:;
				}
			}
			finally
			{
				DeepProfiler.End();
			}
		}

		// Token: 0x060016C6 RID: 5830 RVA: 0x000862BC File Offset: 0x000844BC
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
						}));
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

		// Token: 0x060016C7 RID: 5831 RVA: 0x00086400 File Offset: 0x00084600
		private static bool IsListElement(XmlNode node)
		{
			return node.Name == "li" || (node.ParentNode != null && XmlInheritance.allowDuplicateNodesFieldNames.Contains(node.ParentNode.Name));
		}

		// Token: 0x04000FCF RID: 4047
		private static Dictionary<XmlNode, XmlInheritance.XmlInheritanceNode> resolvedNodes = new Dictionary<XmlNode, XmlInheritance.XmlInheritanceNode>();

		// Token: 0x04000FD0 RID: 4048
		private static List<XmlInheritance.XmlInheritanceNode> unresolvedNodes = new List<XmlInheritance.XmlInheritanceNode>();

		// Token: 0x04000FD1 RID: 4049
		private static Dictionary<string, List<XmlInheritance.XmlInheritanceNode>> nodesByName = new Dictionary<string, List<XmlInheritance.XmlInheritanceNode>>();

		// Token: 0x04000FD2 RID: 4050
		public static HashSet<string> allowDuplicateNodesFieldNames = new HashSet<string>();

		// Token: 0x04000FD3 RID: 4051
		private const string NameAttributeName = "Name";

		// Token: 0x04000FD4 RID: 4052
		private const string ParentNameAttributeName = "ParentName";

		// Token: 0x04000FD5 RID: 4053
		private const string InheritAttributeName = "Inherit";

		// Token: 0x04000FD6 RID: 4054
		private static HashSet<string> tempUsedNodeNames = new HashSet<string>();

		// Token: 0x02001A50 RID: 6736
		private class XmlInheritanceNode
		{
			// Token: 0x040064BC RID: 25788
			public XmlNode xmlNode;

			// Token: 0x040064BD RID: 25789
			public XmlNode resolvedXmlNode;

			// Token: 0x040064BE RID: 25790
			public ModContentPack mod;

			// Token: 0x040064BF RID: 25791
			public XmlInheritance.XmlInheritanceNode parent;

			// Token: 0x040064C0 RID: 25792
			public List<XmlInheritance.XmlInheritanceNode> children = new List<XmlInheritance.XmlInheritanceNode>();
		}
	}
}
