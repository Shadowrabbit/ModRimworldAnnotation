using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C84 RID: 3204
	[StaticConstructorOnStartup]
	public class SectionLayer_BridgeProps : SectionLayer
	{
		// Token: 0x17000CEA RID: 3306
		// (get) Token: 0x06004AC4 RID: 19140 RVA: 0x0004C340 File Offset: 0x0004A540
		public override bool Visible
		{
			get
			{
				return DebugViewSettings.drawTerrain;
			}
		}

		// Token: 0x06004AC5 RID: 19141 RVA: 0x0004C347 File Offset: 0x0004A547
		public SectionLayer_BridgeProps(Section section) : base(section)
		{
			this.relevantChangeTypes = MapMeshFlag.Terrain;
		}

		// Token: 0x06004AC6 RID: 19142 RVA: 0x0018B214 File Offset: 0x00189414
		public override void Regenerate()
		{
			base.ClearSubMeshes(MeshParts.All);
			Map map = base.Map;
			TerrainGrid terrainGrid = map.terrainGrid;
			CellRect cellRect = this.section.CellRect;
			float y = AltitudeLayer.TerrainScatter.AltitudeFor();
			foreach (IntVec3 intVec in cellRect)
			{
				if (this.ShouldDrawPropsBelow(intVec, terrainGrid))
				{
					IntVec3 c = intVec;
					c.x++;
					Material material;
					if (c.InBounds(map) && this.ShouldDrawPropsBelow(c, terrainGrid))
					{
						material = SectionLayer_BridgeProps.PropsLoopMat;
					}
					else
					{
						material = SectionLayer_BridgeProps.PropsRightMat;
					}
					LayerSubMesh subMesh = base.GetSubMesh(material);
					int count = subMesh.verts.Count;
					subMesh.verts.Add(new Vector3((float)intVec.x, y, (float)(intVec.z - 1)));
					subMesh.verts.Add(new Vector3((float)intVec.x, y, (float)intVec.z));
					subMesh.verts.Add(new Vector3((float)(intVec.x + 1), y, (float)intVec.z));
					subMesh.verts.Add(new Vector3((float)(intVec.x + 1), y, (float)(intVec.z - 1)));
					subMesh.uvs.Add(new Vector2(0f, 0f));
					subMesh.uvs.Add(new Vector2(0f, 1f));
					subMesh.uvs.Add(new Vector2(1f, 1f));
					subMesh.uvs.Add(new Vector2(1f, 0f));
					subMesh.tris.Add(count);
					subMesh.tris.Add(count + 1);
					subMesh.tris.Add(count + 2);
					subMesh.tris.Add(count);
					subMesh.tris.Add(count + 2);
					subMesh.tris.Add(count + 3);
				}
			}
			base.FinalizeMesh(MeshParts.All);
		}

		// Token: 0x06004AC7 RID: 19143 RVA: 0x0018B458 File Offset: 0x00189658
		private bool ShouldDrawPropsBelow(IntVec3 c, TerrainGrid terrGrid)
		{
			TerrainDef terrainDef = terrGrid.TerrainAt(c);
			if (terrainDef == null || !terrainDef.bridge)
			{
				return false;
			}
			IntVec3 c2 = c;
			c2.z--;
			Map map = base.Map;
			if (!c2.InBounds(map))
			{
				return false;
			}
			TerrainDef terrainDef2 = terrGrid.TerrainAt(c2);
			return !terrainDef2.bridge && (terrainDef2.passability == Traversability.Impassable || c2.SupportsStructureType(map, TerrainDefOf.Bridge.terrainAffordanceNeeded));
		}

		// Token: 0x04002D58 RID: 11608
		private static readonly Material PropsLoopMat = MaterialPool.MatFrom("Terrain/Misc/BridgeProps_Loop", ShaderDatabase.Transparent);

		// Token: 0x04002D59 RID: 11609
		private static readonly Material PropsRightMat = MaterialPool.MatFrom("Terrain/Misc/BridgeProps_Right", ShaderDatabase.Transparent);
	}
}
