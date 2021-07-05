using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000456 RID: 1110
	public static class DialogDatabase
	{
		// Token: 0x06002194 RID: 8596 RVA: 0x000D1C4F File Offset: 0x000CFE4F
		static DialogDatabase()
		{
			DialogDatabase.LoadAllDialog();
		}

		// Token: 0x06002195 RID: 8597 RVA: 0x000D1C6C File Offset: 0x000CFE6C
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

		// Token: 0x06002196 RID: 8598 RVA: 0x000D1D80 File Offset: 0x000CFF80
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

		// Token: 0x06002197 RID: 8599 RVA: 0x000D1E00 File Offset: 0x000D0000
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
			Log.Error("Did not find node named '" + NodeName + "'.");
			return null;
		}

		// Token: 0x040014F7 RID: 5367
		private static List<DiaNodeMold> Nodes = new List<DiaNodeMold>();

		// Token: 0x040014F8 RID: 5368
		private static List<DiaNodeList> NodeLists = new List<DiaNodeList>();
	}
}
