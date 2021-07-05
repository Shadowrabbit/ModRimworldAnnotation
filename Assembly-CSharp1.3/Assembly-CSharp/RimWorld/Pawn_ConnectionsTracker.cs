using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E68 RID: 3688
	public class Pawn_ConnectionsTracker : IExposable
	{
		// Token: 0x17000EC4 RID: 3780
		// (get) Token: 0x0600558E RID: 21902 RVA: 0x001CFA4C File Offset: 0x001CDC4C
		public List<Thing> ConnectedThings
		{
			get
			{
				return this.connectedThings;
			}
		}

		// Token: 0x0600558F RID: 21903 RVA: 0x001CFA54 File Offset: 0x001CDC54
		public Pawn_ConnectionsTracker()
		{
		}

		// Token: 0x06005590 RID: 21904 RVA: 0x001CFA67 File Offset: 0x001CDC67
		public Pawn_ConnectionsTracker(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x06005591 RID: 21905 RVA: 0x001CFA81 File Offset: 0x001CDC81
		public void ConnectTo(Thing thing)
		{
			this.connectedThings.Add(thing);
		}

		// Token: 0x06005592 RID: 21906 RVA: 0x001CFA90 File Offset: 0x001CDC90
		public void Notify_PawnKilled()
		{
			for (int i = this.connectedThings.Count - 1; i >= 0; i--)
			{
				CompTreeConnection compTreeConnection = this.connectedThings[i].TryGetComp<CompTreeConnection>();
				if (compTreeConnection != null)
				{
					compTreeConnection.Notify_PawnDied(this.pawn);
				}
				this.connectedThings.RemoveAt(i);
			}
		}

		// Token: 0x06005593 RID: 21907 RVA: 0x001CFAE4 File Offset: 0x001CDCE4
		public void Notify_ConnectedThingDestroyed(Thing thing)
		{
			if (this.connectedThings.Remove(thing))
			{
				Pawn_NeedsTracker needs = this.pawn.needs;
				bool flag;
				if (needs == null)
				{
					flag = (null != null);
				}
				else
				{
					Need_Mood mood = needs.mood;
					if (mood == null)
					{
						flag = (null != null);
					}
					else
					{
						ThoughtHandler thoughts = mood.thoughts;
						flag = (((thoughts != null) ? thoughts.memories : null) != null);
					}
				}
				if (flag)
				{
					this.pawn.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.ConnectedTreeDied, null, null);
				}
			}
		}

		// Token: 0x06005594 RID: 21908 RVA: 0x001CFB56 File Offset: 0x001CDD56
		public IEnumerable<Gizmo> GetGizmos()
		{
			if (this.pawn.IsColonistPlayerControlled && Find.Selector.SingleSelectedThing == this.pawn)
			{
				if (!this.connectedThings.Any<Thing>())
				{
					yield break;
				}
				yield return new Command_Action
				{
					defaultLabel = "CommandSelectConnectedTree".Translate(),
					onHover = new Action(this.DrawConnectionLines),
					icon = Widgets.GetIconFor(ThingDefOf.Plant_TreeGauranlen, null, null),
					iconDrawScale = ThingDefOf.Plant_TreeGauranlen.uiIconScale,
					action = delegate()
					{
						if (this.ConnectedThings.Count == 1)
						{
							CameraJumper.TryJumpAndSelect(this.ConnectedThings[0]);
							return;
						}
						if (this.ConnectedThings.Count > 1)
						{
							List<FloatMenuOption> list = new List<FloatMenuOption>();
							for (int i = 0; i < this.ConnectedThings.Count; i++)
							{
								Thing t = this.ConnectedThings[i];
								string str = "NoCaste".Translate();
								CompTreeConnection compTreeConnection = t.TryGetComp<CompTreeConnection>();
								if (compTreeConnection != null && compTreeConnection.Mode != null)
								{
									str = compTreeConnection.Mode.label;
								}
								list.Add(new FloatMenuOption(t.LabelCap + " (" + str + ")", delegate()
								{
									CameraJumper.TryJumpAndSelect(t);
								}, t.def, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
							}
							if (list.Any<FloatMenuOption>())
							{
								Find.WindowStack.Add(new FloatMenu(list));
							}
						}
					}
				};
			}
			yield break;
		}

		// Token: 0x06005595 RID: 21909 RVA: 0x001CFB68 File Offset: 0x001CDD68
		private void DrawConnectionLines()
		{
			foreach (Thing t in this.ConnectedThings)
			{
				this.DrawConnectionLine(t);
			}
		}

		// Token: 0x06005596 RID: 21910 RVA: 0x001CFBBC File Offset: 0x001CDDBC
		public void DrawConnectionLine(Thing t)
		{
			if (t.Spawned && t.Map == this.pawn.Map)
			{
				GenDraw.DrawLineBetween(this.pawn.TrueCenter(), t.TrueCenter(), SimpleColor.Orange, 0.2f);
			}
		}

		// Token: 0x06005597 RID: 21911 RVA: 0x001CFBF8 File Offset: 0x001CDDF8
		public void ExposeData()
		{
			Scribe_Collections.Look<Thing>(ref this.connectedThings, "connectedThings", LookMode.Reference, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.connectedThings.RemoveAll((Thing x) => x == null || x.Destroyed);
			}
		}

		// Token: 0x0400329C RID: 12956
		private Pawn pawn;

		// Token: 0x0400329D RID: 12957
		private List<Thing> connectedThings = new List<Thing>();
	}
}
