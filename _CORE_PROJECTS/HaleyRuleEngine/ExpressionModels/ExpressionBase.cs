using System;
using System.Collections.Generic;
using System.Text;
using Haley.Enums;
using Haley.Abstractions;
using System.Xml.Linq;
using System.Security.Cryptography;
using System.Net.Cache;

namespace Haley.Models
{
    public class ExpressionBase<T>
    {
        public string id { get; }
        public string comments { get; set; }
        public AxiomResponse response { get; set; }
        private AxiomAction<T> action;
        public void invoke(T target,params object[] args)
        {
            try
            {
                response = action.Invoke(target, args);
            }
            catch (Exception ex)
            {
                response = new AxiomResponse(ActionStatus.Exception,ex.ToString());
            }
            finally
            {
                if (response == null)
                {
                    response = new AxiomResponse(ActionStatus.Exception);
                    response.exception = AxiomException.NullInputException;
                }
                response.setID(id);
            }
        }

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
            var _obj = (ExpressionBase<T>)obj;

            return this.id == _obj.id;
        }
        #endregion
        public ExpressionBase(string _axiom_id, AxiomAction<T> _action) 
        {
            id = _axiom_id;
            action = _action;
        }
    }
}
