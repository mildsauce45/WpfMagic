using System;
using System.ComponentModel;
using System.Linq.Expressions;

namespace WpfMagic.Mvvm
{
    public class NotifyableObject : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged<T>(Expression<Func<T>> propertyExpression)
        {
            if (propertyExpression.Body.NodeType == ExpressionType.MemberAccess)
            {
                var memberExpr = propertyExpression.Body as MemberExpression;

                var propertyName = memberExpr.Member.Name;

				SafeRaise(propertyName);
            }
        }

		protected void NotifyPropertyChanged(string propertyName)
		{
			SafeRaise(propertyName);
		}

		private void SafeRaise(string propertyName)
		{
			if (PropertyChanged != null && !string.IsNullOrWhiteSpace(propertyName))
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
		}
    }
}
