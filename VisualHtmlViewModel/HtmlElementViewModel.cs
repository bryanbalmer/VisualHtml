using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using VisualHtmlModel;

namespace VisualHtmlViewModel
{
    public class HtmlElementViewModel : TreeItemViewModel
    {
        private readonly HtmlElement _element;

        private Dictionary<string, string> _attributes;
        private Dictionary<string, ObservableCollection<TreeItemViewModel>> _childrenByName = new Dictionary<string, ObservableCollection<TreeItemViewModel>>();
        private string _innerText = string.Empty;
        private string _name = string.Empty;
        private PropertyChangedEventHandler _propertyChanged;

        private string _selectedChild = string.Empty;

        private ObservableCollection<TreeItemViewModel> _visibleChildren;

        public HtmlElementViewModel(HtmlElement element, HtmlElementViewModel parent, bool isRootElement, PropertyChangedEventHandler propertyChanged)
            : base(parent, isRootElement)
        {
            _element = element;
            _element.InnerText = _element.InnerText.Trim();
            _propertyChanged = propertyChanged;
            InitializeProperties();
        }

        public Dictionary<string, string> Attributes
        {
            get { return _attributes; }
            set
            {
                if (value == _attributes)
                    return;
                _attributes = value;
                RaisePropertyChanged(() => Attributes);
            }
        }

        public ObservableCollection<string> ChildNames { get; set; }

        public bool HasAttributes
        {
            get { return (_attributes != null) ? (_attributes.Count > 0) : false; }
        }

        public bool HasInnerText
        {
            get { return _innerText != string.Empty; }
        }

        public string InnerText
        {
            get { return _innerText; }
            set
            {
                if (value == _innerText)
                    return;
                _innerText = value;
                RaisePropertyChanged(() => InnerText);
            }
        }

        public string Name
        {
            get { return _name; }
            set
            {
                if (value == _name)
                    return;
                _name = value;
                RaisePropertyChanged(() => Name);
            }
        }

        public string SelectedChild
        {
            get { return _selectedChild; }
            set
            {
                if (value == _selectedChild)
                    return;
                _selectedChild = value;
                RaisePropertyChanged(() => SelectedChild);

                if (_childrenByName.ContainsKey(value))
                    VisibleChildren = _childrenByName[value];
            }
        }

        public ObservableCollection<TreeItemViewModel> VisibleChildren
        {
            get { return _visibleChildren; }
            set
            {
                if (value == _visibleChildren)
                    return;
                _visibleChildren = value;
                RaisePropertyChanged(() => VisibleChildren);
            }
        }

        protected override void LoadChildren()
        {
            if (_element.ChildElements != null)
            {
                foreach (HtmlElement element in _element.ChildElements)
                {
                    var item = new HtmlElementViewModel(element, this, true, _propertyChanged);
                    Children.Add(item);

                    if (!_childrenByName.ContainsKey(item.Name))
                        _childrenByName.Add(item.Name, new ObservableCollection<TreeItemViewModel>());
                    _childrenByName[item.Name].Add(item);

                    if (!ChildNames.Contains(element.Name))
                        ChildNames.Add(element.Name);
                }
            }
        }

        private void InitializeProperties()
        {
            ChildNames = new ObservableCollection<string>();
            ChildNames.Add("All");

            Name = _element.Name;

            Attributes = _element.Attributes;

            InnerText = _element.InnerText;
            PropertyChanged += _propertyChanged;
            _childrenByName.Add("All", Children);
            VisibleChildren = _childrenByName["All"];

        }
    }
}