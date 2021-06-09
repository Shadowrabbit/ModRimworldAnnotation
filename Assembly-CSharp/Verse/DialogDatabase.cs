using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x020007A2 RID: 1954
	public static class DialogDatabase
	{
		// Token: 0x06003142 RID: 12610 RVA: 0x00026DBF File Offset: 0x00024FBF
		static DialogDatabase()
		{
			DialogDatabase.LoadAllDialog();
		}

		// Token: 0x06003143 RID: 12611 RVA: 0x00144EA0 File Offset: 0x001430A0
		private static void LoadAllDialog()
		{
			DialogDatabase.Nodes.Clear();
			foreach (UnityEngine.Object @object in Resources.LoadAll("Dialog", typeof(TextAsset)))
			{
				TextAsset ass = @object as TextAsset;
				if (@object.name == "BaseEncounters" || @object.name == "GeneratedDialogs")
				{
					LayerLoader.LoadFileIntoList(ass, DialogDatabase.Nodes, DialogDatabase.NodeLists, DiaNodeType.BaseEncounters);
				}
				if (@object.name == "InsanityBattles")
				{
					LayerLoader.LoadFileIntoList(ass, DialogDatabase.Nodes, DialogDatabase.NodeLists, DiaNodeType.InsanityBattles);
				}
				if (@object.name == "SpecialEncounters")
				{
					LayerLoader.LoadFileIntoList(ass, DialogDatabase.Nodes, DialogDatabase.NodeLists, DiaNodeType.Special);
				}
			}
			foreach (DiaNodeMold diaNodeMold in DialogDatabase.Nodes)
			{
				diaNodeMold.PostLoad();
			}
			LayerLoader.MarkNonRootNodes(DialogDatabase.Nodes);
		}

		// Token: 0x06003144 RID: 12612 RVA: 0x00144FB4 File Offset: 0x001431B4
		public static DiaNodeMold GetRandomEncounterRootNode(DiaNodeType NType)
		{
			List<DiaNodeMold> list = new List<DiaNodeMold>();
			foreach (DiaNodeMold diaNodeMold in DialogDatabase.Nodes)
			{
				if (diaNodeMold.isRoot && (!diaNodeMold.unique || !diaNodeMold.used) && diaNodeMold.nodeType == NType)
				{
					list.Add(diaNodeMold);
				}
			}
			return list.RandomElement<DiaNodeMold>();
		}

		// Token: 0x06003145 RID: 12613 RVA: 0x00145034 File Offset: 0x00143234
		public static DiaNodeMold GetNodeNamed(string NodeName)
		{
			foreach (DiaNodeMold diaNodeMold in DialogDatabase.Nodes)
			{
				if (diaNodeMold.name == NodeName)
				{
					return diaNodeMold;
				}
			}
			foreach (DiaNodeList diaNodeList in DialogDatabase.NodeLists)
			{
				if (diaNodeList.Name == NodeName)
				{
					return diaNodeList.RandomNodeFromList();
				}
			}
			Log.Error("Did not find node named '" + NodeName + "'.", false);
			return null;
		}

		// Token: 0x040021FF RID: 8703
		private static List<DiaNodeMold> Nodes = new List<DiaNodeMold>();

		// Token: 0x04002200 RID: 8704
		private static List<DiaNodeList> NodeLists = new List<DiaNodeList>();
	}
}
