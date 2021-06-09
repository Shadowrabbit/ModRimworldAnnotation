using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;

namespace Verse
{
	// Token: 0x020003B8 RID: 952
	public class Hediff_Psylink : Hediff_ImplantWithLevel
	{
		// Token: 0x060017B2 RID: 6066 RVA: 0x00016AAC File Offset: 0x00014CAC
		public override void PostAdd(DamageInfo? dinfo)
		{
			base.PostAdd(dinfo);
			this.TryGiveAbilityOfLevel_NewTemp(this.level, !this.suppressPostAddLetter);
			Pawn_PsychicEntropyTracker psychicEntropy = this.pawn.psychicEntropy;
			if (psychicEntropy == null)
			{
				return;
			}
			psychicEntropy.Notify_GainedPsylink();
		}

		// Token: 0x060017B3 RID: 6067 RVA: 0x000DD6E4 File Offset: 0x000DB8E4
		public void ChangeLevel(int levelOffset, bool sendLetter)
		{
			if (levelOffset > 0)
			{
				float num = Math.Min((float)levelOffset, this.def.maxSeverity - (float)this.level);
				int num2 = 0;
				while ((float)num2 < num)
				{
					int abilityLevel = this.level + 1 + num2;
					this.TryGiveAbilityOfLevel_NewTemp(abilityLevel, sendLetter);
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

		// Token: 0x060017B4 RID: 6068 RVA: 0x00016ADF File Offset: 0x00014CDF
		public override void ChangeLevel(int levelOffset)
		{
			this.ChangeLevel(levelOffset, true);
		}

		// Token: 0x060017B5 RID: 6069 RVA: 0x000DD750 File Offset: 0x000DB950
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

		// Token: 0x060017B6 RID: 6070 RVA: 0x000DD7F0 File Offset: 0x000DB9F0
		public void TryGiveAbilityOfLevel_NewTemp(int abilityLevel, bool sendLetter = true)
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

		// Token: 0x060017B7 RID: 6071 RVA: 0x00016AE9 File Offset: 0x00014CE9
		[Obsolete("Will be removed in a future version")]
		public void TryGiveAbilityOfLevel(int abilityLevel)
		{
			this.TryGiveAbilityOfLevel_NewTemp(abilityLevel, true);
		}

		// Token: 0x060017B8 RID: 6072 RVA: 0x00016AF3 File Offset: 0x00014CF3
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

		// Token: 0x0400121C RID: 4636
		public bool suppressPostAddLetter;
	}
}
