using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02002199 RID: 8601
	[StaticConstructorOnStartup]
	public class FormCaravanComp : WorldObjectComp
	{
		// Token: 0x17001B36 RID: 6966
		// (get) Token: 0x0600B7AE RID: 47022 RVA: 0x000771CF File Offset: 0x000753CF
		public WorldObjectCompProperties_FormCaravan Props
		{
			get
			{
				return (WorldObjectCompProperties_FormCaravan)this.props;
			}
		}

		// Token: 0x17001B37 RID: 6967
		// (get) Token: 0x0600B7AF RID: 47023 RVA: 0x000771DC File Offset: 0x000753DC
		private MapParent MapParent
		{
			get
			{
				return (MapParent)this.parent;
			}
		}

		// Token: 0x17001B38 RID: 6968
		// (get) Token: 0x0600B7B0 RID: 47024 RVA: 0x000771E9 File Offset: 0x000753E9
		public bool Reform
		{
			get
			{
				return !this.MapParent.HasMap || !this.MapParent.Map.IsPlayerHome;
			}
		}

		// Token: 0x17001B39 RID: 6969
		// (get) Token: 0x0600B7B1 RID: 47025 RVA: 0x0034F214 File Offset: 0x0034D414
		public bool CanFormOrReformCaravanNow
		{
			get
			{
				MapParent mapParent = this.MapParent;
				return mapParent.HasMap && (!this.Reform || (!GenHostility.AnyHostileActiveThreatToPlayer(mapParent.Map, true) && mapParent.Map.mapPawns.FreeColonistsSpawnedCount != 0));
			}
		}

		// Token: 0x0600B7B2 RID: 47026 RVA: 0x0007720D File Offset: 0x0007540D
		public override IEnumerable<Gizmo> GetGizmos()
		{
			MapParent mapParent = (MapParent)this.parent;
			if (mapParent.HasMap)
			{
				if (!this.Reform)
				{
					yield return new Command_Action
					{
						defaultLabel = "CommandFormCaravan".Translate(),
						defaultDesc = "CommandFormCaravanDesc".Translate(),
						icon = FormCaravanComp.FormCaravanCommand,
						hotKey = KeyBindingDefOf.Misc2,
						tutorTag = "FormCaravan",
						action = delegate()
						{
							Find.WindowStack.Add(new Dialog_FormCaravan(mapParent.Map, false, null, false));
						}
					};
				}
				else if (mapParent.Map.mapPawns.FreeColonistsSpawnedCount != 0)
				{
					Command_Action command_Action = new Command_Action();
					command_Action.defaultLabel = "CommandReformCaravan".Translate();
					command_Action.defaultDesc = "CommandReformCaravanDesc".Translate();
					command_Action.icon = FormCaravanComp.FormCaravanCommand;
					command_Action.hotKey = KeyBindingDefOf.Misc2;
					command_Action.tutorTag = "ReformCaravan";
					command_Action.action = delegate()
					{
						Find.WindowStack.Add(new Dialog_FormCaravan(mapParent.Map, true, null, false));
					};
					if (GenHostility.AnyHostileActiveThreatToPlayer(mapParent.Map, true))
					{
						command_Action.Disable("CommandReformCaravanFailHostilePawns".Translate());
					}
					yield return command_Action;
				}
				if (Prefs.DevMode)
				{
					yield return new Command_Action
					{
						defaultLabel = "Dev: Show available exits",
						action = delegate()
						{
							foreach (int tile in CaravanExitMapUtility.AvailableExitTilesAt(mapParent.Map))
							{
								Find.WorldDebugDrawer.FlashTile(tile, 0f, null, 10);
							}
						}
					};
				}
			}
			yield break;
		}

		// Token: 0x04007DA0 RID: 32160
		public static readonly Texture2D FormCaravanCommand = ContentFinder<Texture2D>.Get("UI/Commands/FormCaravan", true);
	}
}
