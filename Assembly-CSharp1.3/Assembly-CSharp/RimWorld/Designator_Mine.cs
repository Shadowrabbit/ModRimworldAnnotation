using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020012B1 RID: 4785
	public class Designator_Mine : Designator
	{
		// Token: 0x170013F6 RID: 5110
		// (get) Token: 0x06007252 RID: 29266 RVA: 0x0009007E File Offset: 0x0008E27E
		public override int DraggableDimensions
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x170013F7 RID: 5111
		// (get) Token: 0x06007253 RID: 29267 RVA: 0x000126F5 File Offset: 0x000108F5
		public override bool DragDrawMeasurements
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170013F8 RID: 5112
		// (get) Token: 0x06007254 RID: 29268 RVA: 0x00262DA5 File Offset: 0x00260FA5
		protected override DesignationDef Designation
		{
			get
			{
				return DesignationDefOf.Mine;
			}
		}

		// Token: 0x06007255 RID: 29269 RVA: 0x00262DAC File Offset: 0x00260FAC
		public Designator_Mine()
		{
			this.defaultLabel = "DesignatorMine".Translate();
			this.icon = ContentFinder<Texture2D>.Get("UI/Designators/Mine", true);
			this.defaultDesc = "DesignatorMineDesc".Translate();
			this.useMouseIcon = true;
			this.soundDragSustain = SoundDefOf.Designate_DragStandard;
			this.soundDragChanged = SoundDefOf.Designate_DragStandard_Changed;
			this.soundSucceeded = SoundDefOf.Designate_Mine;
			this.hotKey = KeyBindingDefOf.Misc10;
			this.tutorTag = "Mine";
		}

		// Token: 0x06007256 RID: 29270 RVA: 0x00262E38 File Offset: 0x00261038
		public override AcceptanceReport CanDesignateCell(IntVec3 c)
		{
			if (!c.InBounds(base.Map))
			{
				return false;
			}
			if (base.Map.designationManager.DesignationAt(c, this.Designation) != null)
			{
				return AcceptanceReport.WasRejected;
			}
			if (c.Fogged(base.Map))
			{
				return true;
			}
			Mineable firstMineable = c.GetFirstMineable(base.Map);
			if (firstMineable == null)
			{
				return "MessageMustDesignateMineable".Translate();
			}
			AcceptanceReport result = this.CanDesignateThing(firstMineable);
			if (!result.Accepted)
			{
				return result;
			}
			return AcceptanceReport.WasAccepted;
		}

		// Token: 0x06007257 RID: 29271 RVA: 0x00262EC6 File Offset: 0x002610C6
		public override AcceptanceReport CanDesignateThing(Thing t)
		{
			if (!t.def.mineable)
			{
				return false;
			}
			if (base.Map.designationManager.DesignationAt(t.Position, this.Designation) != null)
			{
				return AcceptanceReport.WasRejected;
			}
			return true;
		}

		// Token: 0x06007258 RID: 29272 RVA: 0x00262F08 File Offset: 0x00261108
		public override void DesignateSingleCell(IntVec3 loc)
		{
			base.Map.designationManager.AddDesignation(new Designation(loc, this.Designation));
			base.Map.designationManager.TryRemoveDesignation(loc, DesignationDefOf.SmoothWall);
			Designator_Mine.PossiblyWarnPlayerOnDesignatingMining();
			if (DebugSettings.godMode)
			{
				Mineable firstMineable = loc.GetFirstMineable(base.Map);
				if (firstMineable != null)
				{
					firstMineable.DestroyMined(null);
				}
			}
		}

		// Token: 0x06007259 RID: 29273 RVA: 0x00262F6F File Offset: 0x0026116F
		public override void DesignateThing(Thing t)
		{
			this.DesignateSingleCell(t.Position);
		}

		// Token: 0x0600725A RID: 29274 RVA: 0x00262F7D File Offset: 0x0026117D
		protected override void FinalizeDesignationSucceeded()
		{
			base.FinalizeDesignationSucceeded();
			PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.Mining, KnowledgeAmount.SpecificInteraction);
		}

		// Token: 0x0600725B RID: 29275 RVA: 0x00261B2D File Offset: 0x0025FD2D
		public override void SelectedUpdate()
		{
			GenUI.RenderMouseoverBracket();
		}

		// Token: 0x0600725C RID: 29276 RVA: 0x00260D19 File Offset: 0x0025EF19
		public override void RenderHighlight(List<IntVec3> dragCells)
		{
			DesignatorUtility.RenderHighlightOverSelectableCells(this, dragCells);
		}

		// Token: 0x0600725D RID: 29277 RVA: 0x00262F90 File Offset: 0x00261190
		private static void PossiblyWarnPlayerOnDesignatingMining()
		{
			if (!ModsConfig.IdeologyActive)
			{
				return;
			}
			Designator_Mine.tmpIdeoMemberNames.Clear();
			foreach (Ideo ideo in Faction.OfPlayer.ideos.AllIdeos)
			{
				if (ideo.WarnPlayerOnDesignateMine)
				{
					Designator_Mine.tmpIdeoMemberNames.Add(Find.ActiveLanguageWorker.Pluralize(ideo.memberName, -1));
				}
			}
			if (Designator_Mine.tmpIdeoMemberNames.Any<string>())
			{
				Messages.Message("MessageWarningPlayerDesignatedMining".Translate(Designator_Mine.tmpIdeoMemberNames.ToCommaList(true, false)), MessageTypeDefOf.CautionInput, false);
			}
			Designator_Mine.tmpIdeoMemberNames.Clear();
		}

		// Token: 0x04003EC2 RID: 16066
		private static List<string> tmpIdeoMemberNames = new List<string>();
	}
}
