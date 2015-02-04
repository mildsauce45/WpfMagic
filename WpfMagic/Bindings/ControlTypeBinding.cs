using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace WpfMagic.Bindings
{
    /// <summary>
    /// This binding determines the type to return when resolving the control to use for any attribute implementing the IControlBinding interface.
    /// </summary>
    internal class ControlTypeBinding
    {
        public Type ControlType { get; private set; }

        /// <summary>
        /// True if the control type implements the ICommandSource interface
        /// </summary>
        public bool IsCommandSource { get; private set; }

        /// <summary>
        /// True if there is a property on the control type that has a property of type ICommand
        /// </summary>
        public bool HasCommandProperty { get; private set; }

        public ControlTypeBinding(Type ct, bool isCommandSource, bool hasCommandProperty)
        {
            ControlType = ct;
            IsCommandSource = isCommandSource;
            HasCommandProperty = hasCommandProperty;
        }

        public ControlTypeBinding(Type ct)
        {
            ControlType = ct;
            IsCommandSource = typeof(ICommandSource).IsAssignableFrom(ct);
            HasCommandProperty = ct.GetProperty("Command", typeof(ICommand)) != null;
        }
    }
}
