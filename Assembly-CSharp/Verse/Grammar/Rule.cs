using System;
using System.Collections.Generic;
using System.Linq;

namespace Verse.Grammar
{
	// Token: 0x02000905 RID: 2309
	public abstract class Rule
	{
		// Token: 0x1700091D RID: 2333
		// (get) Token: 0x0600395F RID: 14687
		public abstract float BaseSelectionWeight { get; }

		// Token: 0x1700091E RID: 2334
		// (get) Token: 0x06003960 RID: 14688 RVA: 0x00016647 File Offset: 0x00014847
		public virtual float Priority
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x06003961 RID: 14689 RVA: 0x00167294 File Offset: 0x00165494
		public virtual Rule DeepCopy()
		{
			Rule rule = (Rule)Activator.CreateInstance(base.GetType());
			rule.keyword = this.keyword;
			rule.tag = this.tag;
			rule.requiredTag = this.requiredTag;
			if (this.constantConstraints != null)
			{
				rule.constantConstraints = this.constantConstraints.ToList<Rule.ConstantConstraint>();
			}
			return rule;
		}

		// Token: 0x06003962 RID: 14690
		public abstract string Generate();

		// Token: 0x06003963 RID: 14691 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void Init()
		{
		}

		// Token: 0x06003964 RID: 14692 RVA: 0x001672F0 File Offset: 0x001654F0
		public void AddConstantConstraint(string key, string value, Rule.ConstantConstraint.Type type)
		{
			if (this.constantConstraints == null)
			{
				this.constantConstraints = new List<Rule.ConstantConstraint>();
			}
			this.constantConstraints.Add(new Rule.ConstantConstraint
			{
				key = key,
				value = value,
				type = type
			});
		}

		// Token: 0x06003965 RID: 14693 RVA: 0x0016733C File Offset: 0x0016553C
		public void AddConstantConstraint(string key, string value, string op)
		{
			Rule.ConstantConstraint.Type type;
			if (!(op == "=="))
			{
				if (!(op == "!="))
				{
					if (!(op == "<"))
					{
						if (!(op == ">"))
						{
							if (!(op == "<="))
							{
								if (!(op == ">="))
								{
									type = Rule.ConstantConstraint.Type.Equal;
									Log.Error("Unknown ConstantConstraint type: " + op, false);
								}
								else
								{
									type = Rule.ConstantConstraint.Type.GreaterOrEqual;
								}
							}
							else
							{
								type = Rule.ConstantConstraint.Type.LessOrEqual;
							}
						}
						else
						{
							type = Rule.ConstantConstraint.Type.Greater;
						}
					}
					else
					{
						type = Rule.ConstantConstraint.Type.Less;
					}
				}
				else
				{
					type = Rule.ConstantConstraint.Type.NotEqual;
				}
			}
			else
			{
				type = Rule.ConstantConstraint.Type.Equal;
			}
			this.AddConstantConstraint(key, value, type);
		}

		// Token: 0x040027BB RID: 10171
		[MayTranslate]
		public string keyword;

		// Token: 0x040027BC RID: 10172
		[NoTranslate]
		public string tag;

		// Token: 0x040027BD RID: 10173
		[NoTranslate]
		public string requiredTag;

		// Token: 0x040027BE RID: 10174
		public List<Rule.ConstantConstraint> constantConstraints;

		// Token: 0x02000906 RID: 2310
		public struct ConstantConstraint
		{
			// Token: 0x040027BF RID: 10175
			[MayTranslate]
			public string key;

			// Token: 0x040027C0 RID: 10176
			[MayTranslate]
			public string value;

			// Token: 0x040027C1 RID: 10177
			public Rule.ConstantConstraint.Type type;

			// Token: 0x02000907 RID: 2311
			public enum Type
			{
				// Token: 0x040027C3 RID: 10179
				Equal,
				// Token: 0x040027C4 RID: 10180
				NotEqual,
				// Token: 0x040027C5 RID: 10181
				Less,
				// Token: 0x040027C6 RID: 10182
				Greater,
				// Token: 0x040027C7 RID: 10183
				LessOrEqual,
				// Token: 0x040027C8 RID: 10184
				GreaterOrEqual
			}
		}
	}
}
