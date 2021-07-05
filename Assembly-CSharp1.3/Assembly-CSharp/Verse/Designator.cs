using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x02000395 RID: 917
	public abstract class Designator : Command
	{
		// Token: 0x17000587 RID: 1415
		// (get) Token: 0x06001AF0 RID: 6896 RVA: 0x0009CC05 File Offset: 0x0009AE05
		public Map Map
		{
			get
			{
				return Find.CurrentMap;
			}
		}

		// Token: 0x17000588 RID: 1416
		// (get) Token: 0x06001AF1 RID: 6897 RVA: 0x0001276E File Offset: 0x0001096E
		public virtual int DraggableDimensions
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x17000589 RID: 1417
		// (get) Token: 0x06001AF2 RID: 6898 RVA: 0x0001276E File Offset: 0x0001096E
		public virtual bool DragDrawMeasurements
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700058A RID: 1418
		// (get) Token: 0x06001AF3 RID: 6899 RVA: 0x0009CC0C File Offset: 0x0009AE0C
		public virtual bool DragDrawOutline
		{
			get
			{
				return this.DraggableDimensions == 2;
			}
		}

		// Token: 0x1700058B RID: 1419
		// (get) Token: 0x06001AF4 RID: 6900 RVA: 0x0001276E File Offset: 0x0001096E
		protected override bool DoTooltip
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700058C RID: 1420
		// (get) Token: 0x06001AF5 RID: 6901 RVA: 0x00002688 File Offset: 0x00000888
		protected virtual DesignationDef Designation
		{
			get
			{
				return null;
			}
		}

		// Token: 0x1700058D RID: 1421
		// (get) Token: 0x06001AF6 RID: 6902 RVA: 0x000682C5 File Offset: 0x000664C5
		public virtual float PanelReadoutTitleExtraRightMargin
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x1700058E RID: 1422
		// (get) Token: 0x06001AF7 RID: 6903 RVA: 0x0009CC17 File Offset: 0x0009AE17
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

		// Token: 0x1700058F RID: 1423
		// (get) Token: 0x06001AF8 RID: 6904 RVA: 0x0009CC47 File Offset: 0x0009AE47
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

		// Token: 0x17000590 RID: 1424
		// (get) Token: 0x06001AF9 RID: 6905 RVA: 0x0009CC77 File Offset: 0x0009AE77
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

		// Token: 0x17000591 RID: 1425
		// (get) Token: 0x06001AFA RID: 6906 RVA: 0x0009CCA5 File Offset: 0x0009AEA5
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
						}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
					}
					else
					{
						yield return new FloatMenuOption(this.designateAllLabel + " (" + "NoneLower".Translate() + ")", null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
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
						}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
					}
					else
					{
						yield return new FloatMenuOption("RemoveAllDesignations".Translate() + " (" + "NoneLower".Translate() + ")", null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
					}
				}
				yield break;
				yield break;
			}
		}

		// Token: 0x06001AFB RID: 6907 RVA: 0x0009CCB5 File Offset: 0x0009AEB5
		public Designator()
		{
			this.activateSound = SoundDefOf.Tick_Tiny;
			this.designateAllLabel = "DesignateAll".Translate();
		}

		// Token: 0x06001AFC RID: 6908 RVA: 0x0009CCE8 File Offset: 0x0009AEE8
		protected bool CheckCanInteract()
		{
			return !TutorSystem.TutorialMode || TutorSystem.AllowAction(this.TutorTagSelect);
		}

		// Token: 0x06001AFD RID: 6909 RVA: 0x0009CD06 File Offset: 0x0009AF06
		public override void ProcessInput(Event ev)
		{
			if (!this.CheckCanInteract())
			{
				return;
			}
			base.ProcessInput(ev);
			Find.DesignatorManager.Select(this);
		}

		// Token: 0x06001AFE RID: 6910 RVA: 0x0009CD23 File Offset: 0x0009AF23
		public virtual AcceptanceReport CanDesignateThing(Thing t)
		{
			return AcceptanceReport.WasRejected;
		}

		// Token: 0x06001AFF RID: 6911 RVA: 0x0002974C File Offset: 0x0002794C
		public virtual void DesignateThing(Thing t)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06001B00 RID: 6912
		public abstract AcceptanceReport CanDesignateCell(IntVec3 loc);

		// Token: 0x06001B01 RID: 6913 RVA: 0x0009CD2C File Offset: 0x0009AF2C
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

		// Token: 0x06001B02 RID: 6914 RVA: 0x0002974C File Offset: 0x0002794C
		public virtual void DesignateSingleCell(IntVec3 c)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06001B03 RID: 6915 RVA: 0x0001276E File Offset: 0x0001096E
		public virtual bool ShowWarningForCell(IntVec3 c)
		{
			return false;
		}

		// Token: 0x06001B04 RID: 6916 RVA: 0x0009CDD4 File Offset: 0x0009AFD4
		public void Finalize(bool somethingSucceeded)
		{
			if (somethingSucceeded)
			{
				this.FinalizeDesignationSucceeded();
				return;
			}
			this.FinalizeDesignationFailed();
		}

		// Token: 0x06001B05 RID: 6917 RVA: 0x0009CDE6 File Offset: 0x0009AFE6
		protected virtual void FinalizeDesignationSucceeded()
		{
			if (this.soundSucceeded != null)
			{
				this.soundSucceeded.PlayOneShotOnCamera(null);
			}
		}

		// Token: 0x06001B06 RID: 6918 RVA: 0x0009CDFC File Offset: 0x0009AFFC
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

		// Token: 0x06001B07 RID: 6919 RVA: 0x0009CE48 File Offset: 0x0009B048
		public virtual string LabelCapReverseDesignating(Thing t)
		{
			return this.LabelCap;
		}

		// Token: 0x06001B08 RID: 6920 RVA: 0x0009CE50 File Offset: 0x0009B050
		public virtual string DescReverseDesignating(Thing t)
		{
			return this.Desc;
		}

		// Token: 0x06001B09 RID: 6921 RVA: 0x0009CE58 File Offset: 0x0009B058
		public virtual Texture2D IconReverseDesignating(Thing t, out float angle, out Vector2 offset)
		{
			angle = this.iconAngle;
			offset = this.iconOffset;
			return this.icon;
		}

		// Token: 0x06001B0A RID: 6922 RVA: 0x000126F5 File Offset: 0x000108F5
		protected virtual bool RemoveAllDesignationsAffects(LocalTargetInfo target)
		{
			return true;
		}

		// Token: 0x06001B0B RID: 6923 RVA: 0x0009CE74 File Offset: 0x0009B074
		public virtual void DrawMouseAttachments()
		{
			if (this.useMouseIcon)
			{
				GenUI.DrawMouseAttachment(this.icon, "", this.iconAngle, this.iconOffset, null, false, default(Color));
			}
		}

		// Token: 0x06001B0C RID: 6924 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void DrawPanelReadout(ref float curY, float width)
		{
		}

		// Token: 0x06001B0D RID: 6925 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void DoExtraGuiControls(float leftX, float bottomY)
		{
		}

		// Token: 0x06001B0E RID: 6926 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void SelectedUpdate()
		{
		}

		// Token: 0x06001B0F RID: 6927 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void SelectedProcessInput(Event ev)
		{
		}

		// Token: 0x06001B10 RID: 6928 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void Rotate(RotationDirection rotDir)
		{
		}

		// Token: 0x06001B11 RID: 6929 RVA: 0x000126F5 File Offset: 0x000108F5
		public virtual bool CanRemainSelected()
		{
			return true;
		}

		// Token: 0x06001B12 RID: 6930 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void Selected()
		{
		}

		// Token: 0x06001B13 RID: 6931 RVA: 0x0009CEB8 File Offset: 0x0009B0B8
		public virtual void RenderHighlight(List<IntVec3> dragCells)
		{
			DesignatorUtility.RenderHighlightOverSelectableThings(this, dragCells);
		}

		// Token: 0x0400118B RID: 4491
		protected bool useMouseIcon;

		// Token: 0x0400118C RID: 4492
		public bool isOrder;

		// Token: 0x0400118D RID: 4493
		public SoundDef soundDragSustain;

		// Token: 0x0400118E RID: 4494
		public SoundDef soundDragChanged;

		// Token: 0x0400118F RID: 4495
		public SoundDef soundSucceeded;

		// Token: 0x04001190 RID: 4496
		protected SoundDef soundFailed = SoundDefOf.Designate_Failed;

		// Token: 0x04001191 RID: 4497
		protected bool hasDesignateAllFloatMenuOption;

		// Token: 0x04001192 RID: 4498
		protected string designateAllLabel;

		// Token: 0x04001193 RID: 4499
		private string cachedTutorTagSelect;

		// Token: 0x04001194 RID: 4500
		private string cachedTutorTagDesignate;

		// Token: 0x04001195 RID: 4501
		protected string cachedHighlightTag;
	}
}
