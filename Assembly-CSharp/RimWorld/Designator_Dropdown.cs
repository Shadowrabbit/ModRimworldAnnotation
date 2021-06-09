using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200199C RID: 6556
	[StaticConstructorOnStartup]
	public class Designator_Dropdown : Designator
	{
		// Token: 0x170016F0 RID: 5872
		// (get) Token: 0x060090E7 RID: 37095 RVA: 0x00061300 File Offset: 0x0005F500
		public override string Label
		{
			get
			{
				return this.activeDesignator.Label + (this.activeDesignatorSet ? "" : "...");
			}
		}

		// Token: 0x170016F1 RID: 5873
		// (get) Token: 0x060090E8 RID: 37096 RVA: 0x00061326 File Offset: 0x0005F526
		public override string Desc
		{
			get
			{
				return this.activeDesignator.Desc;
			}
		}

		// Token: 0x170016F2 RID: 5874
		// (get) Token: 0x060090E9 RID: 37097 RVA: 0x00061333 File Offset: 0x0005F533
		public override Color IconDrawColor
		{
			get
			{
				return this.activeDesignator.IconDrawColor;
			}
		}

		// Token: 0x170016F3 RID: 5875
		// (get) Token: 0x060090EA RID: 37098 RVA: 0x0029B8D8 File Offset: 0x00299AD8
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

		// Token: 0x170016F4 RID: 5876
		// (get) Token: 0x060090EB RID: 37099 RVA: 0x00061340 File Offset: 0x0005F540
		public List<Designator> Elements
		{
			get
			{
				return this.elements;
			}
		}

		// Token: 0x170016F5 RID: 5877
		// (get) Token: 0x060090EC RID: 37100 RVA: 0x00061348 File Offset: 0x0005F548
		public override float PanelReadoutTitleExtraRightMargin
		{
			get
			{
				return this.activeDesignator.PanelReadoutTitleExtraRightMargin;
			}
		}

		// Token: 0x060090EE RID: 37102 RVA: 0x00061368 File Offset: 0x0005F568
		public override GizmoResult GizmoOnGUI(Vector2 topLeft, float maxWidth)
		{
			GizmoResult result = base.GizmoOnGUI(topLeft, maxWidth);
			Designator_Dropdown.DrawExtraOptionsIcon(topLeft, this.GetWidth(maxWidth));
			return result;
		}

		// Token: 0x060090EF RID: 37103 RVA: 0x0006137F File Offset: 0x0005F57F
		public static void DrawExtraOptionsIcon(Vector2 topLeft, float width)
		{
			GUI.DrawTexture(new Rect(topLeft.x + width - 16f - 1f, topLeft.y + 1f, 16f, 16f), Designator_Dropdown.PlusTex);
		}

		// Token: 0x060090F0 RID: 37104 RVA: 0x000613BA File Offset: 0x0005F5BA
		public void Add(Designator des)
		{
			this.elements.Add(des);
			if (this.activeDesignator == null)
			{
				this.SetActiveDesignator(des, false);
			}
		}

		// Token: 0x060090F1 RID: 37105 RVA: 0x0029B914 File Offset: 0x00299B14
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

		// Token: 0x060090F2 RID: 37106 RVA: 0x000613D8 File Offset: 0x0005F5D8
		public override void DrawMouseAttachments()
		{
			this.activeDesignator.DrawMouseAttachments();
		}

		// Token: 0x060090F3 RID: 37107 RVA: 0x0029B97C File Offset: 0x00299B7C
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
						list.Add(new FloatMenuOption(des.LabelCap, action, designatorCost, MenuOptionPriority.Default, null, null, 0f, null, null));
					}
					else
					{
						list.Add(new FloatMenuOption(des.LabelCap, action, MenuOptionPriority.Default, null, null, 0f, null, null));
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

		// Token: 0x060090F4 RID: 37108 RVA: 0x000613E5 File Offset: 0x0005F5E5
		public override AcceptanceReport CanDesignateCell(IntVec3 loc)
		{
			return this.activeDesignator.CanDesignateCell(loc);
		}

		// Token: 0x060090F5 RID: 37109 RVA: 0x000613F3 File Offset: 0x0005F5F3
		public override void SelectedUpdate()
		{
			this.activeDesignator.SelectedUpdate();
		}

		// Token: 0x060090F6 RID: 37110 RVA: 0x00061400 File Offset: 0x0005F600
		public override void DrawPanelReadout(ref float curY, float width)
		{
			this.activeDesignator.DrawPanelReadout(ref curY, width);
		}

		// Token: 0x060090F7 RID: 37111 RVA: 0x0029BA98 File Offset: 0x00299C98
		private ThingDef GetDesignatorCost(Designator des)
		{
			Designator_Place designator_Place = des as Designator_Place;
			if (designator_Place != null)
			{
				BuildableDef placingDef = designator_Place.PlacingDef;
				if (placingDef.costList.Count > 0)
				{
					return placingDef.costList.MaxBy((ThingDefCountClass c) => c.thingDef.BaseMarketValue * (float)c.count).thingDef;
				}
			}
			return null;
		}

		// Token: 0x04005C0B RID: 23563
		private List<Designator> elements = new List<Designator>();

		// Token: 0x04005C0C RID: 23564
		private Designator activeDesignator;

		// Token: 0x04005C0D RID: 23565
		private bool activeDesignatorSet;

		// Token: 0x04005C0E RID: 23566
		public static readonly Texture2D PlusTex = ContentFinder<Texture2D>.Get("UI/Widgets/PlusOptions", true);

		// Token: 0x04005C0F RID: 23567
		private const float ButSize = 16f;

		// Token: 0x04005C10 RID: 23568
		private const float ButPadding = 1f;
	}
}
