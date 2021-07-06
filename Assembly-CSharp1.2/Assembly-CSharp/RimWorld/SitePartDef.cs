using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
    // Token: 0x02000FDF RID: 4063
    public class SitePartDef : Def
    {
        public ThingDef conditionCauserDef; //条件原因定义
        public float activeThreatDisturbanceFactor = 1f; //主动威胁干扰因子
        public bool defaultHidden; //默认隐藏
        public Type workerClass = typeof(SitePartWorker);
        [NoTranslate] public string siteTexture; //场地纹理
        [NoTranslate] public string expandingIconTexture; //扩展图标
        public bool applyFactionColorToSiteTexture; //是否支持派系颜色
        public bool showFactionInInspectString; //在视察窗显示派系
        public bool requiresFaction; //需要派系
        public TechLevel minFactionTechLevel; //最低需求科技水平
        [MustTranslate] public string approachOrderString; //接近命令
        [MustTranslate] public string approachingReportString; //接近报告
        [NoTranslate] public List<string> tags = new List<string>(); //标签
        [NoTranslate] public List<string> excludesTags = new List<string>(); //包含标签
        [MustTranslate] public string arrivedLetter; //到达信件
        [MustTranslate] public string arrivedLetterLabelPart; //到达信件标题部分
        public List<HediffDef> arrivedLetterHediffHyperlinks; //到达buff
        public LetterDef arrivedLetterDef; //到达信件
        public bool wantsThreatPoints; //想要的威胁点数
        public float minThreatPoints; //最低威胁点数
        public bool increasesPopulation; //是否增加人口
        public bool badEvenIfNoMap; //
        public float forceExitAndRemoveMapCountdownDurationDays = 4f; //强制离开冷却时间
        public bool handlesWorldObjectTimeoutInspectString; //显示时间
        public string mainPartAllThreatsLabel; //主要部分全部威胁标题
        [Unsaved(false)] private SitePartWorker workerInt; //地点部分处理
        [Unsaved(false)] private Texture2D expandingIconTextureInt; //扩展图标
        [Unsaved(false)] private List<GenStepDef> extraGenSteps; //扩展生成定义

        /// <summary>
        /// 部分运作
        /// </summary>
        public SitePartWorker Worker
        {
            get
            {
                if (this.workerInt == null)
                {
                    this.workerInt = (SitePartWorker) Activator.CreateInstance(this.workerClass);
                    this.workerInt.def = this;
                }

                return this.workerInt;
            }
        }

        // Token: 0x17000DAF RID: 3503
        // (get) Token: 0x0600589D RID: 22685 RVA: 0x001D0648 File Offset: 0x001CE848
        public Texture2D ExpandingIconTexture
        {
            get
            {
                if (this.expandingIconTextureInt == null)
                {
                    if (!this.expandingIconTexture.NullOrEmpty())
                    {
                        this.expandingIconTextureInt = ContentFinder<Texture2D>.Get(this.expandingIconTexture, true);
                    }
                    else
                    {
                        this.expandingIconTextureInt = BaseContent.BadTex;
                    }
                }

                return this.expandingIconTextureInt;
            }
        }

        /// <summary>
        /// 通过连接绑定 找到生成步骤定义
        /// </summary>
        public List<GenStepDef> ExtraGenSteps
        {
            get
            {
                if (this.extraGenSteps == null)
                {
                    this.extraGenSteps = new List<GenStepDef>();
                    List<GenStepDef> allDefsListForReading = DefDatabase<GenStepDef>.AllDefsListForReading;
                    for (int i = 0; i < allDefsListForReading.Count; i++)
                    {
                        if (allDefsListForReading[i].linkWithSite == this)
                        {
                            this.extraGenSteps.Add(allDefsListForReading[i]);
                        }
                    }
                }

                return this.extraGenSteps;
            }
        }

        public SitePartDef()
        {
            this.workerClass = typeof(SitePartWorker);
        }

        /// <summary>
        /// 派系是否可以拥有
        /// </summary>
        /// <param name="faction"></param>
        /// <returns></returns>
        public bool FactionCanOwn(Faction faction)
        {
            //不需要派系或者派系存在 并且派系最小科技水平未定义或派系存在并且科技水平大于最低 并且派系不存在或派系不是玩家并且派系没被打败并且派系不是隐藏的 并且worker允许
            return (!this.requiresFaction || faction != null) &&
                   (this.minFactionTechLevel == TechLevel.Undefined ||
                    (faction != null && faction.def.techLevel >= this.minFactionTechLevel)) &&
                   (faction == null || (!faction.IsPlayer && !faction.defeated && !faction.Hidden)) &&
                   this.Worker.FactionCanOwn(faction);
        }

        /// <summary>
        /// 兼容另一个场地部分
        /// </summary>
        /// <param name="part"></param>
        /// <returns></returns>
        public bool CompatibleWith(SitePartDef part)
        {
            for (int i = 0; i < part.excludesTags.Count; i++)
            {
                if (this.tags.Contains(part.excludesTags[i]))
                {
                    return false;
                }
            }

            for (int j = 0; j < this.excludesTags.Count; j++)
            {
                if (part.tags.Contains(this.excludesTags[j]))
                {
                    return false;
                }
            }

            return true;
        }
    }
}