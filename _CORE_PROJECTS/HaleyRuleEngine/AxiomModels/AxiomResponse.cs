using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Haley.Enums;

namespace Haley.Models
{
    public class AxiomResponse
    {
        public string id { get; set; }
        public ActionStatus status { get; set; }
        public AxiomException exception { get; set; }
        public string comments { get; set; }
        public void setID(string axiom_id)
        {
            id = axiom_id;
        }
        #region Override Methods
        public override string ToString()
        {
            return $@"{status.ToString()} : {exception.ToString()}" ;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        public override bool Equals(object obj)
        {
            var _obj = (AxiomResponse)obj;

            return this.id == _obj.id;
        }
        #endregion

        public AxiomResponse(ActionStatus _status, string _comments = null)
        {
            status = _status; 
            comments = _comments;
            exception = AxiomException.NoException;
        }
    }
}
