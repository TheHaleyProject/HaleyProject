using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Haley.Abstractions;
using Haley.Enums;
using Haley.Models;

namespace Haley.RuleEngine
{
    public class RuleEngine
    {
        #region Static Methods
        public static Dictionary<Rule, ExpressionBlock<T>> CompileRules<T>(List<Rule> rule_list)
        {
            Dictionary<Rule, ExpressionBlock<T>> result = new Dictionary<Rule, ExpressionBlock<T>>();

            foreach (Rule rule in rule_list)
            {
                var _expBlockForRule = CompileBlocks<T>(rule.block);
                result.Add(rule, _expBlockForRule);
            }
            return result;
        }

        private static ExpressionBlock<T> CompileBlocks<T>(RuleBlock _ruleBlock)
        {
            //Process all expressions inside the block
            List<ExpressionBase<T>> _exp_list = new List<ExpressionBase<T>>();
            foreach (var _axiom in _ruleBlock.getAxioms())
            {
                ExpressionMaker exp_maker = new ExpressionMaker(_axiom);
                ExpressionBase<T> _exp_inst = new ExpressionBase<T>(_axiom.id, exp_maker.Make);
                _exp_list.Add(_exp_inst);
            }

            //Process all blocks inside the block
            List<ExpressionBlock<T>> _block_list = new List<ExpressionBlock<T>>();
            foreach (var sub_block in _ruleBlock.getBlocks())
            {
                _block_list.Add(CompileBlocks<T>(sub_block));
            }

            //TODO: FIX PARAMETER ARGUMENTS FOR THE EXPRESSION BLOCK
            ExpressionBlock<T> _expblock = new ExpressionBlock<T>(_ruleBlock.getOperator(), _exp_list, _block_list);
            return _expblock;
        }
        #endregion
    }

    public class RuleEngine<T>
    {
        private Dictionary<Rule, ExpressionBlock<T>> rule_dic = new Dictionary<Rule, ExpressionBlock<T>>();

        public void ProcessRules(T target)
        {
            foreach (var item in rule_dic)
            {
                item.Value.validate(target);
                item.Key.status = item.Value.getBlockStatus();
            }
        }

        public void Compile(List<Rule> rules)
        {
            rule_dic = RuleEngine.CompileRules<T>(rules);
        }

        public Dictionary<Rule, ExpressionBlock<T>> getRuleDictionary()
        {
            return rule_dic;
        }
        public RuleEngine(){}
    }
}
