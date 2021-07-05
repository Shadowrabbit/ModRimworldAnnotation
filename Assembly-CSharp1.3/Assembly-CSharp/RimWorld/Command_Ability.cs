using System;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02000D19 RID: 3353
	[StaticConstructorOnStartup]
	public class Command_Ability : Command
	{
		// Token: 0x17000D9A RID: 3482
		// (get) Token: 0x06004EBA RID: 20154 RVA: 0x001A60E0 File Offset: 0x001A42E0
		public Ability Ability
		{
			get
			{
				return this.ability;
			}
		}

		// Token: 0x17000D9B RID: 3483
		// (get) Token: 0x06004EBB RID: 20155 RVA: 0x001A60E8 File Offset: 0x001A42E8
		public override Texture2D BGTexture
		{
			get
			{
				return Command_Ability.BGTex;
			}
		}

		// Token: 0x17000D9C RID: 3484
		// (get) Token: 0x06004EBC RID: 20156 RVA: 0x001A60EF File Offset: 0x001A42EF
		public override Texture2D BGTextureShrunk
		{
			get
			{
				return Command_Ability.BGTexShrunk;
			}
		}

		// Token: 0x17000D9D RID: 3485
		// (get) Token: 0x06004EBD RID: 20157 RVA: 0x001A60F6 File Offset: 0x001A42F6
		public virtual string Tooltip
		{
			get
			{
				return this.ability.Tooltip;
			}
		}

		// Token: 0x06004EBE RID: 20158 RVA: 0x001A6104 File Offset: 0x001A4304
		public Command_Ability(Ability ability)
		{
			this.ability = ability;
			float num = 5f;
			AbilityCategoryDef category = ability.def.category;
			int? num2 = (category != null) ? new int?(category.displayOrder) : null;
			this.order = num + ((num2 != null) ? ((float)num2.GetValueOrDefault()) : 0f) / 100f + (float)ability.def.level / 10000f;
			this.defaultLabel = ability.def.LabelCap;
			this.hotKey = ability.def.hotKey;
			this.icon = ability.def.uiIcon;
		}

		// Token: 0x06004EBF RID: 20159 RVA: 0x001A61B8 File Offset: 0x001A43B8
		public override GizmoResult GizmoOnGUI(Vector2 topLeft, float maxWidth, GizmoRenderParms parms)
		{
			Rect rect = new Rect(topLeft.x, topLeft.y, this.GetWidth(maxWidth), 75f);
			GizmoResult result = base.GizmoOnGUI(topLeft, maxWidth, parms);
			if (this.ability.CooldownTicksRemaining > 0)
			{
				float num = Mathf.InverseLerp((float)this.ability.CooldownTicksTotal, 0f, (float)this.ability.CooldownTicksRemaining);
				Widgets.FillableBar(rect, Mathf.Clamp01(num), Command_Ability.cooldownBarTex, null, false);
				if (this.ability.CooldownTicksRemaining > 0)
				{
					Text.Font = GameFont.Tiny;
					Text.Anchor = TextAnchor.UpperCenter;
					Widgets.Label(rect, num.ToStringPercent("F0"));
					Text.Anchor = TextAnchor.UpperLeft;
				}
			}
			if (result.State == GizmoState.Interacted && this.ability.CanCast)
			{
				return result;
			}
			return new GizmoResult(result.State);
		}

		// Token: 0x06004EC0 RID: 20160 RVA: 0x001A6289 File Offset: 0x001A4489
		protected override GizmoResult GizmoOnGUIInt(Rect butRect, GizmoRenderParms parms)
		{
			if (Mouse.IsOver(butRect))
			{
				this.defaultDesc = this.Tooltip;
			}
			this.DisabledCheck();
			return base.GizmoOnGUIInt(butRect, parms);
		}

		// Token: 0x06004EC1 RID: 20161 RVA: 0x001A62B0 File Offset: 0x001A44B0
		protected virtual void DisabledCheck()
		{
			string str;
			this.disabled = this.ability.GizmoDisabled(out str);
			if (this.disabled)
			{
				this.DisableWithReason(str.CapitalizeFirst());
			}
		}

		// Token: 0x06004EC2 RID: 20162 RVA: 0x001A62E4 File Offset: 0x001A44E4
		public override void ProcessInput(Event ev)
		{
			base.ProcessInput(ev);
			SoundDefOf.Tick_Tiny.PlayOneShotOnCamera(null);
			if (!this.ability.def.targetRequired)
			{
				this.ability.QueueCastingJob(this.ability.pawn, LocalTargetInfo.Invalid);
				return;
			}
			if (!this.ability.def.targetWorldCell)
			{
				Find.Targeter.BeginTargeting(this.ability.verb, null);
				return;
			}
			CameraJumper.TryJump(CameraJumper.GetWorldTarget(this.ability.pawn));
			Find.WorldTargeter.BeginTargeting(delegate(GlobalTargetInfo t)
			{
				if (this.ability.ValidateGlobalTarget(t))
				{
					this.ability.QueueCastingJob(t);
					return true;
				}
				return false;
			}, true, this.ability.def.uiIcon, !this.ability.pawn.IsCaravanMember(), null, new Func<GlobalTargetInfo, string>(this.ability.WorldMapExtraLabel), new Func<GlobalTargetInfo, bool>(this.ability.ValidateGlobalTarget));
		}

		// Token: 0x06004EC3 RID: 20163 RVA: 0x001A63DC File Offset: 0x001A45DC
		public override void GizmoUpdateOnMouseover()
		{
			Verb_CastAbility verb_CastAbility;
			if ((verb_CastAbility = (this.ability.verb as Verb_CastAbility)) != null)
			{
				verb_CastAbility.DrawRadius();
			}
			this.ability.OnGizmoUpdate();
		}

		// Token: 0x06004EC4 RID: 20164 RVA: 0x001A640E File Offset: 0x001A460E
		protected void DisableWithReason(string reason)
		{
			this.disabledReason = reason;
			this.disabled = true;
		}

		// Token: 0x04002F59 RID: 12121
		protected Ability ability;

		// Token: 0x04002F5A RID: 12122
		public new static readonly Texture2D BGTex = ContentFinder<Texture2D>.Get("UI/Widgets/AbilityButBG", true);

		// Token: 0x04002F5B RID: 12123
		public new static readonly Texture2D BGTexShrunk = ContentFinder<Texture2D>.Get("UI/Widgets/AbilityButBGShrunk", true);

		// Token: 0x04002F5C RID: 12124
		private static readonly Texture2D cooldownBarTex = SolidColorMaterials.NewSolidColorTexture(new Color32(9, 203, 4, 64));
	}
}
