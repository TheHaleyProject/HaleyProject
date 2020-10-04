using System;
using System.Collections.Generic;
using System.Text;
using Haley.Abstractions;
using Haley.Enums;
using Haley.Models;
using System.Reflection;
using System.Dynamic;

namespace Haley.RuleEngine
{
    public class ExpressionMaker
    {
        #region Main Methods

        private IAxiom axiom;

        public AxiomResponse Make<T>(T target, params object[] runtime_args) 
        {
            try
            {
                //If method axiom, then return the method without processing further.
                Type axiom_type = axiom.GetType();
                //METHOD AXIOM
                if (axiom_type == typeof(MethodAxiom<T>))
                {
                    var _m_axiom = ((MethodAxiom<T>)axiom);
                    return _m_axiom.action.Invoke(target, _m_axiom.parameters); //This is the only place where the parameters is used as of now.
                }

                //GET SOURCE AND TARGET
                object _source_value = _getSource<T>(target);
                object _target_value = _getTarget<T>(target);

                if (_source_value == null || _target_value == null)
                {
                    StringBuilder sbldr = new StringBuilder();
                    sbldr.AppendLine($@"Cannot proceed because of null input values.");
                    sbldr.AppendLine($@"Validation: {axiom.ToString()}");
                    sbldr.AppendLine($@"Source value : {_source_value ?? "Null"}.");
                    sbldr.AppendLine($@"Target value : {_target_value ?? "Null"}.");
                    return new AxiomResponse(ActionStatus.Exception, sbldr.ToString()) {exception = AxiomException.NullInputException };
                }

                var _result = _validateTarget(_source_value, _target_value, axiom.ignore_case);
                return _result;
            }
            catch (Exception ex)
            {
                return new AxiomResponse(ActionStatus.Exception, ex.ToString());
            }
        }
        #endregion

        #region Property Methods
        private object _getSource<T>(T target)
        {
            object _source_value = target; //FOR BINARY AXIOM

            //FOR PROPERTY AXIOM / COMPARISION AXIOM
            Type axiom_type = axiom.GetType();
            if (axiom_type == typeof(PropertyAxiom) || axiom_type.BaseType == typeof(PropertyAxiom) || axiom_type == typeof(ComparisonAxiom) || axiom_type.BaseType == typeof(ComparisonAxiom))
            {
                Func<T, string, object> _getPropValue = GetPropertyValue<T>;
                if (axiom_type.IsGenericType)
                {
                    _getPropValue = ((IAxiomPropGeneric<T>)axiom).getPropertyDelegate;
                    if (_getPropValue == null) _getPropValue = GetPropertyValue<T>;
                }
                _source_value = _getPropValue.Invoke(target, ((IAxiomProp)axiom).primary_property); //This is possible because, both propertyaxiom and comparison axiom implements IAxiomProp
            }
            return _source_value;
        }
        private object _getTarget<T>(T target)
        {
            object _target_value = axiom.value; //FOR BINARY / PROPERTY AXIOM
            Type axiom_type = axiom.GetType();
            if (axiom_type == typeof(ComparisonAxiom) || axiom_type.BaseType == typeof(ComparisonAxiom))
            {
                Func<T, string, object> _getPropValue = GetPropertyValue<T>;
                if (axiom_type.IsGenericType)
                {
                    _getPropValue = ((IAxiomPropGeneric<T>)axiom).getPropertyDelegate;
                    if (_getPropValue == null) _getPropValue = GetPropertyValue<T>;
                }
                _target_value = _getPropValue.Invoke(target, ((IAxiomComparison)axiom).secondary_property); //This is possible because, both propertyaxiom and comparison axiom implements IAxiomProp
            }
            return _target_value;
        }
        private object GetPropertyValue<T>(T target, string prop_name)
        {
            object result = null;

            //Try to get the property directly or else consider it as a expando object and try to get the property
            PropertyInfo pinfo = target.GetType().GetProperty(prop_name); //Property name should be accurate. Cannot have ignore case.
            if (pinfo != null) result = pinfo.GetValue(target);
            if (pinfo == null && target is IDynamicMetaObjectProvider) //For expando objects
            {
                IDictionary<string, object> objectprovider = (IDictionary<string, object>)target;
                result = objectprovider[prop_name.ToLower()]; //If we are trying to get the value from expando object, set it as lower.
            }
            return result;
        }
        #endregion

