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
	// Token: 0x02000021 RID: 33
	public static class Gen
	{
		// Token: 0x0600018F RID: 399 RVA: 0x00008110 File Offset: 0x00006310
		public static Vector3 AveragePosition(List<IntVec3> cells)
		{
			return new Vector3((float)cells.Average((IntVec3 c) => c.x) + 0.5f, 0f, (float)cells.Average((IntVec3 c) => c.z) + 0.5f);
		}

		// Token: 0x06000190 RID: 400 RVA: 0x00008180 File Offset: 0x00006380
		public static T RandomEnumValue<T>(bool disallowFirstValue)
		{
			int min = disallowFirstValue ? 1 : 0;
			T[] array = (T[])Enum.GetValues(typeof(T));
			int num = Rand.Range(min, array.Length);
			return array[num];
		}

		// Token: 0x06000191 RID: 401 RVA: 0x000081B9 File Offset: 0x000063B9
		public static Vector3 RandomHorizontalVector(float max)
		{
			return new Vector3(Rand.Range(-max, max), 0f, Rand.Range(-max, max));
		}

		// Token: 0x06000192 RID: 402 RVA: 0x000081D8 File Offset: 0x000063D8
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

		// Token: 0x06000193 RID: 403 RVA: 0x000081F9 File Offset: 0x000063F9
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

		// Token: 0x06000194 RID: 404 RVA: 0x00008209 File Offset: 0x00006409
		public static IEnumerable<T> YieldSingle<T>(T val)
		{
			yield return val;
			yield break;
		}

		// Token: 0x06000195 RID: 405 RVA: 0x00008219 File Offset: 0x00006419
		public static IEnumerable YieldSingleNonGeneric<T>(T val)
		{
			yield return val;
			yield break;
		}

		// Token: 0x06000196 RID: 406 RVA: 0x0000822C File Offset: 0x0000642C
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
					Log.ErrorOnce("Exception in ToString(): " + arg, num ^ 1857461521);
				}
				else
				{
					Log.Error("Exception in ToString(): " + arg);
				}
				result = "error";
			}
			return result;
		}

		// Token: 0x06000197 RID: 407 RVA: 0x000082C4 File Offset: 0x000064C4
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
					Log.ErrorOnce("Exception while enumerating: " + arg, num ^ 581736153);
				}
				else
				{
					Log.Error("Exception while enumerating: " + arg);
				}
				result = "error";
			}
			return result;
		}

		// Token: 0x06000198 RID: 408 RVA: 0x000083B0 File Offset: 0x000065B0
		public static void Swap<T>(ref T x, ref T y)
		{
			T t = y;
			y = x;
			x = t;
		}

		// Token: 0x06000199 RID: 409 RVA: 0x000083D7 File Offset: 0x000065D7
		public static T MemberwiseClone<T>(T obj)
		{
			if (Gen.s_memberwiseClone == null)
			{
				Gen.s_memberwiseClone = typeof(object).GetMethod("MemberwiseClone", BindingFlags.Instance | BindingFlags.NonPublic);
			}
			return (T)((object)Gen.s_memberwiseClone.Invoke(obj, null));
		}

		// Token: 0x0600019A RID: 410 RVA: 0x00008418 File Offset: 0x00006618
		public static int FixedTimeStepUpdate(ref float timeBuffer, float fps)
		{
			timeBuffer += Mathf.Min(Time.deltaTime, 1f);
			float num = 1f / fps;
			int num2 = Mathf.FloorToInt(timeBuffer / num);
			timeBuffer -= (float)num2 * num;
			return num2;
		}

		// Token: 0x0600019B RID: 411 RVA: 0x00008458 File Offset: 0x00006658
		public static int HashCombine<T>(int seed, T obj)
		{
			int num = (obj == null) ? 0 : obj.GetHashCode();
			return (int)((long)seed ^ (long)num + (long)((ulong)-1640531527) + (long)((long)seed << 6) + (long)(seed >> 2));
		}

		// Token: 0x0600019C RID: 412 RVA: 0x00008495 File Offset: 0x00006695
		public static int HashCombineStruct<T>(int seed, T obj) where T : struct
		{
			return (int)((long)seed ^ (long)obj.GetHashCode() + (long)((ulong)-1640531527) + (long)((long)seed << 6) + (long)(seed >> 2));
		}

		// Token: 0x0600019D RID: 413 RVA: 0x000084BA File Offset: 0x000066BA
		public static int HashCombineInt(int seed, int value)
		{
			return (int)((long)seed ^ (long)value + (long)((ulong)-1640531527) + (long)((long)seed << 6) + (long)(seed >> 2));
		}

		// Token: 0x0600019E RID: 414 RVA: 0x000084D4 File Offset: 0x000066D4
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

		// Token: 0x0600019F RID: 415 RVA: 0x00008526 File Offset: 0x00006726
		public static int HashOffset(this int baseInt)
		{
			return Gen.HashCombineInt(baseInt, 169495093);
		}

		// Token: 0x060001A0 RID: 416 RVA: 0x00008533 File Offset: 0x00006733
		public static int HashOffset(this Thing t)
		{
			return t.thingIDNumber.HashOffset();
		}

		// Token: 0x060001A1 RID: 417 RVA: 0x00008540 File Offset: 0x00006740
		public static int HashOffset(this WorldObject o)
		{
			return o.ID.HashOffset();
		}

		// Token: 0x060001A2 RID: 418 RVA: 0x0000854D File Offset: 0x0000674D
		public static bool IsHashIntervalTick(this Thing t, int interval)
		{
			return t.HashOffsetTicks() % interval == 0;
		}

		// Token: 0x060001A3 RID: 419 RVA: 0x0000855A File Offset: 0x0000675A
		public static int HashOffsetTicks(this Thing t)
		{
			return Find.TickManager.TicksGame + t.thingIDNumber.HashOffset();
		}

		// Token: 0x060001A4 RID: 420 RVA: 0x00008572 File Offset: 0x00006772
		public static bool IsHashIntervalTick(this WorldObject o, int interval)
		{
			return o.HashOffsetTicks() % interval == 0;
		}

		// Token: 0x060001A5 RID: 421 RVA: 0x0000857F File Offset: 0x0000677F
		public static int HashOffsetTicks(this WorldObject o)
		{
			return Find.TickManager.TicksGame + o.ID.HashOffset();
		}

		// Token: 0x060001A6 RID: 422 RVA: 0x00008597 File Offset: 0x00006797
		public static bool IsHashIntervalTick(this Faction f, int interval)
		{
			return f.HashOffsetTicks() % interval == 0;
		}

		// Token: 0x060001A7 RID: 423 RVA: 0x000085A4 File Offset: 0x000067A4
		public static int HashOffsetTicks(this Faction f)
		{
			return Find.TickManager.TicksGame + f.randomKey.HashOffset();
		}

		// Token: 0x060001A8 RID: 424 RVA: 0x000085BC File Offset: 0x000067BC
		public static int HashOrderless(int v1, int v2)
		{
			return Gen.HashCombineInt(Math.Min(v1, v2), Math.Max(v1, v2));
		}

		// Token: 0x060001A9 RID: 425 RVA: 0x000085D4 File Offset: 0x000067D4
		public static bool IsNestedHashIntervalTick(this Thing t, int outerInterval, int approxInnerInterval)
		{
			int num = Mathf.Max(Mathf.RoundToInt((float)approxInnerInterval / (float)outerInterval), 1);
			return t.HashOffsetTicks() / outerInterval % num == 0;
		}

		// Token: 0x060001AA RID: 426 RVA: 0x00008600 File Offset: 0x00006800
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

		// Token: 0x060001AB RID: 427 RVA: 0x00008690 File Offset: 0x00006890
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
					}));
				}
			}
		}

		// Token: 0x060001AC RID: 428 RVA: 0x00008710 File Offset: 0x00006910
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

		// Token: 0x060001AD RID: 429 RVA: 0x00008792 File Offset: 0x00006992
		public static bool InBounds<T>(this T[,] array, int i, int j)
		{
			return i >= 0 && j >= 0 && i < array.GetLength(0) && j < array.GetLength(1);
		}

		// Token: 0x04000054 RID: 84
		private static MethodInfo s_memberwiseClone;
	}
}
