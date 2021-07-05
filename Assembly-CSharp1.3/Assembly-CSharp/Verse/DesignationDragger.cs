using System;
using System.Collections.Generic;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x02000394 RID: 916
	[StaticConstructorOnStartup]
	public class DesignationDragger
	{
		// Token: 0x17000583 RID: 1411
		// (get) Token: 0x06001AE3 RID: 6883 RVA: 0x0009C50C File Offset: 0x0009A70C
		public bool Dragging
		{
			get
			{
				return this.dragging;
			}
		}

		// Token: 0x17000584 RID: 1412
		// (get) Token: 0x06001AE4 RID: 6884 RVA: 0x0009C514 File Offset: 0x0009A714
		private Designator SelDes
		{
			get
			{
				return Find.DesignatorManager.SelectedDesignator;
			}
		}

		// Token: 0x17000585 RID: 1413
		// (get) Token: 0x06001AE5 RID: 6885 RVA: 0x0009C520 File Offset: 0x0009A720
		public List<IntVec3> DragCells
		{
			get
			{
				this.UpdateDragCellsIfNeeded();
				return this.dragCells;
			}
		}

		// Token: 0x17000586 RID: 1414
		// (get) Token: 0x06001AE6 RID: 6886 RVA: 0x0009C52E File Offset: 0x0009A72E
		public string FailureReason
		{
			get
			{
				this.UpdateDragCellsIfNeeded();
				return this.failureReasonInt;
			}
		}

		// Token: 0x06001AE7 RID: 6887 RVA: 0x0009C53C File Offset: 0x0009A73C
		public void StartDrag()
		{
			this.dragging = true;
			this.startDragCell = UI.MouseCell();
		}

		// Token: 0x06001AE8 RID: 6888 RVA: 0x0009C550 File Offset: 0x0009A750
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

		// Token: 0x06001AE9 RID: 6889 RVA: 0x0009C588 File Offset: 0x0009A788
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

		// Token: 0x06001AEA RID: 6890 RVA: 0x0009C664 File Offset: 0x0009A864
		public void DraggerOnGUI()
		{
			if (this.dragging && this.SelDes != null)
			{
				IntVec3 intVec = this.startDragCell - UI.MouseCell();
				intVec.x = Mathf.Abs(intVec.x) + 1;
				intVec.z = Mathf.Abs(intVec.z) + 1;
				if (this.SelDes.DragDrawOutline && (intVec.x > 1 || intVec.z > 1))
				{
					IntVec3 intVec2 = UI.MouseCell();
					Vector3 v = new Vector3((float)Mathf.Min(this.startDragCell.x, intVec2.x), 0f, (float)Mathf.Min(this.startDragCell.z, intVec2.z));
					Vector3 v2 = new Vector3((float)(Mathf.Max(this.startDragCell.x, intVec2.x) + 1), 0f, (float)(Mathf.Max(this.startDragCell.z, intVec2.z) + 1));
					Vector2 vector = v.MapToUIPosition();
					Vector2 vector2 = v2.MapToUIPosition();
					Widgets.DrawBox(Rect.MinMaxRect(vector.x, vector.y, vector2.x, vector2.y), 1, DesignationDragger.OutlineTex);
				}
				if (this.SelDes.DragDrawMeasurements)
				{
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
		}

		// Token: 0x06001AEB RID: 6891 RVA: 0x0009C864 File Offset: 0x0009AA64
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

		// Token: 0x06001AEC RID: 6892 RVA: 0x0009C8D0 File Offset: 0x0009AAD0
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

		// Token: 0x06001AED RID: 6893 RVA: 0x0009CB74 File Offset: 0x0009AD74
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

		// Token: 0x04001181 RID: 4481
		private bool dragging;

		// Token: 0x04001182 RID: 4482
		private IntVec3 startDragCell;

		// Token: 0x04001183 RID: 4483
		private int lastFrameDragCellsDrawn;

		// Token: 0x04001184 RID: 4484
		private Sustainer sustainer;

		// Token: 0x04001185 RID: 4485
		private float lastDragRealTime = -1000f;

		// Token: 0x04001186 RID: 4486
		private List<IntVec3> dragCells = new List<IntVec3>();

		// Token: 0x04001187 RID: 4487
		private string failureReasonInt;

		// Token: 0x04001188 RID: 4488
		private int lastUpdateFrame = -1;

		// Token: 0x04001189 RID: 4489
		private const int MaxSquareWidth = 50;

		// Token: 0x0400118A RID: 4490
		private static readonly Texture2D OutlineTex = SolidColorMaterials.NewSolidColorTexture(new Color32(109, 139, 79, 100));
	}
}
