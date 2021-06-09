using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02002064 RID: 8292
	public abstract class WorldLayer_WorldObjects : WorldLayer
	{
		// Token: 0x0600AFD9 RID: 45017
		protected abstract bool ShouldSkip(WorldObject worldObject);

		// Token: 0x0600AFDA RID: 45018 RVA: 0x0007257C File Offset: 0x0007077C
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
						Log.ErrorOnce("World object " + worldObject + " returned null material.", Gen.HashCombineInt(1948576891, worldObject.ID), false);
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
