using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x020001C5 RID: 453
	public static class MapComponentUtility
	{
		// Token: 0x06000D2F RID: 3375 RVA: 0x00046A30 File Offset: 0x00044C30
		public static void MapComponentUpdate(Map map)
		{
			List<MapComponent> components = map.components;
			for (int i = 0; i < components.Count; i++)
			{
				try
				{
					components[i].MapComponentUpdate();
				}
				catch (Exception ex)
				{
					Log.Error(ex.ToString());
				}
			}
		}

		// Token: 0x06000D30 RID: 3376 RVA: 0x00046A80 File Offset: 0x00044C80
		public static void MapComponentTick(Map map)
		{
			List<MapComponent> components = map.components;
			for (int i = 0; i < components.Count; i++)
			{
				try
				{
					components[i].MapComponentTick();
				}
				catch (Exception ex)
				{
					Log.Error(ex.ToString());
				}
			}
		}

		// Token: 0x06000D31 RID: 3377 RVA: 0x00046AD0 File Offset: 0x00044CD0
		public static void MapComponentOnGUI(Map map)
		{
			List<MapComponent> components = map.components;
			for (int i = 0; i < components.Count; i++)
			{
				try
				{
					components[i].MapComponentOnGUI();
				}
				catch (Exception ex)
				{
					Log.Error(ex.ToString());
				}
			}
		}

		// Token: 0x06000D32 RID: 3378 RVA: 0x00046B20 File Offset: 0x00044D20
		public static void FinalizeInit(Map map)
		{
			List<MapComponent> components = map.components;
			for (int i = 0; i < components.Count; i++)
			{
				try
				{
					components[i].FinalizeInit();
				}
				catch (Exception ex)
				{
					Log.Error(ex.ToString());
				}
			}
		}

		// Token: 0x06000D33 RID: 3379 RVA: 0x00046B70 File Offset: 0x00044D70
		public static void MapGenerated(Map map)
		{
			List<MapComponent> components = map.components;
			for (int i = 0; i < components.Count; i++)
			{
				try
				{
					components[i].MapGenerated();
				}
				catch (Exception ex)
				{
					Log.Error(ex.ToString());
				}
			}
		}

		// Token: 0x06000D34 RID: 3380 RVA: 0x00046BC0 File Offset: 0x00044DC0
		public static void MapRemoved(Map map)
		{
			List<MapComponent> components = map.components;
			for (int i = 0; i < components.Count; i++)
			{
				try
				{
					components[i].MapRemoved();
				}
				catch (Exception arg)
				{
					Log.Error("Could not notify map component: " + arg);
				}
			}
		}
	}
}
