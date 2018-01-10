using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BigReactorTurbineCalc
{
    public class MainViewModel
    {
        private CalculatorFunctions calcFuncs { get; set; }

        public ObservableCollection<Material> listOfMaterials { get { return calcFuncs.materialsList; }  }

        public Material selectedMaterial { get { return calcFuncs.selectedMaterial; } set { selectedMaterialChanged(value); } }

        public ObservableCollection<TurbineBuild> mostEfficient { get { return calcFuncs.mostEfficientBuilds; } }

        public MainViewModel()
        {
            calcFuncs = new CalculatorFunctions();
            SelectedMaterialChanged = new DelegateCommand<Material>(selectedMaterialChanged);
        }

        public ICommand SelectedMaterialChanged { get; set; }
        private void selectedMaterialChanged(Material mat)
        {
            calcFuncs.selectedMaterial = mat;
            calcFuncs.calcMostEfficientBuilds();
        } 
    }
}
