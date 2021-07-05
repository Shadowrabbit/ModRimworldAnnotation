using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001870 RID: 6256
	public class CompTempControl : ThingComp
	{
		// Token: 0x170015CD RID: 5581
		// (get) Token: 0x06008AC7 RID: 35527 RVA: 0x0005D0C5 File Offset: 0x0005B2C5
		public CompProperties_TempControl Props
		{
			get
			{
				return (CompProperties_TempControl)this.props;
			}
		}

		// Token: 0x06008AC8 RID: 35528 RVA: 0x0005D0D2 File Offset: 0x0005B2D2
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostSpawnSetup(respawningAfterLoad);
			if (this.targetTemperature < -2000f)
			{
				this.targetTemperature = this.Props.defaultTargetTemperature;
			}
		}

		// Token: 0x06008AC9 RID: 35529 RVA: 0x0005D0F9 File Offset: 0x0005B2F9
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<float>(ref this.targetTemperature, "targetTemperature", 0f, false);
		}

		// Token: 0x06008ACA RID: 35530 RVA: 0x0005D117 File Offset: 0x0005B317
		private float RoundedToCurrentTempModeOffset(float celsiusTemp)
		{
			return GenTemperature.ConvertTemperatureOffset((float)Mathf.RoundToInt(GenTemperature.CelsiusToOffset(celsiusTemp, Prefs.TemperatureMode)), Prefs.TemperatureMode, TemperatureDisplayMode.Celsius);
		}

		// Token: 0x06008ACB RID: 35531 RVA: 0x0005D135 File Offset: 0x0005B335
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			foreach (Gizmo gizmo in base.CompGetGizmosExtra())
			{
				yield return gizmo;
			}
			IEnumerator<Gizmo> enumerator = null;
			float offset2 = this.RoundedToCurrentTempModeOffset(-10f);
			yield return new Command_Action
			{
				action = delegate()
				{
					this.InterfaceChangeTargetTemperature(offset2);
				},
				defaultLabel = offset2.ToStringTemperatureOffset("F0"),
				defaultDesc = "CommandLowerTempDesc".Translate(),
				hotKey = KeyBindingDefOf.Misc5,
				icon = ContentFinder<Texture2D>.Get("UI/Commands/TempLower", true)
			};
			float offset3 = this.RoundedToCurrentTempModeOffset(-1f);
			yield return new Command_Action
			{
				action = delegate()
				{
					this.InterfaceChangeTargetTemperature(offset3);
				},
				defaultLabel = offset3.ToStringTemperatureOffset("F0"),
				defaultDesc = "CommandLowerTempDesc".Translate(),
				hotKey = KeyBindingDefOf.Misc4,
				icon = ContentFinder<Texture2D>.Get("UI/Commands/TempLower", true)
			};
			yield return new Command_Action
			{
				action = delegate()
				{
					this.targetTemperature = 21f;
					SoundDefOf.Tick_Tiny.PlayOneShotOnCamera(null);
					this.ThrowCurrentTemperatureText();
				},
				defaultLabel = "CommandResetTemp".Translate(),
				defaultDesc = "CommandResetTempDesc".Translate(),
				hotKey = KeyBindingDefOf.Misc1,
				icon = ContentFinder<Texture2D>.Get("UI/Commands/TempReset", true)
			};
			float offset4 = this.RoundedToCurrentTempModeOffset(1f);
			yield return new Command_Action
			{
				action = delegate()
				{
					this.InterfaceChangeTargetTemperature(offset4);
				},
				defaultLabel = "+" + offset4.ToStringTemperatureOffset("F0"),
				defaultDesc = "CommandRaiseTempDesc".Translate(),
				hotKey = KeyBindingDefOf.Misc2,
				icon = ContentFinder<Texture2D>.Get("UI/Commands/TempRaise", true)
			};
			float offset = this.RoundedToCurrentTempModeOffset(10f);
			yield return new Command_Action
			{
				action = delegate()
				{
					this.InterfaceChangeTargetTemperature(offset);
				},
				defaultLabel = "+" + offset.ToStringTemperatureOffset("F0"),
				defaultDesc = "CommandRaiseTempDesc".Translate(),
				hotKey = KeyBindingDefOf.Misc3,
				icon = ContentFinder<Texture2D>.Get("UI/Commands/TempRaise", true)
			};
			yield break;
			yield break;
		}

		// Token: 0x06008ACC RID: 35532 RVA: 0x0005D145 File Offset: 0x0005B345
		private void InterfaceChangeTargetTemperature(float offset)
		{
			SoundDefOf.DragSlider.PlayOneShotOnCamera(null);
			this.targetTemperature += offset;
			this.targetTemperature = Mathf.Clamp(this.targetTemperature, -273.15f, 1000f);
			this.ThrowCurrentTemperatureText();
		}

		// Token: 0x06008ACD RID: 35533 RVA: 0x00287D44 File Offset: 0x00285F44
		private void ThrowCurrentTemperatureText()
		{
			MoteMaker.ThrowText(this.parent.TrueCenter() + new Vector3(0.5f, 0f, 0.5f), this.parent.Map, this.targetTemperature.ToStringTemperature("F0"), Color.white, -1f);
		}

		// Token: 0x06008ACE RID: 35534 RVA: 0x00287DA0 File Offset: 0x00285FA0
		public override string CompInspectStringExtra()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("TargetTemperature".Translate() + ": ");
			stringBuilder.AppendLine(this.targetTemperature.ToStringTemperature("F0"));
			stringBuilder.Append("PowerConsumptionMode".Translate() + ": ");
			if (this.operatingAtHighPower)
			{
				stringBuilder.Append("PowerConsumptionHigh".Translate().CapitalizeFirst());
			}
			else
			{
				stringBuilder.Append("PowerConsumptionLow".Translate().CapitalizeFirst());
			}
			return stringBuilder.ToString();
		}

		// Token: 0x04005902 RID: 22786
		[Unsaved(false)]
		public bool operatingAtHighPower;

		// Token: 0x04005903 RID: 22787
		public float targetTemperature = -99999f;

		// Token: 0x04005904 RID: 22788
		private const float DefaultTargetTemperature = 21f;
	}
}
