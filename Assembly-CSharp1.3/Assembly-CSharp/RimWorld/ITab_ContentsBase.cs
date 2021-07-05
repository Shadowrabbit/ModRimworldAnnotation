using System;
using System.Collections.Generic;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200133F RID: 4927
	public abstract class ITab_ContentsBase : ITab
	{
		// Token: 0x170014ED RID: 5357
		// (get) Token: 0x06007742 RID: 30530
		public abstract IList<Thing> container { get; }

		// Token: 0x170014EE RID: 5358
		// (get) Token: 0x06007743 RID: 30531 RVA: 0x0029DE78 File Offset: 0x0029C078
		public override bool IsVisible
		{
			get
			{
				return base.SelThing.Faction == Faction.OfPlayer;
			}
		}

		// Token: 0x170014EF RID: 5359
		// (get) Token: 0x06007744 RID: 30532 RVA: 0x0029DE8C File Offset: 0x0029C08C
		public virtual IntVec3 DropOffset
		{
			get
			{
				return IntVec3.Zero;
			}
		}

		// Token: 0x170014F0 RID: 5360
		// (get) Token: 0x06007745 RID: 30533 RVA: 0x000126F5 File Offset: 0x000108F5
		public virtual bool UseDiscardMessage
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06007746 RID: 30534 RVA: 0x0029DE93 File Offset: 0x0029C093
		public ITab_ContentsBase()
		{
			this.size = new Vector2(460f, 450f);
		}

		// Token: 0x06007747 RID: 30535 RVA: 0x0029DEC4 File Offset: 0x0029C0C4
		protected override void FillTab()
		{
			this.thingsToSelect.Clear();
			Rect outRect = new Rect(default(Vector2), this.size).ContractedBy(10f);
			outRect.yMin += 20f;
			Rect rect = new Rect(0f, 0f, outRect.width - 16f, Mathf.Max(this.lastDrawnHeight, outRect.height));
			Text.Font = GameFont.Small;
			Widgets.BeginScrollView(outRect, ref this.scrollPosition, rect, true);
			float num = 0f;
			this.DoItemsLists(rect, ref num);
			this.lastDrawnHeight = num;
			Widgets.EndScrollView();
			if (this.thingsToSelect.Any<Thing>())
			{
				ITab_Pawn_FormingCaravan.SelectNow(this.thingsToSelect);
				this.thingsToSelect.Clear();
			}
		}

		// Token: 0x06007748 RID: 30536 RVA: 0x0029DF90 File Offset: 0x0029C190
		protected virtual void DoItemsLists(Rect inRect, ref float curY)
		{
			GUI.BeginGroup(inRect);
			Widgets.ListSeparator(ref curY, inRect.width, this.containedItemsKey.Translate());
			IList<Thing> container = this.container;
			bool flag = false;
			for (int i = 0; i < container.Count; i++)
			{
				Thing t = container[i];
				if (t != null)
				{
					flag = true;
					ITab_ContentsBase.tmpSingleThing.Clear();
					ITab_ContentsBase.tmpSingleThing.Add(t);
					this.DoThingRow(t.def, t.stackCount, ITab_ContentsBase.tmpSingleThing, inRect.width, ref curY, delegate(int x)
					{
						this.OnDropThing(t, x);
					});
					ITab_ContentsBase.tmpSingleThing.Clear();
				}
			}
			if (!flag)
			{
				Widgets.NoneLabel(ref curY, inRect.width, null);
			}
			GUI.EndGroup();
		}

		// Token: 0x06007749 RID: 30537 RVA: 0x0029E074 File Offset: 0x0029C274
		protected virtual void OnDropThing(Thing t, int count)
		{
			Thing thing;
			GenDrop.TryDropSpawn(t.SplitOff(count), base.SelThing.Position + this.DropOffset, base.SelThing.Map, ThingPlaceMode.Near, out thing, null, null, true);
		}

		// Token: 0x0600774A RID: 30538 RVA: 0x0029E0B8 File Offset: 0x0029C2B8
		protected void DoThingRow(ThingDef thingDef, int count, List<Thing> things, float width, ref float curY, Action<int> discardAction)
		{
			Rect rect = new Rect(0f, curY, width, 28f);
			if (this.canRemoveThings)
			{
				if (count != 1 && Widgets.ButtonImage(new Rect(rect.x + rect.width - 24f, rect.y + (rect.height - 24f) / 2f, 24f, 24f), CaravanThingsTabUtility.AbandonSpecificCountButtonTex, true))
				{
					Find.WindowStack.Add(new Dialog_Slider("RemoveSliderText".Translate(thingDef.label), 1, count, discardAction, int.MinValue, 1f));
				}
				rect.width -= 24f;
				if (Widgets.ButtonImage(new Rect(rect.x + rect.width - 24f, rect.y + (rect.height - 24f) / 2f, 24f, 24f), CaravanThingsTabUtility.AbandonButtonTex, true))
				{
					if (this.UseDiscardMessage)
					{
						string value = thingDef.label;
						if (things.Count == 1 && things[0] is Pawn)
						{
							value = ((Pawn)things[0]).LabelShortCap;
						}
						Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation("ConfirmRemoveItemDialog".Translate(value), delegate
						{
							discardAction(count);
						}, false, null));
					}
					else
					{
						discardAction(count);
					}
				}
				rect.width -= 24f;
			}
			if (things.Count == 1)
			{
				Widgets.InfoCardButton(rect.width - 24f, curY, things[0]);
			}
			else
			{
				Widgets.InfoCardButton(rect.width - 24f, curY, thingDef);
			}
			rect.width -= 24f;
			if (Mouse.IsOver(rect))
			{
				GUI.color = ITab_ContentsBase.ThingHighlightColor;
				GUI.DrawTexture(rect, TexUI.HighlightTex);
			}
			if (thingDef.DrawMatSingle != null && thingDef.DrawMatSingle.mainTexture != null)
			{
				Rect rect2 = new Rect(4f, curY, 28f, 28f);
				if (things.Count == 1)
				{
					Widgets.ThingIcon(rect2, things[0], 1f, null);
				}
				else
				{
					Widgets.ThingIcon(rect2, thingDef, null, null, 1f, null);
				}
			}
			Text.Anchor = TextAnchor.MiddleLeft;
			GUI.color = ITab_ContentsBase.ThingLabelColor;
			Rect rect3 = new Rect(36f, curY, rect.width - 36f, rect.height);
			string str;
			if (things.Count == 1 && count == things[0].stackCount)
			{
				str = things[0].LabelCap;
			}
			else
			{
				str = GenLabel.ThingLabel(thingDef, null, count).CapitalizeFirst();
			}
			Text.WordWrap = false;
			Widgets.Label(rect3, str.Truncate(rect3.width, null));
			Text.WordWrap = true;
			Text.Anchor = TextAnchor.UpperLeft;
			TooltipHandler.TipRegion(rect, str);
			if (Widgets.ButtonInvisible(rect, true))
			{
				this.SelectLater(things);
			}
			if (Mouse.IsOver(rect))
			{
				for (int i = 0; i < things.Count; i++)
				{
					TargetHighlighter.Highlight(things[i], true, true, false);
				}
			}
			curY += 28f;
		}

		// Token: 0x0600774B RID: 30539 RVA: 0x0029E45C File Offset: 0x0029C65C
		private void SelectLater(List<Thing> things)
		{
			this.thingsToSelect.Clear();
			this.thingsToSelect.AddRange(things);
		}

		// Token: 0x04004247 RID: 16967
		private Vector2 scrollPosition;

		// Token: 0x04004248 RID: 16968
		private float lastDrawnHeight;

		// Token: 0x04004249 RID: 16969
		private List<Thing> thingsToSelect = new List<Thing>();

		// Token: 0x0400424A RID: 16970
		public bool canRemoveThings = true;

		// Token: 0x0400424B RID: 16971
		protected static List<Thing> tmpSingleThing = new List<Thing>();

		// Token: 0x0400424C RID: 16972
		protected const float TopPadding = 20f;

		// Token: 0x0400424D RID: 16973
		protected const float SpaceBetweenItemsLists = 10f;

		// Token: 0x0400424E RID: 16974
		protected const float ThingRowHeight = 28f;

		// Token: 0x0400424F RID: 16975
		protected const float ThingIconSize = 28f;

		// Token: 0x04004250 RID: 16976
		protected const float ThingLeftX = 36f;

		// Token: 0x04004251 RID: 16977
		protected static readonly Color ThingLabelColor = ITab_Pawn_Gear.ThingLabelColor;

		// Token: 0x04004252 RID: 16978
		protected static readonly Color ThingHighlightColor = ITab_Pawn_Gear.HighlightColor;

		// Token: 0x04004253 RID: 16979
		public string containedItemsKey;
	}
}
