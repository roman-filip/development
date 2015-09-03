using Microsoft.Practices.ObjectBuilder;

namespace $rootnamespace$.$fileinputname$
{
    /// <summary>
    /// Interaction logic for $fileinputname$View.xaml
    /// </summary>
    public partial class $fileinputname$View
    {
        [InjectionConstructor]
        public $fileinputname$View([CreateNew]$fileinputname$ViewModel viewModel)
        {
            InitializeComponent();

            SetViewModel(viewModel);
        }
    }
}
