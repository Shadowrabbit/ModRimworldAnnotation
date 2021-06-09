using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x0200206B RID: 8299
	[StaticConstructorOnStartup]
	public class WorldRenderer
	{
		// Token: 0x17001A0D RID: 6669
		// (get) Token: 0x0600AFFF RID: 45055 RVA: 0x00331FB4 File Offset: 0x003301B4
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

		// Token: 0x0600B000 RID: 45056 RVA: 0x00332000 File Offset: 0x00330200
		public WorldRenderer()
		{
			foreach (Type type in typeof(WorldLayer).AllLeafSubclasses())
			{
				this.layers.Add((WorldLayer)Activator.CreateInstance(type));
			}
		}

		// Token: 0x0600B001 RID: 45057 RVA: 0x00332078 File Offset: 0x00330278
		public void SetAllLayersDirty()
		{
			for (int i = 0; i < this.layers.Count; i++)
			{
				this.layers[i].SetDirty();
			}
		}

		// Token: 0x0600B002 RID: 45058 RVA: 0x003320AC File Offset: 0x003302AC
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

		// Token: 0x0600B003 RID: 45059 RVA: 0x003320F4 File Offset: 0x003302F4
		public void RegenerateAllLayersNow()
		{
			for (int i = 0; i < this.layers.Count; i++)
			{
				this.layers[i].RegenerateNow();
			}
		}

		// Token: 0x0600B004 RID: 45060 RVA: 0x00072663 File Offset: 0x00070863
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
								Log.Error("Could not regenerate WorldLayer: " + arg, false);
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

		// Token: 0x0600B005 RID: 45061 RVA: 0x00332128 File Offset: 0x00330328
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

		// Token: 0x0600B006 RID: 45062 RVA: 0x00072673 File Offset: 0x00070873
		public void CheckActivateWorldCamera()
		{
			Find.WorldCamera.gameObject.SetActive(WorldRendererUtility.WorldRenderedNow);
		}

		// Token: 0x0600B007 RID: 45063 RVA: 0x00332168 File Offset: 0x00330368
		public void DrawWorldLayers()
		{
			if (this.asynchronousRegenerationActive)
			{
				Log.Error("Called DrawWorldLayers() but already regenerating. This shouldn't ever happen because LongEventHandler should have stopped us.", false);
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
					Log.Error("Error drawing WorldLayer: " + arg, false);
				}
			}
		}

		// Token: 0x0600B008 RID: 45064 RVA: 0x00332200 File Offset: 0x00330400
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

		// Token: 0x04007918 RID: 31000
		private List<WorldLayer> layers = new List<WorldLayer>();

		// Token: 0x04007919 RID: 31001
		public WorldRenderMode wantedMode;

		// Token: 0x0400791A RID: 31002
		private bool asynchronousRegenerationActive;
	}
}
