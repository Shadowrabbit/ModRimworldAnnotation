using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x0200176D RID: 5997
	[StaticConstructorOnStartup]
	public class WorldRenderer
	{
		// Token: 0x17001693 RID: 5779
		// (get) Token: 0x06008A58 RID: 35416 RVA: 0x0031A670 File Offset: 0x00318870
		private bool ShouldRegenerateDirtyLayersInLongEvent
		{
			get
			{
				for (int i = 0; i < this.layers.Count; i++)
				{
					if (this.layers[i].Dirty && this.layers[i] is WorldLayer_Terrain)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x06008A59 RID: 35417 RVA: 0x0031A6BC File Offset: 0x003188BC
		public WorldRenderer()
		{
			foreach (Type type in typeof(WorldLayer).AllLeafSubclasses())
			{
				this.layers.Add((WorldLayer)Activator.CreateInstance(type));
			}
		}

		// Token: 0x06008A5A RID: 35418 RVA: 0x0031A734 File Offset: 0x00318934
		public void SetAllLayersDirty()
		{
			for (int i = 0; i < this.layers.Count; i++)
			{
				this.layers[i].SetDirty();
			}
		}

		// Token: 0x06008A5B RID: 35419 RVA: 0x0031A768 File Offset: 0x00318968
		public void SetDirty<T>() where T : WorldLayer
		{
			for (int i = 0; i < this.layers.Count; i++)
			{
				if (this.layers[i] is T)
				{
					this.layers[i].SetDirty();
				}
			}
		}

		// Token: 0x06008A5C RID: 35420 RVA: 0x0031A7B0 File Offset: 0x003189B0
		public void RegenerateAllLayersNow()
		{
			for (int i = 0; i < this.layers.Count; i++)
			{
				this.layers[i].RegenerateNow();
			}
		}

		// Token: 0x06008A5D RID: 35421 RVA: 0x0031A7E4 File Offset: 0x003189E4
		private IEnumerable RegenerateDirtyLayersNow_Async()
		{
			int num;
			for (int i = 0; i < this.layers.Count; i = num + 1)
			{
				if (this.layers[i].Dirty)
				{
					using (IEnumerator enumerator = this.layers[i].Regenerate().GetEnumerator())
					{
						for (;;)
						{
							try
							{
								if (!enumerator.MoveNext())
								{
									break;
								}
							}
							catch (Exception arg)
							{
								Log.Error("Could not regenerate WorldLayer: " + arg);
								break;
							}
							yield return enumerator.Current;
						}
					}
					yield return null;
					IEnumerator enumerator = null;
				}
				num = i;
			}
			this.asynchronousRegenerationActive = false;
			yield break;
			yield break;
		}

		// Token: 0x06008A5E RID: 35422 RVA: 0x0031A7F4 File Offset: 0x003189F4
		public void Notify_StaticWorldObjectPosChanged()
		{
			for (int i = 0; i < this.layers.Count; i++)
			{
				WorldLayer_WorldObjects worldLayer_WorldObjects = this.layers[i] as WorldLayer_WorldObjects;
				if (worldLayer_WorldObjects != null)
				{
					worldLayer_WorldObjects.SetDirty();
				}
			}
		}

		// Token: 0x06008A5F RID: 35423 RVA: 0x0031A832 File Offset: 0x00318A32
		public void CheckActivateWorldCamera()
		{
			Find.WorldCamera.gameObject.SetActive(WorldRendererUtility.WorldRenderedNow);
		}

		// Token: 0x06008A60 RID: 35424 RVA: 0x0031A848 File Offset: 0x00318A48
		public void DrawWorldLayers()
		{
			if (this.asynchronousRegenerationActive)
			{
				Log.Error("Called DrawWorldLayers() but already regenerating. This shouldn't ever happen because LongEventHandler should have stopped us.");
				return;
			}
			if (this.ShouldRegenerateDirtyLayersInLongEvent)
			{
				this.asynchronousRegenerationActive = true;
				LongEventHandler.QueueLongEvent(this.RegenerateDirtyLayersNow_Async(), "GeneratingPlanet", null, false);
				return;
			}
			WorldRendererUtility.UpdateWorldShadersParams();
			for (int i = 0; i < this.layers.Count; i++)
			{
				try
				{
					this.layers[i].Render();
				}
				catch (Exception arg)
				{
					Log.Error("Error drawing WorldLayer: " + arg);
				}
			}
		}

		// Token: 0x06008A61 RID: 35425 RVA: 0x0031A8DC File Offset: 0x00318ADC
		public int GetTileIDFromRayHit(RaycastHit hit)
		{
			int i = 0;
			int count = this.layers.Count;
			while (i < count)
			{
				WorldLayer_Terrain worldLayer_Terrain = this.layers[i] as WorldLayer_Terrain;
				if (worldLayer_Terrain != null)
				{
					return worldLayer_Terrain.GetTileIDFromRayHit(hit);
				}
				i++;
			}
			return -1;
		}

		// Token: 0x0400580B RID: 22539
		private List<WorldLayer> layers = new List<WorldLayer>();

		// Token: 0x0400580C RID: 22540
		public WorldRenderMode wantedMode;

		// Token: 0x0400580D RID: 22541
		private bool asynchronousRegenerationActive;
	}
}
