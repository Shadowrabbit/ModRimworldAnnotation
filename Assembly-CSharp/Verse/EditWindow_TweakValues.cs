using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200053C RID: 1340
	public class EditWindow_TweakValues : EditWindow
	{
		// Token: 0x1700067F RID: 1663
		// (get) Token: 0x0600226C RID: 8812 RVA: 0x0001D923 File Offset: 0x0001BB23
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(1000f, 600f);
			}
		}

		// Token: 0x17000680 RID: 1664
		// (get) Token: 0x0600226D RID: 8813 RVA: 0x0000A2A7 File Offset: 0x000084A7
		public override bool IsDebug
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600226E RID: 8814 RVA: 0x001099FC File Offset: 0x00107BFC
		public EditWindow_TweakValues()
		{
			this.optionalTitle = "TweakValues";
			if (EditWindow_TweakValues.tweakValueFields == null)
			{
				EditWindow_TweakValues.tweakValueFields = (from field in this.FindAllTweakables()
				select new EditWindow_TweakValues.TweakInfo
				{
					field = field,
					tweakValue = field.TryGetAttribute<TweakValue>(),
					initial = this.GetAsFloat(field)
				} into ti
				orderby string.Format("{0}.{1}", ti.tweakValue.category, ti.field.DeclaringType.Name)
				select ti).ToList<EditWindow_TweakValues.TweakInfo>();
			}
		}

		// Token: 0x0600226F RID: 8815 RVA: 0x0001D934 File Offset: 0x0001BB34
		private IEnumerable<FieldInfo> FindAllTweakables()
		{
			foreach (Type type in GenTypes.AllTypes)
			{
				foreach (FieldInfo fieldInfo in type.GetFields(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic))
				{
					if (fieldInfo.TryGetAttribute<TweakValue>() != null)
					{
						if (!fieldInfo.IsStatic)
						{
							Log.Error(string.Format("Field {0}.{1} is marked with TweakValue, but isn't static; TweakValue won't work", fieldInfo.DeclaringType.FullName, fieldInfo.Name), false);
						}
						else if (fieldInfo.IsLiteral)
						{
							Log.Error(string.Format("Field {0}.{1} is marked with TweakValue, but is const; TweakValue won't work", fieldInfo.DeclaringType.FullName, fieldInfo.Name), false);
						}
						else if (fieldInfo.IsInitOnly)
						{
							Log.Error(string.Format("Field {0}.{1} is marked with TweakValue, but is readonly; TweakValue won't work", fieldInfo.DeclaringType.FullName, fieldInfo.Name), false);
						}
						else
						{
							yield return fieldInfo;
						}
					}
				}
				FieldInfo[] array = null;
			}
			IEnumerator<Type> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x06002270 RID: 8816 RVA: 0x00109A68 File Offset: 0x00107C68
		public override void DoWindowContents(Rect inRect)
		{
			Text.Font = GameFont.Small;
			Rect rect;
			Rect outRect = rect = inRect.ContractedBy(4f);
			rect.xMax -= 33f;
			Rect rect2 = new Rect(0f, 0f, EditWindow_TweakValues.CategoryWidth, Text.CalcHeight("test", 1000f));
			Rect rect3 = new Rect(rect2.xMax, 0f, EditWindow_TweakValues.TitleWidth, rect2.height);
			Rect rect4 = new Rect(rect3.xMax, 0f, EditWindow_TweakValues.NumberWidth, rect2.height);
			Rect rect5 = new Rect(rect4.xMax, 0f, rect.width - rect4.xMax, rect2.height);
			Widgets.BeginScrollView(outRect, ref this.scrollPosition, new Rect(0f, 0f, rect.width, rect2.height * (float)EditWindow_TweakValues.tweakValueFields.Count), true);
			foreach (EditWindow_TweakValues.TweakInfo tweakInfo in EditWindow_TweakValues.tweakValueFields)
			{
				Widgets.Label(rect2, tweakInfo.tweakValue.category);
				Widgets.Label(rect3, string.Format("{0}.{1}", tweakInfo.field.DeclaringType.Name, tweakInfo.field.Name));
				float num;
				bool flag;
				if (tweakInfo.field.FieldType == typeof(float) || tweakInfo.field.FieldType == typeof(int) || tweakInfo.field.FieldType == typeof(ushort))
				{
					float asFloat = this.GetAsFloat(tweakInfo.field);
					num = Widgets.HorizontalSlider(rect5, this.GetAsFloat(tweakInfo.field), tweakInfo.tweakValue.min, tweakInfo.tweakValue.max, false, null, null, null, -1f);
					this.SetFromFloat(tweakInfo.field, num);
					flag = (asFloat != num);
				}
				else if (tweakInfo.field.FieldType == typeof(bool))
				{
					bool flag3;
					bool flag2 = flag3 = (bool)tweakInfo.field.GetValue(null);
					Widgets.Checkbox(rect5.xMin, rect5.yMin, ref flag3, 24f, false, false, null, null);
					tweakInfo.field.SetValue(null, flag3);
					num = (float)(flag3 ? 1 : 0);
					flag = (flag2 != flag3);
				}
				else
				{
					Log.ErrorOnce(string.Format("Attempted to tweakvalue unknown field type {0}", tweakInfo.field.FieldType), 83944645, false);
					flag = false;
					num = tweakInfo.initial;
				}
				if (num != tweakInfo.initial)
				{
					GUI.color = Color.red;
					Widgets.Label(rect4, string.Format("{0} -> {1}", tweakInfo.initial, num));
					GUI.color = Color.white;
					if (Widgets.ButtonInvisible(rect4, true))
					{
						flag = true;
						if (tweakInfo.field.FieldType == typeof(float) || tweakInfo.field.FieldType == typeof(int) || tweakInfo.field.FieldType == typeof(ushort))
						{
							this.SetFromFloat(tweakInfo.field, tweakInfo.initial);
						}
						else if (tweakInfo.field.FieldType == typeof(bool))
						{
							tweakInfo.field.SetValue(null, tweakInfo.initial != 0f);
						}
						else
						{
							Log.ErrorOnce(string.Format("Attempted to tweakvalue unknown field type {0}", tweakInfo.field.FieldType), 83944646, false);
						}
					}
				}
				else
				{
					Widgets.Label(rect4, string.Format("{0}", tweakInfo.initial));
				}
				if (flag)
				{
					MethodInfo method = tweakInfo.field.DeclaringType.GetMethod(tweakInfo.field.Name + "_Changed", BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
					if (method != null)
					{
						method.Invoke(null, null);
					}
				}
				rect2.y += rect2.height;
				rect3.y += rect2.height;
				rect4.y += rect2.height;
				rect5.y += rect2.height;
			}
			Widgets.EndScrollView();
		}

		// Token: 0x06002271 RID: 8817 RVA: 0x00109F30 File Offset: 0x00108130
		private float GetAsFloat(FieldInfo field)
		{
			if (field.FieldType == typeof(float))
			{
				return (float)field.GetValue(null);
			}
			if (field.FieldType == typeof(bool))
			{
				return (float)(((bool)field.GetValue(null)) ? 1 : 0);
			}
			if (field.FieldType == typeof(int))
			{
				return (float)((int)field.GetValue(null));
			}
			if (field.FieldType == typeof(ushort))
			{
				return (float)((ushort)field.GetValue(null));
			}
			Log.ErrorOnce(string.Format("Attempted to return unknown field type {0} as a float", field.FieldType), 83944644, false);
			return 0f;
		}

		// Token: 0x06002272 RID: 8818 RVA: 0x00109FF8 File Offset: 0x001081F8
		private void SetFromFloat(FieldInfo field, float input)
		{
			if (field.FieldType == typeof(float))
			{
				field.SetValue(null, input);
				return;
			}
			if (field.FieldType == typeof(bool))
			{
				field.SetValue(null, input != 0f);
				return;
			}
			if (field.FieldType == typeof(int))
			{
				field.SetValue(field, (int)input);
				return;
			}
			if (field.FieldType == typeof(ushort))
			{
				field.SetValue(field, (ushort)input);
				return;
			}
			Log.ErrorOnce(string.Format("Attempted to set unknown field type {0} from a float", field.FieldType), 83944645, false);
		}

		// Token: 0x04001759 RID: 5977
		[TweakValue("TweakValue", 0f, 300f)]
		public static float CategoryWidth = 180f;

		// Token: 0x0400175A RID: 5978
		[TweakValue("TweakValue", 0f, 300f)]
		public static float TitleWidth = 300f;

		// Token: 0x0400175B RID: 5979
		[TweakValue("TweakValue", 0f, 300f)]
		public static float NumberWidth = 140f;

		// Token: 0x0400175C RID: 5980
		private Vector2 scrollPosition;

		// Token: 0x0400175D RID: 5981
		private static List<EditWindow_TweakValues.TweakInfo> tweakValueFields;

		// Token: 0x0200053D RID: 1341
		private struct TweakInfo
		{
			// Token: 0x0400175E RID: 5982
			public FieldInfo field;

			// Token: 0x0400175F RID: 5983
			public TweakValue tweakValue;

			// Token: 0x04001760 RID: 5984
			public float initial;
		}
	}
}
