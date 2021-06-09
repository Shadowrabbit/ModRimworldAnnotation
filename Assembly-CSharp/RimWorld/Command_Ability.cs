using System;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001361 RID: 4961
	[StaticConstructorOnStartup]
	public class Command_Ability : Command
	{
		// Token: 0x170010A9 RID: 4265
		// (get) Token: 0x06006BEB RID: 27627 RVA: 0x00049753 File Offset: 0x00047953
		public Ability Ability
		{
			get
			{
				return this.ability;
			}
		}

		// Token: 0x170010AA RID: 4266
		// (get) Token: 0x06006BEC RID: 27628 RVA: 0x0004975B File Offset: 0x0004795B
		public override Texture2D BGTexture
		{
			get
			{
				return Command_Ability.BGTex;
			}
		}

		// Token: 0x170010AB RID: 4267
		// (get) Token: 0x06006BED RID: 27629 RVA: 0x00049762 File Offset: 0x00047962
		public override Texture2D BGTextureShrunk
		{
			get
			{
				return Command_Ability.BGTexShrunk;
			}
		}

		// Token: 0x170010AC RID: 4268
		// (get) Token: 0x06006BEE RID: 27630 RVA: 0x00049769 File Offset: 0x00047969
		public virtual string Tooltip
		{
			get
			{
				return this.ability.def.GetTooltip(this.ability.pawn);
			}
		}

		// Token: 0x06006BEF RID: 27631 RVA: 0x002132E8 File Offset: 0x002114E8
		public Command_Ability(Ability ability)
		{
			this.ability = ability;
			this.order = 5f;
			this.defaultLabel = ability.def.LabelCap;
			this.hotKey = ability.def.hotKey;
			this.icon = ability.def.uiIcon;
		}

		// Token: 0x06006BF0 RID: 27632 RVA: 0x00213348 File Offset: 0x00211548
		public override GizmoResult GizmoOnGUI(Vector2 topLeft, float maxWidth)
		{
			Rect rect = new Rect(topLeft.x, topLeft.y, this.GetWidth(maxWidth), 75f);
			GizmoResult result = base.GizmoOnGUI(topLeft, maxWidth);
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

		// Token: 0x06006BF1 RID: 27633 RVA: 0x00049786 File Offset: 0x00047986
		protected override GizmoResult GizmoOnGUIInt(Rect butRect, bool shrunk = false)
		{
			if (Mouse.IsOver(butRect))
			{
				this.defaultDesc = this.Tooltip;
			}
			this.DisabledCheck();
			return base.GizmoOnGUIInt(butRect, shrunk);
		}

		// Token: 0x06006BF2 RID: 27634 RVA: 0x00213418 File Offset: 0x00211618
		protected virtual void DisabledCheck()
		{
			string str;
			this.disabled = this.ability.GizmoDisabled(out str);
			if (this.disabled)
			{
				this.DisableWithReason(str.CapitalizeFirst());
			}
		}

		// Token: 0x06006BF3 RID: 27635 RVA: 0x0021344C File Offset: 0x0021164C
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
			Find.WorldTargeter.BeginTargeting_NewTemp(delegate(GlobalTargetInfo t)
			{
				if (this.ability.ValidateGlobalTarget(t))
				{
					this.ability.QueueCastingJob(t);
					return true;
				}
				return false;
			}, true, this.ability.def.uiIcon, !this.ability.pawn.IsCaravanMember(), null, new Func<GlobalTargetInfo, string>(this.ability.WorldMapExtraLabel), new Func<GlobalTargetInfo, bool>(this.ability.ValidateGlobalTarget));
		}

		// Token: 0x06006BF4 RID: 27636 RVA: 0x00213544 File Offset: 0x00211744
		public override void GizmoUpdateOnMouseover()
		{
			Verb_CastAbility verb_CastAbility;
			if ((verb_CastAbility = (this.ability.verb as Verb_CastAbility)) != null)
			{
				verb_CastAbility.DrawRadius();
			}
		}

		// Token: 0x06006BF5 RID: 27637 RVA: 0x000497AA File Offset: 0x000479AA
		protected void DisableWithReason(string reason)
		{
			this.disabledReason = reason;
			this.disabled = true;
		}

		// Token: 0x040047AC RID: 18348
		protected Ability ability;

		// Token: 0x040047AD RID: 18349
		public new static readonly Texture2D BGTex = ContentFinder<Texture2D>.Get("UI/Widgets/AbilityButBG", true);

		// Token: 0x040047AE RID: 18350
		public new static readonly Texture2D BGTexShrunk = ContentFinder<Texture2D>.Get("UI/Widgets/AbilityButBGShrunk", true);

		// Token: 0x040047AF RID: 18351
		private static readonly Texture2D cooldownBarTex = SolidColorMaterials.NewSolidColorTexture(new Color32(9, 203, 4, 64));
	}
}
