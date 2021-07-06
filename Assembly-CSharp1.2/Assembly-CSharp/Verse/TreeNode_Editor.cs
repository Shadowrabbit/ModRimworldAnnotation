using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200073F RID: 1855
	public class TreeNode_Editor : TreeNode
	{
		// Token: 0x17000707 RID: 1799
		// (get) Token: 0x06002E96 RID: 11926 RVA: 0x000247EB File Offset: 0x000229EB
		public object ParentObj
		{
			get
			{
				return ((TreeNode_Editor)this.parentNode).obj;
			}
		}

		// Token: 0x17000708 RID: 1800
		// (get) Token: 0x06002E97 RID: 11927 RVA: 0x0013834C File Offset: 0x0013654C
		public Type ObjectType
		{
			get
			{
				if (this.owningField != null)
				{
					return this.owningField.FieldType;
				}
				if (this.IsListItem)
				{
					return this.ListRootObject.GetType().GetGenericArguments()[0];
				}
				if (this.obj != null)
				{
					return this.obj.GetType();
				}
				throw new InvalidOperationException();
			}
		}

		// Token: 0x17000709 RID: 1801
		// (get) Token: 0x06002E98 RID: 11928 RVA: 0x001383A8 File Offset: 0x001365A8
		// (set) Token: 0x06002E99 RID: 11929 RVA: 0x00138418 File Offset: 0x00136618
		public object Value
		{
			get
			{
				if (this.owningField != null)
				{
					return this.owningField.GetValue(this.ParentObj);
				}
				if (this.IsListItem)
				{
					return this.ListRootObject.GetType().GetProperty("Item").GetValue(this.ListRootObject, new object[]
					{
						this.owningIndex
					});
				}
				throw new InvalidOperationException();
			}
			set
			{
				if (this.owningField != null)
				{
					this.owningField.SetValue(this.ParentObj, value);
				}
				if (this.IsListItem)
				{
					this.ListRootObject.GetType().GetProperty("Item").SetValue(this.ListRootObject, value, new object[]
					{
						this.owningIndex
					});
				}
			}
		}

		// Token: 0x1700070A RID: 1802
		// (get) Token: 0x06002E9A RID: 11930 RVA: 0x000247FD File Offset: 0x000229FD
		public bool IsListItem
		{
			get
			{
				return this.owningIndex >= 0;
			}
		}

		// Token: 0x1700070B RID: 1803
		// (get) Token: 0x06002E9B RID: 11931 RVA: 0x0002480B File Offset: 0x00022A0B
		private object ListRootObject
		{
			get
			{
				return this.ParentObj;
			}
		}

		// Token: 0x1700070C RID: 1804
		// (get) Token: 0x06002E9C RID: 11932 RVA: 0x00138484 File Offset: 0x00136684
		public override bool Openable
		{
			get
			{
				return this.obj != null && this.nodeType != EditTreeNodeType.TerminalValue && (this.nodeType != EditTreeNodeType.ListRoot || (int)this.obj.GetType().GetProperty("Count").GetValue(this.obj, null) != 0);
			}
		}

		// Token: 0x1700070D RID: 1805
		// (get) Token: 0x06002E9D RID: 11933 RVA: 0x00024813 File Offset: 0x00022A13
		public bool HasContentLines
		{
			get
			{
				return this.nodeType != EditTreeNodeType.TerminalValue;
			}
		}

		// Token: 0x1700070E RID: 1806
		// (get) Token: 0x06002E9E RID: 11934 RVA: 0x00024821 File Offset: 0x00022A21
		public bool HasNewButton
		{
			get
			{
				return (this.nodeType == EditTreeNodeType.ComplexObject && this.obj == null) || (this.owningField != null && this.owningField.FieldType.HasAttribute<EditorReplaceableAttribute>());
			}
		}

		// Token: 0x1700070F RID: 1807
		// (get) Token: 0x06002E9F RID: 11935 RVA: 0x00024858 File Offset: 0x00022A58
		public bool HasDeleteButton
		{
			get
			{
				return this.IsListItem || (this.owningField != null && this.owningField.FieldType.HasAttribute<EditorNullableAttribute>());
			}
		}

		// Token: 0x17000710 RID: 1808
		// (get) Token: 0x06002EA0 RID: 11936 RVA: 0x001384DC File Offset: 0x001366DC
		public string ExtraInfoText
		{
			get
			{
				if (this.obj == null)
				{
					return "null";
				}
				if (this.obj.GetType().HasAttribute<EditorShowClassNameAttribute>())
				{
					return this.obj.GetType().Name;
				}
				if (this.obj.GetType().IsGenericType && this.obj.GetType().GetGenericTypeDefinition() == typeof(List<>))
				{
					int num = (int)this.obj.GetType().GetProperty("Count").GetValue(this.obj, null);
					return string.Concat(new string[]
					{
						"(",
						num.ToString(),
						" ",
						(num == 1) ? "element" : "elements",
						")"
					});
				}
				return "";
			}
		}

		// Token: 0x17000711 RID: 1809
		// (get) Token: 0x06002EA1 RID: 11937 RVA: 0x00024887 File Offset: 0x00022A87
		public string LabelText
		{
			get
			{
				if (this.owningField != null)
				{
					return this.owningField.Name;
				}
				if (this.IsListItem)
				{
					return this.owningIndex.ToString();
				}
				return this.ObjectType.Name;
			}
		}

		// Token: 0x06002EA2 RID: 11938 RVA: 0x000248C2 File Offset: 0x00022AC2
		private TreeNode_Editor()
		{
		}

		// Token: 0x06002EA3 RID: 11939 RVA: 0x000248D8 File Offset: 0x00022AD8
		public static TreeNode_Editor NewRootNode(object rootObj)
		{
			if (rootObj.GetType().IsValueEditable())
			{
				throw new ArgumentException();
			}
			TreeNode_Editor treeNode_Editor = new TreeNode_Editor();
			treeNode_Editor.owningField = null;
			treeNode_Editor.obj = rootObj;
			treeNode_Editor.nestDepth = 0;
			treeNode_Editor.RebuildChildNodes();
			treeNode_Editor.InitiallyCacheData();
			return treeNode_Editor;
		}

		// Token: 0x06002EA4 RID: 11940 RVA: 0x001385C0 File Offset: 0x001367C0
		public static TreeNode_Editor NewChildNodeFromField(TreeNode_Editor parent, FieldInfo fieldInfo)
		{
			TreeNode_Editor treeNode_Editor = new TreeNode_Editor();
			treeNode_Editor.parentNode = parent;
			treeNode_Editor.nestDepth = parent.nestDepth + 1;
			treeNode_Editor.owningField = fieldInfo;
			if (!fieldInfo.FieldType.IsValueEditable())
			{
				treeNode_Editor.obj = fieldInfo.GetValue(parent.obj);
				treeNode_Editor.RebuildChildNodes();
			}
			treeNode_Editor.InitiallyCacheData();
			return treeNode_Editor;
		}

		// Token: 0x06002EA5 RID: 11941 RVA: 0x0013861C File Offset: 0x0013681C
		private static TreeNode_Editor NewChildNodeFromListItem(TreeNode_Editor parent, int listIndex)
		{
			TreeNode_Editor treeNode_Editor = new TreeNode_Editor();
			treeNode_Editor.parentNode = parent;
			treeNode_Editor.nestDepth = parent.nestDepth + 1;
			treeNode_Editor.owningIndex = listIndex;
			object obj = parent.obj;
			Type type = obj.GetType();
			if (!type.GetGenericArguments()[0].IsValueEditable())
			{
				object value = type.GetProperty("Item").GetValue(obj, new object[]
				{
					listIndex
				});
				treeNode_Editor.obj = value;
				treeNode_Editor.RebuildChildNodes();
			}
			treeNode_Editor.InitiallyCacheData();
			return treeNode_Editor;
		}

		// Token: 0x06002EA6 RID: 11942 RVA: 0x001386A0 File Offset: 0x001368A0
		private void InitiallyCacheData()
		{
			if (this.obj != null && this.obj.GetType().IsGenericType && this.obj.GetType().GetGenericTypeDefinition() == typeof(List<>))
			{
				this.nodeType = EditTreeNodeType.ListRoot;
			}
			else if (this.ObjectType.IsValueEditable())
			{
				this.nodeType = EditTreeNodeType.TerminalValue;
			}
			else
			{
				this.nodeType = EditTreeNodeType.ComplexObject;
			}
			if (this.obj != null)
			{
				this.editWidgetsMethod = this.obj.GetType().GetMethod("DoEditWidgets");
			}
		}

		// Token: 0x06002EA7 RID: 11943 RVA: 0x00138734 File Offset: 0x00136934
		public void RebuildChildNodes()
		{
			if (this.obj == null)
			{
				return;
			}
			this.children = new List<TreeNode>();
			Type objType = this.obj.GetType();
			if (objType.IsGenericType && objType.GetGenericTypeDefinition() == typeof(List<>))
			{
				int num = (int)objType.GetProperty("Count").GetValue(this.obj, null);
				for (int i = 0; i < num; i++)
				{
					TreeNode_Editor item = TreeNode_Editor.NewChildNodeFromListItem(this, i);
					this.children.Add(item);
				}
				return;
			}
			IEnumerable<FieldInfo> fields = this.obj.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			Func<FieldInfo, int> <>9__0;
			Func<FieldInfo, int> keySelector;
			if ((keySelector = <>9__0) == null)
			{
				keySelector = (<>9__0 = ((FieldInfo f) => this.InheritanceDistanceBetween(objType, f.DeclaringType)));
			}
			foreach (FieldInfo fieldInfo in fields.OrderByDescending(keySelector))
			{
				if (fieldInfo.GetCustomAttributes(typeof(UnsavedAttribute), true).Length == 0 && fieldInfo.GetCustomAttributes(typeof(EditorHiddenAttribute), true).Length == 0)
				{
					TreeNode_Editor item2 = TreeNode_Editor.NewChildNodeFromField(this, fieldInfo);
					this.children.Add(item2);
				}
			}
		}

		// Token: 0x06002EA8 RID: 11944 RVA: 0x00138894 File Offset: 0x00136A94
		private int InheritanceDistanceBetween(Type childType, Type parentType)
		{
			Type type = childType;
			int num = 0;
			while (!(type == parentType))
			{
				type = type.BaseType;
				num++;
				if (type == null)
				{
					Log.Error(childType + " is not a subclass of " + parentType, false);
					return -1;
				}
			}
			return num;
		}

		// Token: 0x06002EA9 RID: 11945 RVA: 0x001388D8 File Offset: 0x00136AD8
		public void CheckLatentDelete()
		{
			if (this.indexToDelete >= 0)
			{
				this.obj.GetType().GetMethod("RemoveAt").Invoke(this.obj, new object[]
				{
					this.indexToDelete
				});
				this.RebuildChildNodes();
				this.indexToDelete = -1;
			}
		}

		// Token: 0x06002EAA RID: 11946 RVA: 0x00138930 File Offset: 0x00136B30
		public void Delete()
		{
			if (this.owningField != null)
			{
				this.owningField.SetValue(this.obj, null);
				return;
			}
			if (this.IsListItem)
			{
				((TreeNode_Editor)this.parentNode).indexToDelete = this.owningIndex;
				return;
			}
			throw new InvalidOperationException();
		}

		// Token: 0x06002EAB RID: 11947 RVA: 0x00138984 File Offset: 0x00136B84
		public void DoSpecialPreElements(Listing_TreeDefs listing)
		{
			if (this.obj == null)
			{
				return;
			}
			if (this.editWidgetsMethod != null)
			{
				WidgetRow widgetRow = listing.StartWidgetsRow(this.nestDepth);
				this.editWidgetsMethod.Invoke(this.obj, new object[]
				{
					widgetRow
				});
			}
			Editable editable = this.obj as Editable;
			if (editable != null)
			{
				GUI.color = new Color(1f, 0.5f, 0.5f, 1f);
				foreach (string text in editable.ConfigErrors())
				{
					listing.InfoText(text, this.nestDepth);
				}
				GUI.color = Color.white;
			}
		}

		// Token: 0x06002EAC RID: 11948 RVA: 0x00138A50 File Offset: 0x00136C50
		public override string ToString()
		{
			string text = "EditTreeNode(";
			if (this.ParentObj != null)
			{
				text = text + " owningObj=" + this.ParentObj;
			}
			if (this.owningField != null)
			{
				text = text + " owningField=" + this.owningField;
			}
			if (this.owningIndex >= 0)
			{
				text = text + " owningIndex=" + this.owningIndex;
			}
			return text + ")";
		}

		// Token: 0x04001FBD RID: 8125
		public object obj;

		// Token: 0x04001FBE RID: 8126
		public FieldInfo owningField;

		// Token: 0x04001FBF RID: 8127
		public int owningIndex = -1;

		// Token: 0x04001FC0 RID: 8128
		private MethodInfo editWidgetsMethod;

		// Token: 0x04001FC1 RID: 8129
		public EditTreeNodeType nodeType;

		// Token: 0x04001FC2 RID: 8130
		private int indexToDelete = -1;
	}
}
