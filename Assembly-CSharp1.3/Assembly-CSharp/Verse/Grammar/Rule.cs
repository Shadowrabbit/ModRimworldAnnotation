using System;
using System.Collections.Generic;
using System.Linq;

namespace Verse.Grammar
{
	// Token: 0x02000538 RID: 1336
	public abstract class Rule
	{
		// Token: 0x170007E7 RID: 2023
		// (get) Token: 0x06002841 RID: 10305
		public abstract float BaseSelectionWeight { get; }

		// Token: 0x170007E8 RID: 2024
		// (get) Token: 0x06002842 RID: 10306 RVA: 0x000682C5 File Offset: 0x000664C5
		public virtual float Priority
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x06002843 RID: 10307 RVA: 0x000F5CC8 File Offset: 0x000F3EC8
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

		// Token: 0x06002844 RID: 10308
		public abstract string Generate();

		// Token: 0x06002845 RID: 10309 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void Init()
		{
		}

		// Token: 0x06002846 RID: 10310 RVA: 0x000F5D24 File Offset: 0x000F3F24
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

		// Token: 0x06002847 RID: 10311 RVA: 0x000F5D70 File Offset: 0x000F3F70
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
									Log.Error("Unknown ConstantConstraint type: " + op);
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

		// Token: 0x06002848 RID: 10312 RVA: 0x000F5E00 File Offset: 0x000F4000
		public bool ValidateConstraints(Dictionary<string, string> constraints)
		{
			bool result = true;
			if (this.constantConstraints != null)
			{
				for (int i = 0; i < this.constantConstraints.Count; i++)
				{
					Rule.ConstantConstraint constantConstraint = this.constantConstraints[i];
					string text = (constraints != null) ? constraints.TryGetValue(constantConstraint.key, "") : "";
					float num = 0f;
					float num2 = 0f;
					bool flag = !text.NullOrEmpty() && !constantConstraint.value.NullOrEmpty() && float.TryParse(text, out num) && float.TryParse(constantConstraint.value, out num2);
					bool flag2;
					switch (constantConstraint.type)
					{
					case Rule.ConstantConstraint.Type.Equal:
						flag2 = text.EqualsIgnoreCase(constantConstraint.value);
						break;
					case Rule.ConstantConstraint.Type.NotEqual:
						flag2 = !text.EqualsIgnoreCase(constantConstraint.value);
						break;
					case Rule.ConstantConstraint.Type.Less:
						flag2 = (flag && num < num2);
						break;
					case Rule.ConstantConstraint.Type.Greater:
						flag2 = (flag && num > num2);
						break;
					case Rule.ConstantConstraint.Type.LessOrEqual:
						flag2 = (flag && num <= num2);
						break;
					case Rule.ConstantConstraint.Type.GreaterOrEqual:
						flag2 = (flag && num >= num2);
						break;
					default:
						Log.Error("Unknown ConstantConstraint type: " + constantConstraint.type);
						flag2 = false;
						break;
					}
					if (!flag2)
					{
						result = false;
						break;
					}
				}
			}
			return result;
		}

		// Token: 0x040018DD RID: 6365
		[MayTranslate]
		public string keyword;

		// Token: 0x040018DE RID: 6366
		[NoTranslate]
		public string tag;

		// Token: 0x040018DF RID: 6367
		[NoTranslate]
		public string requiredTag;

		// Token: 0x040018E0 RID: 6368
		public int? usesLimit;

		// Token: 0x040018E1 RID: 6369
		public List<Rule.ConstantConstraint> constantConstraints;

		// Token: 0x02001CFD RID: 7421
		public struct ConstantConstraint
		{
			// Token: 0x04007009 RID: 28681
			[MayTranslate]
			public string key;

			// Token: 0x0400700A RID: 28682
			[MayTranslate]
			public string value;

			// Token: 0x0400700B RID: 28683
			public Rule.ConstantConstraint.Type type;

			// Token: 0x02002ABF RID: 10943
			public enum Type
			{
				// Token: 0x0400A0B0 RID: 41136
				Equal,
				// Token: 0x0400A0B1 RID: 41137
				NotEqual,
				// Token: 0x0400A0B2 RID: 41138
				Less,
				// Token: 0x0400A0B3 RID: 41139
				Greater,
				// Token: 0x0400A0B4 RID: 41140
				LessOrEqual,
				// Token: 0x0400A0B5 RID: 41141
				GreaterOrEqual
			}
		}
	}
}
