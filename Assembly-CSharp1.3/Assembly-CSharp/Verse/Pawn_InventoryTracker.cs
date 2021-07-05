using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020002F2 RID: 754
	[StaticConstructorOnStartup]
	public class Pawn_InventoryTracker : IThingHolder, IExposable
	{
		// Token: 0x1700049D RID: 1181
		// (get) Token: 0x060015D0 RID: 5584 RVA: 0x0007F46E File Offset: 0x0007D66E
		// (set) Token: 0x060015D1 RID: 5585 RVA: 0x0007F480 File Offset: 0x0007D680
		public bool UnloadEverything
		{
			get
			{
				return this.unloadEverything && this.HasAnyUnloadableThing;
			}
			set
			{
				if (value && this.HasAnyUnloadableThing)
				{
					this.unloadEverything = true;
					return;
				}
				this.unloadEverything = false;
			}
		}

		// Token: 0x1700049E RID: 1182
		// (get) Token: 0x060015D2 RID: 5586 RVA: 0x0007F49C File Offset: 0x0007D69C
		private bool HasAnyUnloadableThing
		{
			get
			{
				return this.FirstUnloadableThing != default(ThingCount);
			}
		}

		// Token: 0x1700049F RID: 1183
		// (get) Token: 0x060015D3 RID: 5587 RVA: 0x0007F4C0 File Offset: 0x0007D6C0
		public ThingCount FirstUnloadableThing
		{
			get
			{
				if (this.innerContainer.Count == 0)
				{
					return default(ThingCount);
				}
				if (this.pawn.drugs != null && this.pawn.drugs.CurrentPolicy != null)
				{
					DrugPolicy currentPolicy = this.pawn.drugs.CurrentPolicy;
					Pawn_InventoryTracker.tmpDrugsToKeep.Clear();
					for (int i = 0; i < currentPolicy.Count; i++)
					{
						if (currentPolicy[i].takeToInventory > 0)
						{
							Pawn_InventoryTracker.tmpDrugsToKeep.Add(new ThingDefCount(currentPolicy[i].drug, currentPolicy[i].takeToInventory));
						}
					}
					for (int j = 0; j < this.innerContainer.Count; j++)
					{
						if (!this.innerContainer[j].def.IsDrug)
						{
							return new ThingCount(this.innerContainer[j], this.innerContainer[j].stackCount);
						}
						int num = -1;
						for (int k = 0; k < Pawn_InventoryTracker.tmpDrugsToKeep.Count; k++)
						{
							if (this.innerContainer[j].def == Pawn_InventoryTracker.tmpDrugsToKeep[k].ThingDef)
							{
								num = k;
								break;
							}
						}
						if (num < 0)
						{
							return new ThingCount(this.innerContainer[j], this.innerContainer[j].stackCount);
						}
						if (this.innerContainer[j].stackCount > Pawn_InventoryTracker.tmpDrugsToKeep[num].Count)
						{
							return new ThingCount(this.innerContainer[j], this.innerContainer[j].stackCount - Pawn_InventoryTracker.tmpDrugsToKeep[num].Count);
						}
						Pawn_InventoryTracker.tmpDrugsToKeep[num] = new ThingDefCount(Pawn_InventoryTracker.tmpDrugsToKeep[num].ThingDef, Pawn_InventoryTracker.tmpDrugsToKeep[num].Count - this.innerContainer[j].stackCount);
					}
					return default(ThingCount);
				}
				return new ThingCount(this.innerContainer[0], this.innerContainer[0].stackCount);
			}
		}

		// Token: 0x170004A0 RID: 1184
		// (get) Token: 0x060015D4 RID: 5588 RVA: 0x0007F70E File Offset: 0x0007D90E
		public IThingHolder ParentHolder
		{
			get
			{
				return this.pawn;
			}
		}

		// Token: 0x060015D5 RID: 5589 RVA: 0x0007F716 File Offset: 0x0007D916
		public Pawn_InventoryTracker(Pawn pawn)
		{
			this.pawn = pawn;
			this.innerContainer = new ThingOwner<Thing>(this, false, LookMode.Deep);
		}

		// Token: 0x060015D6 RID: 5590 RVA: 0x0007F74C File Offset: 0x0007D94C
		public void ExposeData()
		{
			Scribe_Collections.Look<Thing>(ref this.itemsNotForSale, "itemsNotForSale", LookMode.Reference, Array.Empty<object>());
			Scribe_Deep.Look<ThingOwner<Thing>>(ref this.innerContainer, "innerContainer", new object[]
			{
				this
			});
			Scribe_Values.Look<bool>(ref this.unloadEverything, "unloadEverything", false, false);
		}

		// Token: 0x060015D7 RID: 5591 RVA: 0x0007F79B File Offset: 0x0007D99B
		public void InventoryTrackerTick()
		{
			this.innerContainer.ThingOwnerTick(true);
			if (this.unloadEverything && !this.HasAnyUnloadableThing)
			{
				this.unloadEverything = false;
			}
		}

		// Token: 0x060015D8 RID: 5592 RVA: 0x0007F7C0 File Offset: 0x0007D9C0
		public void InventoryTrackerTickRare()
		{
			this.innerContainer.ThingOwnerTickRare(true);
		}

		// Token: 0x060015D9 RID: 5593 RVA: 0x0007F7D0 File Offset: 0x0007D9D0
		public void DropAllNearPawn(IntVec3 pos, bool forbid = false, bool unforbid = false)
		{
			if (this.pawn.MapHeld == null)
			{
				Log.Error("Tried to drop all inventory near pawn but the pawn is unspawned. pawn=" + this.pawn);
				return;
			}
			Pawn_InventoryTracker.tmpThingList.Clear();
			Pawn_InventoryTracker.tmpThingList.AddRange(this.innerContainer);
			Action<Thing, int> <>9__0;
			for (int i = 0; i < Pawn_InventoryTracker.tmpThingList.Count; i++)
			{
				ThingOwner<Thing> thingOwner = this.innerContainer;
				Thing thing = Pawn_InventoryTracker.tmpThingList[i];
				Map mapHeld = this.pawn.MapHeld;
				ThingPlaceMode mode = ThingPlaceMode.Near;
				Action<Thing, int> placedAction;
				if ((placedAction = <>9__0) == null)
				{
					placedAction = (<>9__0 = delegate(Thing t, int unused)
					{
						if (forbid)
						{
							t.SetForbiddenIfOutsideHomeArea();
						}
						if (unforbid)
						{
							t.SetForbidden(false, false);
						}
						if (t.def.IsPleasureDrug)
						{
							LessonAutoActivator.TeachOpportunity(ConceptDefOf.DrugBurning, OpportunityType.Important);
						}
					});
				}
				Thing thing2;
				thingOwner.TryDrop(thing, pos, mapHeld, mode, out thing2, placedAction, null);
			}
		}

		// Token: 0x060015DA RID: 5594 RVA: 0x0007F88C File Offset: 0x0007DA8C
		public void DropCount(ThingDef def, int count, bool forbid = false, bool unforbid = false)
		{
			if (this.pawn.MapHeld == null)
			{
				Log.Error("Tried to drop a thing near pawn but the pawn is unspawned. pawn=" + this.pawn);
				return;
			}
			Pawn_InventoryTracker.tmpThingList.Clear();
			Pawn_InventoryTracker.tmpThingList.AddRange(this.innerContainer);
			int num = 0;
			Action<Thing, int> <>9__0;
			for (int i = 0; i < Pawn_InventoryTracker.tmpThingList.Count; i++)
			{
				Thing thing = Pawn_InventoryTracker.tmpThingList[i];
				if (thing.def == def)
				{
					int num2 = Math.Min(thing.stackCount, count);
					ThingOwner<Thing> thingOwner = this.innerContainer;
					Thing thing2 = Pawn_InventoryTracker.tmpThingList[i];
					IntVec3 position = this.pawn.Position;
					Map mapHeld = this.pawn.MapHeld;
					ThingPlaceMode mode = ThingPlaceMode.Near;
					int count2 = num2;
					Action<Thing, int> placedAction;
					if ((placedAction = <>9__0) == null)
					{
						placedAction = (<>9__0 = delegate(Thing t, int unused)
						{
							if (forbid)
							{
								t.SetForbiddenIfOutsideHomeArea();
							}
							if (unforbid)
							{
								t.SetForbidden(false, false);
							}
							if (t.def.IsPleasureDrug)
							{
								LessonAutoActivator.TeachOpportunity(ConceptDefOf.DrugBurning, OpportunityType.Important);
							}
						});
					}
					Thing thing3;
					thingOwner.TryDrop(thing2, position, mapHeld, mode, count2, out thing3, placedAction, null);
					num += num2;
					if (num >= count)
					{
						break;
					}
				}
			}
		}

		// Token: 0x060015DB RID: 5595 RVA: 0x0007F988 File Offset: 0x0007DB88
		public void RemoveCount(ThingDef def, int count, bool destroy = true)
		{
			Pawn_InventoryTracker.tmpThingList.Clear();
			Pawn_InventoryTracker.tmpThingList.AddRange(this.innerContainer);
			foreach (Thing thing in Pawn_InventoryTracker.tmpThingList)
			{
				if (thing.def == def)
				{
					if (thing.stackCount > count)
					{
						thing.stackCount -= count;
						break;
					}
					this.innerContainer.Remove(thing);
					if (destroy)
					{
						thing.Destroy(DestroyMode.Vanish);
						break;
					}
					break;
				}
			}
		}

		// Token: 0x060015DC RID: 5596 RVA: 0x0007FA28 File Offset: 0x0007DC28
		public void DestroyAll(DestroyMode mode = DestroyMode.Vanish)
		{
			this.innerContainer.ClearAndDestroyContents(mode);
		}

		// Token: 0x060015DD RID: 5597 RVA: 0x0007FA36 File Offset: 0x0007DC36
		public bool Contains(Thing item)
		{
			return this.innerContainer.Contains(item);
		}

		// Token: 0x060015DE RID: 5598 RVA: 0x0007FA44 File Offset: 0x0007DC44
		public int Count(ThingDef def)
		{
			int num = 0;
			foreach (Thing thing in this.innerContainer)
			{
				if (thing.def == def)
				{
					num += thing.stackCount;
				}
			}
			return num;
		}

		// Token: 0x060015DF RID: 5599 RVA: 0x0007FAA8 File Offset: 0x0007DCA8
		public int Count(Func<Thing, bool> validator)
		{
			int num = 0;
			foreach (Thing thing in this.innerContainer)
			{
				if (validator(thing))
				{
					num += thing.stackCount;
				}
			}
			return num;
		}

		// Token: 0x060015E0 RID: 5600 RVA: 0x0007FB0C File Offset: 0x0007DD0C
		public bool NotForSale(Thing item)
		{
			return this.itemsNotForSale.Contains(item);
		}

		// Token: 0x060015E1 RID: 5601 RVA: 0x0007FB1A File Offset: 0x0007DD1A
		public void TryAddItemNotForSale(Thing item)
		{
			if (this.innerContainer.TryAdd(item, false))
			{
				this.itemsNotForSale.Add(item);
			}
		}

		// Token: 0x060015E2 RID: 5602 RVA: 0x0007FB37 File Offset: 0x0007DD37
		public void Notify_ItemRemoved(Thing item)
		{
			this.itemsNotForSale.Remove(item);
			if (this.unloadEverything && !this.HasAnyUnloadableThing)
			{
				this.unloadEverything = false;
			}
		}

		// Token: 0x060015E3 RID: 5603 RVA: 0x0007FB5D File Offset: 0x0007DD5D
		public ThingOwner GetDirectlyHeldThings()
		{
			return this.innerContainer;
		}

		// Token: 0x060015E4 RID: 5604 RVA: 0x0007FB65 File Offset: 0x0007DD65
		public void GetChildHolders(List<IThingHolder> outChildren)
		{
			ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, this.GetDirectlyHeldThings());
		}

		// Token: 0x060015E5 RID: 5605 RVA: 0x0007FB73 File Offset: 0x0007DD73
		public IEnumerable<Thing> GetDrugs()
		{
			foreach (Thing thing in this.innerContainer)
			{
				if (thing.TryGetComp<CompDrug>() != null)
				{
					yield return thing;
				}
			}
			List<Thing>.Enumerator enumerator = default(List<Thing>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x060015E6 RID: 5606 RVA: 0x0007FB83 File Offset: 0x0007DD83
		public IEnumerable<Thing> GetCombatEnhancingDrugs()
		{
			foreach (Thing thing in this.innerContainer)
			{
				CompDrug compDrug = thing.TryGetComp<CompDrug>();
				if (compDrug != null && compDrug.Props.isCombatEnhancingDrug)
				{
					yield return thing;
				}
			}
			List<Thing>.Enumerator enumerator = default(List<Thing>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x060015E7 RID: 5607 RVA: 0x0007FB93 File Offset: 0x0007DD93
		public Thing FindCombatEnhancingDrug()
		{
			return this.GetCombatEnhancingDrugs().FirstOrDefault<Thing>();
		}

		// Token: 0x060015E8 RID: 5608 RVA: 0x0007FBA0 File Offset: 0x0007DDA0
		public IEnumerable<Gizmo> GetGizmos()
		{
			if (this.pawn.IsColonistPlayerControlled && this.pawn.Drafted && Find.Selector.SingleSelectedThing == this.pawn && !this.pawn.IsTeetotaler())
			{
				this.usableDrugsTmp.Clear();
				foreach (Thing thing in this.GetDrugs())
				{
					if (FoodUtility.WillIngestFromInventoryNow(this.pawn, thing))
					{
						this.usableDrugsTmp.Add(thing);
					}
				}
				if (this.usableDrugsTmp.Count == 0)
				{
					yield break;
				}
				if (this.usableDrugsTmp.Count == 1)
				{
					Thing drug = this.usableDrugsTmp[0];
					yield return new Command_Action
					{
						defaultLabel = "ConsumeThing".Translate(drug.LabelNoCount, drug),
						defaultDesc = drug.LabelCapNoCount + ": " + drug.def.description.CapitalizeFirst(),
						icon = drug.def.uiIcon,
						iconAngle = drug.def.uiIconAngle,
						iconOffset = drug.def.uiIconOffset,
						action = delegate()
						{
							FoodUtility.IngestFromInventoryNow(this.pawn, drug);
						}
					};
				}
				else
				{
					yield return new Command_Action
					{
						defaultLabel = "TakeDrug".Translate(),
						defaultDesc = "TakeDrugDesc".Translate(),
						icon = Pawn_InventoryTracker.DrugTex,
						action = delegate()
						{
							List<FloatMenuOption> list = new List<FloatMenuOption>();
							using (List<Thing>.Enumerator enumerator2 = this.usableDrugsTmp.GetEnumerator())
							{
								while (enumerator2.MoveNext())
								{
									Thing drug = enumerator2.Current;
									list.Add(new FloatMenuOption("ConsumeThing".Translate(drug.LabelNoCount, drug), delegate()
									{
										FoodUtility.IngestFromInventoryNow(this.pawn, drug);
									}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
								}
							}
							Find.WindowStack.Add(new FloatMenu(list));
						}
					};
				}
			}
			yield break;
		}

		// Token: 0x04000F38 RID: 3896
		public Pawn pawn;

		// Token: 0x04000F39 RID: 3897
		public ThingOwner<Thing> innerContainer;

		// Token: 0x04000F3A RID: 3898
		private bool unloadEverything;

		// Token: 0x04000F3B RID: 3899
		private List<Thing> itemsNotForSale = new List<Thing>();

		// Token: 0x04000F3C RID: 3900
		public static readonly Texture2D DrugTex = ContentFinder<Texture2D>.Get("UI/Commands/TakeDrug", true);

		// Token: 0x04000F3D RID: 3901
		private static List<ThingDefCount> tmpDrugsToKeep = new List<ThingDefCount>();

		// Token: 0x04000F3E RID: 3902
		private static List<Thing> tmpThingList = new List<Thing>();

		// Token: 0x04000F3F RID: 3903
		private List<Thing> usableDrugsTmp = new List<Thing>();
	}
}
