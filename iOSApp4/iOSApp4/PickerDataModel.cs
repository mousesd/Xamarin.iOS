using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;

namespace iOSApp4
{
    public class PickerDataModel<T> : UIPickerViewModel
    {
        public event EventHandler<EventArgs> ValueChanged;

        private int selectedIndex = 0;
        public IList<NamedValue<T>> Items { get; }

        public NamedValue<T> SelectedItem
        {
            get
            {
                return this.Items != null && selectedIndex >= 0 && selectedIndex < this.Items.Count
                    ? this.Items[selectedIndex] : null;
            }
        }

        public PickerDataModel()
        {
            this.Items = new List<NamedValue<T>>();
        }

        public override nint GetRowsInComponent(UIPickerView pickerView, nint component)
        {
            return this.Items?.Count ?? 0;
        }

        public override string GetTitle(UIPickerView pickerView, nint row, nint component)
        {
            return this.Items != null && this.Items.Count > row ? this.Items[(int)row].Name : null;
        }

        public override nint GetComponentCount(UIPickerView pickerView)
        {
            return 1;
        }

        public override void Selected(UIPickerView pickerView, nint row, nint component)
        {
            selectedIndex = (int)row;
            this.ValueChanged?.Invoke(this, new EventArgs());
        }
    }

    public class NamedValue<T>
    {
        public string Name { get; }
        public T Value { get; }

        public NamedValue(string name, T value)
        {
            this.Name = name;
            this.Value = value;
        }
    }
}