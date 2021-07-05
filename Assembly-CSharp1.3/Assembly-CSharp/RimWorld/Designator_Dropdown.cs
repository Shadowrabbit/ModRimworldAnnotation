using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020012AB RID: 4779
	[StaticConstructorOnStartup]
	public class Designator_Dropdown : Designator
	{
		// Token: 0x170013EA RID: 5098
		// (get) Token: 0x0600721C RID: 29212 RVA: 0x00262191 File Offset: 0x00260391
		public override string Label
		{
			get
			{
				return this.activeDesignator.Label + (this.activeDesignatorSet ? "" : "...");
			}
		}

		// Token: 0x170013EB RID: 5099
		// (get) Token: 0x0600721D RID: 29213 RVA: 0x002621B7 File Offset: 0x002603B7
		public override string Desc
		{
			get
			{
				return this.activeDesignator.Desc;
			}
		}

		// Token: 0x170013EC RID: 5100
		// (get) Token: 0x0600721E RID: 29214 RVA: 0x002621C4 File Offset: 0x002603C4
		public override Color IconDrawColor
		{
			get
			{
				return this.activeDesignator.IconDrawColor;
			}
		}

		// Token: 0x170013ED RID: 5101
		// (get) Token: 0x0600721F RID: 29215 RVA: 0x002621D4 File Offset: 0x002603D4
		public override bool Visible
		{
			get
			{
				for (int i = 0; i < this.elements.Count; i++)
				{
					if (this.elements[i].Visible)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x170013EE RID: 5102
		// (get) Token: 0x06007220 RID: 29216 RVA: 0x0026220D File Offset: 0x0026040D
		public List<Designator> Elements
		{
			get
			{
				return this.elements;
			}
		}

		// Token: 0x170013EF RID: 5103
		// (get) Token: 0x06007221 RID: 29217 RVA: 0x00262215 File Offset: 0x00260415
		public override float PanelReadoutTitleExtraRightMargin
		{
			get
			{
				return this.activeDesignator.PanelReadoutTitleExtraRightMargin;
			}
		}

		// Token: 0x06007223 RID: 29219 RVA: 0x00262235 File Offset: 0x00260435
		public override GizmoResult GizmoOnGUI(Vector2 topLeft, float maxWidth, GizmoRenderParms parms)
		{
			GizmoResult result = base.GizmoOnGUI(topLeft, maxWidth, parms);
			Designator_Dropdown.DrawExtraOptionsIcon(topLeft, this.GetWidth(maxWidth));
			return result;
		}

		// Token: 0x06007224 RID: 29220 RVA: 0x0026224D File Offset: 0x0026044D
		public static void DrawExtraOptionsIcon(Vector2 topLeft, float width)
		{
			GUI.DrawTexture(new Rect(topLeft.x + width - 16f - 1f, topLeft.y + 1f, 16f, 16f), Designator_Dropdown.PlusTex);
		}

		// Token: 0x06007225 RID: 29221 RVA: 0x00262288 File Offset: 0x00260488
		public void Add(Designator des)
		{
			this.elements.Add(des);
			if (this.activeDesignator == null)
			{
				this.SetActiveDesignator(des, false);
			}
		}

		// Token: 0x06007226 RID: 29222 RVA: 0x002622A8 File Offset: 0x002604A8
		public void SetActiveDesignator(Designator des, bool explicitySet = true)
		{
			this.activeDesignator = des;
			this.icon = des.icon;
			this.iconDrawScale = des.iconDrawScale;
			this.iconProportions = des.iconProportions;
			this.iconTexCoords = des.iconTexCoords;
			this.iconAngle = des.iconAngle;
			this.iconOffset = des.iconOffset;
			if (explicitySet)
			{
				this.activeDesignatorSet = true;
			}
		}

		// Token: 0x06007227 RID: 29223 RVA: 0x0026230E File Offset: 0x0026050E
		public override void DrawMouseAttachments()
		{
			this.activeDesignator.DrawMouseAttachments();
		}

		// Token: 0x06007228 RID: 29224 RVA: 0x0026231C File Offset: 0x0026051C
		public override void ProcessInput(Event ev)
		{
			List<FloatMenuOption> list = new List<FloatMenuOption>();
			for (int i = 0; i < this.elements.Count; i++)
			{
				Designator des = this.elements[i];
				if (des.Visible)
				{
					Action action = delegate()
					{
						this.<>n__0(ev);
						Find.DesignatorManager.Select(des);
						this.SetActiveDesignator(des, true);
					};
					ThingDef designatorCost = this.GetDesignatorCost(des);
					if (designatorCost != null)
					{
						list.Add(new FloatMenuOption(des.LabelCap, action, designatorCost, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
					}
					else
					{
						list.Add(new FloatMenuOption(des.LabelCap, action, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
					}
				}
			}
			FloatMenu floatMenu = new FloatMenu(list);
			floatMenu.vanishIfMouseDistant = true;
			floatMenu.onCloseCallback = delegate()
			{
				this.activeDesignatorSet = true;
			};
			Find.WindowStack.Add(floatMenu);
			Find.DesignatorManager.Select(this.activeDesignator);
		}

		// Token: 0x06007229 RID: 29225 RVA: 0x0026243A File Offset: 0x0026063A
		public override AcceptanceReport CanDesignateCell(IntVec3 loc)
		{
			return this.activeDesignator.CanDesignateCell(loc);
		}

		// Token: 0x0600722A RID: 29226 RVA: 0x00262448 File Offset: 0x00260648
		public override void SelectedUpdate()
		{
			this.activeDesignator.SelectedUpdate();
		}

		// Token: 0x0600722B RID: 29227 RVA: 0x00262455 File Offset: 0x00260655
		public override void DrawPanelReadout(ref float curY, float width)
		{
			this.activeDesignator.DrawPanelReadout(ref curY, width);
		}

		// Token: 0x0600722C RID: 29228 RVA: 0x00262464 File Offset: 0x00260664
		private ThingDef GetDesignatorCost(Designator des)
		{
			Designator_Place designator_Place = des as Designator_Place;
			if (designator_Place != null)
			{
				BuildableDef placingDef = designator_Place.PlacingDef;
				if (placingDef.CostList != null && placingDef.CostList.Count > 0)
				{
					return placingDef.CostList.MaxBy((ThingDefCountClass c) => c.thingDef.BaseMarketValue * (float)c.count).thingDef;
				}
			}
			return null;
		}

		// Token: 0x04003EBB RID: 16059
		private List<Designator> elements = new List<Designator>();

		// Token: 0x04003EBC RID: 16060
		private Designator activeDesignator;

		// Token: 0x04003EBD RID: 16061
		private bool activeDesignatorSet;

		// Token: 0x04003EBE RID: 16062
		public static readonly Texture2D PlusTex = ContentFinder<Texture2D>.Get("UI/Widgets/PlusOptions", true);

		// Token: 0x04003EBF RID: 16063
		private const float ButSize = 16f;

		// Token: 0x04003EC0 RID: 16064
		private const float ButPadding = 1f;
	}
}
