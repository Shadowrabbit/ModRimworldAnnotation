using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Xml;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020007CB RID: 1995
	public static class ConvertHelper
	{
		// Token: 0x0600321B RID: 12827 RVA: 0x000275E8 File Offset: 0x000257E8
		public static bool CanConvert<T>(object obj)
		{
			return ConvertHelper.CanConvert(obj, typeof(T));
		}

		// Token: 0x0600321C RID: 12828 RVA: 0x0014C170 File Offset: 0x0014A370
		public static bool CanConvert(object obj, Type to)
		{
			if (obj == null)
			{
				return true;
			}
			if (to.IsAssignableFrom(obj.GetType()))
			{
				return true;
			}
			if (to == typeof(string))
			{
				return true;
			}
			if (obj is string && !to.IsPrimitive && ParseHelper.CanParse(to, (string)obj))
			{
				return true;
			}
			if (obj is string && typeof(Def).IsAssignableFrom(to))
			{
				return true;
			}
			if (obj is string && to == typeof(Faction))
			{
				return true;
			}
			if (ConvertHelper.CanConvertBetweenDataTypes(obj.GetType(), to))
			{
				return true;
			}
			if (ConvertHelper.IsXml(obj) && !to.IsPrimitive)
			{
				return true;
			}
			if (to.IsGenericType && (to.GetGenericTypeDefinition() == typeof(IEnumerable<>) || to.GetGenericTypeDefinition() == typeof(List<>)) && to.GetGenericArguments().Length >= 1 && (!(to.GetGenericArguments()[0] == typeof(string)) || !(obj is string)))
			{
				IEnumerable enumerable = obj as IEnumerable;
				if (enumerable != null)
				{
					Type to2 = to.GetGenericArguments()[0];
					bool flag = true;
					using (IEnumerator enumerator = enumerable.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							if (!ConvertHelper.CanConvert(enumerator.Current, to2))
							{
								flag = false;
								break;
							}
						}
					}
					if (flag)
					{
						return true;
					}
				}
			}
			if (obj is IEnumerable && !(obj is string))
			{
				IEnumerable e = (IEnumerable)obj;
				if (GenCollection.Count_EnumerableBase(e) == 1 && ConvertHelper.CanConvert(GenCollection.FirstOrDefault_EnumerableBase(e), to))
				{
					return true;
				}
			}
			if (typeof(IList).IsAssignableFrom(to))
			{
				Type[] genericArguments = to.GetGenericArguments();
				return genericArguments.Length < 1 || ConvertHelper.CanConvert(obj, genericArguments[0]);
			}
			if (to == typeof(IEnumerable))
			{
				return true;
			}
			if (to.IsGenericType && to.GetGenericTypeDefinition() == typeof(IEnumerable<>))
			{
				Type[] genericArguments2 = to.GetGenericArguments();
				return genericArguments2.Length < 1 || ConvertHelper.CanConvert(obj, genericArguments2[0]);
			}
			IConvertible convertible = obj as IConvertible;
			if (convertible == null)
			{
				return false;
			}
			Type left = Nullable.GetUnderlyingType(to) ?? to;
			if (left != typeof(bool) && left != typeof(byte) && left != typeof(char) && left != typeof(DateTime) && left != typeof(decimal) && left != typeof(double) && left != typeof(short) && left != typeof(int) && left != typeof(long) && left != typeof(sbyte) && left != typeof(float) && left != typeof(string) && left != typeof(ushort) && left != typeof(uint) && left != typeof(ulong))
			{
				return false;
			}
			try
			{
				ConvertHelper.ConvertToPrimitive(convertible, to, null);
			}
			catch (FormatException)
			{
				return false;
			}
			return true;
		}

		// Token: 0x0600321D RID: 12829 RVA: 0x0014C504 File Offset: 0x0014A704
		public static T Convert<T>(object obj)
		{
			return (T)((object)ConvertHelper.Convert(obj, typeof(T), default(T)));
		}

		// Token: 0x0600321E RID: 12830 RVA: 0x000275FA File Offset: 0x000257FA
		public static object Convert(object obj, Type to)
		{
			if (to.IsValueType)
			{
				return ConvertHelper.Convert(obj, to, Activator.CreateInstance(to));
			}
			return ConvertHelper.Convert(obj, to, null);
		}

		// Token: 0x0600321F RID: 12831 RVA: 0x0014C534 File Offset: 0x0014A734
		public static object Convert(object obj, Type to, object defaultValue)
		{
			if (obj == null)
			{
				return defaultValue;
			}
			if (to.IsAssignableFrom(obj.GetType()))
			{
				return obj;
			}
			if (to == typeof(string))
			{
				return obj.ToString();
			}
			string text = obj as string;
			if (text != null && !to.IsPrimitive && ParseHelper.CanParse(to, (string)obj))
			{
				if (text == "")
				{
					return defaultValue;
				}
				return ParseHelper.FromString(text, to);
			}
			else if (text != null && typeof(Def).IsAssignableFrom(to))
			{
				if (text == "")
				{
					return defaultValue;
				}
				return GenDefDatabase.GetDef(to, text, true);
			}
			else if (text != null && to == typeof(Faction))
			{
				if (text == "")
				{
					return defaultValue;
				}
				List<Faction> allFactionsListForReading = Find.FactionManager.AllFactionsListForReading;
				for (int i = 0; i < allFactionsListForReading.Count; i++)
				{
					if (allFactionsListForReading[i].GetUniqueLoadID() == text)
					{
						return allFactionsListForReading[i];
					}
				}
				for (int j = 0; j < allFactionsListForReading.Count; j++)
				{
					if (allFactionsListForReading[j].HasName && allFactionsListForReading[j].Name == text)
					{
						return allFactionsListForReading[j];
					}
				}
				for (int k = 0; k < allFactionsListForReading.Count; k++)
				{
					if (allFactionsListForReading[k].def.defName == text)
					{
						return allFactionsListForReading[k];
					}
				}
				return defaultValue;
			}
			else
			{
				if (ConvertHelper.CanConvertBetweenDataTypes(obj.GetType(), to))
				{
					return ConvertHelper.ConvertBetweenDataTypes(obj, to);
				}
				if (ConvertHelper.IsXml(obj) && !to.IsPrimitive)
				{
					try
					{
						Type type = to;
						if (type == typeof(IEnumerable))
						{
							type = typeof(List<string>);
						}
						if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IEnumerable<>) && type.GetGenericArguments().Length >= 1)
						{
							type = typeof(List<>).MakeGenericType(new Type[]
							{
								type.GetGenericArguments()[0]
							});
						}
						XmlDocument xmlDocument = new XmlDocument();
						xmlDocument.LoadXml("<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n<root>\n" + text + "\n</root>");
						object result = DirectXmlToObject.GetObjectFromXmlMethod(type)(xmlDocument.DocumentElement, true);
						DirectXmlCrossRefLoader.ResolveAllWantedCrossReferences(FailMode.LogErrors);
						return result;
					}
					finally
					{
						DirectXmlCrossRefLoader.Clear();
					}
				}
				if (to.IsGenericType && (to.GetGenericTypeDefinition() == typeof(IEnumerable<>) || to.GetGenericTypeDefinition() == typeof(List<>)) && to.GetGenericArguments().Length >= 1 && (!(to.GetGenericArguments()[0] == typeof(string)) || !(obj is string)))
				{
					IEnumerable enumerable = obj as IEnumerable;
					if (enumerable != null)
					{
						Type type2 = to.GetGenericArguments()[0];
						bool flag = true;
						using (IEnumerator enumerator = enumerable.GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								if (!ConvertHelper.CanConvert(enumerator.Current, type2))
								{
									flag = false;
									break;
								}
							}
						}
						if (flag)
						{
							IList list = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(new Type[]
							{
								type2
							}));
							foreach (object obj2 in enumerable)
							{
								list.Add(ConvertHelper.Convert(obj2, type2));
							}
							return list;
						}
					}
				}
				if (obj is IEnumerable && !(obj is string))
				{
					IEnumerable e = (IEnumerable)obj;
					if (GenCollection.Count_EnumerableBase(e) == 1)
					{
						object obj3 = GenCollection.FirstOrDefault_EnumerableBase(e);
						if (ConvertHelper.CanConvert(obj3, to))
						{
							return ConvertHelper.Convert(obj3, to);
						}
					}
				}
				if (typeof(IList).IsAssignableFrom(to))
				{
					IList list2 = (IList)Activator.CreateInstance(to);
					Type[] genericArguments = to.GetGenericArguments();
					if (genericArguments.Length >= 1)
					{
						list2.Add(ConvertHelper.Convert(obj, genericArguments[0]));
					}
					else
					{
						list2.Add(obj);
					}
					return list2;
				}
				if (to == typeof(IEnumerable))
				{
					return Gen.YieldSingleNonGeneric<object>(obj);
				}
				object result2;
				if (to.IsGenericType && to.GetGenericTypeDefinition() == typeof(IEnumerable<>))
				{
					Type[] genericArguments2 = to.GetGenericArguments();
					if (genericArguments2.Length >= 1)
					{
						IList list3 = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(new Type[]
						{
							genericArguments2[0]
						}));
						list3.Add(ConvertHelper.Convert(obj, genericArguments2[0]));
						return list3;
					}
					return Gen.YieldSingleNonGeneric<object>(obj);
				}
				else
				{
					IConvertible convertible = obj as IConvertible;
					if (convertible == null)
					{
						return defaultValue;
					}
					try
					{
						result2 = ConvertHelper.ConvertToPrimitive(convertible, to, defaultValue);
					}
					catch (FormatException)
					{
						result2 = defaultValue;
					}
				}
				return result2;
			}
		}

		// Token: 0x06003220 RID: 12832 RVA: 0x0014CA38 File Offset: 0x0014AC38
		public static bool IsXml(object obj)
		{
			if (obj is TaggedString)
			{
				return false;
			}
			string text = obj as string;
			if (text == null || text.IndexOf('<') < 0 || text.IndexOf('>') < 0)
			{
				return false;
			}
			string text2 = text.Trim();
			return text2[0] == '<' && text2[text2.Length - 1] == '>';
		}

		// Token: 0x06003221 RID: 12833 RVA: 0x0014CA98 File Offset: 0x0014AC98
		private static object ConvertToPrimitive(IConvertible obj, Type to, object defaultValue)
		{
			CultureInfo invariantCulture = CultureInfo.InvariantCulture;
			Type left = Nullable.GetUnderlyingType(to) ?? to;
			if (left == typeof(bool))
			{
				return System.Convert.ToBoolean(obj, invariantCulture);
			}
			if (left == typeof(byte))
			{
				return System.Convert.ToByte(obj, invariantCulture);
			}
			if (left == typeof(char))
			{
				return System.Convert.ToChar(obj, invariantCulture);
			}
			if (left == typeof(DateTime))
			{
				return System.Convert.ToDateTime(obj, invariantCulture);
			}
			if (left == typeof(decimal))
			{
				return System.Convert.ToDecimal(obj, invariantCulture);
			}
			if (left == typeof(double))
			{
				return System.Convert.ToDouble(obj, invariantCulture);
			}
			if (left == typeof(short))
			{
				return System.Convert.ToInt16(obj, invariantCulture);
			}
			if (left == typeof(int))
			{
				return System.Convert.ToInt32(obj, invariantCulture);
			}
			if (left == typeof(long))
			{
				return System.Convert.ToInt64(obj, invariantCulture);
			}
			if (left == typeof(sbyte))
			{
				return System.Convert.ToSByte(obj, invariantCulture);
			}
			if (left == typeof(float))
			{
				return System.Convert.ToSingle(obj, invariantCulture);
			}
			if (left == typeof(string))
			{
				return System.Convert.ToString(obj, invariantCulture);
			}
			if (left == typeof(ushort))
			{
				return System.Convert.ToUInt16(obj, invariantCulture);
			}
			if (left == typeof(uint))
			{
				return System.Convert.ToUInt32(obj, invariantCulture);
			}
			if (left == typeof(ulong))
			{
				return System.Convert.ToUInt64(obj, invariantCulture);
			}
			return defaultValue;
		}

		// Token: 0x06003222 RID: 12834 RVA: 0x0014CC84 File Offset: 0x0014AE84
		private static bool CanConvertBetweenDataTypes(Type from, Type to)
		{
			return (from == typeof(IntRange) && to == typeof(FloatRange)) || (from == typeof(FloatRange) && to == typeof(IntRange));
		}

		// Token: 0x06003223 RID: 12835 RVA: 0x0014CCDC File Offset: 0x0014AEDC
		private static object ConvertBetweenDataTypes(object from, Type to)
		{
			if (from is IntRange)
			{
				IntRange intRange = (IntRange)from;
				if (to == typeof(FloatRange))
				{
					return new FloatRange((float)intRange.min, (float)intRange.max);
				}
			}
			if (from is FloatRange)
			{
				FloatRange floatRange = (FloatRange)from;
				if (to == typeof(IntRange))
				{
					return new IntRange(Mathf.RoundToInt(floatRange.min), Mathf.RoundToInt(floatRange.max));
				}
			}
			return null;
		}
	}
}
