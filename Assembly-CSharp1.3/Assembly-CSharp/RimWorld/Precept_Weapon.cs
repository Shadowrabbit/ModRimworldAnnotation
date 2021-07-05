using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000EFA RID: 3834
	public class Precept_Weapon : Precept
	{
		// Token: 0x17000FF4 RID: 4084
		// (get) Token: 0x06005B5D RID: 23389 RVA: 0x001F9251 File Offset: 0x001F7451
		public override string TipLabel
		{
			get
			{
				return this.def.issue.LabelCap + ": " + this.def.LabelCap;
			}
		}

		// Token: 0x17000FF5 RID: 4085
		// (get) Token: 0x06005B5E RID: 23390 RVA: 0x001F9284 File Offset: 0x001F7484
		public override string UIInfoFirstLine
		{
			get
			{
				return "Noble".Translate().CapitalizeFirst() + ": " + this.noble.LabelCap;
			}
		}

		// Token: 0x17000FF6 RID: 4086
		// (get) Token: 0x06005B5F RID: 23391 RVA: 0x001F92C4 File Offset: 0x001F74C4
		public override string UIInfoSecondLine
		{
			get
			{
				return "Despised".Translate().CapitalizeFirst() + ": " + this.despised.LabelCap;
			}
		}

		// Token: 0x17000FF7 RID: 4087
		// (get) Token: 0x06005B60 RID: 23392 RVA: 0x0001A4C7 File Offset: 0x000186C7
		public override Color LabelColor
		{
			get
			{
				return Color.white;
			}
		}

		// Token: 0x17000FF8 RID: 4088
		// (get) Token: 0x06005B61 RID: 23393 RVA: 0x001F9302 File Offset: 0x001F7502
		protected HashSet<ThingDef> NobleWeapons
		{
			get
			{
				if (this.nobleWeaponsCached.EnumerableNullOrEmpty<ThingDef>())
				{
					this.nobleWeaponsCached = new HashSet<ThingDef>(from x in DefDatabase<ThingDef>.AllDefs
					where x.IsWeapon && x.PlayerAcquirable && !x.weaponClasses.NullOrEmpty<WeaponClassDef>() && x.weaponClasses.Contains(this.noble)
					select x);
				}
				return this.nobleWeaponsCached;
			}
		}

		// Token: 0x17000FF9 RID: 4089
		// (get) Token: 0x06005B62 RID: 23394 RVA: 0x001F9338 File Offset: 0x001F7538
		protected HashSet<ThingDef> DespisedWeapons
		{
			get
			{
				if (this.despisedWeaponsCached.EnumerableNullOrEmpty<ThingDef>())
				{
					this.despisedWeaponsCached = new HashSet<ThingDef>(from x in DefDatabase<ThingDef>.AllDefs
					where x.IsWeapon && !x.weaponClasses.NullOrEmpty<WeaponClassDef>() && x.weaponClasses.Contains(this.despised)
					select x);
				}
				return this.despisedWeaponsCached;
			}
		}

		// Token: 0x06005B63 RID: 23395 RVA: 0x001F936E File Offset: 0x001F756E
		private void RecacheData()
		{
			this.nobleWeaponsCached = null;
			this.despisedWeaponsCached = null;
			this.iconWeapon = null;
			this.ClearTipCache();
		}

		// Token: 0x06005B64 RID: 23396 RVA: 0x001F938C File Offset: 0x001F758C
		public override void DrawIcon(Rect rect)
		{
			if (this.iconWeapon == null)
			{
				Rand.PushState(this.randomSeed);
				this.iconWeapon = (from w in this.NobleWeapons
				where !w.IsStuff
				select w).RandomElementWithFallback(null);
				Rand.PopState();
			}
			if (this.iconWeapon == null)
			{
				Rand.PushState(this.randomSeed);
				this.iconWeapon = (from w in this.DespisedWeapons
				where !w.IsStuff
				select w).RandomElementWithFallback(null);
				Rand.PopState();
			}
			Widgets.DefIcon(rect, this.iconWeapon, null, 1f, this.ideo.GetStyleFor(this.iconWeapon), false, null);
		}

		// Token: 0x06005B65 RID: 23397 RVA: 0x001F9464 File Offset: 0x001F7664
		public override string GetTip()
		{
			if (this.tipCached.NullOrEmpty())
			{
				string[] array = new string[9];
				array[0] = base.GetTip();
				array[1] = "\n\n";
				array[2] = base.ColorizeDescTitle("Noble".Translate().CapitalizeFirst());
				array[3] = ":\n";
				array[4] = (from x in this.NobleWeapons
				where x.canGenerateDefaultDesignator
				select x.LabelCap.Resolve()).ToCommaList(false, false);
				array[5] = "\n\n";
				array[6] = base.ColorizeDescTitle("Despised".Translate().CapitalizeFirst());
				array[7] = ":\n";
				array[8] = (from x in this.DespisedWeapons
				where x.canGenerateDefaultDesignator
				select x.LabelCap.Resolve()).ToCommaList(false, false);
				this.tipCached = string.Concat(array);
			}
			return this.tipCached;
		}

		// Token: 0x06005B66 RID: 23398 RVA: 0x001F95A8 File Offset: 0x001F77A8
		public override IEnumerable<FloatMenuOption> EditFloatMenuOptions()
		{
			yield return new FloatMenuOption("SwapNobleAndDespised".Translate(), delegate()
			{
				Gen.Swap<WeaponClassDef>(ref this.noble, ref this.despised);
				this.RecacheData();
			}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			yield break;
		}

		// Token: 0x06005B67 RID: 23399 RVA: 0x001F95B8 File Offset: 0x001F77B8
		public IdeoWeaponDisposition GetDispositionForWeapon(ThingDef td)
		{
			if (this.NobleWeapons.Contains(td))
			{
				return IdeoWeaponDisposition.Noble;
			}
			if (this.DespisedWeapons.Contains(td))
			{
				return IdeoWeaponDisposition.Despised;
			}
			return IdeoWeaponDisposition.None;
		}

		// Token: 0x06005B68 RID: 23400 RVA: 0x001F95DC File Offset: 0x001F77DC
		public override bool CompatibleWith(Precept other)
		{
			Precept_Weapon wep;
			if ((wep = (other as Precept_Weapon)) != null)
			{
				return !this.NobleWeapons.Any((ThingDef x) => wep.DespisedWeapons.Contains(x)) && !this.DespisedWeapons.Any((ThingDef x) => wep.NobleWeapons.Contains(x));
			}
			return base.CompatibleWith(other);
		}

		// Token: 0x06005B69 RID: 23401 RVA: 0x001F963D File Offset: 0x001F783D
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<WeaponClassDef>(ref this.noble, "noble");
			Scribe_Defs.Look<WeaponClassDef>(ref this.despised, "despised");
		}

		// Token: 0x0400353A RID: 13626
		public WeaponClassDef noble;

		// Token: 0x0400353B RID: 13627
		public WeaponClassDef despised;

		// Token: 0x0400353C RID: 13628
		[Unsaved(false)]
		private ThingDef iconWeapon;

		// Token: 0x0400353D RID: 13629
		[Unsaved(false)]
		private HashSet<ThingDef> nobleWeaponsCached;

		// Token: 0x0400353E RID: 13630
		[Unsaved(false)]
		private HashSet<ThingDef> despisedWeaponsCached;
	}
}
