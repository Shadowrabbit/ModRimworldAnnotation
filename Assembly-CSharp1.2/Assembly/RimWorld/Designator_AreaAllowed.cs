using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001989 RID: 6537
	public abstract class Designator_AreaAllowed : Designator_Area
	{
		// Token: 0x170016DF RID: 5855
		// (get) Token: 0x06009090 RID: 37008 RVA: 0x0001B6B4 File Offset: 0x000198B4
		public override int DraggableDimensions
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x170016E0 RID: 5856
		// (get) Token: 0x06009091 RID: 37009 RVA: 0x0000A2A7 File Offset: 0x000084A7
		public override bool DragDrawMeasurements
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170016E1 RID: 5857
		// (get) Token: 0x06009092 RID: 37010 RVA: 0x00060FCA File Offset: 0x0005F1CA
		public static Area SelectedArea
		{
			get
			{
				return Designator_AreaAllowed.selectedArea;
			}
		}

		// Token: 0x06009093 RID: 37011 RVA: 0x00060FD1 File Offset: 0x0005F1D1
		public Designator_AreaAllowed(DesignateMode mode)
		{
			this.soundDragSustain = SoundDefOf.Designate_DragStandard;
			this.soundDragChanged = SoundDefOf.Designate_DragStandard_Changed;
			this.useMouseIcon = true;
		}

		// Token: 0x06009094 RID: 37012 RVA: 0x00060FF6 File Offset: 0x0005F1F6
		public static void ClearSelectedArea()
		{
			Designator_AreaAllowed.selectedArea = null;
		}

		// Token: 0x06009095 RID: 37013 RVA: 0x00060FFE File Offset: 0x0005F1FE
		public override void SelectedUpdate()
		{
			GenUI.RenderMouseoverBracket();
			if (Designator_AreaAllowed.selectedArea != null && Find.WindowStack.FloatMenu == null)
			{
				Designator_AreaAllowed.selectedArea.MarkForDraw();
			}
		}

		// Token: 0x06009096 RID: 37014 RVA: 0x0029A75C File Offset: 0x0029895C
		public override void ProcessInput(Event ev)
		{
			if (!base.CheckCanInteract())
			{
				return;
			}
			if (Designator_AreaAllowed.selectedArea != null)
			{
				base.ProcessInput(ev);
			}
			AreaUtility.MakeAllowedAreaListFloatMenu(delegate(Area a)
			{
				Designator_AreaAllowed.selectedArea = a;
				this.<>n__0(ev);
			}, false, true, base.Map);
		}

		// Token: 0x06009097 RID: 37015 RVA: 0x00061022 File Offset: 0x0005F222
		protected override void FinalizeDesignationSucceeded()
		{
			base.FinalizeDesignationSucceeded();
			PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.AllowedAreas, KnowledgeAmount.SpecificInteraction);
		}

		// Token: 0x04005BFD RID: 23549
		private static Area selectedArea;
	}
}
