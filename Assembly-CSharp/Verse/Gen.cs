using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000029 RID: 41
	public static class Gen
	{
		// Token: 0x060001D8 RID: 472 RVA: 0x0007DD1C File Offset: 0x0007BF1C
		public static Vector3 AveragePosition(List<IntVec3> cells)
		{
			return new Vector3((float)cells.Average((IntVec3 c) => c.x) + 0.5f, 0f, (float)cells.Average((IntVec3 c) => c.z) + 0.5f);
		}

		// Token: 0x060001D9 RID: 473 RVA: 0x0000844C File Offset: 0x0000664C
		public static T RandomEnumValue<T>(bool disallowFirstValue)
		{
			return (T)((object)Rand.Range(disallowFirstValue ? 1 : 0, Enum.GetValues(typeof(T)).Length));
		}

		// Token: 0x060001DA RID: 474 RVA: 0x00008478 File Offset: 0x00006678
		public static Vector3 RandomHorizontalVector(float max)
		{
			return new Vector3(Rand.Range(-max, max), 0f, Rand.Range(-max, max));
		}

		// Token: 0x060001DB RID: 475 RVA: 0x0007DD8C File Offset: 0x0007BF8C
		public static int GetBitCountOf(long lValue)
		{
			int num = 0;
			while (lValue != 0L)
			{
				lValue &= lValue - 1L;
				num++;
			}
			return num;
		}

		// Token: 0x060001DC RID: 476 RVA: 0x00008494 File Offset: 0x00006694
		public static IEnumerable<T> GetAllSelectedItems<T>(this Enum value)
		{
			CultureInfo cult = CultureInfo.InvariantCulture;
			int valueAsInt = Convert.ToInt32(value, cult);
			foreach (object obj in Enum.GetValues(typeof(T)))
			{
				int num = Convert.ToInt32(obj, cult);
				if (num == (valueAsInt & num))
				{
					yield return (T)((object)obj);
				}
			}
			IEnumerator enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x060001DD RID: 477 RVA: 0x000084A4 File Offset: 0x000066A4
		public static IEnumerable<T> YieldSingle<T>(T val)
		{
			yield return val;
			yield break;
		}

		// Token: 0x060001DE RID: 478 RVA: 0x000084B4 File Offset: 0x000066B4
		public static IEnumerable YieldSingleNonGeneric<T>(T val)
		{
			yield return val;
			yield break;
		}

		// Token: 0x060001DF RID: 479 RVA: 0x0007DDB0 File Offset: 0x0007BFB0
		public static string ToStringSafe<T>(this T obj)
		{
			if (obj == null)
			{
				return "null";
			}
			string result;
			try
			{
				result = obj.ToString();
			}
			catch (Exception arg)
			{
				int num = 0;
				bool flag = false;
				try
				{
					num = obj.GetHashCode();
					flag = true;
				}
				catch
				{
				}
				if (flag)
				{
					Log.ErrorOnce("Exception in ToString(): " + arg, num ^ 1857461521, false);
				}
				else
				{
					Log.Error("Exception in ToString(): " + arg, false);
				}
				result = "error";
			}
			return result;
		}

		// Token: 0x060001E0 RID: 480 RVA: 0x0007DE4C File Offset: 0x0007C04C
		public static string ToStringSafeEnumerable(this IEnumerable enumerable)
		{
			if (enumerable == null)
			{
				return "null";
			}
			string result;
			try
			{
				string text = "";
				foreach (object obj in enumerable)
				{
					if (text.Length > 0)
					{
						text += ", ";
					}
					text += obj.ToStringSafe<object>();
				}
				result = text;
			}
			catch (Exception arg)
			{
				int num = 0;
				bool flag = false;
				try
				{
					num = enumerable.GetHashCode();
					flag = true;
				}
				catch
				{
				}
				if (flag)
				{
					Log.ErrorOnce("Exception while enumerating: " + arg, num ^ 581736153, false);
				}
				else
				{
					Log.Error("Exception while enumerating: " + arg, false);
				}
				result = "error";
			}
			return result;
		}

		// Token: 0x060001E1 RID: 481 RVA: 0x0007DF38 File Offset: 0x0007C138
		public static void Swap<T>(ref T x, ref T y)
		{
			T t = y;
			y = x;
			x = t;
		}

		// Token: 0x060001E2 RID: 482 RVA: 0x000084C4 File Offset: 0x000066C4
		public static T MemberwiseClone<T>(T obj)
		{
			if (Gen.s_memberwiseClone == null)
			{
				Gen.s_memberwiseClone = typeof(object).GetMethod("MemberwiseClone", BindingFlags.Instance | BindingFlags.NonPublic);
			}
			return (T)((object)Gen.s_memberwiseClone.Invoke(obj, null));
		}

		// Token: 0x060001E3 RID: 483 RVA: 0x0007DF60 File Offset: 0x0007C160
		public static int FixedTimeStepUpdate(ref float timeBuffer, float fps)
		{
			timeBuffer += Mathf.Min(Time.deltaTime, 1f);
			float num = 1f / fps;
			int num2 = Mathf.FloorToInt(timeBuffer / num);
			timeBuffer -= (float)num2 * num;
			return num2;
		}

		// Token: 0x060001E4 RID: 484 RVA: 0x0007DFA0 File Offset: 0x0007C1A0
		public static int HashCombine<T>(int seed, T obj)
		{
			int num = (obj == null) ? 0 : obj.GetHashCode();
			return (int)((long)seed ^ (long)num + (long)((ulong)-1640531527) + (long)((long)seed << 6) + (long)(seed >> 2));
		}

		// Token: 0x060001E5 RID: 485 RVA: 0x00008504 File Offset: 0x00006704
		public static int HashCombineStruct<T>(int seed, T obj) where T : struct
		{
			return (int)((long)seed ^ (long)obj.GetHashCode() + (long)((ulong)-1640531527) + (long)((long)seed << 6) + (long)(seed >> 2));
		}

		// Token: 0x060001E6 RID: 486 RVA: 0x00008529 File Offset: 0x00006729
		public static int HashCombineInt(int seed, int value)
		{
			return (int)((long)seed ^ (long)value + (long)((ulong)-1640531527) + (long)((long)seed << 6) + (long)(seed >> 2));
		}

		// Token: 0x060001E7 RID: 487 RVA: 0x0007DFE0 File Offset: 0x0007C1E0
		public static int HashCombineInt(int v1, int v2, int v3, int v4)
		{
			int num = 352654597;
			int num2 = num;
			num = ((num << 5) + num + (num >> 27) ^ v1);
			num2 = ((num2 << 5) + num2 + (num2 >> 27) ^ v2);
			num = ((num << 5) + num + (num >> 27) ^ v3);
			num2 = ((num2 << 5) + num2 + (num2 >> 27) ^ v4);
			return num + num2 * 1566083941;
		}

		// Token: 0x060001E8 RID: 488 RVA: 0x00008542 File Offset: 0x00006742
		public static int HashOffset(this int baseInt)
		{
			return Gen.HashCombineInt(baseInt, 169495093);
		}

		// Token: 0x060001E9 RID: 489 RVA: 0x0000854F File Offset: 0x0000674F
		public static int HashOffset(this Thing t)
		{
			return t.thingIDNumber.HashOffset();
		}

		// Token: 0x060001EA RID: 490 RVA: 0x0000855C File Offset: 0x0000675C
		public static int HashOffset(this WorldObject o)
		{
			return o.ID.HashOffset();
		}

		// Token: 0x060001EB RID: 491 RVA: 0x00008569 File Offset: 0x00006769
		public static bool IsHashIntervalTick(this Thing t, int interval)
		{
			return t.HashOffsetTicks() % interval == 0;
		}

		// Token: 0x060001EC RID: 492 RVA: 0x00008576 File Offset: 0x00006776
		public static int HashOffsetTicks(this Thing t)
		{
			return Find.TickManager.TicksGame + t.thingIDNumber.HashOffset();
		}

		// Token: 0x060001ED RID: 493 RVA: 0x0000858E File Offset: 0x0000678E
		public static bool IsHashIntervalTick(this WorldObject o, int interval)
		{
			return o.HashOffsetTicks() % interval == 0;
		}

		// Token: 0x060001EE RID: 494 RVA: 0x0000859B File Offset: 0x0000679B
		public static int HashOffsetTicks(this WorldObject o)
		{
			return Find.TickManager.TicksGame + o.ID.HashOffset();
		}

		// Token: 0x060001EF RID: 495 RVA: 0x000085B3 File Offset: 0x000067B3
		public static bool IsHashIntervalTick(this Faction f, int interval)
		{
			return f.HashOffsetTicks() % interval == 0;
		}

		// Token: 0x060001F0 RID: 496 RVA: 0x000085C0 File Offset: 0x000067C0
		public static int HashOffsetTicks(this Faction f)
		{
			return Find.TickManager.TicksGame + f.randomKey.HashOffset();
		}

		// Token: 0x060001F1 RID: 497 RVA: 0x0007E034 File Offset: 0x0007C234
		public static bool IsNestedHashIntervalTick(this Thing t, int outerInterval, int approxInnerInterval)
		{
			int num = Mathf.Max(Mathf.RoundToInt((float)approxInnerInterval / (float)outerInterval), 1);
			return t.HashOffsetTicks() / outerInterval % num == 0;
		}

		// Token: 0x060001F2 RID: 498 RVA: 0x0007E060 File Offset: 0x0007C260
		public static void ReplaceNullFields<T>(ref T replaceIn, T replaceWith)
		{
			if (replaceIn == null || replaceWith == null)
			{
				return;
			}
			foreach (FieldInfo fieldInfo in typeof(T).GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
			{
				if (fieldInfo.GetValue(replaceIn) == null)
				{
					object value = fieldInfo.GetValue(replaceWith);
					if (value != null)
					{
						object obj = replaceIn;
						fieldInfo.SetValue(obj, value);
						replaceIn = (T)((object)obj);
					}
				}
			}
		}

		// Token: 0x060001F3 RID: 499 RVA: 0x0007E0F0 File Offset: 0x0007C2F0
		public static void EnsureAllFieldsNullable(Type type)
		{
			foreach (FieldInfo fieldInfo in type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
			{
				Type fieldType = fieldInfo.FieldType;
				if (fieldType.IsValueType && !(Nullable.GetUnderlyingType(fieldType) != null))
				{
					Log.Warning(string.Concat(new string[]
					{
						"Field ",
						type.Name,
						".",
						fieldInfo.Name,
						" is not nullable."
					}), false);
				}
			}
		}

		// Token: 0x060001F4 RID: 500 RVA: 0x0007E170 File Offset: 0x0007C370
		public static string GetNonNullFieldsDebugInfo(object obj)
		{
			if (obj == null)
			{
				return "";
			}
			StringBuilder stringBuilder = new StringBuilder();
			foreach (FieldInfo fieldInfo in obj.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
			{
				object value = fieldInfo.GetValue(obj);
				if (value != null)
				{
					if (stringBuilder.Length > 0)
					{
						stringBuilder.Append(", ");
					}
					stringBuilder.Append(fieldInfo.Name + "=" + value.ToStringSafe<object>());
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x04000092 RID: 146
		private static MethodInfo s_memberwiseClone;
	}
}
