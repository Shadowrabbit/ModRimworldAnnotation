using System;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02000EFE RID: 3838
	[StaticConstructorOnStartup]
	public class Command_Ritual : Command
	{
		// Token: 0x06005BA6 RID: 23462 RVA: 0x001FAE44 File Offset: 0x001F9044
		public Command_Ritual(Precept_Ritual ritual, TargetInfo targetInfo, RitualObligation forObligation = null)
		{
			this.ritual = ritual;
			this.targetInfo = targetInfo;
			this.obligation = forObligation;
			string text = (this.obligation != null) ? ritual.obligationTargetFilter.LabelExtraPart(this.obligation) : "";
			if (text.NullOrEmpty() || ritual.mergeGizmosForObligations)
			{
				this.defaultLabel = "BeginRitual".Translate(ritual.Label);
			}
			else
			{
				this.defaultLabel = "BeginRitualFor".Translate(ritual.Label, text);
			}
			this.defaultDesc = ritual.def.description;
			this.icon = ritual.Icon;
			this.defaultIconColor = ritual.ideo.Color;
		}

		// Token: 0x17001009 RID: 4105
		// (get) Token: 0x06005BA7 RID: 23463 RVA: 0x001FAF22 File Offset: 0x001F9122
		public override string Desc
		{
			get
			{
				return this.ritual.TipLabel.Colorize(ColoredText.TipSectionTitleColor) + "\n\n" + this.ritual.TipMainPart();
			}
		}

		// Token: 0x1700100A RID: 4106
		// (get) Token: 0x06005BA8 RID: 23464 RVA: 0x001FAF4E File Offset: 0x001F914E
		public override string DescPostfix
		{
			get
			{
				return this.ritual.TipExtraPart();
			}
		}

		// Token: 0x06005BA9 RID: 23465 RVA: 0x001FAF5C File Offset: 0x001F915C
		protected override void DrawIcon(Rect rect, Material buttonMat, GizmoRenderParms parms)
		{
			base.DrawIcon(rect, buttonMat, parms);
			if (this.ritual.RepeatPenaltyActive)
			{
				float value = Mathf.InverseLerp(1200000f, 0f, (float)this.ritual.TicksSinceLastPerformed);
				Widgets.FillableBar(rect.ContractedBy(1f), Mathf.Clamp01(value), Command_Ritual.CooldownBarTex, null, false);
				Text.Font = GameFont.Tiny;
				Text.Anchor = TextAnchor.UpperCenter;
				float num = (float)(1200000 - this.ritual.TicksSinceLastPerformed) / 60000f;
				if (num >= 1f)
				{
					num = (float)Mathf.RoundToInt(num);
				}
				else
				{
					num = (float)((int)(num * 10f)) / 10f;
				}
				Widgets.Label(rect, num + " " + "DaysLower".Translate());
				Text.Anchor = TextAnchor.UpperLeft;
				GUI.DrawTexture(new Rect(rect.xMax - (float)this.PenaltyIconSize.x, rect.yMin + 4f, (float)this.PenaltyIconSize.x, (float)this.PenaltyIconSize.z), Command_Ritual.PenaltyArrowTex);
			}
		}

		// Token: 0x06005BAA RID: 23466 RVA: 0x001FB078 File Offset: 0x001F9278
		protected override GizmoResult GizmoOnGUIInt(Rect butRect, GizmoRenderParms parms)
		{
			string text = this.ritual.behavior.CanStartRitualNow(this.targetInfo, this.ritual, null, null);
			if (!text.NullOrEmpty())
			{
				this.disabled = true;
				this.disabledReason = text;
			}
			return base.GizmoOnGUIInt(butRect, parms);
		}

		// Token: 0x06005BAB RID: 23467 RVA: 0x001FB0C2 File Offset: 0x001F92C2
		public override void ProcessInput(Event ev)
		{
			base.ProcessInput(ev);
			this.ritual.ShowRitualBeginWindow(this.targetInfo, null, null);
			SoundDefOf.Tick_Tiny.PlayOneShotOnCamera(null);
		}

		// Token: 0x04003563 RID: 13667
		private Precept_Ritual ritual;

		// Token: 0x04003564 RID: 13668
		private RitualObligation obligation;

		// Token: 0x04003565 RID: 13669
		private TargetInfo targetInfo;

		// Token: 0x04003566 RID: 13670
		private readonly IntVec2 PenaltyIconSize = new IntVec2(16, 16);

		// Token: 0x04003567 RID: 13671
		private static readonly Texture2D CooldownBarTex = SolidColorMaterials.NewSolidColorTexture(new Color32(170, 150, 0, 60));

		// Token: 0x04003568 RID: 13672
		private static readonly Texture2D PenaltyArrowTex = ContentFinder<Texture2D>.Get("UI/Icons/Rituals/QualityPenalty", true);
	}
}
