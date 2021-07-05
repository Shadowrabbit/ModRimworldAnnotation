using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200040E RID: 1038
	public class TreeNode_Editor : TreeNode
	{
		// Token: 0x170005F3 RID: 1523
		// (get) Token: 0x06001F0C RID: 7948 RVA: 0x000C1C88 File Offset: 0x000BFE88
		public object ParentObj
		{
			get
			{
				return ((TreeNode_Editor)this.parentNode).obj;
			}
		}

		// Token: 0x170005F4 RID: 1524
		// (get) Token: 0x06001F0D RID: 7949 RVA: 0x000C1C9C File Offset: 0x000BFE9C
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

		// Token: 0x170005F5 RID: 1525
		// (get) Token: 0x06001F0E RID: 7950 RVA: 0x000C1CF8 File Offset: 0x000BFEF8
		// (set) Token: 0x06001F0F RID: 7951 RVA: 0x000C1D68 File Offset: 0x000BFF68
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

		// Token: 0x170005F6 RID: 1526
		// (get) Token: 0x06001F10 RID: 7952 RVA: 0x000C1DD2 File Offset: 0x000BFFD2
		public bool IsListItem
		{
			get
			{
				return this.owningIndex >= 0;
			}
		}

		// Token: 0x170005F7 RID: 1527
		// (get) Token: 0x06001F11 RID: 7953 RVA: 0x000C1DE0 File Offset: 0x000BFFE0
		private object ListRootObject
		{
			get
			{
				return this.ParentObj;
			}
		}

		// Token: 0x170005F8 RID: 1528
		// (get) Token: 0x06001F12 RID: 7954 RVA: 0x000C1DE8 File Offset: 0x000BFFE8
		public override bool Openable
		{
			get
			{
				return this.obj != null && this.nodeType != EditTreeNodeType.TerminalValue && (this.nodeType != EditTreeNodeType.ListRoot || (int)this.obj.GetType().GetProperty("Count").GetValue(this.obj, null) != 0);
			}
		}

		// Token: 0x170005F9 RID: 1529
		// (get) Token: 0x06001F13 RID: 7955 RVA: 0x000C1E3E File Offset: 0x000C003E
		public bool HasContentLines
		{
			get
			{
				return this.nodeType != EditTreeNodeType.TerminalValue;
			}
		}

		// Token: 0x170005FA RID: 1530
		// (get) Token: 0x06001F14 RID: 7956 RVA: 0x000C1E4C File Offset: 0x000C004C
		public bool HasNewButton
		{
			get
			{
				return (this.nodeType == EditTreeNodeType.ComplexObject && this.obj == null) || (this.owningField != null && this.owningField.FieldType.HasAttribute<EditorReplaceableAttribute>());
			}
		}

		// Token: 0x170005FB RID: 1531
		// (get) Token: 0x06001F15 RID: 7957 RVA: 0x000C1E83 File Offset: 0x000C0083
		public bool HasDeleteButton
		{
			get
			{
				return this.IsListItem || (this.owningField != null && this.owningField.FieldType.HasAttribute<EditorNullableAttribute>());
			}
		}

		// Token: 0x170005FC RID: 1532
		// (get) Token: 0x06001F16 RID: 7958 RVA: 0x000C1EB4 File Offset: 0x000C00B4
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

		// Token: 0x170005FD RID: 1533
		// (get) Token: 0x06001F17 RID: 7959 RVA: 0x000C1F95 File Offset: 0x000C0195
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

		// Token: 0x06001F18 RID: 7960 RVA: 0x000C1FD0 File Offset: 0x000C01D0
		private TreeNode_Editor()
		{
		}

		// Token: 0x06001F19 RID: 7961 RVA: 0x000C1FE6 File Offset: 0x000C01E6
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

		// Token: 0x06001F1A RID: 7962 RVA: 0x000C2024 File Offset: 0x000C0224
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

		// Token: 0x06001F1B RID: 7963 RVA: 0x000C2080 File Offset: 0x000C0280
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

		// Token: 0x06001F1C RID: 7964 RVA: 0x000C2104 File Offset: 0x000C0304
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

		// Token: 0x06001F1D RID: 7965 RVA: 0x000C2198 File Offset: 0x000C0398
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

		// Token: 0x06001F1E RID: 7966 RVA: 0x000C22F8 File Offset: 0x000C04F8
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
					Log.Error(childType + " is not a subclass of " + parentType);
					return -1;
				}
			}
			return num;
		}

		// Token: 0x06001F1F RID: 7967 RVA: 0x000C233C File Offset: 0x000C053C
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

		// Token: 0x06001F20 RID: 7968 RVA: 0x000C2394 File Offset: 0x000C0594
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

		// Token: 0x06001F21 RID: 7969 RVA: 0x000C23E8 File Offset: 0x000C05E8
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

		// Token: 0x06001F22 RID: 7970 RVA: 0x000C24B4 File Offset: 0x000C06B4
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

		// Token: 0x040012EF RID: 4847
		public object obj;

		// Token: 0x040012F0 RID: 4848
		public FieldInfo owningField;

		// Token: 0x040012F1 RID: 4849
		public int owningIndex = -1;

		// Token: 0x040012F2 RID: 4850
		private MethodInfo editWidgetsMethod;

		// Token: 0x040012F3 RID: 4851
		public EditTreeNodeType nodeType;

		// Token: 0x040012F4 RID: 4852
		private int indexToDelete = -1;
	}
}
