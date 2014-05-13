using GalaSoft.MvvmLight;
using System.Collections.ObjectModel;

namespace VisualHtmlViewModel
{
    public class TreeItemViewModel : ViewModelBase
    {
        static readonly TreeItemViewModel DummyChild = new TreeItemViewModel();
        private readonly ObservableCollection<TreeItemViewModel> _children;
        private readonly TreeItemViewModel _parent;

        private bool _isExpanded;
        private bool _isSelected;

        protected TreeItemViewModel(TreeItemViewModel parent, bool lazyLoadChildren)
        {
            _parent = parent;
            _children = new ObservableCollection<TreeItemViewModel>();

            if (lazyLoadChildren)
            {
                _children.Add(DummyChild);
            }
        }

        private TreeItemViewModel()
        {
        }

        public ObservableCollection<TreeItemViewModel> Children
        {
            get { return _children; }
        }

        public bool HasDummyChild
        {
            get { return _children.Count == 1 && _children[0] == DummyChild; }
        }

        public bool IsExpanded
        {
            get { return _isExpanded; }
            set
            {
                if (!value == _isExpanded)
                {
                    _isExpanded = value;
                    RaisePropertyChanged(() => IsExpanded);
                }

                if (_isExpanded && _parent != null)
                {
                    _parent.IsExpanded = true;
                }

                if (HasDummyChild)
                {
                    _children.Remove(DummyChild);
                    LoadChildren();
                }
            }
        }

        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                if (!value == _isSelected)
                {
                    _isSelected = value;
                    RaisePropertyChanged(() => IsSelected);
                }
            }
        }

        public TreeItemViewModel Parent
        {
            get { return _parent; }
        }

        protected virtual void LoadChildren() { }
    }
}