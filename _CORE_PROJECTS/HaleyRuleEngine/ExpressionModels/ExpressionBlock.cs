using System;
using System.Collections.Generic;
using System.Text;
using Haley.Enums;
using Haley.Abstractions;
using System.ComponentModel;
using System.Linq;
using System.Xml;

namespace Haley.Models
{
    public class ExpressionBlock<T>
    {
        public string id { get;  }
        private LogicalOperator @operator;

        //Below list of blocks and expressions remain same throughout runtime.
        private List<ExpressionBlock<T>> blocks = new List<ExpressionBlock<T>>();
        private List<ExpressionBase<T>> expressions = new List<ExpressionBase<T>>();

        //Below list holds only temporary value for each target object. So, clear it before validating new object.
        private List<AxiomResponse> response_list = new List<AxiomResponse>();
        private ActionStatus block_status = ActionStatus.None;

        public void validate(T target)
        {
            //We run all the axiom actions irrepsective of the operator. However, we should return result only based on the operator
            try
            {
                response_list = new List<AxiomResponse>();
                block_status = ActionStatus.None;

                //First run all actions and then invoke sub expressions
                //TODO : Run asynchronously
                expressions.ForEach(p =>
                {
                    p.invoke(target);
                    lock(response_list)
                    {
                        response_list.Add(p.response);
                    }
                });
                blocks.ForEach(p => p.validate(target));

                //After validation is done, get block level status and also, flattened response list.
                block_status = _getStatus();
                response_list = _generateResponseList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<AxiomResponse> getFlatReponseList()
        {
            return response_list;
        }
        public ActionStatus getBlockStatus()
        {
            return block_status;
        }

        #region Private Methods
        private List<AxiomResponse> _generateResponseList()
        {
            List<AxiomResponse> result = new List<AxiomResponse>();
            result.AddRange(response_list); //Add base list

            foreach (var block in blocks)  //This should go through all the blocks
            {
                result.AddRange(block._generateResponseList());
            }
            return result;
        }
        private ActionStatus _getStatus()
        {
            List<ActionStatus> _level_status = new List<ActionStatus>();
            //Get all expression status
            expressions.ForEach(p => _level_status.Add(p.response.status));
            //Get all block status
            blocks.ForEach(p => _level_status.Add(p._getStatus()));

            switch (@operator)
            {
                case LogicalOperator.And:
                    if (_level_status.All(p => p == ActionStatus.Pass)) return ActionStatus.Pass; //Only if all pass.
                    break;
                case LogicalOperator.Or:
                    if (_level_status.Any(p => p == ActionStatus.Pass)) return ActionStatus.Pass; //Only if all pass.
                    break;
            }
            return ActionStatus.Fail;
        }
        #endregion

        #region Override Methods
        public override string ToString()
        {
            return id;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        public override bool Equals(object obj)
        {
            var _obj = (ExpressionBlock<T>)obj;

            return this.id == _obj.id;
        }
        #endregion

        public ExpressionBlock(LogicalOperator _operator, List<ExpressionBase<T>> _expressions = null, List<ExpressionBlock<T>> _blocks =null) 
        { 
            id = Guid.NewGuid().ToString(); 
            @operator = _operator;
            blocks = _blocks ?? new List<ExpressionBlock<T>>();
            expressions = _expressions ?? new List<ExpressionBase<T>>();
        }
    }
}
