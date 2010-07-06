﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace VisualValidation
{
    internal class UiTreeHelper 
    {
        public IValidationContainer FindValidationContainer(DependencyObject dependencyObject)
        {
            var container = dependencyObject as IValidationContainer;
            if(container != null) return container;

            var parent = LogicalTreeHelper.GetParent(dependencyObject);
            if(parent == null) return null;

            return FindValidationContainer(parent);
        }
    }
}
