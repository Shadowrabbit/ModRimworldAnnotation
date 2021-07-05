using System;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x02000397 RID: 919
	public class DesignatorManager
	{
		// Token: 0x17000592 RID: 1426
		// (get) Token: 0x06001B19 RID: 6937 RVA: 0x0009D0FD File Offset: 0x0009B2FD
		public Designator SelectedDesignator
		{
			get
			{
				return this.selectedDesignator;
			}
		}

		// Token: 0x17000593 RID: 1427
		// (get) Token: 0x06001B1A RID: 6938 RVA: 0x0009D105 File Offset: 0x0009B305
		public DesignationDragger Dragger
		{
			get
			{
				return this.dragger;
			}
		}

		// Token: 0x06001B1B RID: 6939 RVA: 0x0009D10D File Offset: 0x0009B30D
		public void Select(Designator des)
		{
			this.Deselect();
			this.selectedDesignator = des;
			this.selectedDesignator.Selected();
		}

		// Token: 0x06001B1C RID: 6940 RVA: 0x0009D127 File Offset: 0x0009B327
		public void Deselect()
		{
			if (this.selectedDesignator != null)
			{
				this.selectedDesignator = null;
				this.dragger.EndDrag();
			}
		}

		// Token: 0x06001B1D RID: 6941 RVA: 0x0009D143 File Offset: 0x0009B343
		private bool CheckSelectedDesignatorValid()
		{
			if (this.selectedDesignator == null)
			{
				return false;
			}
			if (!this.selectedDesignator.CanRemainSelected())
			{
				this.Deselect();
				return false;
			}
			return true;
		}

		// Token: 0x06001B1E RID: 6942 RVA: 0x0009D168 File Offset: 0x0009B368
		public void ProcessInputEvents()
		{
			if (!this.CheckSelectedDesignatorValid())
			{
				return;
			}
			if (Event.current.type == EventType.MouseDown && Event.current.button == 0)
			{
				if (this.selectedDesignator.DraggableDimensions == 0)
				{
					Designator designator = this.selectedDesignator;
					AcceptanceReport acceptanceReport = this.selectedDesignator.CanDesignateCell(UI.MouseCell());
					if (acceptanceReport.Accepted)
					{
						designator.DesignateSingleCell(UI.MouseCell());
						designator.Finalize(true);
					}
					else
					{
						Messages.Message(acceptanceReport.Reason, MessageTypeDefOf.SilentInput, false);
						this.selectedDesignator.Finalize(false);
					}
				}
				else
				{
					this.dragger.StartDrag();
				}
				Event.current.Use();
			}
			if ((Event.current.type == EventType.MouseDown && Event.current.button == 1) || KeyBindingDefOf.Cancel.KeyDownEvent)
			{
				SoundDefOf.CancelMode.PlayOneShotOnCamera(null);
				this.Deselect();
				this.dragger.EndDrag();
				Event.current.Use();
				TutorSystem.Notify_Event("ClearDesignatorSelection");
			}
			if (Event.current.type == EventType.MouseUp && Event.current.button == 0 && this.dragger.Dragging)
			{
				this.selectedDesignator.DesignateMultiCell(this.dragger.DragCells);
				this.dragger.EndDrag();
				Event.current.Use();
			}
		}

		// Token: 0x06001B1F RID: 6943 RVA: 0x0009D2BB File Offset: 0x0009B4BB
		public void DesignationManagerOnGUI()
		{
			this.dragger.DraggerOnGUI();
			if (this.CheckSelectedDesignatorValid())
			{
				this.selectedDesignator.DrawMouseAttachments();
			}
		}

		// Token: 0x06001B20 RID: 6944 RVA: 0x0009D2DB File Offset: 0x0009B4DB
		public void DesignatorManagerUpdate()
		{
			this.dragger.DraggerUpdate();
			if (this.CheckSelectedDesignatorValid())
			{
				this.selectedDesignator.SelectedUpdate();
			}
		}

		// Token: 0x0400119A RID: 4506
		private Designator selectedDesignator;

		// Token: 0x0400119B RID: 4507
		private DesignationDragger dragger = new DesignationDragger();
	}
}
