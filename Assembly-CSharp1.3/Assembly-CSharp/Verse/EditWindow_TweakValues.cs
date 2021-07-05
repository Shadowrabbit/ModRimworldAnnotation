using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000392 RID: 914
	public class EditWindow_TweakValues : EditWindow
	{
		// Token: 0x17000581 RID: 1409
		// (get) Token: 0x06001AD7 RID: 6871 RVA: 0x0009BD05 File Offset: 0x00099F05
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(1000f, 600f);
			}
		}

		// Token: 0x17000582 RID: 1410
		// (get) Token: 0x06001AD8 RID: 6872 RVA: 0x000126F5 File Offset: 0x000108F5
		public override bool IsDebug
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06001AD9 RID: 6873 RVA: 0x0009BD18 File Offset: 0x00099F18
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

		// Token: 0x06001ADA RID: 6874 RVA: 0x0009BD82 File Offset: 0x00099F82
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
							Log.Error(string.Format("Field {0}.{1} is marked with TweakValue, but isn't static; TweakValue won't work", fieldInfo.DeclaringType.FullName, fieldInfo.Name));
						}
						else if (fieldInfo.IsLiteral)
						{
							Log.Error(string.Format("Field {0}.{1} is marked with TweakValue, but is const; TweakValue won't work", fieldInfo.DeclaringType.FullName, fieldInfo.Name));
						}
						else if (fieldInfo.IsInitOnly)
						{
							Log.Error(string.Format("Field {0}.{1} is marked with TweakValue, but is readonly; TweakValue won't work", fieldInfo.DeclaringType.FullName, fieldInfo.Name));
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

		// Token: 0x06001ADB RID: 6875 RVA: 0x0009BD8C File Offset: 0x00099F8C
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
					Log.ErrorOnce(string.Format("Attempted to tweakvalue unknown field type {0}", tweakInfo.field.FieldType), 83944645);
					flag = false;
					num = tweakInfo.initial;
				}
				if (num != tweakInfo.initial)
				{
					GUI.color = Color.red;
					Text.WordWrap = false;
					Widgets.Label(rect4, string.Format("{0} -> {1}", tweakInfo.initial, num));
					Text.WordWrap = true;
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
							Log.ErrorOnce(string.Format("Attempted to tweakvalue unknown field type {0}", tweakInfo.field.FieldType), 83944646);
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

		// Token: 0x06001ADC RID: 6876 RVA: 0x0009C25C File Offset: 0x0009A45C
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
			Log.ErrorOnce(string.Format("Attempted to return unknown field type {0} as a float", field.FieldType), 83944644);
			return 0f;
		}

		// Token: 0x06001ADD RID: 6877 RVA: 0x0009C324 File Offset: 0x0009A524
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
			Log.ErrorOnce(string.Format("Attempted to set unknown field type {0} from a float", field.FieldType), 83944645);
		}

		// Token: 0x0400117B RID: 4475
		[TweakValue("TweakValue", 0f, 300f)]
		public static float CategoryWidth = 180f;

		// Token: 0x0400117C RID: 4476
		[TweakValue("TweakValue", 0f, 300f)]
		public static float TitleWidth = 300f;

		// Token: 0x0400117D RID: 4477
		[TweakValue("TweakValue", 0f, 300f)]
		public static float NumberWidth = 140f;

		// Token: 0x0400117E RID: 4478
		private Vector2 scrollPosition;

		// Token: 0x0400117F RID: 4479
		private static List<EditWindow_TweakValues.TweakInfo> tweakValueFields;

		// Token: 0x02001A90 RID: 6800
		private struct TweakInfo
		{
			// Token: 0x040065AD RID: 26029
			public FieldInfo field;

			// Token: 0x040065AE RID: 26030
			public TweakValue tweakValue;

			// Token: 0x040065AF RID: 26031
			public float initial;
		}
	}
}
