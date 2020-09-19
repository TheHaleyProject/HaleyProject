using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Haley.MVVM.Interfaces;

namespace Haley.MVVM.IOC
{
    public class UserControlService
    {
        #region Initation
        private Dictionary<Type, Type> viewmodel_controls_mappings { get; set; } //Generic dictionary to store the controls
        private Dictionary<string,Tuple<Type,Type>> main_mapping { get; set; } //Dictionary to store enumvalue and viewmodel type as key and usercontrol as value

        public UserControlService()
        {
            viewmodel_controls_mappings = new Dictionary<Type, Type>();
            main_mapping = new Dictionary<string, Tuple<Type, Type>>();
        }
        #endregion

        #region Helper Methods

        private string _getEnumKey(Enum @enum)
        {
            try
            {
                string enum_type_name = @enum.GetType().Name;
                string enum_value_name = @enum.ToString();
                string enum_key = enum_type_name + "." + enum_value_name; //Concatenated value for storing as key
                return enum_key;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Register Methods

        public void register<ViewModelType, ControlType>()
            where ViewModelType : IFlipperControlVM
            where ControlType : IFlipperControl
        {
            try
            {
                if (viewmodel_controls_mappings.ContainsKey(typeof(ViewModelType)) == true)
                {
                    throw new ArgumentException($"{typeof(ViewModelType)} is already registered to {viewmodel_controls_mappings[typeof(ViewModelType)]}");
                }

                viewmodel_controls_mappings.Add(typeof(ViewModelType), typeof(ControlType));
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public void register<ViewModelType, ControlType>(Enum @enum)
           where ViewModelType : IFlipperControlVM
           where ControlType : IFlipperControl
        {
            try
            {
                //Get the enum value and its type name to prepare a string
                string _key = _getEnumKey(@enum);
                if (main_mapping.ContainsKey(_key) == true)
                {
                    throw new ArgumentException($@"Key : {_key} is already registered to - VM : {main_mapping[_key].Item1} and View : {main_mapping[_key].Item2}");
                }
                Tuple<Type, Type> key_tuple = new Tuple<Type, Type>(typeof(ViewModelType), typeof(ControlType));
                main_mapping.Add(_key, key_tuple);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Retrieval Methods

        public IFlipperControl obtainControl<ViewModelType>(ViewModelType InputViewModel) //Return a generic type which implements IFlipperControl
        where ViewModelType : IFlipperControlVM
        {
            try
            {
                if (viewmodel_controls_mappings.Count == 0)
                {
                    throw new ArgumentException("No viewmodels are registered yet.");
                }

                if (viewmodel_controls_mappings.ContainsKey(typeof(ViewModelType)) == false)
                {
                    throw new ArgumentException($"{typeof(ViewModelType)} is not registered to any controls. Please check.");
                }

                Type ResultControlType = viewmodel_controls_mappings[typeof(ViewModelType)]; //We are using the typeof(viewmodeltype) merely as a reference so that it can fetch the corresponding control from the dictionary. We assign the actual "InputViewModel" as Datacontext later.

                IFlipperControl ResultControl = (IFlipperControl)Activator.CreateInstance(ResultControlType);
                ResultControl.DataContext = InputViewModel;

                return ResultControl;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IFlipperControl obtainControl(object InputViewModel, Enum @enum) //Return a generic type which implements IFlipperControl
        {
            try
            {
                //Get the enum value and its type name to prepare a string
                string _key = _getEnumKey(@enum);

                //Check if this key is already used or not.
                if (main_mapping.Count == 0)
                {
                    throw new ArgumentException("No viewmodel/views are registered yet.");
                }

                if (main_mapping.ContainsKey(_key) == false)
                {
                    throw new ArgumentException($"Key {_key} is not registered to any controls. Please check.");
                }

                var result_tuple = main_mapping[_key];
                Type resultViewModelType = result_tuple.Item1;
                if (InputViewModel.GetType() != resultViewModelType)
                {
                    throw new ArgumentException($"For the key : {_key}, the type of view model expected is : {resultViewModelType} ");
                }

                Type resultControlType = result_tuple.Item2; // Get the type of control for the provided input types
                IFlipperControl resultcontrol = (IFlipperControl)Activator.CreateInstance(resultControlType);
                resultcontrol.DataContext = InputViewModel; //Assinging actual viewmodel
                return resultcontrol;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public object obtainVMInstance(Enum @enum)
        {
            try
            {
                //Get the enum value and its type name to prepare a string
                string _key = _getEnumKey(@enum);
                Type resultViewModelType = main_mapping[_key].Item1;
                var resultVM = Activator.CreateInstance(resultViewModelType);
                return resultVM;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion
    }
}

