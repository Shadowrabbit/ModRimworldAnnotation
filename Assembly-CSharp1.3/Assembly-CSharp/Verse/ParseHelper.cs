using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using RimWorld;
using RimWorld.QuestGen;
using Steamworks;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000033 RID: 51
	public static class ParseHelper
	{
		// Token: 0x060002F7 RID: 759 RVA: 0x0001076B File Offset: 0x0000E96B
		public static string ParseString(string str)
		{
			return str.Replace("\\n", "\n");
		}

		// Token: 0x060002F8 RID: 760 RVA: 0x00010780 File Offset: 0x0000E980
		public static int ParseIntPermissive(string str)
		{
			int result;
			if (!int.TryParse(str, NumberStyles.Any, CultureInfo.InvariantCulture, out result))
			{
				result = (int)float.Parse(str, CultureInfo.InvariantCulture);
				Log.Warning("Parsed " + str + " as int.");
			}
			return result;
		}

		// Token: 0x060002F9 RID: 761 RVA: 0x000107C4 File Offset: 0x0000E9C4
		public static Vector3 FromStringVector3(string Str)
		{
			Str = Str.TrimStart(new char[]
			{
				'('
			});
			Str = Str.TrimEnd(new char[]
			{
				')'
			});
			string[] array = Str.Split(new char[]
			{
				','
			});
			CultureInfo invariantCulture = CultureInfo.InvariantCulture;
			float x = Convert.ToSingle(array[0], invariantCulture);
			float y = Convert.ToSingle(array[1], invariantCulture);
			float z = Convert.ToSingle(array[2], invariantCulture);
			return new Vector3(x, y, z);
		}

		// Token: 0x060002FA RID: 762 RVA: 0x00010834 File Offset: 0x0000EA34
		public static Vector2 FromStringVector2(string Str)
		{
			Str = Str.TrimStart(new char[]
			{
				'('
			});
			Str = Str.TrimEnd(new char[]
			{
				')'
			});
			string[] array = Str.Split(new char[]
			{
				','
			});
			CultureInfo invariantCulture = CultureInfo.InvariantCulture;
			float x;
			float y;
			if (array.Length == 1)
			{
				y = (x = Convert.ToSingle(array[0], invariantCulture));
			}
			else
			{
				if (array.Length != 2)
				{
					throw new InvalidOperationException();
				}
				x = Convert.ToSingle(array[0], invariantCulture);
				y = Convert.ToSingle(array[1], invariantCulture);
			}
			return new Vector2(x, y);
		}

		// Token: 0x060002FB RID: 763 RVA: 0x000108BC File Offset: 0x0000EABC
		public static Vector4 FromStringVector4Adaptive(string Str)
		{
			Str = Str.TrimStart(new char[]
			{
				'('
			});
			Str = Str.TrimEnd(new char[]
			{
				')'
			});
			string[] array = Str.Split(new char[]
			{
				','
			});
			CultureInfo invariantCulture = CultureInfo.InvariantCulture;
			float x = 0f;
			float y = 0f;
			float z = 0f;
			float w = 0f;
			if (array.Length >= 1)
			{
				x = Convert.ToSingle(array[0], invariantCulture);
			}
			if (array.Length >= 2)
			{
				y = Convert.ToSingle(array[1], invariantCulture);
			}
			if (array.Length >= 3)
			{
				z = Convert.ToSingle(array[2], invariantCulture);
			}
			if (array.Length >= 4)
			{
				w = Convert.ToSingle(array[3], invariantCulture);
			}
			if (array.Length >= 5)
			{
				Log.ErrorOnce(string.Format("Too many elements in vector {0}", Str), 16139142);
			}
			return new Vector4(x, y, z, w);
		}

		// Token: 0x060002FC RID: 764 RVA: 0x0001098C File Offset: 0x0000EB8C
		public static Rect FromStringRect(string str)
		{
			str = str.TrimStart(new char[]
			{
				'('
			});
			str = str.TrimEnd(new char[]
			{
				')'
			});
			string[] array = str.Split(new char[]
			{
				','
			});
			CultureInfo invariantCulture = CultureInfo.InvariantCulture;
			float x = Convert.ToSingle(array[0], invariantCulture);
			float y = Convert.ToSingle(array[1], invariantCulture);
			float width = Convert.ToSingle(array[2], invariantCulture);
			float height = Convert.ToSingle(array[3], invariantCulture);
			return new Rect(x, y, width, height);
		}

		// Token: 0x060002FD RID: 765 RVA: 0x00010A08 File Offset: 0x0000EC08
		public static float ParseFloat(string str)
		{
			return float.Parse(str, CultureInfo.InvariantCulture);
		}

		// Token: 0x060002FE RID: 766 RVA: 0x00010A15 File Offset: 0x0000EC15
		public static bool ParseBool(string str)
		{
			return bool.Parse(str);
		}

		// Token: 0x060002FF RID: 767 RVA: 0x00010A1D File Offset: 0x0000EC1D
		public static long ParseLong(string str)
		{
			return long.Parse(str, CultureInfo.InvariantCulture);
		}

		// Token: 0x06000300 RID: 768 RVA: 0x00010A2A File Offset: 0x0000EC2A
		public static double ParseDouble(string str)
		{
			return double.Parse(str, CultureInfo.InvariantCulture);
		}

		// Token: 0x06000301 RID: 769 RVA: 0x00010A37 File Offset: 0x0000EC37
		public static sbyte ParseSByte(string str)
		{
			return sbyte.Parse(str, CultureInfo.InvariantCulture);
		}

		// Token: 0x06000302 RID: 770 RVA: 0x00010A44 File Offset: 0x0000EC44
		public static Type ParseType(string str)
		{
			if (str == "null" || str == "Null")
			{
				return null;
			}
			Type typeInAnyAssembly = GenTypes.GetTypeInAnyAssembly(str, null);
			if (typeInAnyAssembly == null)
			{
				Log.Error("Could not find a type named " + str);
			}
			return typeInAnyAssembly;
		}

		// Token: 0x06000303 RID: 771 RVA: 0x00010A84 File Offset: 0x0000EC84
		public static Action ParseAction(string str)
		{
			string[] array = str.Split(new char[]
			{
				'.'
			});
			string methodName = array[array.Length - 1];
			string typeName;
			if (array.Length == 3)
			{
				typeName = array[0] + "." + array[1];
			}
			else
			{
				typeName = array[0];
			}
			MethodInfo method = GenTypes.GetTypeInAnyAssembly(typeName, null).GetMethods().First((MethodInfo m) => m.Name == methodName);
			return (Action)Delegate.CreateDelegate(typeof(Action), method);
		}

		// Token: 0x06000304 RID: 772 RVA: 0x00010B08 File Offset: 0x0000ED08
		public static Color ParseColor(string str)
		{
			str = str.TrimStart(ParseHelper.colorTrimStartParameters);
			str = str.TrimEnd(ParseHelper.colorTrimEndParameters);
			string[] array = str.Split(new char[]
			{
				','
			});
			float num = ParseHelper.ParseFloat(array[0]);
			float num2 = ParseHelper.ParseFloat(array[1]);
			float num3 = ParseHelper.ParseFloat(array[2]);
			bool flag = num > 1f || num3 > 1f || num2 > 1f;
			float num4 = (float)(flag ? 255 : 1);
			if (array.Length == 4)
			{
				num4 = ParseHelper.FromString<float>(array[3]);
			}
			Color result;
			if (!flag)
			{
				result.r = num;
				result.g = num2;
				result.b = num3;
				result.a = num4;
			}
			else
			{
				result = GenColor.FromBytes(Mathf.RoundToInt(num), Mathf.RoundToInt(num2), Mathf.RoundToInt(num3), Mathf.RoundToInt(num4));
			}
			return result;
		}

		// Token: 0x06000305 RID: 773 RVA: 0x00010BDC File Offset: 0x0000EDDC
		public static PublishedFileId_t ParsePublishedFileId(string str)
		{
			return new PublishedFileId_t(ulong.Parse(str));
		}

		// Token: 0x06000306 RID: 774 RVA: 0x00010BE9 File Offset: 0x0000EDE9
		public static IntVec2 ParseIntVec2(string str)
		{
			return IntVec2.FromString(str);
		}

		// Token: 0x06000307 RID: 775 RVA: 0x00010BF1 File Offset: 0x0000EDF1
		public static IntVec3 ParseIntVec3(string str)
		{
			return IntVec3.FromString(str);
		}

		// Token: 0x06000308 RID: 776 RVA: 0x00010BF9 File Offset: 0x0000EDF9
		public static Rot4 ParseRot4(string str)
		{
			return Rot4.FromString(str);
		}

		// Token: 0x06000309 RID: 777 RVA: 0x00010C01 File Offset: 0x0000EE01
		public static CellRect ParseCellRect(string str)
		{
			return CellRect.FromString(str);
		}

		// Token: 0x0600030A RID: 778 RVA: 0x00010C09 File Offset: 0x0000EE09
		public static CurvePoint ParseCurvePoint(string str)
		{
			return CurvePoint.FromString(str);
		}

		// Token: 0x0600030B RID: 779 RVA: 0x00010C11 File Offset: 0x0000EE11
		public static NameTriple ParseNameTriple(string str)
		{
			NameTriple nameTriple = NameTriple.FromString(str);
			nameTriple.ResolveMissingPieces(null);
			return nameTriple;
		}

		// Token: 0x0600030C RID: 780 RVA: 0x00010C20 File Offset: 0x0000EE20
		public static FloatRange ParseFloatRange(string str)
		{
			return FloatRange.FromString(str);
		}

		// Token: 0x0600030D RID: 781 RVA: 0x00010C28 File Offset: 0x0000EE28
		public static IntRange ParseIntRange(string str)
		{
			return IntRange.FromString(str);
		}

		// Token: 0x0600030E RID: 782 RVA: 0x00010C30 File Offset: 0x0000EE30
		public static QualityRange ParseQualityRange(string str)
		{
			return QualityRange.FromString(str);
		}

		// Token: 0x0600030F RID: 783 RVA: 0x00010C38 File Offset: 0x0000EE38
		public static ColorInt ParseColorInt(string str)
		{
			str = str.TrimStart(ParseHelper.colorTrimStartParameters);
			str = str.TrimEnd(ParseHelper.colorTrimEndParameters);
			string[] array = str.Split(new char[]
			{
				','
			});
			ColorInt result = new ColorInt(255, 255, 255, 255);
			result.r = ParseHelper.ParseIntPermissive(array[0]);
			result.g = ParseHelper.ParseIntPermissive(array[1]);
			result.b = ParseHelper.ParseIntPermissive(array[2]);
			if (array.Length == 4)
			{
				result.a = ParseHelper.ParseIntPermissive(array[3]);
			}
			else
			{
				result.a = 255;
			}
			return result;
		}

		// Token: 0x06000310 RID: 784 RVA: 0x00010CDD File Offset: 0x0000EEDD
		public static TaggedString ParseTaggedString(string str)
		{
			return str;
		}

		// Token: 0x06000311 RID: 785 RVA: 0x00010CE8 File Offset: 0x0000EEE8
		static ParseHelper()
		{
			ParseHelper.Parsers<string>.Register(new Func<string, string>(ParseHelper.ParseString));
			ParseHelper.Parsers<int>.Register(new Func<string, int>(ParseHelper.ParseIntPermissive));
			ParseHelper.Parsers<Vector3>.Register(new Func<string, Vector3>(ParseHelper.FromStringVector3));
			ParseHelper.Parsers<Vector2>.Register(new Func<string, Vector2>(ParseHelper.FromStringVector2));
			ParseHelper.Parsers<Vector4>.Register(new Func<string, Vector4>(ParseHelper.FromStringVector4Adaptive));
			ParseHelper.Parsers<Rect>.Register(new Func<string, Rect>(ParseHelper.FromStringRect));
			ParseHelper.Parsers<float>.Register(new Func<string, float>(ParseHelper.ParseFloat));
			ParseHelper.Parsers<bool>.Register(new Func<string, bool>(ParseHelper.ParseBool));
			ParseHelper.Parsers<long>.Register(new Func<string, long>(ParseHelper.ParseLong));
			ParseHelper.Parsers<double>.Register(new Func<string, double>(ParseHelper.ParseDouble));
			ParseHelper.Parsers<sbyte>.Register(new Func<string, sbyte>(ParseHelper.ParseSByte));
			ParseHelper.Parsers<Type>.Register(new Func<string, Type>(ParseHelper.ParseType));
			ParseHelper.Parsers<Action>.Register(new Func<string, Action>(ParseHelper.ParseAction));
			ParseHelper.Parsers<Color>.Register(new Func<string, Color>(ParseHelper.ParseColor));
			ParseHelper.Parsers<PublishedFileId_t>.Register(new Func<string, PublishedFileId_t>(ParseHelper.ParsePublishedFileId));
			ParseHelper.Parsers<IntVec2>.Register(new Func<string, IntVec2>(ParseHelper.ParseIntVec2));
			ParseHelper.Parsers<IntVec3>.Register(new Func<string, IntVec3>(ParseHelper.ParseIntVec3));
			ParseHelper.Parsers<Rot4>.Register(new Func<string, Rot4>(ParseHelper.ParseRot4));
			ParseHelper.Parsers<CellRect>.Register(new Func<string, CellRect>(ParseHelper.ParseCellRect));
			ParseHelper.Parsers<CurvePoint>.Register(new Func<string, CurvePoint>(ParseHelper.ParseCurvePoint));
			ParseHelper.Parsers<NameTriple>.Register(new Func<string, NameTriple>(ParseHelper.ParseNameTriple));
			ParseHelper.Parsers<FloatRange>.Register(new Func<string, FloatRange>(ParseHelper.ParseFloatRange));
			ParseHelper.Parsers<IntRange>.Register(new Func<string, IntRange>(ParseHelper.ParseIntRange));
			ParseHelper.Parsers<QualityRange>.Register(new Func<string, QualityRange>(ParseHelper.ParseQualityRange));
			ParseHelper.Parsers<ColorInt>.Register(new Func<string, ColorInt>(ParseHelper.ParseColorInt));
			ParseHelper.Parsers<TaggedString>.Register(new Func<string, TaggedString>(ParseHelper.ParseTaggedString));
		}

		// Token: 0x06000312 RID: 786 RVA: 0x00010EE0 File Offset: 0x0000F0E0
		public static T FromString<T>(string str)
		{
			Func<string, T> parser = ParseHelper.Parsers<T>.parser;
			if (parser != null)
			{
				return parser(str);
			}
			return (T)((object)ParseHelper.FromString(str, typeof(T)));
		}

		// Token: 0x06000313 RID: 787 RVA: 0x00010F14 File Offset: 0x0000F114
		public static object FromString(string str, Type itemType)
		{
			object result;
			try
			{
				itemType = (Nullable.GetUnderlyingType(itemType) ?? itemType);
				if (itemType.IsEnum)
				{
					try
					{
						object obj = BackCompatibility.BackCompatibleEnum(itemType, str);
						if (obj != null)
						{
							return obj;
						}
						return Enum.Parse(itemType, str);
					}
					catch (ArgumentException innerException)
					{
						throw new ArgumentException(string.Concat(new object[]
						{
							"'",
							str,
							"' is not a valid value for ",
							itemType,
							". Valid values are: \n"
						}) + GenText.StringFromEnumerable(Enum.GetValues(itemType)), innerException);
					}
				}
				Func<string, object> func;
				if (ParseHelper.parsers.TryGetValue(itemType, out func))
				{
					result = func(str);
				}
				else
				{
					if (!typeof(ISlateRef).IsAssignableFrom(itemType))
					{
						throw new ArgumentException(string.Concat(new string[]
						{
							"Trying to parse to unknown data type ",
							itemType.Name,
							". Content is '",
							str,
							"'."
						}));
					}
					ISlateRef slateRef = (ISlateRef)Activator.CreateInstance(itemType);
					slateRef.SlateRef = str;
					result = slateRef;
				}
			}
			catch (Exception innerException2)
			{
				throw new ArgumentException(string.Concat(new object[]
				{
					"Exception parsing ",
					itemType,
					" from \"",
					str,
					"\""
				}), innerException2);
			}
			return result;
		}

		// Token: 0x06000314 RID: 788 RVA: 0x00011064 File Offset: 0x0000F264
		public static bool HandlesType(Type type)
		{
			type = (Nullable.GetUnderlyingType(type) ?? type);
			return type.IsPrimitive || type.IsEnum || ParseHelper.parsers.ContainsKey(type) || typeof(ISlateRef).IsAssignableFrom(type);
		}

		// Token: 0x06000315 RID: 789 RVA: 0x000110A4 File Offset: 0x0000F2A4
		public static bool CanParse(Type type, string str)
		{
			if (!ParseHelper.HandlesType(type))
			{
				return false;
			}
			try
			{
				ParseHelper.FromString(str, type);
			}
			catch (ArgumentException)
			{
				return false;
			}
			catch (FormatException)
			{
				return false;
			}
			return true;
		}

		// Token: 0x04000092 RID: 146
		private static Dictionary<Type, Func<string, object>> parsers = new Dictionary<Type, Func<string, object>>();

		// Token: 0x04000093 RID: 147
		private static readonly char[] colorTrimStartParameters = new char[]
		{
			'(',
			'R',
			'G',
			'B',
			'A'
		};

		// Token: 0x04000094 RID: 148
		private static readonly char[] colorTrimEndParameters = new char[]
		{
			')'
		};

		// Token: 0x02001892 RID: 6290
		public static class Parsers<T>
		{
			// Token: 0x060093F2 RID: 37874 RVA: 0x0034E0A4 File Offset: 0x0034C2A4
			public static void Register(Func<string, T> method)
			{
				ParseHelper.Parsers<T>.parser = method;
				ParseHelper.parsers.Add(typeof(T), (string str) => method(str));
			}

			// Token: 0x04005E0D RID: 24077
			public static Func<string, T> parser;

			// Token: 0x04005E0E RID: 24078
			public static readonly string profilerLabel = "ParseHelper.FromString<" + typeof(T).FullName + ">()";
		}
	}
}
