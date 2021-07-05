using System;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Profiling;

namespace Verse
{
	// Token: 0x020003B1 RID: 945
	public static class DebugOutputsSystem
	{
		// Token: 0x06001D4C RID: 7500 RVA: 0x000B5F1C File Offset: 0x000B411C
		[DebugOutput("System", false)]
		public static void LoadedAssets()
		{
			StringBuilder stringBuilder = new StringBuilder();
			UnityEngine.Object[] array = Resources.FindObjectsOfTypeAll(typeof(Mesh));
			stringBuilder.AppendLine(string.Concat(new object[]
			{
				"Meshes: ",
				array.Length,
				" (",
				DebugOutputsSystem.TotalBytes(array).ToStringBytes("F2"),
				")"
			}));
			UnityEngine.Object[] array2 = Resources.FindObjectsOfTypeAll(typeof(Material));
			stringBuilder.AppendLine(string.Concat(new object[]
			{
				"Materials: ",
				array2.Length,
				" (",
				DebugOutputsSystem.TotalBytes(array2).ToStringBytes("F2"),
				")"
			}));
			stringBuilder.AppendLine("   Damaged: " + DamagedMatPool.MatCount);
			stringBuilder.AppendLine(string.Concat(new object[]
			{
				"   Faded: ",
				FadedMaterialPool.TotalMaterialCount,
				" (",
				FadedMaterialPool.TotalMaterialBytes.ToStringBytes("F2"),
				")"
			}));
			stringBuilder.AppendLine("   SolidColorsSimple: " + SolidColorMaterials.SimpleColorMatCount);
			UnityEngine.Object[] array3 = Resources.FindObjectsOfTypeAll(typeof(Texture));
			stringBuilder.AppendLine(string.Concat(new object[]
			{
				"Textures: ",
				array3.Length,
				" (",
				DebugOutputsSystem.TotalBytes(array3).ToStringBytes("F2"),
				")"
			}));
			stringBuilder.AppendLine();
			stringBuilder.AppendLine("Texture list:");
			UnityEngine.Object[] array4 = array3;
			for (int i = 0; i < array4.Length; i++)
			{
				string text = ((Texture)array4[i]).name;
				if (text.NullOrEmpty())
				{
					text = "-";
				}
				stringBuilder.AppendLine(text);
			}
			Log.Message(stringBuilder.ToString());
		}

		// Token: 0x06001D4D RID: 7501 RVA: 0x000B6114 File Offset: 0x000B4314
		private static long TotalBytes(UnityEngine.Object[] arr)
		{
			long num = 0L;
			foreach (UnityEngine.Object o in arr)
			{
				num += Profiler.GetRuntimeMemorySizeLong(o);
			}
			return num;
		}

		// Token: 0x06001D4E RID: 7502 RVA: 0x000B6142 File Offset: 0x000B4342
		[DebugOutput("System", true)]
		public static void DynamicDrawThingsList()
		{
			Find.CurrentMap.dynamicDrawManager.LogDynamicDrawThings();
		}

		// Token: 0x06001D4F RID: 7503 RVA: 0x000B6154 File Offset: 0x000B4354
		[DebugOutput("System", false)]
		public static void RandByCurveTests()
		{
			DebugHistogram debugHistogram = new DebugHistogram((from x in Enumerable.Range(0, 30)
			select (float)x).ToArray<float>());
			SimpleCurve curve = new SimpleCurve
			{
				{
					new CurvePoint(0f, 0f),
					true
				},
				{
					new CurvePoint(10f, 1f),
					true
				},
				{
					new CurvePoint(15f, 2f),
					true
				},
				{
					new CurvePoint(20f, 2f),
					true
				},
				{
					new CurvePoint(21f, 0.5f),
					true
				},
				{
					new CurvePoint(30f, 0f),
					true
				}
			};
			float num = 0f;
			for (int i = 0; i < 1000000; i++)
			{
				float num2 = Rand.ByCurve(curve);
				num += num2;
				debugHistogram.Add(num2);
			}
			debugHistogram.Display();
			Log.Message(string.Format("Average {0}, calculated as {1}", num / 1000000f, Rand.ByCurveAverage(curve)));
		}
	}
}
