using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using VisualHtmlModel;
using VisualHtmlModel.Data;

namespace VisualHtmlViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private IHtmlParser _htmlParser;

        public MainViewModel(IHtmlParser htmlParser)
        {
            _htmlParser = htmlParser;
            Elements = new ObservableCollection<HtmlElementViewModel>();
            InitializeCommands();
        }

        #region Properties

        private string _url = string.Empty;
        public string Url
        {
            get { return _url; }
            set
            {
                if (value == _url)
                    return;
                _url = value;
                RaisePropertyChanged(() => Url);
            }
        }

        public ObservableCollection<HtmlElementViewModel> Elements
        {
            get;
            set;
        }

        private Dictionary<string, string> _selectedElementAttributes = new Dictionary<string, string>();
        public Dictionary<string, string> SelectedElementAttributes 
        {
            get { return _selectedElementAttributes; }
            set 
            {
                if (value == _selectedElementAttributes)
                    return;
                _selectedElementAttributes = value;
                RaisePropertyChanged(() => SelectedElementAttributes);
            }
        }

        private string _selectedElementInnerText = string.Empty;
        public string SelectedElementInnerText
        {
            get { return _selectedElementInnerText; }
            set
            {
                if (value == _selectedElementInnerText)
                    return;
                _selectedElementInnerText = value;
                RaisePropertyChanged(() => SelectedElementInnerText);
            }
        }
       
        #endregion

        #region Commands

        private void InitializeCommands()
        {
            GetPageCommand = new RelayCommand(() => ExecuteGetHtmlCommand());
        }

        public RelayCommand GetPageCommand { get; private set; }

        private async void ExecuteGetHtmlCommand()
        {
            var status = await _htmlParser.Load(Url);

            if (status == 0)
            {
                Elements.Clear();
                var root = await Task.Run<List<HtmlElement>>(() => _htmlParser.GetRootElements());

                foreach (var item in root)
                {
                    var itemViewModel = new HtmlElementViewModel(item, null, true, HtmlElementViewModel_PropertyChanged);
                    Elements.Add(itemViewModel);
                }
            }
            else
            {
                MessageBox.Show("Url not responding.");
            }
        }

        void HtmlElementViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (sender.GetType() == typeof(HtmlElementViewModel))
            {
                var senderItem = (HtmlElementViewModel)sender;
                if (e.PropertyName.Equals("IsSelected"))
                {
                    SelectedElementAttributes = (senderItem.HasAttributes) ? senderItem.Attributes : null;
                    SelectedElementInnerText = senderItem.InnerText;
                }
            }
        }

        #endregion

        #region Methods

 
        #endregion
    }
}