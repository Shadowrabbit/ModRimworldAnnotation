using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200040C RID: 1036
	public class Listing_TreeDefs : Listing_Tree
	{
		// Token: 0x170005F2 RID: 1522
		// (get) Token: 0x06001F03 RID: 7939 RVA: 0x000C149C File Offset: 0x000BF69C
		protected override float LabelWidth
		{
			get
			{
				return this.labelWidthInt;
			}
		}

		// Token: 0x06001F04 RID: 7940 RVA: 0x000C14A4 File Offset: 0x000BF6A4
		public Listing_TreeDefs(float labelColumnWidth)
		{
			this.labelWidthInt = labelColumnWidth;
		}

		// Token: 0x06001F05 RID: 7941 RVA: 0x000C14B4 File Offset: 0x000BF6B4
		public void ContentLines(TreeNode_Editor node, int indentLevel)
		{
			node.DoSpecialPreElements(this);
			if (node.children == null)
			{
				Log.Error(node + " children is null.");
				return;
			}
			for (int i = 0; i < node.children.Count; i++)
			{
				this.Node((TreeNode_Editor)node.children[i], indentLevel, 64);
			}
		}

		// Token: 0x06001F06 RID: 7942 RVA: 0x000C1514 File Offset: 0x000BF714
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
			if (this.IsOpen(node, openMask))
			{
				this.ContentLines(node, indentLevel + 1);
			}
			if (node.nodeType == EditTreeNodeType.ListRoot)
			{
				node.CheckLatentDelete();
			}
		}

		// Token: 0x06001F07 RID: 7943 RVA: 0x000C15E4 File Offset: 0x000BF7E4
		private void ControlButtonsRight(TreeNode_Editor node, WidgetRow widgetRow)
		{
			if (node.HasNewButton && widgetRow.ButtonIcon(TexButton.NewItem, null, null, null, null, true))
			{
				Action<object> addAction = delegate(object o)
				{
					node.owningField.SetValue(node.ParentObj, o);
					((TreeNode_Editor)node.parentNode).RebuildChildNodes();
				};
				this.MakeCreateNewObjectMenu(node, node.owningField, node.owningField.FieldType, addAction);
			}
			if (node.nodeType == EditTreeNodeType.ListRoot && widgetRow.ButtonIcon(TexButton.Add, null, null, null, null, true))
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
			if (node.HasDeleteButton && widgetRow.ButtonIcon(TexButton.DeleteX, null, new Color?(GenUI.SubtleMouseoverColor), null, null, true))
			{
				node.Delete();
			}
		}

		// Token: 0x06001F08 RID: 7944 RVA: 0x000C1728 File Offset: 0x000BF928
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
				widgetRow.Label(extraInfoText, -1f, null, -1f);
				GUI.color = Color.white;
			}
		}

		// Token: 0x06001F09 RID: 7945 RVA: 0x000C17B4 File Offset: 0x000BF9B4
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
			base.LabelLeft(node.LabelText, tipText, indentLevel, 0f, null);
		}

		// Token: 0x06001F0A RID: 7946 RVA: 0x000C181C File Offset: 0x000BFA1C
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
				list2.Add(new FloatMenuOption(type.ToString(), action, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
			}
			Find.WindowStack.Add(new FloatMenu(list2));
		}

		// Token: 0x06001F0B RID: 7947 RVA: 0x000C18D8 File Offset: 0x000BFAD8
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
						}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
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

		// Token: 0x040012EA RID: 4842
		private float labelWidthInt;
	}
}
