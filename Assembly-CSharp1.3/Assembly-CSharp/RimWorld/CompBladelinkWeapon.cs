using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02001112 RID: 4370
	public class CompBladelinkWeapon : CompBiocodable
	{
		// Token: 0x170011F2 RID: 4594
		// (get) Token: 0x060068E4 RID: 26852 RVA: 0x00236735 File Offset: 0x00234935
		public List<WeaponTraitDef> TraitsListForReading
		{
			get
			{
				return this.traits;
			}
		}

		// Token: 0x170011F3 RID: 4595
		// (get) Token: 0x060068E5 RID: 26853 RVA: 0x0023673D File Offset: 0x0023493D
		public int TicksSinceLastKill
		{
			get
			{
				if (this.lastKillTick < 0)
				{
					return 0;
				}
				return Find.TickManager.TicksAbs - this.lastKillTick;
			}
		}

		// Token: 0x170011F4 RID: 4596
		// (get) Token: 0x060068E6 RID: 26854 RVA: 0x0023675C File Offset: 0x0023495C
		public override bool Biocodable
		{
			get
			{
				if (!this.traits.NullOrEmpty<WeaponTraitDef>())
				{
					for (int i = 0; i < this.traits.Count; i++)
					{
						if (this.traits[i].neverBond)
						{
							return false;
						}
					}
				}
				return true;
			}
		}

		// Token: 0x060068E7 RID: 26855 RVA: 0x002367A2 File Offset: 0x002349A2
		public override void PostPostMake()
		{
			this.InitializeTraits();
		}

		// Token: 0x060068E8 RID: 26856 RVA: 0x002367AA File Offset: 0x002349AA
		public override void PostDestroy(DestroyMode mode, Map previousMap)
		{
			this.UnCode();
		}

		// Token: 0x060068E9 RID: 26857 RVA: 0x002367B4 File Offset: 0x002349B4
		private void InitializeTraits()
		{
			IEnumerable<WeaponTraitDef> allDefs = DefDatabase<WeaponTraitDef>.AllDefs;
			if (this.traits == null)
			{
				this.traits = new List<WeaponTraitDef>();
			}
			Rand.PushState(this.parent.HashOffset());
			int randomInRange = CompBladelinkWeapon.TraitsRange.RandomInRange;
			for (int i = 0; i < randomInRange; i++)
			{
				IEnumerable<WeaponTraitDef> source = from x in allDefs
				where this.CanAddTrait(x)
				select x;
				if (source.Any<WeaponTraitDef>())
				{
					this.traits.Add(source.RandomElementByWeight((WeaponTraitDef x) => x.commonality));
				}
			}
			Rand.PopState();
		}

		// Token: 0x060068EA RID: 26858 RVA: 0x00236858 File Offset: 0x00234A58
		private bool CanAddTrait(WeaponTraitDef trait)
		{
			if (!this.traits.NullOrEmpty<WeaponTraitDef>())
			{
				for (int i = 0; i < this.traits.Count; i++)
				{
					if (trait.Overlaps(this.traits[i]))
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x060068EB RID: 26859 RVA: 0x002368A0 File Offset: 0x00234AA0
		public override void Notify_Equipped(Pawn pawn)
		{
			if (!ModLister.CheckRoyalty("Persona weapon"))
			{
				return;
			}
			base.Notify_Equipped(pawn);
			if (!this.traits.NullOrEmpty<WeaponTraitDef>())
			{
				for (int i = 0; i < this.traits.Count; i++)
				{
					this.traits[i].Worker.Notify_Equipped(pawn);
				}
			}
		}

		// Token: 0x060068EC RID: 26860 RVA: 0x002368FC File Offset: 0x00234AFC
		public override void CodeFor(Pawn pawn)
		{
			if (!base.Biocodable)
			{
				return;
			}
			if (pawn.IsColonistPlayerControlled && base.CodedPawn == null)
			{
				Find.LetterStack.ReceiveLetter("LetterBladelinkWeaponBondedLabel".Translate(pawn.Named("PAWN"), this.parent.Named("WEAPON")), "LetterBladelinkWeaponBonded".Translate(pawn.Named("PAWN"), this.parent.Named("WEAPON")), LetterDefOf.PositiveEvent, new LookTargets(pawn), null, null, null, null);
			}
			base.CodeFor(pawn);
		}

		// Token: 0x060068ED RID: 26861 RVA: 0x0023698C File Offset: 0x00234B8C
		protected override void OnCodedFor(Pawn pawn)
		{
			this.lastKillTick = GenTicks.TicksAbs;
			pawn.equipment.bondedWeapon = this.parent;
			if (!this.traits.NullOrEmpty<WeaponTraitDef>())
			{
				for (int i = 0; i < this.traits.Count; i++)
				{
					this.traits[i].Worker.Notify_Bonded(pawn);
				}
			}
		}

		// Token: 0x060068EE RID: 26862 RVA: 0x002369F0 File Offset: 0x00234BF0
		public override void Notify_KilledPawn(Pawn pawn)
		{
			this.lastKillTick = Find.TickManager.TicksAbs;
			if (!this.traits.NullOrEmpty<WeaponTraitDef>())
			{
				for (int i = 0; i < this.traits.Count; i++)
				{
					this.traits[i].Worker.Notify_KilledPawn(pawn);
				}
			}
		}

		// Token: 0x060068EF RID: 26863 RVA: 0x00236A48 File Offset: 0x00234C48
		public void Notify_EquipmentLost(Pawn pawn)
		{
			if (!this.traits.NullOrEmpty<WeaponTraitDef>())
			{
				for (int i = 0; i < this.traits.Count; i++)
				{
					this.traits[i].Worker.Notify_EquipmentLost(pawn);
				}
			}
		}

		// Token: 0x060068F0 RID: 26864 RVA: 0x00236A90 File Offset: 0x00234C90
		public void Notify_WieldedOtherWeapon()
		{
			if (!this.traits.NullOrEmpty<WeaponTraitDef>())
			{
				for (int i = 0; i < this.traits.Count; i++)
				{
					this.traits[i].Worker.Notify_OtherWeaponWielded(this);
				}
			}
		}

		// Token: 0x060068F1 RID: 26865 RVA: 0x00236AD8 File Offset: 0x00234CD8
		public override void UnCode()
		{
			if (base.CodedPawn != null)
			{
				base.CodedPawn.equipment.bondedWeapon = null;
				if (!this.traits.NullOrEmpty<WeaponTraitDef>())
				{
					for (int i = 0; i < this.traits.Count; i++)
					{
						this.traits[i].Worker.Notify_Unbonded(base.CodedPawn);
					}
				}
			}
			base.UnCode();
			this.lastKillTick = -1;
		}

		// Token: 0x060068F2 RID: 26866 RVA: 0x00236B4C File Offset: 0x00234D4C
		public override string CompInspectStringExtra()
		{
			string text = "";
			if (!this.traits.NullOrEmpty<WeaponTraitDef>())
			{
				text += "Stat_Thing_PersonaWeaponTrait_Label".Translate() + ": " + (from x in this.traits
				select x.label).ToCommaList(false, false).CapitalizeFirst();
			}
			if (this.Biocodable)
			{
				if (!text.NullOrEmpty())
				{
					text += "\n";
				}
				if (base.CodedPawn == null)
				{
					text += "NotBonded".Translate();
				}
				else
				{
					text += "BondedWith".Translate(base.CodedPawnLabel.ApplyTag(TagType.Name, null)).Resolve();
				}
			}
			return text;
		}

		// Token: 0x060068F3 RID: 26867 RVA: 0x00236C30 File Offset: 0x00234E30
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.lastKillTick, "lastKillTick", -1, false);
			Scribe_Collections.Look<WeaponTraitDef>(ref this.traits, "traits", LookMode.Def, Array.Empty<object>());
			if (Scribe.mode != LoadSaveMode.Saving)
			{
				Scribe_Values.Look<bool>(ref this.oldBonded, "bonded", false, false);
				Scribe_Values.Look<string>(ref this.oldBondedPawnLabel, "bondedPawnLabel", null, false);
				Scribe_References.Look<Pawn>(ref this.oldBondedPawn, "bondedPawn", true);
			}
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (this.oldBonded)
				{
					this.CodeFor(this.oldBondedPawn);
				}
				if (this.traits == null)
				{
					this.traits = new List<WeaponTraitDef>();
				}
				if (this.oldBondedPawn != null)
				{
					if (string.IsNullOrEmpty(this.oldBondedPawnLabel) || !this.oldBonded)
					{
						this.codedPawnLabel = this.oldBondedPawn.Name.ToStringFull;
						this.biocoded = true;
					}
					if (this.oldBondedPawn.equipment.bondedWeapon == null)
					{
						this.oldBondedPawn.equipment.bondedWeapon = this.parent;
						return;
					}
					if (this.oldBondedPawn.equipment.bondedWeapon != this.parent)
					{
						this.UnCode();
					}
				}
			}
		}

		// Token: 0x060068F4 RID: 26868 RVA: 0x000210E7 File Offset: 0x0001F2E7
		public override string TransformLabel(string label)
		{
			return label;
		}

		// Token: 0x060068F5 RID: 26869 RVA: 0x00236D5B File Offset: 0x00234F5B
		public override IEnumerable<StatDrawEntry> SpecialDisplayStats()
		{
			foreach (Faction faction in Find.FactionManager.AllFactions)
			{
				RoyalTitleDef minTitleToUse = ThingRequiringRoyalPermissionUtility.GetMinTitleToUse(this.parent.def, faction, 0);
				if (minTitleToUse != null)
				{
					yield return new StatDrawEntry(StatCategoryDefOf.BasicsNonPawnImportant, "Stat_Thing_MinimumRoyalTitle_Name".Translate(faction.Named("FACTION")).Resolve(), minTitleToUse.GetLabelCapForBothGenders(), "Stat_Thing_Weapon_MinimumRoyalTitle_Desc".Translate(faction.Named("FACTION")).Resolve(), 2100, null, null, false);
				}
			}
			IEnumerator<Faction> enumerator = null;
			if (!this.traits.NullOrEmpty<WeaponTraitDef>())
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendLine("Stat_Thing_PersonaWeaponTrait_Desc".Translate());
				stringBuilder.AppendLine();
				for (int i = 0; i < this.traits.Count; i++)
				{
					stringBuilder.AppendLine(this.traits[i].LabelCap + ": " + this.traits[i].description);
					if (i < this.traits.Count - 1)
					{
						stringBuilder.AppendLine();
					}
				}
				yield return new StatDrawEntry(StatCategoryDefOf.Weapon, "Stat_Thing_PersonaWeaponTrait_Label".Translate(), (from x in this.traits
				select x.label).ToCommaList(false, false).CapitalizeFirst(), stringBuilder.ToString(), 1104, null, null, false);
			}
			yield break;
			yield break;
		}

		// Token: 0x04003AC1 RID: 15041
		private int lastKillTick = -1;

		// Token: 0x04003AC2 RID: 15042
		private List<WeaponTraitDef> traits = new List<WeaponTraitDef>();

		// Token: 0x04003AC3 RID: 15043
		private static readonly IntRange TraitsRange = new IntRange(1, 2);

		// Token: 0x04003AC4 RID: 15044
		private bool oldBonded;

		// Token: 0x04003AC5 RID: 15045
		private string oldBondedPawnLabel;

		// Token: 0x04003AC6 RID: 15046
		private Pawn oldBondedPawn;
	}
}
