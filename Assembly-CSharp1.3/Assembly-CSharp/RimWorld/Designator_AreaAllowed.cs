using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200129B RID: 4763
	public abstract class Designator_AreaAllowed : Designator_Area
	{
		// Token: 0x170013D9 RID: 5081
		// (get) Token: 0x060071CD RID: 29133 RVA: 0x0009007E File Offset: 0x0008E27E
		public override int DraggableDimensions
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x170013DA RID: 5082
		// (get) Token: 0x060071CE RID: 29134 RVA: 0x000126F5 File Offset: 0x000108F5
		public override bool DragDrawMeasurements
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170013DB RID: 5083
		// (get) Token: 0x060071CF RID: 29135 RVA: 0x00260D2A File Offset: 0x0025EF2A
		public static Area SelectedArea
		{
			get
			{
				return Designator_AreaAllowed.selectedArea;
			}
		}

		// Token: 0x060071D0 RID: 29136 RVA: 0x00260D31 File Offset: 0x0025EF31
		public Designator_AreaAllowed(DesignateMode mode)
		{
			this.soundDragSustain = SoundDefOf.Designate_DragStandard;
			this.soundDragChanged = SoundDefOf.Designate_DragStandard_Changed;
			this.useMouseIcon = true;
		}

		// Token: 0x060071D1 RID: 29137 RVA: 0x00260D56 File Offset: 0x0025EF56
		public static void ClearSelectedArea()
		{
			Designator_AreaAllowed.selectedArea = null;
		}

		// Token: 0x060071D2 RID: 29138 RVA: 0x00260D5E File Offset: 0x0025EF5E
		public override void SelectedUpdate()
		{
			GenUI.RenderMouseoverBracket();
			if (Designator_AreaAllowed.selectedArea != null && Find.WindowStack.FloatMenu == null)
			{
				Designator_AreaAllowed.selectedArea.MarkForDraw();
			}
		}

		// Token: 0x060071D3 RID: 29139 RVA: 0x00260D84 File Offset: 0x0025EF84
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

		// Token: 0x060071D4 RID: 29140 RVA: 0x00260DDA File Offset: 0x0025EFDA
		protected override void FinalizeDesignationSucceeded()
		{
			base.FinalizeDesignationSucceeded();
			PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.AllowedAreas, KnowledgeAmount.SpecificInteraction);
		}

		// Token: 0x04003EB3 RID: 16051
		private static Area selectedArea;
	}
}