        #region Validation Helpers
        private AxiomResponse _validateTarget(object _source_value, object _target_value, bool ignore_case = true)
        {
            //Problem here is the sourceobject can be a string or a double. What we need to do is, just consider whatever it is and then convert it to string
            StringBuilder sbuilder = new StringBuilder();
            try
            {
                
                var source_value = ConvertToString(_source_value);
                var target_value = ConvertToString(_target_value);

                sbuilder.AppendLine($@"To validate : {source_value} {axiom.@operator.ToString()} {target_value}");
                sbuilder.AppendLine($@"Axiom: {axiom.ToString()}");
                sbuilder.AppendLine($@"Source value : {source_value}.");
                sbuilder.AppendLine($@"Target value : {target_value}.");

                AxiomResponse _response = null;

               switch (ignore_case)
                {
                    case true:
                        _response = _baseCheck(source_value.ToLower(), target_value.ToLower());
                        _response.comments = sbuilder.ToString();
                        break;
                    case false:
                        _response  = _baseCheck(source_value, target_value);
                        _response.comments = sbuilder.ToString();
                        break;
                }
                return _response;
            }
            catch (Exception ex)
            {
                sbuilder.AppendLine($@"Exception: {ex.ToString()}");
                return new AxiomResponse(ActionStatus.Fail, sbuilder.ToString());
            }
        }
        private AxiomResponse _baseCheck(string _source, string _target)
        {
            try
            {
                double _sourcedbl;
                double _targetdbl;
                switch (axiom.@operator)
                {
                    case AxiomOperator.Contains:
                        if (_source.Contains(_target)) return new AxiomResponse(ActionStatus.Pass);
                        break;
                    case AxiomOperator.NotContains:
                        if (!_source.Contains(_target)) return new AxiomResponse(ActionStatus.Pass);
                        break;
                    case AxiomOperator.Equals:
                        if (_source.Equals(_target)) return new AxiomResponse(ActionStatus.Pass);
                        break;
                    case AxiomOperator.NotEquals:
                        if (!_source.Equals(_target)) return new AxiomResponse(ActionStatus.Pass);
                        break;
                    case AxiomOperator.StartsWith:
                        if (_source.StartsWith(_target)) return new AxiomResponse(ActionStatus.Pass);
                        break;
                    case AxiomOperator.EndsWith:
                        if (_source.EndsWith(_target)) return new AxiomResponse(ActionStatus.Pass);
                        break;
                    case AxiomOperator.GreaterThan:
                        _sourcedbl = ConvertToDouble(_source);
                        _targetdbl = ConvertToDouble(_target);
                        if (_sourcedbl > _targetdbl) return new AxiomResponse(ActionStatus.Pass);
                        break;
                    case AxiomOperator.LessThan:
                        _sourcedbl = ConvertToDouble(_source);
                        _targetdbl = ConvertToDouble(_target);
                        if (_sourcedbl < _targetdbl) return new AxiomResponse(ActionStatus.Pass);
                        break;
                }
                return new AxiomResponse(ActionStatus.Fail);
            }
            catch (Exception ex)
            {
                return new AxiomResponse(ActionStatus.Exception, ex.ToString()) { exception = AxiomException.FormatException};
            }

        }
        #endregion

        #region TypeConverters
        private string ConvertToString(object input)
        {
            try
            {
                string result = null;
                if (input == null) return result;
                //Input can be a string type or numerical type or custom type.
                if (input is string)
                {
                    result = (string)input;
                }
                else if (input is double || input is int || input is float || input is decimal) //Can be double, int, float, decimal or anything.
                {
                    result = (Convert.ToDouble(input)).ToString();
                }
                else
                {
                    result = input.ToString();
                }
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private double ConvertToDouble(object input)
        {
            try
            {
                double result = 0;
                if (input == null) return result;
                //Input can be string type or numerical type.
                if (input is string)
                {
                    result = double.Parse(((string)input));
                }
                else //Can be double, int, float, decimal or anything.
                {
                    result = Convert.ToDouble(input);
                }
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }
        #endregion  

        public ExpressionMaker(IAxiom _axiom) { axiom = _axiom; }
    }
}
