using System;
using System.Text;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000D1C RID: 3356
	public class Command_Psycast : Command_Ability
	{
		// Token: 0x06004ECC RID: 20172 RVA: 0x001A6719 File Offset: 0x001A4919
		public Command_Psycast(Psycast ability) : base(ability)
		{
			this.shrinkable = true;
		}

		// Token: 0x06004ECD RID: 20173 RVA: 0x001A672C File Offset: 0x001A492C
		protected override void DisabledCheck()
		{
			AbilityDef def = this.ability.def;
			Pawn pawn = this.ability.pawn;
			this.disabled = false;
			if (def.EntropyGain > 1E-45f)
			{
				if (pawn.GetPsylinkLevel() < def.level)
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

		// Token: 0x17000DA0 RID: 3488
		// (get) Token: 0x06004ECE RID: 20174 RVA: 0x001A67D8 File Offset: 0x001A49D8
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

		// Token: 0x17000DA1 RID: 3489
		// (get) Token: 0x06004ECF RID: 20175 RVA: 0x001A6914 File Offset: 0x001A4B14
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
