using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02001768 RID: 5992
	public abstract class WorldLayer_WorldObjects : WorldLayer
	{
		// Token: 0x06008A3F RID: 35391
		protected abstract bool ShouldSkip(WorldObject worldObject);

		// Token: 0x06008A40 RID: 35392 RVA: 0x00319B9A File Offset: 0x00317D9A
		public override IEnumerable Regenerate()
		{
			foreach (object obj in base.Regenerate())
			{
				yield return obj;
			}
			IEnumerator enumerator = null;
			List<WorldObject> allWorldObjects = Find.WorldObjects.AllWorldObjects;
			for (int i = 0; i < allWorldObjects.Count; i++)
			{
				WorldObject worldObject = allWorldObjects[i];
				if (!worldObject.def.useDynamicDrawer && !this.ShouldSkip(worldObject))
				{
					Material material = worldObject.Material;
					if (material == null)
					{
						Log.ErrorOnce("World object " + worldObject + " returned null material.", Gen.HashCombineInt(1948576891, worldObject.ID));
					}
					else
					{
						LayerSubMesh subMesh = base.GetSubMesh(material);
						Rand.PushState();
						Rand.Seed = worldObject.ID;
						worldObject.Print(subMesh);
						Rand.PopState();
					}
				}
			}
			base.FinalizeMesh(MeshParts.All);
			yield break;
			yield break;
		}
	}
}
