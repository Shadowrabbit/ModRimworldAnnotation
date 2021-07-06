using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001784 RID: 6020
	public class CompCauseGameCondition_TemperatureOffset : CompCauseGameCondition
	{
		// Token: 0x1700148A RID: 5258
		// (get) Token: 0x060084C2 RID: 33986 RVA: 0x00058F17 File Offset: 0x00057117
		public new CompProperties_CausesGameCondition_ClimateAdjuster Props
		{
			get
			{
				return (CompProperties_CausesGameCondition_ClimateAdjuster)this.props;
			}
		}

		// Token: 0x060084C3 RID: 33987 RVA: 0x00058F24 File Offset: 0x00057124
		public override void Initialize(CompProperties props)
		{
			base.Initialize(props);
			this.temperatureOffset = this.Props.temperatureOffsetRange.min;
		}

		// Token: 0x060084C4 RID: 33988 RVA: 0x00058F43 File Offset: 0x00057143
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<float>(ref this.temperatureOffset, "temperatureOffset", 0f, false);
		}

		// Token: 0x060084C5 RID: 33989 RVA: 0x00058F61 File Offset: 0x00057161
		private string GetFloatStringWithSign(float val)
		{
			if (val < 0f)
			{
				return val.ToString("0");
			}
			return "+" + val.ToString("0");
		}

		// Token: 0x060084C6 RID: 33990 RVA: 0x00058F8E File Offset: 0x0005718E
		public void SetTemperatureOffset(float offset)
		{
			this.temperatureOffset = this.Props.temperatureOffsetRange.ClampToRange(offset);
			base.ReSetupAllConditions();
		}

		// Token: 0x060084C7 RID: 33991 RVA: 0x00058FAD File Offset: 0x000571AD
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			if (!Prefs.DevMode)
			{
				yield break;
			}
			Command_Action command_Action = new Command_Action();
			command_Action.defaultLabel = "-10";
			Command_Action command_Action2 = command_Action;
			command_Action2.action = (Action)Delegate.Combine(command_Action2.action, new Action(delegate()
			{
				this.SetTemperatureOffset(this.temperatureOffset - 10f);
			}));
			command_Action.hotKey = KeyBindingDefOf.Misc1;
			yield return command_Action;
			Command_Action command_Action3 = new Command_Action();
			command_Action3.defaultLabel = "-1";
			Command_Action command_Action4 = command_Action3;
			command_Action4.action = (Action)Delegate.Combine(command_Action4.action, new Action(delegate()
			{
				this.SetTemperatureOffset(this.temperatureOffset - 1f);
			}));
			command_Action3.hotKey = KeyBindingDefOf.Misc2;
			yield return command_Action3;
			Command_Action command_Action5 = new Command_Action();
			command_Action5.defaultLabel = "+1";
			Command_Action command_Action6 = command_Action5;
			command_Action6.action = (Action)Delegate.Combine(command_Action6.action, new Action(delegate()
			{
				this.SetTemperatureOffset(this.temperatureOffset + 1f);
			}));
			command_Action5.hotKey = KeyBindingDefOf.Misc3;
			yield return command_Action5;
			Command_Action command_Action7 = new Command_Action();
			command_Action7.defaultLabel = "+10";
			Command_Action command_Action8 = command_Action7;
			command_Action8.action = (Action)Delegate.Combine(command_Action8.action, new Action(delegate()
			{
				this.SetTemperatureOffset(this.temperatureOffset + 10f);
			}));
			command_Action7.hotKey = KeyBindingDefOf.Misc4;
			yield return command_Action7;
			yield break;
		}

		// Token: 0x060084C8 RID: 33992 RVA: 0x002746A4 File Offset: 0x002728A4
		public override string CompInspectStringExtra()
		{
			string text = base.CompInspectStringExtra();
			if (!text.NullOrEmpty())
			{
				text += "\n";
			}
			return text + ("Temperature".Translate() + ": " + this.GetFloatStringWithSign(this.temperatureOffset));
		}

		// Token: 0x060084C9 RID: 33993 RVA: 0x00058FBD File Offset: 0x000571BD
		protected override void SetupCondition(GameCondition condition, Map map)
		{
			base.SetupCondition(condition, map);
			((GameCondition_TemperatureOffset)condition).tempOffset = this.temperatureOffset;
		}

		// Token: 0x060084CA RID: 33994 RVA: 0x00274700 File Offset: 0x00272900
		public override void RandomizeSettings_NewTemp_NewTemp(Site site)
		{
			bool flag = false;
			bool flag2 = false;
			foreach (WorldObject worldObject in Find.WorldObjects.AllWorldObjects)
			{
				Settlement settlement;
				if ((settlement = (worldObject as Settlement)) != null && settlement.Faction == Faction.OfPlayer)
				{
					if (settlement.Map != null)
					{
						bool flag3 = false;
						foreach (GameCondition gameCondition in settlement.Map.GameConditionManager.ActiveConditions)
						{
							if (gameCondition is GameCondition_TemperatureOffset)
							{
								float num = gameCondition.TemperatureOffset();
								if (num > 0f)
								{
									flag3 = true;
									flag = true;
									flag2 = false;
								}
								else if (num < 0f)
								{
									flag3 = true;
									flag2 = true;
									flag = false;
								}
								if (flag3)
								{
									break;
								}
							}
						}
						if (flag3)
						{
							break;
						}
					}
					int tile = worldObject.Tile;
					if ((float)Find.WorldGrid.TraversalDistanceBetween(site.Tile, tile, true, this.Props.worldRange + 1) <= (float)this.Props.worldRange)
					{
						float num2 = GenTemperature.MinTemperatureAtTile(tile);
						float num3 = GenTemperature.MaxTemperatureAtTile(tile);
						if (num2 < -5f)
						{
							flag2 = true;
						}
						if (num3 > 20f)
						{
							flag = true;
						}
					}
				}
			}
			if (flag2 == flag)
			{
				this.temperatureOffset = (Rand.Bool ? this.Props.temperatureOffsetRange.min : this.Props.temperatureOffsetRange.max);
				return;
			}
			if (flag2)
			{
				this.temperatureOffset = this.Props.temperatureOffsetRange.min;
				return;
			}
			if (flag)
			{
				this.temperatureOffset = this.Props.temperatureOffsetRange.max;
			}
		}

		// Token: 0x040055F4 RID: 22004
		public float temperatureOffset;

		// Token: 0x040055F5 RID: 22005
		private const float MaxTempForMinOffset = -5f;

		// Token: 0x040055F6 RID: 22006
		private const float MinTempForMaxOffset = 20f;
	}
}
