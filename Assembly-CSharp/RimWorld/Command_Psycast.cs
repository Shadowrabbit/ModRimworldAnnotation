using System;
using System.Linq;
using System.Text;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001363 RID: 4963
	public class Command_Psycast : Command_Ability
	{
		// Token: 0x06006BFA RID: 27642 RVA: 0x00049822 File Offset: 0x00047A22
		public Command_Psycast(Psycast ability) : base(ability)
		{
			this.shrinkable = true;
		}

		// Token: 0x06006BFB RID: 27643 RVA: 0x00213638 File Offset: 0x00211838
		protected override void DisabledCheck()
		{
			AbilityDef def = this.ability.def;
			Pawn pawn = this.ability.pawn;
			this.disabled = false;
			if (def.EntropyGain > 1E-45f)
			{
				Hediff hediff = pawn.health.hediffSet.hediffs.FirstOrDefault((Hediff h) => h.def == HediffDefOf.PsychicAmplifier);
				if (hediff == null || hediff.Severity < (float)def.level)
				{
					base.DisableWithReason("CommandPsycastHigherLevelPsylinkRequired".Translate(def.level));
				}
				else if (pawn.psychicEntropy.WouldOverflowEntropy(def.EntropyGain + PsycastUtility.TotalEntropyFromQueuedPsycasts(pawn)))
				{
					base.DisableWithReason("CommandPsycastWouldExceedEntropy".Translate(def.label));
				}
			}
			base.DisabledCheck();
		}

		// Token: 0x170010AE RID: 4270
		// (get) Token: 0x06006BFC RID: 27644 RVA: 0x00213720 File Offset: 0x00211920
		public override string Label
		{
			get
			{
				if (this.ability.pawn.IsCaravanMember())
				{
					Pawn pawn = this.ability.pawn;
					Pawn_PsychicEntropyTracker psychicEntropy = pawn.psychicEntropy;
					StringBuilder stringBuilder = new StringBuilder(base.Label + " (" + pawn.LabelShort);
					if (this.ability.def.PsyfocusCost > 1E-45f)
					{
						stringBuilder.Append(", " + "PsyfocusLetter".Translate() + ":" + psychicEntropy.CurrentPsyfocus.ToStringPercent("0"));
					}
					if (this.ability.def.EntropyGain > 1E-45f)
					{
						if (this.ability.def.PsyfocusCost > 1E-45f)
						{
							stringBuilder.Append(",");
						}
						stringBuilder.Append(" " + "NeuralHeatLetter".Translate() + ":" + Mathf.Round(psychicEntropy.EntropyValue));
					}
					stringBuilder.Append(")");
					return stringBuilder.ToString();
				}
				return base.Label;
			}
		}

		// Token: 0x170010AF RID: 4271
		// (get) Token: 0x06006BFD RID: 27645 RVA: 0x0021385C File Offset: 0x00211A5C
		public override string TopRightLabel
		{
			get
			{
				AbilityDef def = this.ability.def;
				string text = "";
				if (def.EntropyGain > 1E-45f)
				{
					text += "NeuralHeatLetter".Translate() + ": " + def.EntropyGain.ToString() + "\n";
				}
				if (def.PsyfocusCost > 1E-45f)
				{
					string t;
					if (def.AnyCompOverridesPsyfocusCost)
					{
						if (def.PsyfocusCostRange.Span > 1E-45f)
						{
							t = def.PsyfocusCostRange.min * 100f + "-" + def.PsyfocusCostPercentMax;
						}
						else
						{
							t = def.PsyfocusCostPercentMax;
						}
					}
					else
					{
						t = def.PsyfocusCostPercent;
					}
					text += "PsyfocusLetter".Translate() + ": " + t;
				}
				return text.TrimEndNewlines();
			}
		}
	}
}
