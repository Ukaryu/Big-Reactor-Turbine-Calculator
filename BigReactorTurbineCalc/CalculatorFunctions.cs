using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigReactorTurbineCalc
{
    class CalculatorFunctions
    {
        List<Materials> materialsList = new List<Materials>();

        public void LoadJson()
        {
            using (StreamReader r = new StreamReader("materials.json"))
            {
                string json = r.ReadToEnd();
                materialsList = JsonConvert.DeserializeObject<List<Materials>>(json);
            }
        }

    }

    class Materials
    {
        private string name;
        private double dragCoefficient;
        private double energyBonus;
        private double efficiency;

        public Materials(string name, double dragCoefficient, double energyBonus, double efficiency)
        {
            this.name = name;
            this.dragCoefficient = dragCoefficient;
            this.energyBonus = energyBonus;
            this.efficiency = efficiency;
        }
    }

    
}
