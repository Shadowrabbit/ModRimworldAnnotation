// Decompiled with JetBrains decompiler
// Type: RimWorld.Planet.Site
// Assembly: Assembly-CSharp, Version=1.2.7705.25110, Culture=neutral, PublicKeyToken=null
// MVID: C36F9493-C984-4DDA-A7FB-5C788416098F
// Assembly location: E:\Program Files (x86)\Steam\steamapps\common\RimWorld\Mods\ModRimworldFactionalWar\Source\ModRimworldFactionalWar\Lib\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
    //Site由N个SitePart组成，每个SitePart上有N个StepDef，每个StepDef对应N个symbol；symbol与ruleDef1:1对应
    //地图生成找到site中对应的按顺序依次创建sitePart，最终由ruleDef决定生成的内容
    [StaticConstructorOnStartup]
    public class Site : MapParent
    {
        public string customLabel;
        public List<SitePart> parts = new List<SitePart>();
        public bool sitePartsKnown = true;
        public bool factionMustRemainHostile;
        public float desiredThreatPoints;
        private SiteCoreBackCompat coreBackCompat;
        private bool anyEnemiesInitially;
        private bool caravanAssaultSuccessfulTaleRecorded;
        private bool allEnemiesDefeatedSignalSent;
        private Material cachedMat;
        private static List<string> tmpSitePartsLabels = new List<string>();

        public override string Label
        {
            get
            {
                if (!this.customLabel.NullOrEmpty())
                    return this.customLabel;
                return this.MainSitePartDef == SitePartDefOf.PreciousLump && this.MainSitePart.parms.preciousLumpResources != null
                    ? (string)"PreciousLumpLabel".Translate((NamedArgument)this.MainSitePart.parms.preciousLumpResources.label)
                    : this.MainSitePartDef.label;
            }
        }

        public override Texture2D ExpandingIcon => this.MainSitePartDef.ExpandingIconTexture;

        public override bool HandlesConditionCausers => true;

        public override Material Material
        {
            get
            {
                if ((UnityEngine.Object)this.cachedMat == (UnityEngine.Object)null)
                {
                    Color color = !this.MainSitePartDef.applyFactionColorToSiteTexture || this.Faction == null
                        ? Color.white
                        : this.Faction.Color;
                    this.cachedMat = MaterialPool.MatFrom(this.MainSitePartDef.siteTexture, ShaderDatabase.WorldOverlayTransparentLit,
                        color, WorldMaterials.WorldObjectRenderQueue);
                }
                return this.cachedMat;
            }
        }

        public override bool AppendFactionToInspectString =>
            this.MainSitePartDef.applyFactionColorToSiteTexture || this.MainSitePartDef.showFactionInInspectString;

        private SitePart MainSitePart
        {
            get
            {
                if (!this.parts.Any<SitePart>())
                {
                    Log.ErrorOnce("Site without any SitePart at " + (object)this.Tile, this.ID ^ 93890909);
                    return (SitePart)null;
                }
                if (!this.parts[0].hidden)
                    return this.parts[0];
                Log.ErrorOnce("Site with first SitePart hidden at " + (object)this.Tile, this.ID ^ 48471239);
                return this.parts[0];
            }
        }

        public SitePartDef MainSitePartDef => this.MainSitePart.def;

        public override IEnumerable<GenStepWithParams> ExtraGenStepDefs
        {
            get
            {
                foreach (GenStepWithParams extraGenStepDef in base.ExtraGenStepDefs)
                    yield return extraGenStepDef;
                for (int i = 0; i < this.parts.Count; ++i)
                {
                    GenStepParams partGenStepParams = new GenStepParams();
                    partGenStepParams.sitePart = this.parts[i];
                    List<GenStepDef> partGenStepDefs = this.parts[i].def.ExtraGenSteps;
                    for (int j = 0; j < partGenStepDefs.Count; ++j)
                        yield return new GenStepWithParams(partGenStepDefs[j], partGenStepParams);
                    partGenStepParams = new GenStepParams();
                    partGenStepDefs = (List<GenStepDef>)null;
                }
            }
        }

        public string ApproachOrderString => (string)(this.MainSitePartDef.approachOrderString.NullOrEmpty()
            ? "ApproachSite".Translate((NamedArgument)this.Label)
            : this.MainSitePartDef.approachOrderString.Formatted((NamedArgument)this.Label));

        public string ApproachingReportString => (string)(this.MainSitePartDef.approachingReportString.NullOrEmpty()
            ? "ApproachingSite".Translate((NamedArgument)this.Label)
            : this.MainSitePartDef.approachingReportString.Formatted((NamedArgument)this.Label));

        public float ActualThreatPoints
        {
            get
            {
                float num = 0.0f;
                for (int index = 0; index < this.parts.Count; ++index)
                    num += this.parts[index].parms.threatPoints;
                return num;
            }
        }

        public bool IncreasesPopulation
        {
            get
            {
                if (this.HasMap)
                    return false;
                for (int index = 0; index < this.parts.Count; ++index)
                {
                    if (this.parts[index].def.Worker.IncreasesPopulation(this.parts[index].parms))
                        return true;
                }
                return false;
            }
        }

        public bool BadEvenIfNoMap
        {
            get
            {
                for (int index = 0; index < this.parts.Count; ++index)
                {
                    if (this.parts[index].def.badEvenIfNoMap)
                        return true;
                }
                return false;
            }
        }

        public bool HasWorldObjectTimeout => this.WorldObjectTimeoutTicksLeft != -1;

        public int WorldObjectTimeoutTicksLeft
        {
            get
            {
                List<Quest> questsListForReading = Find.QuestManager.QuestsListForReading;
                for (int index1 = 0; index1 < questsListForReading.Count; ++index1)
                {
                    Quest quest = questsListForReading[index1];
                    if (quest.State == QuestState.Ongoing)
                    {
                        for (int index2 = 0; index2 < quest.PartsListForReading.Count; ++index2)
                        {
                            if (quest.PartsListForReading[index2] is QuestPart_WorldObjectTimeout worldObjectTimeout &&
                                worldObjectTimeout.State == QuestPartState.Enabled && worldObjectTimeout.worldObject == this)
                                return worldObjectTimeout.TicksLeft;
                        }
                    }
                }
                return -1;
            }
        }

        public override void Destroy()
        {
            base.Destroy();
            for (int index = 0; index < this.parts.Count; ++index)
                this.parts[index].def.Worker.PostDestroy(this.parts[index]);
            for (int index = 0; index < this.parts.Count; ++index)
                this.parts[index].PostDestroy();
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look<string>(ref this.customLabel, "customLabel");
            Scribe_Deep.Look<SiteCoreBackCompat>(ref this.coreBackCompat, "core");
            Scribe_Collections.Look<SitePart>(ref this.parts, "parts", LookMode.Deep);
            Scribe_Values.Look<bool>(ref this.anyEnemiesInitially, "anyEnemiesInitially");
            Scribe_Values.Look<bool>(ref this.caravanAssaultSuccessfulTaleRecorded, "caravanAssaultSuccessfulTaleRecorded");
            Scribe_Values.Look<bool>(ref this.allEnemiesDefeatedSignalSent, "allEnemiesDefeatedSignalSent");
            Scribe_Values.Look<bool>(ref this.factionMustRemainHostile, "factionMustRemainHostile");
            Scribe_Values.Look<float>(ref this.desiredThreatPoints, "desiredThreatPoints");
            if (Scribe.mode != LoadSaveMode.PostLoadInit)
                return;
            if (this.coreBackCompat != null && this.coreBackCompat.def != null)
            {
                this.parts.Insert(0, new SitePart(this, this.coreBackCompat.def, this.coreBackCompat.parms));
                this.coreBackCompat = (SiteCoreBackCompat)null;
            }
            if (this.parts.RemoveAll((Predicate<SitePart>)(x => x == null || x.def == null)) != 0)
                Log.Error("Some site parts were null after loading.");
            for (int index = 0; index < this.parts.Count; ++index)
                this.parts[index].site = this;
            BackCompatibility.PostExposeData((object)this);
        }

        public void AddPart(SitePart part)
        {
            this.parts.Add(part);
            part.def.Worker.Init(this, part);
        }

        public override void Tick()
        {
            base.Tick();
            for (int index = 0; index < this.parts.Count; ++index)
                this.parts[index].SitePartTick();
            for (int index = 0; index < this.parts.Count; ++index)
                this.parts[index].def.Worker.SitePartWorkerTick(this.parts[index]);
            if (!this.HasMap)
                return;
            this.CheckRecordAssaultSuccessfulTale();
            this.CheckAllEnemiesDefeated();
        }

        public override void PostMapGenerate()
        {
            base.PostMapGenerate();
            Map map = this.Map;
            for (int index = 0; index < this.parts.Count; ++index)
                this.parts[index].def.Worker.PostMapGenerate(map);
            float a = 0.0f;
            for (int index = 0; index < this.parts.Count; ++index)
                a = Mathf.Max(a, this.parts[index].def.forceExitAndRemoveMapCountdownDurationDays);
            int ticks = Mathf.RoundToInt(a * MapParentTuning.SiteDetectionCountdownMultiplier.RandomInRange * 60000f);
            this.GetComponent<TimedDetectionRaids>().StartDetectionCountdown(ticks);
            this.allEnemiesDefeatedSignalSent = false;
        }

        public override void DrawExtraSelectionOverlays()
        {
            base.DrawExtraSelectionOverlays();
            for (int index = 0; index < this.parts.Count; ++index)
                this.parts[index].def.Worker.PostDrawExtraSelectionOverlays(this.parts[index]);
        }

        public override void Notify_MyMapAboutToBeRemoved()
        {
            base.Notify_MyMapAboutToBeRemoved();
            for (int index = 0; index < this.parts.Count; ++index)
                this.parts[index].def.Worker.Notify_SiteMapAboutToBeRemoved(this.parts[index]);
        }

        public override bool ShouldRemoveMapNow(out bool alsoRemoveWorldObject)
        {
            if (!this.Map.mapPawns.AnyPawnBlockingMapRemoval)
            {
                alsoRemoveWorldObject = !this.parts.Any<SitePart>((Predicate<SitePart>)(x =>
                    x.def.Worker is SitePartWorker_ConditionCauser && x.conditionCauser != null && !x.conditionCauser.Destroyed));
                return true;
            }
            alsoRemoveWorldObject = false;
            return false;
        }

        public override void GetChildHolders(List<IThingHolder> outChildren)
        {
            base.GetChildHolders(outChildren);
            for (int index = 0; index < this.parts.Count; ++index)
                outChildren.Add((IThingHolder)this.parts[index]);
        }

        public override IEnumerable<FloatMenuOption> GetFloatMenuOptions(
            Caravan caravan)
        {
            Site site = this;
            // ISSUE: reference to a compiler-generated method
            foreach (FloatMenuOption floatMenuOption in site.\u003C\u003En__1(caravan))
                yield return floatMenuOption;
            if (!site.HasMap)
            {
                foreach (FloatMenuOption floatMenuOption in CaravanArrivalAction_VisitSite.GetFloatMenuOptions(caravan, site))
                    yield return floatMenuOption;
            }
        }

        public override IEnumerable<FloatMenuOption> GetTransportPodsFloatMenuOptions(
            IEnumerable<IThingHolder> pods,
            CompLaunchable representative)
        {
            Site site = this;
            // ISSUE: reference to a compiler-generated method
            foreach (FloatMenuOption floatMenuOption in site.\u003C\u003En__2(pods, representative))
                yield return floatMenuOption;
            foreach (FloatMenuOption floatMenuOption in TransportPodsArrivalAction_VisitSite.GetFloatMenuOptions(representative, pods,
                site))
                yield return floatMenuOption;
        }

        public override IEnumerable<Gizmo> GetGizmos()
        {
            Site site = this;
            // ISSUE: reference to a compiler-generated method
            foreach (Gizmo gizmo in site.\u003C\u003En__3())
                yield return gizmo;
            if (site.HasMap && Find.WorldSelector.SingleSelectedObject == site)
                yield return (Gizmo)SettleInExistingMapUtility.SettleCommand(site.Map, true);
        }

        private void CheckRecordAssaultSuccessfulTale()
        {
            if (!this.anyEnemiesInitially || this.caravanAssaultSuccessfulTaleRecorded ||
                GenHostility.AnyHostileActiveThreatToPlayer(this.Map))
                return;
            this.caravanAssaultSuccessfulTaleRecorded = true;
            if (!this.Map.mapPawns.FreeColonists.Any<Pawn>())
                return;
            TaleRecorder.RecordTale(TaleDefOf.CaravanAssaultSuccessful, (object)this.Map.mapPawns.FreeColonists.RandomElement<Pawn>());
        }

        private void CheckAllEnemiesDefeated()
        {
            if (this.allEnemiesDefeatedSignalSent || !this.HasMap || GenHostility.AnyHostileActiveThreatToPlayer(this.Map, true))
                return;
            QuestUtility.SendQuestTargetSignals(this.questTags, "AllEnemiesDefeated", this.Named("SUBJECT"));
            this.allEnemiesDefeatedSignalSent = true;
        }

        public override bool AllMatchingObjectsOnScreenMatchesWith(WorldObject other) =>
            other is Site site && site.MainSitePartDef == this.MainSitePartDef;

        public override string GetInspectString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(base.GetInspectString());
            Site.tmpSitePartsLabels.Clear();
            for (int index = 0; index < this.parts.Count; ++index)
            {
                if (!this.parts[index].hidden)
                {
                    if (this.MainSitePart == this.parts[index] && !this.parts[index].def.mainPartAllThreatsLabel.NullOrEmpty() &&
                        (double)this.ActualThreatPoints > 0.0)
                    {
                        stringBuilder.Length = 0;
                        stringBuilder.Append(this.parts[index].def.mainPartAllThreatsLabel.CapitalizeFirst());
                        break;
                    }
                    string processedThreatLabel = this.parts[index].def.Worker.GetPostProcessedThreatLabel(this, this.parts[index]);
                    if (!processedThreatLabel.NullOrEmpty())
                    {
                        if (stringBuilder.Length != 0)
                            stringBuilder.AppendLine();
                        stringBuilder.Append(processedThreatLabel.CapitalizeFirst());
                    }
                }
            }
            return stringBuilder.ToString();
        }

        public override string GetDescription()
        {
            string description1 = this.MainSitePartDef.description;
            string description2 = base.GetDescription();
            if (!description2.NullOrEmpty())
            {
                if (!description1.NullOrEmpty())
                    description1 += "\n\n";
                description1 += description2;
            }
            return description1;
        }
    }
}
