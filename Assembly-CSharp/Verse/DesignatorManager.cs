using System;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x02000549 RID: 1353
	public class DesignatorManager
	{
		// Token: 0x17000693 RID: 1683
		// (get) Token: 0x060022C6 RID: 8902 RVA: 0x0001DC83 File Offset: 0x0001BE83
		public Designator SelectedDesignator
		{
			get
			{
				return this.selectedDesignator;
			}
		}

		// Token: 0x17000694 RID: 1684
		// (get) Token: 0x060022C7 RID: 8903 RVA: 0x0001DC8B File Offset: 0x0001BE8B
		public DesignationDragger Dragger
		{
			get
			{
				return this.dragger;
			}
		}

		// Token: 0x060022C8 RID: 8904 RVA: 0x0001DC93 File Offset: 0x0001BE93
		public void Select(Designator des)
		{
			this.Deselect();
			this.selectedDesignator = des;
			this.selectedDesignator.Selected();
		}

		// Token: 0x060022C9 RID: 8905 RVA: 0x0001DCAD File Offset: 0x0001BEAD
		public void Deselect()
		{
			if (this.selectedDesignator != null)
			{
				this.selectedDesignator = null;
				this.dragger.EndDrag();
			}
		}

		// Token: 0x060022CA RID: 8906 RVA: 0x0001DCC9 File Offset: 0x0001BEC9
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

		// Token: 0x060022CB RID: 8907 RVA: 0x0010B154 File Offset: 0x00109354
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

		// Token: 0x060022CC RID: 8908 RVA: 0x0001DCEB File Offset: 0x0001BEEB
		public void DesignationManagerOnGUI()
		{
			this.dragger.DraggerOnGUI();
			if (this.CheckSelectedDesignatorValid())
			{
				this.selectedDesignator.DrawMouseAttachments();
			}
		}

		// Token: 0x060022CD RID: 8909 RVA: 0x0001DD0B File Offset: 0x0001BF0B
		public void DesignatorManagerUpdate()
		{
			this.dragger.DraggerUpdate();
			if (this.CheckSelectedDesignatorValid())
			{
				this.selectedDesignator.SelectedUpdate();
			}
		}

		// Token: 0x04001790 RID: 6032
		private Designator selectedDesignator;

		// Token: 0x04001791 RID: 6033
		private DesignationDragger dragger = new DesignationDragger();
	}
}
