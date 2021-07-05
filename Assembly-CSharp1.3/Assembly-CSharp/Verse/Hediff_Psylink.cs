using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;

namespace Verse
{
	// Token: 0x020002CF RID: 719
	public class Hediff_Psylink : Hediff_Level
	{
		// Token: 0x0600137A RID: 4986 RVA: 0x0006E7EA File Offset: 0x0006C9EA
		public override void PostAdd(DamageInfo? dinfo)
		{
			base.PostAdd(dinfo);
			this.TryGiveAbilityOfLevel(this.level, !this.suppressPostAddLetter);
			Pawn_PsychicEntropyTracker psychicEntropy = this.pawn.psychicEntropy;
			if (psychicEntropy == null)
			{
				return;
			}
			psychicEntropy.Notify_GainedPsylink();
		}

		// Token: 0x0600137B RID: 4987 RVA: 0x0006E820 File Offset: 0x0006CA20
		public void ChangeLevel(int levelOffset, bool sendLetter)
		{
			if (levelOffset > 0)
			{
				float num = Math.Min((float)levelOffset, this.def.maxSeverity - (float)this.level);
				int num2 = 0;
				while ((float)num2 < num)
				{
					int abilityLevel = this.level + 1 + num2;
					this.TryGiveAbilityOfLevel(abilityLevel, sendLetter);
					Pawn_PsychicEntropyTracker psychicEntropy = this.pawn.psychicEntropy;
					if (psychicEntropy != null)
					{
						psychicEntropy.Notify_GainedPsylink();
					}
					num2++;
				}
			}
			base.ChangeLevel(levelOffset);
		}

		// Token: 0x0600137C RID: 4988 RVA: 0x0006E889 File Offset: 0x0006CA89
		public override void ChangeLevel(int levelOffset)
		{
			this.ChangeLevel(levelOffset, true);
		}

		// Token: 0x0600137D RID: 4989 RVA: 0x0006E894 File Offset: 0x0006CA94
		public static string MakeLetterTextNewPsylinkLevel(Pawn pawn, int abilityLevel, IEnumerable<AbilityDef> newAbilities = null)
		{
			string text = ((abilityLevel == 1) ? "LetterPsylinkLevelGained_First" : "LetterPsylinkLevelGained_NotFirst").Translate(pawn.Named("USER"));
			if (!newAbilities.EnumerableNullOrEmpty<AbilityDef>())
			{
				text += "\n\n" + "LetterPsylinkLevelGained_PsycastLearned".Translate(pawn.Named("USER"), abilityLevel, (from a in newAbilities
				select a.LabelCap.Resolve()).ToLineList(null, false));
			}
			return text;
		}

		// Token: 0x0600137E RID: 4990 RVA: 0x0006E934 File Offset: 0x0006CB34
		public void TryGiveAbilityOfLevel(int abilityLevel, bool sendLetter = true)
		{
			string str = "LetterLabelPsylinkLevelGained".Translate() + ": " + this.pawn.LabelShortCap;
			string str2;
			if (!this.pawn.abilities.abilities.Any((Ability a) => a.def.level == abilityLevel))
			{
				AbilityDef abilityDef = (from a in DefDatabase<AbilityDef>.AllDefs
				where a.level == abilityLevel
				select a).RandomElement<AbilityDef>();
				this.pawn.abilities.GainAbility(abilityDef);
				str2 = Hediff_Psylink.MakeLetterTextNewPsylinkLevel(this.pawn, abilityLevel, Gen.YieldSingle<AbilityDef>(abilityDef));
			}
			else
			{
				str2 = Hediff_Psylink.MakeLetterTextNewPsylinkLevel(this.pawn, abilityLevel, null);
			}
			if (sendLetter && PawnUtility.ShouldSendNotificationAbout(this.pawn))
			{
				Find.LetterStack.ReceiveLetter(str, str2, LetterDefOf.PositiveEvent, this.pawn, null, null, null, null);
			}
		}

		// Token: 0x0600137F RID: 4991 RVA: 0x0006EA31 File Offset: 0x0006CC31
		public override void PostRemoved()
		{
			base.PostRemoved();
			Pawn_NeedsTracker needs = this.pawn.needs;
			if (needs == null)
			{
				return;
			}
			needs.AddOrRemoveNeedsAsAppropriate();
		}

		// Token: 0x04000E5B RID: 3675
		public bool suppressPostAddLetter;
	}
}
