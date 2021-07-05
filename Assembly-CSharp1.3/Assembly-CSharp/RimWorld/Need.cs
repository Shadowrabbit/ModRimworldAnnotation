using System;
using System.Collections.Generic;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E3C RID: 3644
	[StaticConstructorOnStartup]
	public abstract class Need : IExposable
	{
		// Token: 0x17000E5C RID: 3676
		// (get) Token: 0x06005464 RID: 21604 RVA: 0x001C9B91 File Offset: 0x001C7D91
		public string LabelCap
		{
			get
			{
				return this.def.LabelCap;
			}
		}

		// Token: 0x17000E5D RID: 3677
		// (get) Token: 0x06005465 RID: 21605 RVA: 0x001C9BA3 File Offset: 0x001C7DA3
		public float CurInstantLevelPercentage
		{
			get
			{
				return this.CurInstantLevel / this.MaxLevel;
			}
		}

		// Token: 0x17000E5E RID: 3678
		// (get) Token: 0x06005466 RID: 21606 RVA: 0x0001276E File Offset: 0x0001096E
		public virtual int GUIChangeArrow
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x17000E5F RID: 3679
		// (get) Token: 0x06005467 RID: 21607 RVA: 0x00059779 File Offset: 0x00057979
		public virtual float CurInstantLevel
		{
			get
			{
				return -1f;
			}
		}

		// Token: 0x17000E60 RID: 3680
		// (get) Token: 0x06005468 RID: 21608 RVA: 0x0001F15E File Offset: 0x0001D35E
		public virtual float MaxLevel
		{
			get
			{
				return 1f;
			}
		}

		// Token: 0x17000E61 RID: 3681
		// (get) Token: 0x06005469 RID: 21609 RVA: 0x001C9BB2 File Offset: 0x001C7DB2
		// (set) Token: 0x0600546A RID: 21610 RVA: 0x001C9BBA File Offset: 0x001C7DBA
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

		// Token: 0x17000E62 RID: 3682
		// (get) Token: 0x0600546B RID: 21611 RVA: 0x001C9BD3 File Offset: 0x001C7DD3
		// (set) Token: 0x0600546C RID: 21612 RVA: 0x001C9BE2 File Offset: 0x001C7DE2
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

		// Token: 0x17000E63 RID: 3683
		// (get) Token: 0x0600546D RID: 21613 RVA: 0x001C9BF4 File Offset: 0x001C7DF4
		protected virtual bool IsFrozen
		{
			get
			{
				return this.pawn.Suspended || (this.def.freezeWhileSleeping && !this.pawn.Awake()) || (this.def.freezeInMentalState && this.pawn.InMentalState) || !this.IsPawnInteractableOrVisible;
			}
		}

		// Token: 0x17000E64 RID: 3684
		// (get) Token: 0x0600546E RID: 21614 RVA: 0x001C9C51 File Offset: 0x001C7E51
		private bool IsPawnInteractableOrVisible
		{
			get
			{
				return this.pawn.SpawnedOrAnyParentSpawned || this.pawn.IsCaravanMember() || PawnUtility.IsTravelingInTransportPodWorldObject(this.pawn);
			}
		}

		// Token: 0x17000E65 RID: 3685
		// (get) Token: 0x0600546F RID: 21615 RVA: 0x001C9C81 File Offset: 0x001C7E81
		public virtual bool ShowOnNeedList
		{
			get
			{
				return this.def.showOnNeedList;
			}
		}

		// Token: 0x06005470 RID: 21616 RVA: 0x000033AC File Offset: 0x000015AC
		public Need()
		{
		}

		// Token: 0x06005471 RID: 21617 RVA: 0x001C9C8E File Offset: 0x001C7E8E
		public Need(Pawn newPawn)
		{
			this.pawn = newPawn;
			this.SetInitialLevel();
		}

		// Token: 0x06005472 RID: 21618 RVA: 0x001C9CA3 File Offset: 0x001C7EA3
		public virtual void ExposeData()
		{
			Scribe_Defs.Look<NeedDef>(ref this.def, "def");
			Scribe_Values.Look<float>(ref this.curLevelInt, "curLevel", 0f, false);
		}

		// Token: 0x06005473 RID: 21619
		public abstract void NeedInterval();

		// Token: 0x06005474 RID: 21620 RVA: 0x001C9CCC File Offset: 0x001C7ECC
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

		// Token: 0x06005475 RID: 21621 RVA: 0x001C9D19 File Offset: 0x001C7F19
		public virtual void SetInitialLevel()
		{
			this.CurLevelPercentage = 0.5f;
		}

		// Token: 0x06005476 RID: 21622 RVA: 0x001C9D28 File Offset: 0x001C7F28
		public virtual void DrawOnGUI(Rect rect, int maxThresholdMarkers = 2147483647, float customMargin = -1f, bool drawArrows = true, bool doTooltip = true, Rect? rectForTooltip = null)
		{
			if (rect.height > 70f)
			{
				float num = (rect.height - 70f) / 2f;
				rect.height = 70f;
				rect.y += num;
			}
			Rect rect2 = rectForTooltip ?? rect;
			if (Mouse.IsOver(rect2))
			{
				Widgets.DrawHighlight(rect2);
			}
			if (doTooltip && Mouse.IsOver(rect2))
			{
				TooltipHandler.TipRegion(rect2, new TipSignal(() => this.GetTipString(), rect2.GetHashCode()));
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
			Rect rect3 = new Rect(rect.x, rect.y + rect.height / 2f, rect.width, rect.height / 2f);
			rect3 = new Rect(rect3.x + num3, rect3.y, rect3.width - num3 * 2f, rect3.height - num2);
			Rect rect4 = rect3;
			float num4 = 1f;
			if (this.def.scaleBar && this.MaxLevel < 1f)
			{
				num4 = this.MaxLevel;
			}
			rect4.width *= num4;
			Rect barRect = Widgets.FillableBar(rect4, this.CurLevelPercentage);
			if (drawArrows)
			{
				Widgets.FillableBarChangeArrows(rect4, this.GUIChangeArrow);
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
				this.DrawBarInstantMarkerAt(rect3, curInstantLevelPercentage * num4);
			}
			if (!this.def.tutorHighlightTag.NullOrEmpty())
			{
				UIHighlighter.HighlightOpportunity(rect, this.def.tutorHighlightTag);
			}
			Text.Font = GameFont.Small;
		}

		// Token: 0x06005477 RID: 21623 RVA: 0x001CA000 File Offset: 0x001C8200
		protected void DrawBarInstantMarkerAt(Rect barRect, float pct)
		{
			if (pct > 1f)
			{
				Log.ErrorOnce(this.def + " drawing bar percent > 1 : " + pct, 6932178);
			}
			float num = 12f;
			if (barRect.width < 150f)
			{
				num /= 2f;
			}
			Vector2 vector = new Vector2(barRect.x + barRect.width * pct, barRect.y + barRect.height);
			GUI.DrawTexture(new Rect(vector.x - num / 2f, vector.y, num, num), Need.BarInstantMarkerTex);
		}

		// Token: 0x06005478 RID: 21624 RVA: 0x001CA0A0 File Offset: 0x001C82A0
		protected void DrawBarThreshold(Rect barRect, float threshPct)
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

		// Token: 0x06005479 RID: 21625 RVA: 0x001CA168 File Offset: 0x001C8368
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

		// Token: 0x040031B9 RID: 12729
		public NeedDef def;

		// Token: 0x040031BA RID: 12730
		protected Pawn pawn;

		// Token: 0x040031BB RID: 12731
		protected float curLevelInt;

		// Token: 0x040031BC RID: 12732
		protected List<float> threshPercents;

		// Token: 0x040031BD RID: 12733
		public const float MaxDrawHeight = 70f;

		// Token: 0x040031BE RID: 12734
		private static readonly Texture2D BarInstantMarkerTex = ContentFinder<Texture2D>.Get("UI/Misc/BarInstantMarker", true);

		// Token: 0x040031BF RID: 12735
		private static readonly Texture2D NeedUnitDividerTex = ContentFinder<Texture2D>.Get("UI/Misc/NeedUnitDivider", true);

		// Token: 0x040031C0 RID: 12736
		private const float BarInstantMarkerSize = 12f;
	}
}
