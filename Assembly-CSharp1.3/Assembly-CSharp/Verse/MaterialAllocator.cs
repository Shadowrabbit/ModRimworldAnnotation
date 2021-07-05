using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000304 RID: 772
	internal static class MaterialAllocator
	{
		// Token: 0x06001647 RID: 5703 RVA: 0x00081D10 File Offset: 0x0007FF10
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

		// Token: 0x06001648 RID: 5704 RVA: 0x00081D58 File Offset: 0x0007FF58
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

		// Token: 0x06001649 RID: 5705 RVA: 0x00081DA0 File Offset: 0x0007FFA0
		public static void Destroy(Material material)
		{
			if (!MaterialAllocator.references.ContainsKey(material))
			{
				Log.Error(string.Format("Destroying material {0}, but that material was not created through the MaterialTracker", material));
			}
			MaterialAllocator.references.Remove(material);
			UnityEngine.Object.Destroy(material);
		}

		// Token: 0x0600164A RID: 5706 RVA: 0x00081DD4 File Offset: 0x0007FFD4
		public static void TryReport()
		{
			if (MaterialAllocator.MaterialWarningThreshold() > MaterialAllocator.nextWarningThreshold)
			{
				MaterialAllocator.nextWarningThreshold = MaterialAllocator.MaterialWarningThreshold();
			}
			if (MaterialAllocator.references.Count > MaterialAllocator.nextWarningThreshold)
			{
				Log.Error(string.Format("Material allocator has allocated {0} materials; this may be a sign of a material leak", MaterialAllocator.references.Count));
				if (Prefs.DevMode)
				{
					MaterialAllocator.MaterialReport();
				}
				MaterialAllocator.nextWarningThreshold *= 2;
			}
		}

		// Token: 0x0600164B RID: 5707 RVA: 0x00081E3E File Offset: 0x0008003E
		public static int MaterialWarningThreshold()
		{
			return int.MaxValue;
		}

		// Token: 0x0600164C RID: 5708 RVA: 0x00081E48 File Offset: 0x00080048
		[DebugOutput("System", false)]
		public static void MaterialReport()
		{
			foreach (string text in (from kvp in MaterialAllocator.references
			group kvp by kvp.Value.stackTrace into g
			orderby g.Count<KeyValuePair<Material, MaterialAllocator.MaterialInfo>>() descending
			select string.Format("{0}: {1}", g.Count<KeyValuePair<Material, MaterialAllocator.MaterialInfo>>(), g.FirstOrDefault<KeyValuePair<Material, MaterialAllocator.MaterialInfo>>().Value.stackTrace)).Take(20))
			{
				Log.Error(text);
			}
		}

		// Token: 0x0600164D RID: 5709 RVA: 0x00081F04 File Offset: 0x00080104
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

		// Token: 0x0600164E RID: 5710 RVA: 0x00081F90 File Offset: 0x00080190
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
				Log.Error(text);
			}
		}

		// Token: 0x04000F83 RID: 3971
		private static Dictionary<Material, MaterialAllocator.MaterialInfo> references = new Dictionary<Material, MaterialAllocator.MaterialInfo>();

		// Token: 0x04000F84 RID: 3972
		public static int nextWarningThreshold;

		// Token: 0x04000F85 RID: 3973
		private static Dictionary<string, int> snapshot = new Dictionary<string, int>();

		// Token: 0x02001A3D RID: 6717
		private struct MaterialInfo
		{
			// Token: 0x0400647D RID: 25725
			public string stackTrace;
		}
	}
}
