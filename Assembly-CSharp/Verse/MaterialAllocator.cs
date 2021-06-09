using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200046D RID: 1133
	internal static class MaterialAllocator
	{
		// Token: 0x06001CBC RID: 7356 RVA: 0x000F1A40 File Offset: 0x000EFC40
		public static Material Create(Material material)
		{
			Material material2 = new Material(material);
			MaterialAllocator.references[material2] = new MaterialAllocator.MaterialInfo
			{
				stackTrace = (Prefs.DevMode ? Environment.StackTrace : "(unavailable)")
			};
			MaterialAllocator.TryReport();
			return material2;
		}

		// Token: 0x06001CBD RID: 7357 RVA: 0x000F1A88 File Offset: 0x000EFC88
		public static Material Create(Shader shader)
		{
			Material material = new Material(shader);
			MaterialAllocator.references[material] = new MaterialAllocator.MaterialInfo
			{
				stackTrace = (Prefs.DevMode ? Environment.StackTrace : "(unavailable)")
			};
			MaterialAllocator.TryReport();
			return material;
		}

		// Token: 0x06001CBE RID: 7358 RVA: 0x00019F8E File Offset: 0x0001818E
		public static void Destroy(Material material)
		{
			if (!MaterialAllocator.references.ContainsKey(material))
			{
				Log.Error(string.Format("Destroying material {0}, but that material was not created through the MaterialTracker", material), false);
			}
			MaterialAllocator.references.Remove(material);
			UnityEngine.Object.Destroy(material);
		}

		// Token: 0x06001CBF RID: 7359 RVA: 0x000F1AD0 File Offset: 0x000EFCD0
		public static void TryReport()
		{
			if (MaterialAllocator.MaterialWarningThreshold() > MaterialAllocator.nextWarningThreshold)
			{
				MaterialAllocator.nextWarningThreshold = MaterialAllocator.MaterialWarningThreshold();
			}
			if (MaterialAllocator.references.Count > MaterialAllocator.nextWarningThreshold)
			{
				Log.Error(string.Format("Material allocator has allocated {0} materials; this may be a sign of a material leak", MaterialAllocator.references.Count), false);
				if (Prefs.DevMode)
				{
					MaterialAllocator.MaterialReport();
				}
				MaterialAllocator.nextWarningThreshold *= 2;
			}
		}

		// Token: 0x06001CC0 RID: 7360 RVA: 0x00019FC0 File Offset: 0x000181C0
		public static int MaterialWarningThreshold()
		{
			return int.MaxValue;
		}

		// Token: 0x06001CC1 RID: 7361 RVA: 0x000F1B3C File Offset: 0x000EFD3C
		[DebugOutput("System", false)]
		public static void MaterialReport()
		{
			foreach (string text in (from kvp in MaterialAllocator.references
			group kvp by kvp.Value.stackTrace into g
			orderby g.Count<KeyValuePair<Material, MaterialAllocator.MaterialInfo>>() descending
			select string.Format("{0}: {1}", g.Count<KeyValuePair<Material, MaterialAllocator.MaterialInfo>>(), g.FirstOrDefault<KeyValuePair<Material, MaterialAllocator.MaterialInfo>>().Value.stackTrace)).Take(20))
			{
				Log.Error(text, false);
			}
		}

		// Token: 0x06001CC2 RID: 7362 RVA: 0x000F1BFC File Offset: 0x000EFDFC
		[DebugOutput("System", false)]
		public static void MaterialSnapshot()
		{
			MaterialAllocator.snapshot = new Dictionary<string, int>();
			foreach (IGrouping<string, KeyValuePair<Material, MaterialAllocator.MaterialInfo>> grouping in from kvp in MaterialAllocator.references
			group kvp by kvp.Value.stackTrace)
			{
				MaterialAllocator.snapshot[grouping.Key] = grouping.Count<KeyValuePair<Material, MaterialAllocator.MaterialInfo>>();
			}
		}

		// Token: 0x06001CC3 RID: 7363 RVA: 0x000F1C88 File Offset: 0x000EFE88
		[DebugOutput("System", false)]
		public static void MaterialDelta()
		{
			IEnumerable<string> source = (from v in MaterialAllocator.references.Values
			select v.stackTrace).Concat(MaterialAllocator.snapshot.Keys).Distinct<string>();
			Dictionary<string, int> currentSnapshot = new Dictionary<string, int>();
			foreach (IGrouping<string, KeyValuePair<Material, MaterialAllocator.MaterialInfo>> grouping in from kvp in MaterialAllocator.references
			group kvp by kvp.Value.stackTrace)
			{
				currentSnapshot[grouping.Key] = grouping.Count<KeyValuePair<Material, MaterialAllocator.MaterialInfo>>();
			}
			foreach (string text in (from k in source
			select new KeyValuePair<string, int>(k, currentSnapshot.TryGetValue(k, 0) - MaterialAllocator.snapshot.TryGetValue(k, 0)) into kvp
			orderby kvp.Value descending
			select kvp into g
			select string.Format("{0}: {1}", g.Value, g.Key)).Take(20))
			{
				Log.Error(text, false);
			}
		}

		// Token: 0x04001488 RID: 5256
		private static Dictionary<Material, MaterialAllocator.MaterialInfo> references = new Dictionary<Material, MaterialAllocator.MaterialInfo>();

		// Token: 0x04001489 RID: 5257
		public static int nextWarningThreshold;

		// Token: 0x0400148A RID: 5258
		private static Dictionary<string, int> snapshot = new Dictionary<string, int>();

		// Token: 0x0200046E RID: 1134
		private struct MaterialInfo
		{
			// Token: 0x0400148B RID: 5259
			public string stackTrace;
		}
	}
}
