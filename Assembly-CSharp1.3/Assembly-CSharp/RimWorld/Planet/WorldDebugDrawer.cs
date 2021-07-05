using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02001754 RID: 5972
	public class WorldDebugDrawer
	{
		// Token: 0x060089D9 RID: 35289 RVA: 0x00318414 File Offset: 0x00316614
		public void FlashTile(int tile, float colorPct = 0f, string text = null, int duration = 50)
		{
			DebugTile debugTile = new DebugTile();
			debugTile.tile = tile;
			debugTile.displayString = text;
			debugTile.colorPct = colorPct;
			debugTile.ticksLeft = duration;
			this.debugTiles.Add(debugTile);
		}

		// Token: 0x060089DA RID: 35290 RVA: 0x00318450 File Offset: 0x00316650
		public void FlashTile(int tile, Material mat, string text = null, int duration = 50)
		{
			DebugTile debugTile = new DebugTile();
			debugTile.tile = tile;
			debugTile.displayString = text;
			debugTile.customMat = mat;
			debugTile.ticksLeft = duration;
			this.debugTiles.Add(debugTile);
		}

		// Token: 0x060089DB RID: 35291 RVA: 0x0031848C File Offset: 0x0031668C
		public void FlashLine(Vector3 a, Vector3 b, bool onPlanetSurface = false, int duration = 50)
		{
			DebugWorldLine debugWorldLine = new DebugWorldLine(a, b, onPlanetSurface);
			debugWorldLine.TicksLeft = duration;
			this.debugLines.Add(debugWorldLine);
		}

		// Token: 0x060089DC RID: 35292 RVA: 0x003184B8 File Offset: 0x003166B8
		public void FlashLine(int tileA, int tileB, int duration = 50)
		{
			WorldGrid worldGrid = Find.WorldGrid;
			Vector3 tileCenter = worldGrid.GetTileCenter(tileA);
			Vector3 tileCenter2 = worldGrid.GetTileCenter(tileB);
			DebugWorldLine debugWorldLine = new DebugWorldLine(tileCenter, tileCenter2, true);
			debugWorldLine.TicksLeft = duration;
			this.debugLines.Add(debugWorldLine);
		}

		// Token: 0x060089DD RID: 35293 RVA: 0x003184F8 File Offset: 0x003166F8
		public void WorldDebugDrawerUpdate()
		{
			for (int i = 0; i < this.debugTiles.Count; i++)
			{
				this.debugTiles[i].Draw();
			}
			for (int j = 0; j < this.debugLines.Count; j++)
			{
				this.debugLines[j].Draw();
			}
		}

		// Token: 0x060089DE RID: 35294 RVA: 0x00318554 File Offset: 0x00316754
		public void WorldDebugDrawerTick()
		{
			for (int i = this.debugTiles.Count - 1; i >= 0; i--)
			{
				DebugTile debugTile = this.debugTiles[i];
				debugTile.ticksLeft--;
				if (debugTile.ticksLeft <= 0)
				{
					this.debugTiles.RemoveAt(i);
				}
			}
			for (int j = this.debugLines.Count - 1; j >= 0; j--)
			{
				DebugWorldLine debugWorldLine = this.debugLines[j];
				debugWorldLine.ticksLeft--;
				if (debugWorldLine.ticksLeft <= 0)
				{
					this.debugLines.RemoveAt(j);
				}
			}
		}

		// Token: 0x060089DF RID: 35295 RVA: 0x003185F0 File Offset: 0x003167F0
		public void WorldDebugDrawerOnGUI()
		{
			Text.Font = GameFont.Tiny;
			Text.Anchor = TextAnchor.MiddleCenter;
			GUI.color = new Color(1f, 1f, 1f, 0.5f);
			for (int i = 0; i < this.debugTiles.Count; i++)
			{
				if (this.debugTiles[i].DistanceToCamera <= 39f)
				{
					this.debugTiles[i].OnGUI();
				}
			}
			GUI.color = Color.white;
			Text.Anchor = TextAnchor.UpperLeft;
		}

		// Token: 0x040057A0 RID: 22432
		private List<DebugTile> debugTiles = new List<DebugTile>();

		// Token: 0x040057A1 RID: 22433
		private List<DebugWorldLine> debugLines = new List<DebugWorldLine>();

		// Token: 0x040057A2 RID: 22434
		private const int DefaultLifespanTicks = 50;

		// Token: 0x040057A3 RID: 22435
		private const float MaxDistToCameraToDisplayLabel = 39f;
	}
}
