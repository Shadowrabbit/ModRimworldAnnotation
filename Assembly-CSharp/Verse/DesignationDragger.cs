using System;
using System.Collections.Generic;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x02000542 RID: 1346
	[StaticConstructorOnStartup]
	public class DesignationDragger
	{
		// Token: 0x17000683 RID: 1667
		// (get) Token: 0x06002284 RID: 8836 RVA: 0x0001D9E2 File Offset: 0x0001BBE2
		public bool Dragging
		{
			get
			{
				return this.dragging;
			}
		}

		// Token: 0x17000684 RID: 1668
		// (get) Token: 0x06002285 RID: 8837 RVA: 0x0001D9EA File Offset: 0x0001BBEA
		private Designator SelDes
		{
			get
			{
				return Find.DesignatorManager.SelectedDesignator;
			}
		}

		// Token: 0x17000685 RID: 1669
		// (get) Token: 0x06002286 RID: 8838 RVA: 0x0001D9F6 File Offset: 0x0001BBF6
		public List<IntVec3> DragCells
		{
			get
			{
				this.UpdateDragCellsIfNeeded();
				return this.dragCells;
			}
		}

		// Token: 0x17000686 RID: 1670
		// (get) Token: 0x06002287 RID: 8839 RVA: 0x0001DA04 File Offset: 0x0001BC04
		public string FailureReason
		{
			get
			{
				this.UpdateDragCellsIfNeeded();
				return this.failureReasonInt;
			}
		}

		// Token: 0x06002288 RID: 8840 RVA: 0x0001DA12 File Offset: 0x0001BC12
		public void StartDrag()
		{
			this.dragging = true;
			this.startDragCell = UI.MouseCell();
		}

		// Token: 0x06002289 RID: 8841 RVA: 0x0001DA26 File Offset: 0x0001BC26
		public void EndDrag()
		{
			this.dragging = false;
			this.lastDragRealTime = -99999f;
			this.lastFrameDragCellsDrawn = 0;
			if (this.sustainer != null)
			{
				this.sustainer.End();
				this.sustainer = null;
			}
		}

		// Token: 0x0600228A RID: 8842 RVA: 0x0010A3B4 File Offset: 0x001085B4
		public void DraggerUpdate()
		{
			if (this.dragging)
			{
				List<IntVec3> list = this.DragCells;
				this.SelDes.RenderHighlight(list);
				if (list.Count != this.lastFrameDragCellsDrawn)
				{
					this.lastDragRealTime = Time.realtimeSinceStartup;
					this.lastFrameDragCellsDrawn = list.Count;
					if (this.SelDes.soundDragChanged != null)
					{
						this.SelDes.soundDragChanged.PlayOneShotOnCamera(null);
					}
				}
				if (this.sustainer == null || this.sustainer.Ended)
				{
					if (this.SelDes.soundDragSustain != null)
					{
						this.sustainer = this.SelDes.soundDragSustain.TrySpawnSustainer(SoundInfo.OnCamera(MaintenanceType.PerFrame));
						return;
					}
				}
				else
				{
					this.sustainer.externalParams["TimeSinceDrag"] = Time.realtimeSinceStartup - this.lastDragRealTime;
					this.sustainer.Maintain();
				}
			}
		}

		// Token: 0x0600228B RID: 8843 RVA: 0x0010A490 File Offset: 0x00108690
		public void DraggerOnGUI()
		{
			if (this.dragging && this.SelDes != null && this.SelDes.DragDrawMeasurements)
			{
				IntVec3 intVec = this.startDragCell - UI.MouseCell();
				intVec.x = Mathf.Abs(intVec.x) + 1;
				intVec.z = Mathf.Abs(intVec.z) + 1;
				if (intVec.x >= 3)
				{
					Vector2 screenPos = (this.startDragCell.ToUIPosition() + UI.MouseCell().ToUIPosition()) / 2f;
					screenPos.y = this.startDragCell.ToUIPosition().y;
					Widgets.DrawNumberOnMap(screenPos, intVec.x, Color.white);
				}
				if (intVec.z >= 3)
				{
					Vector2 screenPos2 = (this.startDragCell.ToUIPosition() + UI.MouseCell().ToUIPosition()) / 2f;
					screenPos2.x = this.startDragCell.ToUIPosition().x;
					Widgets.DrawNumberOnMap(screenPos2, intVec.z, Color.white);
				}
			}
		}

		// Token: 0x0600228C RID: 8844 RVA: 0x0010A5B0 File Offset: 0x001087B0
		[Obsolete]
		private void DrawNumber(Vector2 screenPos, int number)
		{
			Text.Anchor = TextAnchor.MiddleCenter;
			Text.Font = GameFont.Medium;
			Rect rect = new Rect(screenPos.x - 20f, screenPos.y - 15f, 40f, 30f);
			GUI.DrawTexture(rect, TexUI.GrayBg);
			rect.y += 3f;
			Widgets.Label(rect, number.ToStringCached());
		}

		// Token: 0x0600228D RID: 8845 RVA: 0x0010A61C File Offset: 0x0010881C
		private void UpdateDragCellsIfNeeded()
		{
			if (Time.frameCount == this.lastUpdateFrame)
			{
				return;
			}
			this.lastUpdateFrame = Time.frameCount;
			this.dragCells.Clear();
			this.failureReasonInt = null;
			IntVec3 intVec = this.startDragCell;
			IntVec3 intVec2 = UI.MouseCell();
			if (this.SelDes.DraggableDimensions == 1)
			{
				bool flag = true;
				if (Mathf.Abs(intVec.x - intVec2.x) < Mathf.Abs(intVec.z - intVec2.z))
				{
					flag = false;
				}
				if (flag)
				{
					int z = intVec.z;
					if (intVec.x > intVec2.x)
					{
						IntVec3 intVec3 = intVec;
						intVec = intVec2;
						intVec2 = intVec3;
					}
					for (int i = intVec.x; i <= intVec2.x; i++)
					{
						this.TryAddDragCell(new IntVec3(i, intVec.y, z));
					}
				}
				else
				{
					int x = intVec.x;
					if (intVec.z > intVec2.z)
					{
						IntVec3 intVec4 = intVec;
						intVec = intVec2;
						intVec2 = intVec4;
					}
					for (int j = intVec.z; j <= intVec2.z; j++)
					{
						this.TryAddDragCell(new IntVec3(x, intVec.y, j));
					}
				}
			}
			if (this.SelDes.DraggableDimensions == 2)
			{
				IntVec3 intVec5 = intVec;
				IntVec3 intVec6 = intVec2;
				if (intVec6.x > intVec5.x + 50)
				{
					intVec6.x = intVec5.x + 50;
				}
				if (intVec6.z > intVec5.z + 50)
				{
					intVec6.z = intVec5.z + 50;
				}
				if (intVec6.x < intVec5.x)
				{
					if (intVec6.x < intVec5.x - 50)
					{
						intVec6.x = intVec5.x - 50;
					}
					int x2 = intVec5.x;
					intVec5 = new IntVec3(intVec6.x, intVec5.y, intVec5.z);
					intVec6 = new IntVec3(x2, intVec6.y, intVec6.z);
				}
				if (intVec6.z < intVec5.z)
				{
					if (intVec6.z < intVec5.z - 50)
					{
						intVec6.z = intVec5.z - 50;
					}
					int z2 = intVec5.z;
					intVec5 = new IntVec3(intVec5.x, intVec5.y, intVec6.z);
					intVec6 = new IntVec3(intVec6.x, intVec6.y, z2);
				}
				for (int k = intVec5.x; k <= intVec6.x; k++)
				{
					for (int l = intVec5.z; l <= intVec6.z; l++)
					{
						this.TryAddDragCell(new IntVec3(k, intVec5.y, l));
					}
				}
			}
		}

		// Token: 0x0600228E RID: 8846 RVA: 0x0010A8C0 File Offset: 0x00108AC0
		private void TryAddDragCell(IntVec3 c)
		{
			AcceptanceReport acceptanceReport = this.SelDes.CanDesignateCell(c);
			if (acceptanceReport.Accepted)
			{
				this.dragCells.Add(c);
				return;
			}
			if (!acceptanceReport.Reason.NullOrEmpty())
			{
				this.failureReasonInt = acceptanceReport.Reason;
			}
		}

		// Token: 0x0400176C RID: 5996
		private bool dragging;

		// Token: 0x0400176D RID: 5997
		private IntVec3 startDragCell;

		// Token: 0x0400176E RID: 5998
		private int lastFrameDragCellsDrawn;

		// Token: 0x0400176F RID: 5999
		private Sustainer sustainer;

		// Token: 0x04001770 RID: 6000
		private float lastDragRealTime = -1000f;

		// Token: 0x04001771 RID: 6001
		private List<IntVec3> dragCells = new List<IntVec3>();

		// Token: 0x04001772 RID: 6002
		private string failureReasonInt;

		// Token: 0x04001773 RID: 6003
		private int lastUpdateFrame = -1;

		// Token: 0x04001774 RID: 6004
		private const int MaxSquareWidth = 50;
	}
}
