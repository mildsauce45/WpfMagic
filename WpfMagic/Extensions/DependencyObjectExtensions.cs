using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Collections;

namespace WpfMagic.Extensions
{
    internal static class DependencyObjectExtensions
    {
        public static IEnumerable<T> ChildrenOfType<T>(this DependencyObject d)
        {
            if (d == null)
                yield break;

            for (var i = 0; i < VisualTreeHelper.GetChildrenCount(d); i++)
            {
                var child = VisualTreeHelper.GetChild(d, i) as object;
                if (child != null && child is T)
                    yield return (T)child;

                if (child is DependencyObject)
                {
                    foreach (T grandchild in (child as DependencyObject).ChildrenOfType<T>())
                        yield return grandchild;
                }
            } 
        }

        public static IEnumerable<T> LogicalChildrenOfType<T>(this DependencyObject d)
        {
            if (d == null)
                yield break;

            var children = LogicalTreeHelper.GetChildren(d);

            foreach (var c in children)
            {
                if (c != null && c is T)
                    yield return (T)c;

                if (c is DependencyObject)
                {
                    foreach (T grandchild in (c as DependencyObject).LogicalChildrenOfType<T>())
                        yield return grandchild;
                }
            }
        }
    }
}
