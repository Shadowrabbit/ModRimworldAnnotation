using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001336 RID: 4918
	public static class LightningBoltMeshPool
	{
		// Token: 0x1700106C RID: 4204
		// (get) Token: 0x06006AB2 RID: 27314 RVA: 0x0020F3AC File Offset: 0x0020D5AC
		public static Mesh RandomBoltMesh
		{
			get
			{
				if (LightningBoltMeshPool.boltMeshes.Count < 20)
				{
					Mesh mesh = LightningBoltMeshMaker.NewBoltMesh();
					LightningBoltMeshPool.boltMeshes.Add(mesh);
					return mesh;
				}
				return LightningBoltMeshPool.boltMeshes.RandomElement<Mesh>();
			}
		}

		// Token: 0x040046FC RID: 18172
		private static List<Mesh> boltMeshes = new List<Mesh>();

		// Token: 0x040046FD RID: 18173
		private const int NumBoltMeshesMax = 20;
	}
}
