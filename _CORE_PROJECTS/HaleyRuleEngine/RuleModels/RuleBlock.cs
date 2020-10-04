using Haley.Abstractions;
using Haley.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Haley.Models
{
    public class RuleBlock
    {
        public string id { get; }
        private LogicalOperator @operator;
        private List<RuleBlock> _block_list = new List<RuleBlock>();
        private List<IAxiom> _axiom_list = new List<IAxiom>();

        #region RuleBlock 
        public bool add(RuleBlock block)
        {
            _block_list.Add(block); //before adding check if already exists
            return true;
        }

        public bool remove(RuleBlock block)
        {
            _block_list.Remove(block);
            return true;
        }
        public List<RuleBlock> getBlocks()
        {
            return _block_list;
        }
        #endregion

        #region Axiom 
        public bool add(IAxiom axiom)
        {
            _axiom_list.Add(axiom);
            return true;
        }

        public bool remove(IAxiom axiom)
        {
            _axiom_list.Remove(axiom);
            return true;
        }
        public List<IAxiom> getAxioms()
        {
            return _axiom_list;
        }
        #endregion

        #region Operators
        public void setOperator(LogicalOperator _operator)
        {
            @operator = _operator;
        }
        public LogicalOperator getOperator()
        {
            return @operator;
        }
        #endregion

        #region Override Methods
        public override string ToString()
        {
            return $@"{@operator.ToString()} : {id}";
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        public override bool Equals(object obj)
        {
            var _obj = (RuleBlock)obj;

            return this.id == _obj.id;
        }
        #endregion

        public RuleBlock(LogicalOperator _operator = LogicalOperator.And) { id = Guid.NewGuid().ToString(); @operator = _operator; }
    }
}
