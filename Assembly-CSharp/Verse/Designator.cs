using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x02000543 RID: 1347
	public abstract class Designator : Command
	{
		// Token: 0x17000687 RID: 1671
		// (get) Token: 0x06002290 RID: 8848 RVA: 0x0001DA80 File Offset: 0x0001BC80
		public Map Map
		{
			get
			{
				return Find.CurrentMap;
			}
		}

		// Token: 0x17000688 RID: 1672
		// (get) Token: 0x06002291 RID: 8849 RVA: 0x0000A2E4 File Offset: 0x000084E4
		public virtual int DraggableDimensions
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x17000689 RID: 1673
		// (get) Token: 0x06002292 RID: 8850 RVA: 0x0000A2E4 File Offset: 0x000084E4
		public virtual bool DragDrawMeasurements
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700068A RID: 1674
		// (get) Token: 0x06002293 RID: 8851 RVA: 0x0000A2E4 File Offset: 0x000084E4
		protected override bool DoTooltip
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700068B RID: 1675
		// (get) Token: 0x06002294 RID: 8852 RVA: 0x0000C32E File Offset: 0x0000A52E
		protected virtual DesignationDef Designation
		{
			get
			{
				return null;
			}
		}

		// Token: 0x1700068C RID: 1676
		// (get) Token: 0x06002295 RID: 8853 RVA: 0x00016647 File Offset: 0x00014847
		public virtual float PanelReadoutTitleExtraRightMargin
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x1700068D RID: 1677
		// (get) Token: 0x06002296 RID: 8854 RVA: 0x0001DA87 File Offset: 0x0001BC87
		public override string TutorTagSelect
		{
			get
			{
				if (this.tutorTag == null)
				{
					return null;
				}
				if (this.cachedTutorTagSelect == null)
				{
					this.cachedTutorTagSelect = "SelectDesignator-" + this.tutorTag;
				}
				return this.cachedTutorTagSelect;
			}
		}

		// Token: 0x1700068E RID: 1678
		// (get) Token: 0x06002297 RID: 8855 RVA: 0x0001DAB7 File Offset: 0x0001BCB7
		public string TutorTagDesignate
		{
			get
			{
				if (this.tutorTag == null)
				{
					return null;
				}
				if (this.cachedTutorTagDesignate == null)
				{
					this.cachedTutorTagDesignate = "Designate-" + this.tutorTag;
				}
				return this.cachedTutorTagDesignate;
			}
		}

		// Token: 0x1700068F RID: 1679
		// (get) Token: 0x06002298 RID: 8856 RVA: 0x0001DAE7 File Offset: 0x0001BCE7
		public override string HighlightTag
		{
			get
			{
				if (this.cachedHighlightTag == null && this.tutorTag != null)
				{
					this.cachedHighlightTag = "Designator-" + this.tutorTag;
				}
				return this.cachedHighlightTag;
			}
		}

		// Token: 0x17000690 RID: 1680
		// (get) Token: 0x06002299 RID: 8857 RVA: 0x0001DB15 File Offset: 0x0001BD15
		public override IEnumerable<FloatMenuOption> RightClickFloatMenuOptions
		{
			get
			{
				foreach (FloatMenuOption floatMenuOption in base.RightClickFloatMenuOptions)
				{
					yield return floatMenuOption;
				}
				IEnumerator<FloatMenuOption> enumerator = null;
				if (this.hasDesignateAllFloatMenuOption)
				{
					int num = 0;
					List<Thing> things = this.Map.listerThings.AllThings;
					for (int i = 0; i < things.Count; i++)
					{
						Thing t = things[i];
						if (!t.Fogged() && this.CanDesignateThing(t).Accepted)
						{
							num++;
						}
					}
					if (num > 0)
					{
						yield return new FloatMenuOption(this.designateAllLabel + " (" + "CountToDesignate".Translate(num) + ")", delegate()
						{
							for (int k = 0; k < things.Count; k++)
							{
								Thing t2 = things[k];
								if (!t2.Fogged() && this.CanDesignateThing(t2).Accepted)
								{
									this.DesignateThing(things[k]);
								}
							}
						}, MenuOptionPriority.Default, null, null, 0f, null, null);
					}
					else
					{
						yield return new FloatMenuOption(this.designateAllLabel + " (" + "NoneLower".Translate() + ")", null, MenuOptionPriority.Default, null, null, 0f, null, null);
					}
				}
				DesignationDef designation = this.Designation;
				if (this.Designation != null)
				{
					int num2 = 0;
					List<Designation> designations = this.Map.designationManager.allDesignations;
					for (int j = 0; j < designations.Count; j++)
					{
						if (designations[j].def == designation && this.RemoveAllDesignationsAffects(designations[j].target))
						{
							num2++;
						}
					}
					if (num2 > 0)
					{
						yield return new FloatMenuOption("RemoveAllDesignations".Translate() + " (" + num2 + ")", delegate()
						{
							for (int k = designations.Count - 1; k >= 0; k--)
							{
								if (designations[k].def == designation && this.RemoveAllDesignationsAffects(designations[k].target))
								{
									this.Map.designationManager.RemoveDesignation(designations[k]);
								}
							}
						}, MenuOptionPriority.Default, null, null, 0f, null, null);
					}
					else
					{
						yield return new FloatMenuOption("RemoveAllDesignations".Translate() + " (" + "NoneLower".Translate() + ")", null, MenuOptionPriority.Default, null, null, 0f, null, null);
					}
				}
				yield break;
				yield break;
			}
		}

		// Token: 0x0600229A RID: 8858 RVA: 0x0001DB25 File Offset: 0x0001BD25
		public Designator()
		{
			this.activateSound = SoundDefOf.Tick_Tiny;
			this.designateAllLabel = "DesignateAll".Translate();
		}

		// Token: 0x0600229B RID: 8859 RVA: 0x0001DB58 File Offset: 0x0001BD58
		protected bool CheckCanInteract()
		{
			return !TutorSystem.TutorialMode || TutorSystem.AllowAction(this.TutorTagSelect);
		}

		// Token: 0x0600229C RID: 8860 RVA: 0x0001DB76 File Offset: 0x0001BD76
		public override void ProcessInput(Event ev)
		{
			if (!this.CheckCanInteract())
			{
				return;
			}
			base.ProcessInput(ev);
			Find.DesignatorManager.Select(this);
		}

		// Token: 0x0600229D RID: 8861 RVA: 0x0001DB93 File Offset: 0x0001BD93
		public virtual AcceptanceReport CanDesignateThing(Thing t)
		{
			return AcceptanceReport.WasRejected;
		}

		// Token: 0x0600229E RID: 8862 RVA: 0x0000FC33 File Offset: 0x0000DE33
		public virtual void DesignateThing(Thing t)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600229F RID: 8863
		public abstract AcceptanceReport CanDesignateCell(IntVec3 loc);

		// Token: 0x060022A0 RID: 8864 RVA: 0x0010A90C File Offset: 0x00108B0C
		public virtual void DesignateMultiCell(IEnumerable<IntVec3> cells)
		{
			if (TutorSystem.TutorialMode && !TutorSystem.AllowAction(new EventPack(this.TutorTagDesignate, cells)))
			{
				return;
			}
			bool somethingSucceeded = false;
			bool flag = false;
			foreach (IntVec3 intVec in cells)
			{
				if (this.CanDesignateCell(intVec).Accepted)
				{
					this.DesignateSingleCell(intVec);
					somethingSucceeded = true;
					if (!flag)
					{
						flag = this.ShowWarningForCell(intVec);
					}
				}
			}
			this.Finalize(somethingSucceeded);
			if (TutorSystem.TutorialMode)
			{
				TutorSystem.Notify_Event(new EventPack(this.TutorTagDesignate, cells));
			}
		}

		// Token: 0x060022A1 RID: 8865 RVA: 0x0000FC33 File Offset: 0x0000DE33
		public virtual void DesignateSingleCell(IntVec3 c)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060022A2 RID: 8866 RVA: 0x0000A2E4 File Offset: 0x000084E4
		public virtual bool ShowWarningForCell(IntVec3 c)
		{
			return false;
		}

		// Token: 0x060022A3 RID: 8867 RVA: 0x0001DB9A File Offset: 0x0001BD9A
		public void Finalize(bool somethingSucceeded)
		{
			if (somethingSucceeded)
			{
				this.FinalizeDesignationSucceeded();
				return;
			}
			this.FinalizeDesignationFailed();
		}

		// Token: 0x060022A4 RID: 8868 RVA: 0x0001DBAC File Offset: 0x0001BDAC
		protected virtual void FinalizeDesignationSucceeded()
		{
			if (this.soundSucceeded != null)
			{
				this.soundSucceeded.PlayOneShotOnCamera(null);
			}
		}

		// Token: 0x060022A5 RID: 8869 RVA: 0x0010A9B4 File Offset: 0x00108BB4
		protected virtual void FinalizeDesignationFailed()
		{
			if (this.soundFailed != null)
			{
				this.soundFailed.PlayOneShotOnCamera(null);
			}
			if (Find.DesignatorManager.Dragger.FailureReason != null)
			{
				Messages.Message(Find.DesignatorManager.Dragger.FailureReason, MessageTypeDefOf.RejectInput, false);
			}
		}

		// Token: 0x060022A6 RID: 8870 RVA: 0x0001DBC2 File Offset: 0x0001BDC2
		public virtual string LabelCapReverseDesignating(Thing t)
		{
			return this.LabelCap;
		}

		// Token: 0x060022A7 RID: 8871 RVA: 0x0001DBCA File Offset: 0x0001BDCA
		public virtual string DescReverseDesignating(Thing t)
		{
			return this.Desc;
		}

		// Token: 0x060022A8 RID: 8872 RVA: 0x0001DBD2 File Offset: 0x0001BDD2
		public virtual Texture2D IconReverseDesignating(Thing t, out float angle, out Vector2 offset)
		{
			angle = this.iconAngle;
			offset = this.iconOffset;
			return this.icon;
		}

		// Token: 0x060022A9 RID: 8873 RVA: 0x0000A2A7 File Offset: 0x000084A7
		protected virtual bool RemoveAllDesignationsAffects(LocalTargetInfo target)
		{
			return true;
		}

		// Token: 0x060022AA RID: 8874 RVA: 0x0010AA00 File Offset: 0x00108C00
		public virtual void DrawMouseAttachments()
		{
			if (this.useMouseIcon)
			{
				GenUI.DrawMouseAttachment(this.icon, "", this.iconAngle, this.iconOffset, null, false, default(Color));
			}
		}

		// Token: 0x060022AB RID: 8875 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void DrawPanelReadout(ref float curY, float width)
		{
		}

		// Token: 0x060022AC RID: 8876 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void DoExtraGuiControls(float leftX, float bottomY)
		{
		}

		// Token: 0x060022AD RID: 8877 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void SelectedUpdate()
		{
		}

		// Token: 0x060022AE RID: 8878 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void SelectedProcessInput(Event ev)
		{
		}

		// Token: 0x060022AF RID: 8879 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void Rotate(RotationDirection rotDir)
		{
		}

		// Token: 0x060022B0 RID: 8880 RVA: 0x0000A2A7 File Offset: 0x000084A7
		public virtual bool CanRemainSelected()
		{
			return true;
		}

		// Token: 0x060022B1 RID: 8881 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void Selected()
		{
		}

		// Token: 0x060022B2 RID: 8882 RVA: 0x0001DBEE File Offset: 0x0001BDEE
		public virtual void RenderHighlight(List<IntVec3> dragCells)
		{
			DesignatorUtility.RenderHighlightOverSelectableThings(this, dragCells);
		}

		// Token: 0x04001775 RID: 6005
		protected bool useMouseIcon;

		// Token: 0x04001776 RID: 6006
		public bool isOrder;

		// Token: 0x04001777 RID: 6007
		public SoundDef soundDragSustain;

		// Token: 0x04001778 RID: 6008
		public SoundDef soundDragChanged;

		// Token: 0x04001779 RID: 6009
		protected SoundDef soundSucceeded;

		// Token: 0x0400177A RID: 6010
		protected SoundDef soundFailed = SoundDefOf.Designate_Failed;

		// Token: 0x0400177B RID: 6011
		protected bool hasDesignateAllFloatMenuOption;

		// Token: 0x0400177C RID: 6012
		protected string designateAllLabel;

		// Token: 0x0400177D RID: 6013
		private string cachedTutorTagSelect;

		// Token: 0x0400177E RID: 6014
		private string cachedTutorTagDesignate;

		// Token: 0x0400177F RID: 6015
		protected string cachedHighlightTag;
	}
}
