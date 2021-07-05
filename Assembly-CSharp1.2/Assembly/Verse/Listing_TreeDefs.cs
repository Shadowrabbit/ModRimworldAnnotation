﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000738 RID: 1848
	public class Listing_TreeDefs : Listing_Tree
	{
		// Token: 0x17000706 RID: 1798
		// (get) Token: 0x06002E84 RID: 11908 RVA: 0x00024752 File Offset: 0x00022952
		protected override float LabelWidth
		{
			get
			{
				return this.labelWidthInt;
			}
		}

		// Token: 0x06002E85 RID: 11909 RVA: 0x0002475A File Offset: 0x0002295A
		public Listing_TreeDefs(float labelColumnWidth)
		{
			this.labelWidthInt = labelColumnWidth;
		}

		// Token: 0x06002E86 RID: 11910 RVA: 0x00137B4C File Offset: 0x00135D4C
		public void ContentLines(TreeNode_Editor node, int indentLevel)
		{
			node.DoSpecialPreElements(this);
			if (node.children == null)
			{
				Log.Error(node + " children is null.", false);
				return;
			}
			for (int i = 0; i < node.children.Count; i++)
			{
				this.Node((TreeNode_Editor)node.children[i], indentLevel, 64);
			}
		}

		// Token: 0x06002E87 RID: 11911 RVA: 0x00137BAC File Offset: 0x00135DAC
		private void Node(TreeNode_Editor node, int indentLevel, int openMask)
		{
			if (node.nodeType == EditTreeNodeType.TerminalValue)
			{
				node.DoSpecialPreElements(this);
				base.OpenCloseWidget(node, indentLevel, openMask);
				this.NodeLabelLeft(node, indentLevel);
				WidgetRow widgetRow = new WidgetRow(this.LabelWidth, this.curY, UIDirection.RightThenUp, 99999f, 4f);
				this.ControlButtonsRight(node, widgetRow);
				this.ValueEditWidgetRight(node, widgetRow.FinalX);
				base.EndLine();
				return;
			}
			base.OpenCloseWidget(node, indentLevel, openMask);
			this.NodeLabelLeft(node, indentLevel);
			WidgetRow widgetRow2 = new WidgetRow(this.LabelWidth, this.curY, UIDirection.RightThenUp, 99999f, 4f);
			this.ControlButtonsRight(node, widgetRow2);
			this.ExtraInfoText(node, widgetRow2);
			base.EndLine();
			if (node.IsOpen(openMask))
			{
				this.ContentLines(node, indentLevel + 1);
			}
			if (node.nodeType == EditTreeNodeType.ListRoot)
			{
				node.CheckLatentDelete();
			}
		}

		// Token: 0x06002E88 RID: 11912 RVA: 0x00137C7C File Offset: 0x00135E7C
		private void ControlButtonsRight(TreeNode_Editor node, WidgetRow widgetRow)
		{
			if (node.HasNewButton && widgetRow.ButtonIcon(TexButton.NewItem, null, null, true))
			{
				Action<object> addAction = delegate(object o)
				{
					node.owningField.SetValue(node.ParentObj, o);
					((TreeNode_Editor)node.parentNode).RebuildChildNodes();
				};
				this.MakeCreateNewObjectMenu(node, node.owningField, node.owningField.FieldType, addAction);
			}
			if (node.nodeType == EditTreeNodeType.ListRoot && widgetRow.ButtonIcon(TexButton.Add, null, null, true))
			{
				Type baseType = node.obj.GetType().GetGenericArguments()[0];
				Action<object> addAction2 = delegate(object o)
				{
					node.obj.GetType().GetMethod("Add").Invoke(node.obj, new object[]
					{
						o
					});
				};
				this.MakeCreateNewObjectMenu(node, node.owningField, baseType, addAction2);
			}
			if (node.HasDeleteButton && widgetRow.ButtonIcon(TexButton.DeleteX, null, new Color?(GenUI.SubtleMouseoverColor), true))
			{
				node.Delete();
			}
		}

		// Token: 0x06002E89 RID: 11913 RVA: 0x00137D88 File Offset: 0x00135F88
		private void ExtraInfoText(TreeNode_Editor node, WidgetRow widgetRow)
		{
			string extraInfoText = node.ExtraInfoText;
			if (extraInfoText != "")
			{
				if (extraInfoText == "null")
				{
					GUI.color = new Color(1f, 0.6f, 0.6f, 0.5f);
				}
				else
				{
					GUI.color = new Color(1f, 1f, 1f, 0.5f);
				}
				widgetRow.Label(extraInfoText, -1f);
				GUI.color = Color.white;
			}
		}

		// Token: 0x06002E8A RID: 11914 RVA: 0x00137E0C File Offset: 0x0013600C
		protected void NodeLabelLeft(TreeNode_Editor node, int indentLevel)
		{
			string tipText = "";
			if (node.owningField != null)
			{
				DescriptionAttribute[] array = (DescriptionAttribute[])node.owningField.GetCustomAttributes(typeof(DescriptionAttribute), true);
				if (array.Length != 0)
				{
					tipText = array[0].description;
				}
			}
			base.LabelLeft(node.LabelText, tipText, indentLevel, 0f);
		}

		// Token: 0x06002E8B RID: 11915 RVA: 0x00137E6C File Offset: 0x0013606C
		protected void MakeCreateNewObjectMenu(TreeNode_Editor owningNode, FieldInfo owningField, Type baseType, Action<object> addAction)
		{
			List<Type> list = baseType.InstantiableDescendantsAndSelf().ToList<Type>();
			List<FloatMenuOption> list2 = new List<FloatMenuOption>();
			foreach (Type type in list)
			{
				Type creatingType = type;
				Action action = delegate()
				{
					owningNode.SetOpen(-1, true);
					object obj;
					if (creatingType == typeof(string))
					{
						obj = "";
					}
					else
					{
						obj = Activator.CreateInstance(creatingType);
					}
					addAction(obj);
					if (owningNode != null)
					{
						owningNode.RebuildChildNodes();
					}
				};
				list2.Add(new FloatMenuOption(type.ToString(), action, MenuOptionPriority.Default, null, null, 0f, null, null));
			}
			Find.WindowStack.Add(new FloatMenu(list2));
		}

		// Token: 0x06002E8C RID: 11916 RVA: 0x00137F28 File Offset: 0x00136128
		protected void ValueEditWidgetRight(TreeNode_Editor node, float leftX)
		{
			if (node.nodeType != EditTreeNodeType.TerminalValue)
			{
				throw new ArgumentException();
			}
			Rect rect = new Rect(leftX, this.curY, base.ColumnWidth - leftX, this.lineHeight);
			object obj = node.Value;
			Type objectType = node.ObjectType;
			if (objectType == typeof(string))
			{
				string text = (string)obj;
				string text2 = text;
				if (text2 == null)
				{
					text2 = "";
				}
				string b = text2;
				text2 = Widgets.TextField(rect, text2);
				if (text2 != b)
				{
					text = text2;
				}
				obj = text;
			}
			else if (objectType == typeof(bool))
			{
				bool flag = (bool)obj;
				Widgets.Checkbox(new Vector2(rect.x, rect.y), ref flag, this.lineHeight, false, false, null, null);
				obj = flag;
			}
			else if (objectType == typeof(int))
			{
				rect.width = 100f;
				int num;
				if (int.TryParse(Widgets.TextField(rect, obj.ToString()), out num))
				{
					obj = num;
				}
			}
			else if (objectType == typeof(float))
			{
				EditSliderRangeAttribute[] array = (EditSliderRangeAttribute[])node.owningField.GetCustomAttributes(typeof(EditSliderRangeAttribute), true);
				if (array.Length != 0)
				{
					float num2 = (float)obj;
					num2 = Widgets.HorizontalSlider(new Rect(this.LabelWidth + 60f + 4f, this.curY, base.EditAreaWidth - 60f - 8f, this.lineHeight), num2, array[0].min, array[0].max, false, null, null, null, -1f);
					obj = num2;
				}
				rect.width = 60f;
				string text3 = obj.ToString();
				text3 = Widgets.TextField(rect, text3);
				float num3;
				if (float.TryParse(text3, out num3))
				{
					obj = num3;
				}
			}
			else if (objectType.IsEnum)
			{
				if (Widgets.ButtonText(rect, obj.ToString(), true, true, true))
				{
					List<FloatMenuOption> list = new List<FloatMenuOption>();
					foreach (object obj2 in Enum.GetValues(objectType))
					{
						object localVal = obj2;
						list.Add(new FloatMenuOption(obj2.ToString(), delegate()
						{
							node.Value = localVal;
						}, MenuOptionPriority.Default, null, null, 0f, null, null));
					}
					Find.WindowStack.Add(new FloatMenu(list));
				}
			}
			else if (objectType == typeof(FloatRange))
			{
				float sliderMin = 0f;
				float sliderMax = 100f;
				EditSliderRangeAttribute[] array2 = (EditSliderRangeAttribute[])node.owningField.GetCustomAttributes(typeof(EditSliderRangeAttribute), true);
				if (array2.Length != 0)
				{
					sliderMin = array2[0].min;
					sliderMax = array2[0].max;
				}
				FloatRange floatRange = (FloatRange)obj;
				Widgets.FloatRangeWithTypeIn(rect, rect.GetHashCode(), ref floatRange, sliderMin, sliderMax, ToStringStyle.FloatTwo, null);
				obj = floatRange;
			}
			else
			{
				GUI.color = new Color(1f, 1f, 1f, 0.4f);
				Widgets.Label(rect, "uneditable value type");
				GUI.color = Color.white;
			}
			node.Value = obj;
		}

		// Token: 0x04001FB0 RID: 8112
		private float labelWidthInt;
	}
}
