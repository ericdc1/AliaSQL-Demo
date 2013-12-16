using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Web.Mvc;

namespace Demo.Website.Models
{
    /// <summary>
    /// This is the base class inhereted by all data container (model) classes. This class can not be instanciated.
    /// </summary>
    /// <remarks></remarks>
    public abstract class BaseModel
    {

        private Dictionary<string, string> _errors = new Dictionary<string, string>();

        /// <summary>
        /// Gets or sets the Errors property value
        /// </summary>
        /// <value></value>
        /// <returns>A Dictionary(Of String, String)</returns>
        /// <remarks></remarks>
        [Editable(false)]
        public Dictionary<string, string> Errors
        {
            get { return _errors; }
            set { _errors = value; }
        }
        /// <summary>
        /// Gets the HasErrors property value
        /// </summary>
        /// <value></value>
        /// <returns>A Boolean</returns>
        /// <remarks></remarks>
        [Editable(false)]
        public bool HasErrors
        {
            get
            {
                return Errors.Count > 0;
            }
        }

        /// <summary>
        /// Returns the string name of the specific property provided in expression.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propertyExpression"></param>
        /// <returns></returns>
        public static string GetPropertyName<T>(Expression<Func<T>> propertyExpression)
        {
            var me = propertyExpression.Body as MemberExpression;
            return me != null ? me.Member.Name : string.Empty;
        }
        /// <summary>
        /// Updates modelstate with the errors from the list of errors
        /// </summary>
        /// <param name="modelstate"></param>
        public void UpdateModelState(ModelStateDictionary modelstate)
        {
            foreach (var error in Errors)
            {
                modelstate.AddModelError(error.Key, error.Value);
            }
        }
    }
}