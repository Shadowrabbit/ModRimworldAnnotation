using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000EF3 RID: 3827
	public class Precept_Apparel : Precept
	{
		// Token: 0x17000FDB RID: 4059
		// (get) Token: 0x06005B0C RID: 23308 RVA: 0x0001276E File Offset: 0x0001096E
		public override bool SortByImpact
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000FDC RID: 4060
		// (get) Token: 0x06005B0D RID: 23309 RVA: 0x001F7BC9 File Offset: 0x001F5DC9
		public override bool CanRegenerate
		{
			get
			{
				return this.def.Worker.ThingDefs.Count<PreceptThingChance>() > 1;
			}
		}

		// Token: 0x17000FDD RID: 4061
		// (get) Token: 0x06005B0E RID: 23310 RVA: 0x001F7BE3 File Offset: 0x001F5DE3
		public override string TipLabel
		{
			get
			{
				return this.UIInfoFirstLine + ": " + this.def.LabelCap;
			}
		}

		// Token: 0x17000FDE RID: 4062
		// (get) Token: 0x06005B0F RID: 23311 RVA: 0x001F7C0A File Offset: 0x001F5E0A
		public override string UIInfoFirstLine
		{
			get
			{
				if (this.apparelDef != null)
				{
					return this.apparelDef.LabelCap;
				}
				return base.UIInfoFirstLine;
			}
		}

		// Token: 0x17000FDF RID: 4063
		// (get) Token: 0x06005B10 RID: 23312 RVA: 0x001F7C2C File Offset: 0x001F5E2C
		public override string UIInfoSecondLine
		{
			get
			{
				if (this.TargetGender == Gender.None)
				{
					return "Everyone".Translate() + ": " + base.UIInfoSecondLine;
				}
				return this.TargetGender.GetLabel(false).CapitalizeFirst() + ": " + base.UIInfoSecondLine;
			}
		}

		// Token: 0x17000FE0 RID: 4064
		// (get) Token: 0x06005B11 RID: 23313 RVA: 0x001F7C88 File Offset: 0x001F5E88
		public Gender TargetGender
		{
			get
			{
				Gender? gender = this.overrideGender;
				if (gender == null)
				{
					return this.targetGender;
				}
				return gender.GetValueOrDefault();
			}
		}

		// Token: 0x06005B12 RID: 23314 RVA: 0x001F7CB4 File Offset: 0x001F5EB4
		public override void Init(Ideo ideo, FactionDef generatingFor = null)
		{
			base.Init(ideo, null);
			for (int i = 0; i < this.def.comps.Count; i++)
			{
				PreceptComp_Apparel preceptComp_Apparel;
				if ((preceptComp_Apparel = (this.def.comps[i] as PreceptComp_Apparel)) != null)
				{
					this.targetGender = preceptComp_Apparel.AffectedGender(ideo);
					break;
				}
			}
			this.InitDescription();
			ideo.RegenerateAllApparelRequirements(generatingFor);
		}

		// Token: 0x06005B13 RID: 23315 RVA: 0x001F7D1C File Offset: 0x001F5F1C
		public override void Regenerate(Ideo ideo, FactionDef generatingFor = null)
		{
			IEnumerable<PreceptThingChance> source;
			if (!this.def.canUseAlreadyUsedThingDef)
			{
				Precept_Apparel.usedThingDefsTmp.Clear();
				using (List<Precept>.Enumerator enumerator = ideo.PreceptsListForReading.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Precept_Apparel precept_Apparel;
						if ((precept_Apparel = (enumerator.Current as Precept_Apparel)) != null)
						{
							Precept_Apparel.usedThingDefsTmp.Add(precept_Apparel.apparelDef);
						}
					}
				}
				source = from bd in this.def.Worker.ThingDefsForIdeo(ideo)
				where !Precept_Apparel.usedThingDefsTmp.Contains(bd.def)
				select bd;
			}
			else
			{
				source = this.def.Worker.ThingDefsForIdeo(ideo);
			}
			if (source.Any<PreceptThingChance>())
			{
				this.apparelDef = source.RandomElementByWeight((PreceptThingChance d) => d.chance).def;
			}
			else
			{
				this.apparelDef = this.def.Worker.ThingDefsForIdeo(ideo).RandomElementByWeight((PreceptThingChance d) => d.chance).def;
				Log.Warning("Failed to generate a unique apparel for " + ideo.name);
			}
			if (ideo.SupremeGender == Gender.None && Rand.Value < 0.5f)
			{
				this.overrideGender = new Gender?((Rand.Value < 0.5f) ? Gender.Male : Gender.Female);
			}
			else
			{
				this.overrideGender = null;
			}
			ideo.RegenerateAllApparelRequirements(generatingFor);
			base.Regenerate(ideo, null);
		}

		// Token: 0x06005B14 RID: 23316 RVA: 0x001F7EC4 File Offset: 0x001F60C4
		public override void DrawIcon(Rect rect)
		{
			Widgets.ThingIcon(rect, this.apparelDef, null, this.ideo.GetStyleFor(this.apparelDef), 1f, new Color?(this.ideo.ApparelColor));
		}

		// Token: 0x06005B15 RID: 23317 RVA: 0x001F7EF9 File Offset: 0x001F60F9
		public override IEnumerable<FloatMenuOption> EditFloatMenuOptions()
		{
			string text = "SetTargetGender".Translate() + ": ";
			FloatMenuOption floatMenuOption = new FloatMenuOption(text + Gender.Male.GetLabel(false).CapitalizeFirst(), delegate()
			{
				this.overrideGender = new Gender?(Gender.Male);
				this.InitDescription();
			}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			FloatMenuOption setFemale = new FloatMenuOption(text + Gender.Female.GetLabel(false).CapitalizeFirst(), delegate()
			{
				this.overrideGender = new Gender?(Gender.Female);
				this.InitDescription();
			}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			FloatMenuOption setNone = new FloatMenuOption(text + "Everyone".Translate().CapitalizeFirst(), delegate()
			{
				this.overrideGender = new Gender?(Gender.None);
				this.InitDescription();
			}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			switch (this.TargetGender)
			{
			case Gender.None:
				yield return floatMenuOption;
				yield return setFemale;
				break;
			case Gender.Male:
				yield return setFemale;
				yield return setNone;
				break;
			case Gender.Female:
				yield return floatMenuOption;
				yield return setNone;
				break;
			}
			if (this.def.apparelPreceptSwapDef != null)
			{
				yield return new FloatMenuOption("SetApparelType".Translate() + ": " + this.def.apparelPreceptSwapDef.LabelCap, delegate()
				{
					this.def = this.def.apparelPreceptSwapDef;
				}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			}
			yield break;
		}

		// Token: 0x06005B16 RID: 23318 RVA: 0x001F7F0C File Offset: 0x001F610C
		private void InitDescription()
		{
			string arg = (this.TargetGender != Gender.None) ? this.TargetGender.GetLabel(false) : "All".Translate();
			this.descOverride = this.def.description.Formatted(arg.Named("GENDER"), this.apparelDef.Named("APPAREL")).CapitalizeFirst();
		}

		// Token: 0x06005B17 RID: 23319 RVA: 0x001F7F80 File Offset: 0x001F6180
		public override bool CompatibleWith(Precept other)
		{
			Precept_Apparel precept_Apparel;
			return (!other.def.prefersNudity || (this.TargetGender != Gender.None && this.TargetGender != other.def.genderPrefersNudity)) && ((precept_Apparel = (other as Precept_Apparel)) == null || this.def != other.def || (this.TargetGender != precept_Apparel.TargetGender && this.TargetGender != Gender.None && precept_Apparel.TargetGender != Gender.None) || ApparelUtility.CanWearTogether(this.apparelDef, precept_Apparel.apparelDef, BodyDefOf.Human)) && base.CompatibleWith(other);
		}

		// Token: 0x06005B18 RID: 23320 RVA: 0x001F8010 File Offset: 0x001F6210
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<ThingDef>(ref this.apparelDef, "apparelDef");
			Scribe_Values.Look<Gender>(ref this.targetGender, "targetGender", Gender.None, false);
			Scribe_Values.Look<Gender?>(ref this.overrideGender, "overrideGender", null, false);
		}

		// Token: 0x0400352A RID: 13610
		public ThingDef apparelDef;

		// Token: 0x0400352B RID: 13611
		private Gender targetGender;

		// Token: 0x0400352C RID: 13612
		private Gender? overrideGender;

		// Token: 0x0400352D RID: 13613
		private static List<ThingDef> usedThingDefsTmp = new List<ThingDef>();
	}
}
