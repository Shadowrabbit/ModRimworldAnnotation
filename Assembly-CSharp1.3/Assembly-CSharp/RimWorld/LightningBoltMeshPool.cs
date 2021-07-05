using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000D00 RID: 3328
	public static class LightningBoltMeshPool
	{
		// Token: 0x17000D69 RID: 3433
		// (get) Token: 0x06004DC9 RID: 19913 RVA: 0x001A18E4 File Offset: 0x0019FAE4
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

		// Token: 0x04002EF0 RID: 12016
		private static List<Mesh> boltMeshes = new List<Mesh>();

		// Token: 0x04002EF1 RID: 12017
		private const int NumBoltMeshesMax = 20;
	}
}
