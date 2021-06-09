using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x020017A2 RID: 6050
	public class CompBladelinkWeapon : ThingComp
	{
		// Token: 0x170014AD RID: 5293
		// (get) Token: 0x060085A0 RID: 34208 RVA: 0x0005995E File Offset: 0x00057B5E
		public List<WeaponTraitDef> TraitsListForReading
		{
			get
			{
				return this.traits;
			}
		}

		// Token: 0x170014AE RID: 5294
		// (get) Token: 0x060085A1 RID: 34209 RVA: 0x00059966 File Offset: 0x00057B66
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

		// Token: 0x170014AF RID: 5295
		// (get) Token: 0x060085A2 RID: 34210 RVA: 0x002765E4 File Offset: 0x002747E4
		public bool Bondable
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

		// Token: 0x060085A3 RID: 34211 RVA: 0x00059984 File Offset: 0x00057B84
		public override void PostPostMake()
		{
			this.InitializeTraits();
		}

		// Token: 0x060085A4 RID: 34212 RVA: 0x0005998C File Offset: 0x00057B8C
		public override void PostDestroy(DestroyMode mode, Map previousMap)
		{
			this.UnBond();
		}

		// Token: 0x060085A5 RID: 34213 RVA: 0x0027662C File Offset: 0x0027482C
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

		// Token: 0x060085A6 RID: 34214 RVA: 0x002766D0 File Offset: 0x002748D0
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

		// Token: 0x060085A7 RID: 34215 RVA: 0x00276718 File Offset: 0x00274918
		public override void Notify_Equipped(Pawn pawn)
		{
			if (!ModLister.RoyaltyInstalled)
			{
				Log.ErrorOnce("Persona weapons are a Royalty-specific game system. If you want to use this code please check ModLister.RoyaltyInstalled before calling it. See rules on the Ludeon forum for more info.", 988331, false);
				return;
			}
			if (this.Bondable)
			{
				this.BondToPawn(pawn);
			}
			if (!this.traits.NullOrEmpty<WeaponTraitDef>())
			{
				for (int i = 0; i < this.traits.Count; i++)
				{
					this.traits[i].Worker.Notify_Equipped(pawn);
				}
			}
		}

		// Token: 0x060085A8 RID: 34216 RVA: 0x00276788 File Offset: 0x00274988
		private void BondToPawn(Pawn pawn)
		{
			if (pawn.IsColonistPlayerControlled && this.bondedPawn == null)
			{
				Find.LetterStack.ReceiveLetter("LetterBladelinkWeaponBondedLabel".Translate(pawn.Named("PAWN"), this.parent.Named("WEAPON")), "LetterBladelinkWeaponBonded".Translate(pawn.Named("PAWN"), this.parent.Named("WEAPON")), LetterDefOf.PositiveEvent, new LookTargets(pawn), null, null, null, null);
			}
			this.bonded = true;
			this.bondedPawnLabel = pawn.Name.ToStringFull;
			this.bondedPawn = pawn;
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

		// Token: 0x060085A9 RID: 34217 RVA: 0x00276880 File Offset: 0x00274A80
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

		// Token: 0x060085AA RID: 34218 RVA: 0x002768D8 File Offset: 0x00274AD8
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

		// Token: 0x060085AB RID: 34219 RVA: 0x00276920 File Offset: 0x00274B20
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

		// Token: 0x060085AC RID: 34220 RVA: 0x00276968 File Offset: 0x00274B68
		public void UnBond()
		{
			if (this.bondedPawn != null)
			{
				this.bondedPawn.equipment.bondedWeapon = null;
				if (!this.traits.NullOrEmpty<WeaponTraitDef>())
				{
					for (int i = 0; i < this.traits.Count; i++)
					{
						this.traits[i].Worker.Notify_Unbonded(this.bondedPawn);
					}
				}
			}
			this.bonded = false;
			this.bondedPawn = null;
			this.bondedPawnLabel = null;
			this.lastKillTick = -1;
		}

		// Token: 0x060085AD RID: 34221 RVA: 0x002769EC File Offset: 0x00274BEC
		public override string CompInspectStringExtra()
		{
			string text = "";
			if (!this.traits.NullOrEmpty<WeaponTraitDef>())
			{
				text += "Stat_Thing_PersonaWeaponTrait_Label".Translate() + ": " + (from x in this.traits
				select x.label).ToCommaList(false).CapitalizeFirst();
			}
			if (this.Bondable)
			{
				if (!text.NullOrEmpty())
				{
					text += "\n";
				}
				if (this.bondedPawn == null)
				{
					text += "NotBonded".Translate();
				}
				else
				{
					text += "BondedWith".Translate(this.bondedPawnLabel.ApplyTag(TagType.Name, null)).Resolve();
				}
			}
			return text;
		}

		// Token: 0x060085AE RID: 34222 RVA: 0x00276AD0 File Offset: 0x00274CD0
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<bool>(ref this.bonded, "bonded", false, false);
			Scribe_Values.Look<string>(ref this.bondedPawnLabel, "bondedPawnLabel", null, false);
			Scribe_Values.Look<int>(ref this.lastKillTick, "lastKillTick", -1, false);
			Scribe_References.Look<Pawn>(ref this.bondedPawn, "bondedPawn", true);
			Scribe_Collections.Look<WeaponTraitDef>(ref this.traits, "traits", LookMode.Def, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (this.traits == null)
				{
					this.traits = new List<WeaponTraitDef>();
				}
				if (this.bondedPawn != null)
				{
					if (string.IsNullOrEmpty(this.bondedPawnLabel) || !this.bonded)
					{
						this.bondedPawnLabel = this.bondedPawn.Name.ToStringFull;
						this.bonded = true;
					}
					if (this.bondedPawn.equipment.bondedWeapon == null)
					{
						this.bondedPawn.equipment.bondedWeapon = this.parent;
						return;
					}
					if (this.bondedPawn.equipment.bondedWeapon != this.parent)
					{
						this.UnBond();
					}
				}
			}
		}

		// Token: 0x060085AF RID: 34223 RVA: 0x00059994 File Offset: 0x00057B94
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
				select x.label).ToCommaList(false).CapitalizeFirst(), stringBuilder.ToString(), 5404, null, null, false);
			}
			yield break;
			yield break;
		}

		// Token: 0x060085B0 RID: 34224 RVA: 0x00006A05 File Offset: 0x00004C05
		[Obsolete("Will be removed in the future")]
		public override void Notify_UsedWeapon(Pawn pawn)
		{
		}

		// Token: 0x04005637 RID: 22071
		private bool bonded;

		// Token: 0x04005638 RID: 22072
		private string bondedPawnLabel;

		// Token: 0x04005639 RID: 22073
		private int lastKillTick = -1;

		// Token: 0x0400563A RID: 22074
		private List<WeaponTraitDef> traits = new List<WeaponTraitDef>();

		// Token: 0x0400563B RID: 22075
		public Pawn bondedPawn;

		// Token: 0x0400563C RID: 22076
		private static readonly IntRange TraitsRange = new IntRange(1, 2);
	}
}
