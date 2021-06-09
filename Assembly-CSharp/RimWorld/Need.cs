using System;
using System.Collections.Generic;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020014D9 RID: 5337
	[StaticConstructorOnStartup]
	public abstract class Need : IExposable
	{
		// Token: 0x1700118E RID: 4494
		// (get) Token: 0x06007304 RID: 29444 RVA: 0x0004D5BC File Offset: 0x0004B7BC
		public string LabelCap
		{
			get
			{
				return this.def.LabelCap;
			}
		}

		// Token: 0x1700118F RID: 4495
		// (get) Token: 0x06007305 RID: 29445 RVA: 0x0004D5CE File Offset: 0x0004B7CE
		public float CurInstantLevelPercentage
		{
			get
			{
				return this.CurInstantLevel / this.MaxLevel;
			}
		}

		// Token: 0x17001190 RID: 4496
		// (get) Token: 0x06007306 RID: 29446 RVA: 0x0000A2E4 File Offset: 0x000084E4
		public virtual int GUIChangeArrow
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x17001191 RID: 4497
		// (get) Token: 0x06007307 RID: 29447 RVA: 0x00014941 File Offset: 0x00012B41
		public virtual float CurInstantLevel
		{
			get
			{
				return -1f;
			}
		}

		// Token: 0x17001192 RID: 4498
		// (get) Token: 0x06007308 RID: 29448 RVA: 0x0000CE6C File Offset: 0x0000B06C
		public virtual float MaxLevel
		{
			get
			{
				return 1f;
			}
		}

		// Token: 0x17001193 RID: 4499
		// (get) Token: 0x06007309 RID: 29449 RVA: 0x0004D5DD File Offset: 0x0004B7DD
		// (set) Token: 0x0600730A RID: 29450 RVA: 0x0004D5E5 File Offset: 0x0004B7E5
		public virtual float CurLevel
		{
			get
			{
				return this.curLevelInt;
			}
			set
			{
				this.curLevelInt = Mathf.Clamp(value, 0f, this.MaxLevel);
			}
		}

		// Token: 0x17001194 RID: 4500
		// (get) Token: 0x0600730B RID: 29451 RVA: 0x0004D5FE File Offset: 0x0004B7FE
		// (set) Token: 0x0600730C RID: 29452 RVA: 0x0004D60D File Offset: 0x0004B80D
		public float CurLevelPercentage
		{
			get
			{
				return this.CurLevel / this.MaxLevel;
			}
			set
			{
				this.CurLevel = value * this.MaxLevel;
			}
		}

		// Token: 0x17001195 RID: 4501
		// (get) Token: 0x0600730D RID: 29453 RVA: 0x002322EC File Offset: 0x002304EC
		protected virtual bool IsFrozen
		{
			get
			{
				return this.pawn.Suspended || (this.def.freezeWhileSleeping && !this.pawn.Awake()) || (this.def.freezeInMentalState && this.pawn.InMentalState) || !this.IsPawnInteractableOrVisible;
			}
		}

		// Token: 0x17001196 RID: 4502
		// (get) Token: 0x0600730E RID: 29454 RVA: 0x0004D61D File Offset: 0x0004B81D
		private bool IsPawnInteractableOrVisible
		{
			get
			{
				return this.pawn.SpawnedOrAnyParentSpawned || this.pawn.IsCaravanMember() || PawnUtility.IsTravelingInTransportPodWorldObject(this.pawn);
			}
		}

		// Token: 0x17001197 RID: 4503
		// (get) Token: 0x0600730F RID: 29455 RVA: 0x0004D64D File Offset: 0x0004B84D
		public virtual bool ShowOnNeedList
		{
			get
			{
				return this.def.showOnNeedList;
			}
		}

		// Token: 0x06007310 RID: 29456 RVA: 0x00006B8B File Offset: 0x00004D8B
		public Need()
		{
		}

		// Token: 0x06007311 RID: 29457 RVA: 0x0004D65A File Offset: 0x0004B85A
		public Need(Pawn newPawn)
		{
			this.pawn = newPawn;
			this.SetInitialLevel();
		}

		// Token: 0x06007312 RID: 29458 RVA: 0x0004D66F File Offset: 0x0004B86F
		public virtual void ExposeData()
		{
			Scribe_Defs.Look<NeedDef>(ref this.def, "def");
			Scribe_Values.Look<float>(ref this.curLevelInt, "curLevel", 0f, false);
		}

		// Token: 0x06007313 RID: 29459
		public abstract void NeedInterval();

		// Token: 0x06007314 RID: 29460 RVA: 0x0023234C File Offset: 0x0023054C
		public virtual string GetTipString()
		{
			return string.Concat(new string[]
			{
				this.LabelCap,
				": ",
				this.CurLevelPercentage.ToStringPercent(),
				"\n",
				this.def.description
			});
		}

		// Token: 0x06007315 RID: 29461 RVA: 0x0004D697 File Offset: 0x0004B897
		public virtual void SetInitialLevel()
		{
			this.CurLevelPercentage = 0.5f;
		}

		// Token: 0x06007316 RID: 29462 RVA: 0x0023239C File Offset: 0x0023059C
		public virtual void DrawOnGUI(Rect rect, int maxThresholdMarkers = 2147483647, float customMargin = -1f, bool drawArrows = true, bool doTooltip = true)
		{
			if (rect.height > 70f)
			{
				float num = (rect.height - 70f) / 2f;
				rect.height = 70f;
				rect.y += num;
			}
			if (Mouse.IsOver(rect))
			{
				Widgets.DrawHighlight(rect);
			}
			if (doTooltip && Mouse.IsOver(rect))
			{
				TooltipHandler.TipRegion(rect, new TipSignal(() => this.GetTipString(), rect.GetHashCode()));
			}
			float num2 = 14f;
			float num3 = (customMargin >= 0f) ? customMargin : (num2 + 15f);
			if (rect.height < 50f)
			{
				num2 *= Mathf.InverseLerp(0f, 50f, rect.height);
			}
			Text.Font = ((rect.height > 55f) ? GameFont.Small : GameFont.Tiny);
			Text.Anchor = TextAnchor.LowerLeft;
			Widgets.Label(new Rect(rect.x + num3 + rect.width * 0.1f, rect.y, rect.width - num3 - rect.width * 0.1f, rect.height / 2f), this.LabelCap);
			Text.Anchor = TextAnchor.UpperLeft;
			Rect rect2 = new Rect(rect.x, rect.y + rect.height / 2f, rect.width, rect.height / 2f);
			rect2 = new Rect(rect2.x + num3, rect2.y, rect2.width - num3 * 2f, rect2.height - num2);
			Rect rect3 = rect2;
			float num4 = 1f;
			if (this.def.scaleBar && this.MaxLevel < 1f)
			{
				num4 = this.MaxLevel;
			}
			rect3.width *= num4;
			Rect barRect = Widgets.FillableBar(rect3, this.CurLevelPercentage);
			if (drawArrows)
			{
				Widgets.FillableBarChangeArrows(rect3, this.GUIChangeArrow);
			}
			if (this.threshPercents != null)
			{
				for (int i = 0; i < Mathf.Min(this.threshPercents.Count, maxThresholdMarkers); i++)
				{
					this.DrawBarThreshold(barRect, this.threshPercents[i] * num4);
				}
			}
			if (this.def.scaleBar)
			{
				int num5 = 1;
				while ((float)num5 < this.MaxLevel)
				{
					this.DrawBarDivision(barRect, (float)num5 / this.MaxLevel * num4);
					num5++;
				}
			}
			float curInstantLevelPercentage = this.CurInstantLevelPercentage;
			if (curInstantLevelPercentage >= 0f)
			{
				this.DrawBarInstantMarkerAt(rect2, curInstantLevelPercentage * num4);
			}
			if (!this.def.tutorHighlightTag.NullOrEmpty())
			{
				UIHighlighter.HighlightOpportunity(rect, this.def.tutorHighlightTag);
			}
			Text.Font = GameFont.Small;
		}

		// Token: 0x06007317 RID: 29463 RVA: 0x00232658 File Offset: 0x00230858
		protected void DrawBarInstantMarkerAt(Rect barRect, float pct)
		{
			if (pct > 1f)
			{
				Log.ErrorOnce(this.def + " drawing bar percent > 1 : " + pct, 6932178, false);
			}
			float num = 12f;
			if (barRect.width < 150f)
			{
				num /= 2f;
			}
			Vector2 vector = new Vector2(barRect.x + barRect.width * pct, barRect.y + barRect.height);
			GUI.DrawTexture(new Rect(vector.x - num / 2f, vector.y, num, num), Need.BarInstantMarkerTex);
		}

		// Token: 0x06007318 RID: 29464 RVA: 0x002326F8 File Offset: 0x002308F8
		private void DrawBarThreshold(Rect barRect, float threshPct)
		{
			float num = (float)((barRect.width > 60f) ? 2 : 1);
			Rect position = new Rect(barRect.x + barRect.width * threshPct - (num - 1f), barRect.y + barRect.height / 2f, num, barRect.height / 2f);
			Texture2D image;
			if (threshPct < this.CurLevelPercentage)
			{
				image = BaseContent.BlackTex;
				GUI.color = new Color(1f, 1f, 1f, 0.9f);
			}
			else
			{
				image = BaseContent.GreyTex;
				GUI.color = new Color(1f, 1f, 1f, 0.5f);
			}
			GUI.DrawTexture(position, image);
			GUI.color = Color.white;
		}

		// Token: 0x06007319 RID: 29465 RVA: 0x002327C0 File Offset: 0x002309C0
		private void DrawBarDivision(Rect barRect, float threshPct)
		{
			float num = 5f;
			Rect rect = new Rect(barRect.x + barRect.width * threshPct - (num - 1f), barRect.y, num, barRect.height);
			if (threshPct < this.CurLevelPercentage)
			{
				GUI.color = new Color(0f, 0f, 0f, 0.9f);
			}
			else
			{
				GUI.color = new Color(0.5f, 0.5f, 0.5f, 0.5f);
			}
			Rect position = rect;
			position.yMax = position.yMin + 4f;
			GUI.DrawTextureWithTexCoords(position, Need.NeedUnitDividerTex, new Rect(0f, 0.5f, 1f, 0.5f));
			Rect position2 = rect;
			position2.yMin = position2.yMax - 4f;
			GUI.DrawTextureWithTexCoords(position2, Need.NeedUnitDividerTex, new Rect(0f, 0f, 1f, 0.5f));
			Rect position3 = rect;
			position3.yMin = position.yMax;
			position3.yMax = position2.yMin;
			if (position3.height > 0f)
			{
				GUI.DrawTextureWithTexCoords(position3, Need.NeedUnitDividerTex, new Rect(0f, 0.4f, 1f, 0.2f));
			}
			GUI.color = Color.white;
		}

		// Token: 0x04004BBB RID: 19387
		public NeedDef def;

		// Token: 0x04004BBC RID: 19388
		protected Pawn pawn;

		// Token: 0x04004BBD RID: 19389
		protected float curLevelInt;

		// Token: 0x04004BBE RID: 19390
		protected List<float> threshPercents;

		// Token: 0x04004BBF RID: 19391
		public const float MaxDrawHeight = 70f;

		// Token: 0x04004BC0 RID: 19392
		private static readonly Texture2D BarInstantMarkerTex = ContentFinder<Texture2D>.Get("UI/Misc/BarInstantMarker", true);

		// Token: 0x04004BC1 RID: 19393
		private static readonly Texture2D NeedUnitDividerTex = ContentFinder<Texture2D>.Get("UI/Misc/NeedUnitDivider", true);

		// Token: 0x04004BC2 RID: 19394
		private const float BarInstantMarkerSize = 12f;
	}
}
