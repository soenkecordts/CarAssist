using System;
using System.Reflection;
using Xamarin.Forms;

namespace CarAssist.Utils
{
    public class LoggerView : ILogger
    {
        private View _view;
        private PropertyInfo _propertyInfo;

        public LoggerView(View view, bool overwrite = true)
        {
            var type = view.GetType();
            var attr = Attribute.GetCustomAttribute(type, typeof(ContentPropertyAttribute)) as ContentPropertyAttribute;

            _propertyInfo = type.GetProperty(attr.Name);

            if (_propertyInfo.PropertyType != typeof(string))
                throw new ArgumentException("Wrong view for logging, must contain an ContentPropertyAttribute of type string.");

            _view = view;
        }

        public bool Overwrite { get; set; }

        public void WriteLine(object obj)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                string text = "";

                if(Overwrite)
                    text = obj.ToString() + '\n';
                else
                    text = (string)_propertyInfo.GetValue(_view) + obj.ToString() + '\n';

                _propertyInfo.SetValue(_view, text);

                if (_view.Parent is ScrollView)
                    (_view.Parent as ScrollView).ScrollToAsync(_view, ScrollToPosition.End, false);
            });
        }
    }
}
